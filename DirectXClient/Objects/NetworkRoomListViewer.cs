using GameFramework;
using GameFramework.Manager;
using GameFramework.Structure;
using SharpDX;
using System;
using System.Collections.Generic;
using TCPNetwork.Client;

namespace DirectXClient
{
    class NetworkRoomListViewer : GameObject
    {
        List<Tuple<NetworkRoomTitle, Button, Button>> networkRoomList;

        GameTexture PageBar { get; set; } = null;

        private const int listHeight = 5;

        private int LastIndex = 0;
        public Vector3 Position { get; set; } = default;
        public bool IsShouldRefresh { get; set; } = false;

        public override void Initialize()
        {
            NetworkClientManager.Instance.OnUpdateRoomListEvent += OnUpdateRoomList;

            networkRoomList = new List<Tuple<NetworkRoomTitle, Button, Button>>();

            D3D9Manager.Instance.CreateTexture("JoinButton", "./Resource/Join.png");
            D3D9Manager.Instance.CreateTexture("DeleteButton", "./Resource/Delete.png");
            D3D9Manager.Instance.CreateTexture("RefreshButton", "./Resource/RefreshButton.png");
            D3D9Manager.Instance.CreateTexture("NextButton", "./Resource/NextButton.png");
            D3D9Manager.Instance.CreateTexture("PrevButton", "./Resource/PrevButton.png");

            D3D9Manager.Instance.CreateFont("PageFont", "메이플스토리 Bold", 40, false);

            PageBar = D3D9Manager.Instance.CreateTexture("PageBar", "./Resource/PageBar.png");

            Button refreshButton = GameObjectManager.Instance
                   .AddObject(new Button()
                   {
                       ButtonTexture = D3D9Manager.Instance.FindTexture("RefreshButton"),
                       IsMouseOverResize = true,
                       Position = new Vector3(139, 50, 0) + Position,
                       Scale = new Vector3(1, 1, 1)
                   });

            refreshButton.OnButtonClick += delegate
            {
                LastIndex = 0;
                NetworkClientManager.Instance.RoomListUpdateRequest();
            };

            Button prevButton = GameObjectManager.Instance
                   .AddObject(new Button()
                   {
                       ButtonTexture = D3D9Manager.Instance.FindTexture("PrevButton"),
                       IsMouseOverResize = true,
                       Position = new Vector3(139 + 265, 50, 0) + Position,
                       Scale = new Vector3(1, 1, 1)
                   });

            prevButton.OnButtonClick += delegate
            {
                if (LastIndex > 0)
                    LastIndex--;
                NetworkClientManager.Instance.RoomListUpdateRequest();
            };

            Button nextButton = GameObjectManager.Instance
                   .AddObject(new Button()
                   {
                       ButtonTexture = D3D9Manager.Instance.FindTexture("NextButton"),
                       IsMouseOverResize = true,
                       Position = new Vector3(139 + 265 * 2, 50, 0) + Position,
                       Scale = new Vector3(1, 1, 1)
                   });

            nextButton.OnButtonClick += delegate
            {
                if (HasList(LastIndex + 1))
                    LastIndex++;
                NetworkClientManager.Instance.RoomListUpdateRequest();
            };

            NetworkClientManager.Instance.RoomListUpdateRequest();
        }

        public override void FrameUpdate()
        {
            if (IsShouldRefresh)
            {
                RefreshList(LastIndex, NetworkClientManager.Instance.RoomList);
                IsShouldRefresh = false;
            }
        }

        public override void FrameRender()
        {
            D3D9Manager.Instance.DrawTexture(PageBar, Position +
                new Vector3(404, 480, 0), new Vector3(1, 1, 1));
            D3D9Manager.Instance.DrawFont_NotSetTransform("PageFont", new Vector3(0, -17, 0), (LastIndex + 1).ToString(),
                new Color(127, 127, 127, 255));
        }

        public override void Release()
        {
            NetworkClientManager.Instance.OnUpdateRoomListEvent -= OnUpdateRoomList;
        }

        public void RefreshList(int index, List<string> list)
        {
            LastIndex = index;

            foreach (Tuple<NetworkRoomTitle, Button, Button> Iter in networkRoomList)
            {
                Destroy(Iter.Item1);
                Destroy(Iter.Item2);
                Destroy(Iter.Item3);
            }
            networkRoomList.Clear();

            for (int i = index * listHeight; i < index * listHeight + listHeight; i++)
            {
                if (i >= list.Count)
                    break;

                string roomName = list[i];

                NetworkRoomTitle roomTitle = GameObjectManager.Instance
                    .AddObject(new NetworkRoomTitle(roomName)
                    {
                        Position = new Vector3(240, 120 + (i - index * listHeight) * 70, 0) + Position
                    });

                Button roomJoinButton = GameObjectManager.Instance
                    .AddObject(new Button()
                    {
                        ButtonTexture = D3D9Manager.Instance.FindTexture("JoinButton"),
                        IsMouseOverResize = true,
                        Position = new Vector3(530, 120 + (i - index * listHeight) * 70, 0) + Position,
                        Scale = new Vector3(1, 1, 1)
                    });

                Button roomDeleteButton = GameObjectManager.Instance
                   .AddObject(new Button()
                   {
                       ButtonTexture = D3D9Manager.Instance.FindTexture("DeleteButton"),
                       IsMouseOverResize = true,
                       Position = new Vector3(697, 120 + (i - index * listHeight) * 70, 0) + Position,
                       Scale = new Vector3(1, 1, 1)
                   });

                roomJoinButton.OnButtonClick += delegate (object sender, EventArgs e)
                {
                    NetworkClientManager.Instance.JoinRoomRequest(roomTitle.RoomTitle);
                };

                roomDeleteButton.OnButtonClick += delegate
                {
                    NetworkClientManager.Instance.DestroyRoomRequest(roomTitle.RoomTitle);
                    NetworkClientManager.Instance.RoomListUpdateRequest();
                };

                networkRoomList.Add(new Tuple<NetworkRoomTitle, Button, Button>(
                    roomTitle, roomJoinButton, roomDeleteButton));
            }
        }

        public bool HasList(int index)
        {
            if (NetworkClientManager.Instance.RoomListCount > index * listHeight)
                return true;
            else
                return false;
        }

        public void OnUpdateRoomList(object sender, List<string> roomList)
        {
            IsShouldRefresh = true;
        }
    }
}

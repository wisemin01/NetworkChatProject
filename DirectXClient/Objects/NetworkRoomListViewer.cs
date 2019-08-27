using ChattingNetwork.Client;
using GameFramework;
using GameFramework.Manager;
using GameFramework.Structure;
using SharpDX;
using System;
using System.Collections.Generic;

namespace DirectXClient
{
    class NetworkRoomListViewer : GameObject
    {
        List<Tuple<NetworkRoomTitle, Button>> networkRoomList;

        GameTexture PageBar { get; set; } = null;

        Vector3 refreshButtonPosition = new Vector3(0, 127.5f, 0);
        Vector3 createRoomButtonPosition = new Vector3(0, 172.5f, 0);
        Vector3 prevButtonPosition = new Vector3(-96, 79.5f, 0);
        Vector3 nextButtonPosition = new Vector3(100, 79.5f, 0);

        private const int listHeight = 3;

        Vector3[] selectButtonPosition = new Vector3[listHeight];

        private int LastIndex = 0;

        private int listCountCache = 0;

        Button refreshButton = null;
        Button createRoomButton = null;
        Button prevButton = null;
        Button nextButton = null;
        Button[] selectButton = new Button[listHeight];


        public override void Initialize()
        {
            networkRoomList = new List<Tuple<NetworkRoomTitle, Button>>();

            D3D9Manager.Instance.CreateTexture("RefreshButton", "./Resource/RefreshButton.png");
            D3D9Manager.Instance.CreateTexture("NextButton", "./Resource/NextButton.png");
            D3D9Manager.Instance.CreateTexture("PrevButton", "./Resource/PrevButton.png");

            D3D9Manager.Instance.CreateFont("PageFont", "Segoe UI", 40, false);

            refreshButton = GameObjectManager.Instance
                   .AddObject(new Button()
                   {
                       ButtonTexture = D3D9Manager.Instance.FindTexture("RefreshButton"),
                       IsMouseOverResize = true,
                       Position = refreshButtonPosition + Position,
                       Scale = new Vector3(1, 1, 1),
                       Parent = this
                   });

            refreshButton.OnButtonClick += delegate
            {
                LastIndex = 0;
                ClientManager.Instance.GetRoomList();
            };

            prevButton = GameObjectManager.Instance
                   .AddObject(new Button()
                   {
                       ButtonTexture = D3D9Manager.Instance.FindTexture("PrevButton"),
                       IsMouseOverResize = true,
                       Position = prevButtonPosition + Position,
                       Scale = new Vector3(1, 1, 1),
                       Parent = this
                   });

            prevButton.OnButtonClick += delegate
            {
                if (LastIndex > 0)
                    LastIndex--;
                else
                    MessageBox.Show("이전 페이지가 존재하지 않습니다,", "경고");

                ClientManager.Instance.GetRoomList();
            };

            nextButton = GameObjectManager.Instance
                   .AddObject(new Button()
                   {
                       ButtonTexture = D3D9Manager.Instance.FindTexture("NextButton"),
                       IsMouseOverResize = true,
                       Position = nextButtonPosition + Position,
                       Scale = new Vector3(1, 1, 1),
                       Parent = this
                   });

            nextButton.OnButtonClick += delegate
            {
                if (HasList(LastIndex + 1))
                    LastIndex++;
                else
                    MessageBox.Show("다음 페이지가 존재하지 않습니다,", "경고");

                ClientManager.Instance.GetRoomList();
            };

            createRoomButton = GameObjectManager.Instance
                .AddObject(new Button()
                {
                    ButtonTexture = D3D9Manager.Instance.CreateTexture("CreateRoomButton", "./Resource/CreateRoomButton.png"),
                    IsMouseOverResize = true,
                    Position = createRoomButtonPosition + Position,
                    Scale = new Vector3(1, 1, 1),
                    Parent = this
                });

            createRoomButton.OnButtonClick += delegate
            {
                GameObjectManager.Instance.AddObject(new CreateRoomWindow()
                {
                    Position = new Vector3(ClientWindow.Width / 2, ClientWindow.Height / 2, 0)
                });
            };

            ClientManager.Instance.GetRoomList();
            ClientManager.Instance.OnRoomListRefresh += OnRoomListRefresh;
        }

        private void OnRoomListRefresh(object sender, List<string> e)
        {
            listCountCache = e.Count;

            RefreshList(LastIndex, e);
        }

        public override void FrameUpdate()
        {
            Fold();
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
            ClientManager.Instance.OnRoomListRefresh -= OnRoomListRefresh;
        }

        public void RefreshList(int index, List<string> list)
        {
            LastIndex = index;

            foreach (var Iter in networkRoomList)
            {
                Destroy(Iter.Item1);
                Destroy(Iter.Item2);
            }

            networkRoomList.Clear();

            Vector3 defaultV = new Vector3(0, -151.5f, 0);

            for (int i = index * listHeight; i < index * listHeight + listHeight; i++)
            {
                int realIndex = i - index * listHeight;

                if (i >= list.Count)
                {
                    selectButton[realIndex] = null;
                    break;
                }

                string roomName = list[i];


                Button roomJoinButton = GameObjectManager.Instance
                    .AddObject(new Button()
                    {
                        ButtonTexture = D3D9Manager.Instance.CreateTexture("RoomTitle", "./Resource/RoomTitle.png"),
                        IsMouseOverResize = true,
                        Position = new Vector3(0, realIndex * 85, 0) + defaultV + Position,
                        Scale = new Vector3(1, 1, 1),
                        Parent = this
                    });

                NetworkRoomTitle roomTitle = GameObjectManager.Instance
                    .AddObject(new NetworkRoomTitle(roomName)
                    {
                        Position = new Vector3(-140, -17.5f, 0),
                        Parent = roomJoinButton
                    });

                selectButtonPosition[realIndex] = new Vector3(0, realIndex * 85, 0) + defaultV;
                selectButton[realIndex] = roomJoinButton;

                roomJoinButton.OnButtonClick += delegate (object sender, EventArgs e)
                {
                    ClientManager.Instance.JoinRoom(roomTitle.RoomTitle);
                };

                networkRoomList.Add(new Tuple<NetworkRoomTitle, Button>(
                    roomTitle, roomJoinButton));
            }

            if(IsActive == false)
                FocusUIs(new Vector3(1088, 56, 0));
        }

        public bool HasList(int index)
        {
            if (listCountCache > index * listHeight)
                return true;
            else
                return false;
        }

        public void Fold()
        {
            const float lerpSpeed = 1;

            if (refreshButton != null)
                refreshButton.Position = Vector3.Lerp(refreshButton.Position, Position + refreshButtonPosition, lerpSpeed);
            if (createRoomButton != null)
                createRoomButton.Position = Vector3.Lerp(createRoomButton.Position, Position + createRoomButtonPosition, lerpSpeed);
            if (prevButton != null)
                prevButton.Position = Vector3.Lerp(prevButton.Position, Position + prevButtonPosition, lerpSpeed);
            if (nextButton != null)
                nextButton.Position = Vector3.Lerp(nextButton.Position, Position + nextButtonPosition, lerpSpeed);

            for (int i = 0; i < listHeight; i++)
            {
                if (selectButton[i] != null)
                    selectButton[i].Position = Vector3.Lerp(selectButton[i].Position, Position + selectButtonPosition[i], lerpSpeed);
            }
        }

        public override void OnEnable()
        {
        }

        public override void OnDisable()
        {
            FocusUIs(new Vector3(1088, 56, 0));
        }

        private void FocusUIs(Vector3 pos)
        {
            if (refreshButton != null)
                refreshButton.Position = pos;
            if (createRoomButton != null)
                createRoomButton.Position = pos;
            if (prevButton != null)
                prevButton.Position = pos + new Vector3(-100, 0, 0);
            if (nextButton != null)
                nextButton.Position = pos + new Vector3(100, 0, 0);

            for (int i = 0; i < listHeight; i++)
            {
                if (selectButton[i] != null)
                    selectButton[i].Position = pos;
            }
        }
    }
}

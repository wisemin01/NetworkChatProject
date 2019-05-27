using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFramework;
using GameFramework.Manager;
using TCPNetwork.Client;
using SharpDX;

namespace DirectXClient
{
    class NetworkRoomListViewer : GameObject
    {
        List<Tuple<NetworkRoomTitle, Button, Button>> networkRoomList;

        private int LastIndex = 0;
        public Vector3 Position { get; set; } = default;

        public override void Initialize()
        {
            networkRoomList = new List<Tuple<NetworkRoomTitle, Button, Button>>();

            D3D9Manager.Instance.CreateTexture("JoinButton", "./Resource/Join.png");
            D3D9Manager.Instance.CreateTexture("DeleteButton", "./Resource/Delete.png");
            D3D9Manager.Instance.CreateTexture("RefreshButton", "./Resource/RefreshButton.png");

            Button refreshButton = GameObjectManager.Instance
                   .AddObject(new Button()
                   {
                       ButtonTexture        = D3D9Manager.Instance.FindTexture("RefreshButton"),
                       IsMouseOverResize    = true,
                       Position             = new Vector3(820, 60, 0) + Position,
                       Scale                = new Vector3(1, 1, 1)
                   });

            refreshButton.OnButtonClick += delegate { RefreshList(); };

            RefreshList(0);
        }

        public override void FrameUpdate()
        {
            NetworkClientManager.Instance.SendMessageToServer("/GetRoomList");
        }

        public override void FrameRender()
        {

        }

        public override void Release()
        {

        }

        public void RefreshList(int index = 0)
        {
            LastIndex = index;

            foreach (var Iter in networkRoomList)
            {
                Destroy(Iter.Item1);
                Destroy(Iter.Item2);
                Destroy(Iter.Item3);
            }
            networkRoomList.Clear();

            List<string> list = NetworkClientManager.Instance.GetNetworkRooms();

            for (int i = index * 5; i < index + 5; i++)
            {
                if (i >= list.Count)
                    break;

                string roomName = list[i];

                NetworkRoomTitle roomTitle = GameObjectManager.Instance
                    .AddObject(new NetworkRoomTitle(roomName)
                    {
                        Position = new Vector3(240, 60 + (i - index) * 70, 0) + Position
                    });

                Button roomJoinButton = GameObjectManager.Instance
                    .AddObject(new Button()
                    {
                        ButtonTexture = D3D9Manager.Instance.FindTexture("JoinButton"),
                        IsMouseOverResize = true,
                        Position = new Vector3(530, 60 + (i - index) * 70, 0) + Position,
                        Scale = new Vector3(1, 1, 1)
                    });

                Button roomDeleteButton = GameObjectManager.Instance
                   .AddObject(new Button()
                   {
                       ButtonTexture = D3D9Manager.Instance.FindTexture("DeleteButton"),
                       IsMouseOverResize = true,
                       Position = new Vector3(697, 60 + (i - index) * 70, 0) + Position,
                       Scale = new Vector3(1, 1, 1)
                   });

                roomJoinButton.OnButtonClick += delegate (object sender, EventArgs e)
                {
                    NetworkClientManager.Instance.SendMessageToServer($"/Join/{roomTitle.RoomTitle}");
                };

                roomDeleteButton.OnButtonClick += delegate
                {
                    NetworkClientManager.Instance.SendMessageToServer($"/DestroyRoom/{roomTitle.RoomTitle}");
                    RefreshList(LastIndex);
                };

                networkRoomList.Add(new Tuple<NetworkRoomTitle, Button, Button>(
                    roomTitle, roomJoinButton, roomDeleteButton));
            }
        }
    }
}

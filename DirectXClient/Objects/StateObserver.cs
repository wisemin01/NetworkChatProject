using GameFramework;
using GameFramework.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectXClient
{
    class StateObserver : GameObject
    {
        public override void FrameRender()
        {
        }

        public override void FrameUpdate()
        {
        }

        public override void Initialize()
        {
            TCPNetwork.Client.NetworkClientManager.Instance.OnChangeRoomEvent += OnChangeRoom;
        }

        public override void Release()
        {
            TCPNetwork.Client.NetworkClientManager.Instance.OnChangeRoomEvent -= OnChangeRoom;
        }

        private void OnChangeRoom(object sender, string text)
        {
            if (text == "Lobby")
            {
                SceneManager.Instance.ChangeScene("Lobby");
            }
        }
    }
}

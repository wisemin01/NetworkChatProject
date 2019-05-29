using GameFramework;
using GameFramework.Manager;

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
            else
            {
                if (TCPNetwork.Client.NetworkClientManager.Instance.IsConnection)
                {
                    SceneManager.Instance.ChangeScene("Chat");
                }
                else
                {
                    SceneManager.Instance.ChangeScene("Login");
                }
            }
        }
    }
}

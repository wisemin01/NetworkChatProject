using GameFramework;
using GameFramework.Manager;

namespace DirectXClient
{
    class SceneObserver : GameObject
    {
        public bool IsShouldChangeScene { get; set; } = false;

        public string LatestSceneName { get; private set; } = string.Empty;

        public override void FrameRender()
        {
        }

        public override void FrameUpdate()
        {
            if (IsShouldChangeScene)
            {
                SceneManager.Instance.ChangeScene(LatestSceneName);
                IsShouldChangeScene = false;
            }
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
                LatestSceneName = "Lobby";
            }
            else
            {
                if (TCPNetwork.Client.NetworkClientManager.Instance.IsConnection)
                {
                    LatestSceneName = "Chat";
                }
                else
                {
                    LatestSceneName = "Login";
                }
            }

            IsShouldChangeScene = true;
        }
    }
}

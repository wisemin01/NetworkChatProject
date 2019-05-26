using System;
using GameFramework;
using GameFramework.Manager;
using SharpDX;
using TCPNetwork.Client;
using SharpDX.DirectInput;

namespace DirectXClient
{
    public class ChatScene : Scene
    {
        public ChatScene() : base() { }

        public override void Initialize()
        {
            Direct3D9Manager.Instance.CreateFont("ChatListFont", "메이플스토리 Light", 25, false);
            
            var TextList
                = GameObjectManager.Instance.AddObject(new TextList(25,
                new Vector3(15, Direct3D9Manager.Instance.WindowHeight - 200, 0),
                "ChatListFont"));

            if (NetworkClientManager.Instance.IsConnection == false)
            {
                NetworkClientManager.Instance.TextDraw = TextList;
                bool serverConnectSuccess = NetworkClientManager.Instance.ConnectToServer();

                if (serverConnectSuccess == false)
                {
                    return;
                }
            }

            var TextInput = GameObjectManager.Instance.AddObject(new TextInput("ChatListFont")
            {
                Position = new Vector3(15, Direct3D9Manager.Instance.WindowHeight - 100, 0)
            });

            TextInput.OnEnter += delegate (object sender, string s) { NetworkClientManager.Instance.SendMessageToServer(s); };
        }

        public override void FrameRender()
        {
            GameObjectManager.Instance.RenderObjects();
        }

        public override void FrameUpdate()
        {
            if (NetworkClientManager.Instance.IsConnection == false)
            {
                SceneManager.Instance.ChangeScene("Login");
            }

            GameObjectManager.Instance.UpdateObjects();
        }

        public override void Release()
        {
            GameObjectManager.Instance.ReleaseObjects();
            Direct3D9Manager.Instance.FontDispose();
            Direct3D9Manager.Instance.TextureDispose();
        }
    }
}

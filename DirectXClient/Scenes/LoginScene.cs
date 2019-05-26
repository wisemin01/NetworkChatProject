using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameFramework;
using GameFramework.Manager;
using TCPNetwork.Client;
using SharpDX;
using System.Windows.Forms;

namespace DirectXClient
{
    class LoginScene : Scene
    {
        public override void Initialize()
        {
            Direct3D9Manager.Instance.CreateTexture("LoginButton", "./Resource/LoginButton.png");

            Direct3D9Manager.Instance.CreateFont("InputField", "메이플스토리 Bold", 35, false);

            var textInput = GameObjectManager.Instance.AddObject(new TextInput("InputField")
            {
                Position = new Vector3(300, 300, 0)
            });

            var loginButton = GameObjectManager.Instance.AddObject(new Button() {
                ButtonTexture   = Direct3D9Manager.Instance.FindTexture("LoginButton"),
                Position        = new Vector3(700, 300, 0),
                Scale           = new Vector3(1.0f, 1.0f, 1.0f)
            });
            
            loginButton.OnButtonClick += textInput.EnterText;

            textInput.OnEnter += delegate (object sender, string userName) {
                NetworkClientManager.Instance.Initialize(userName, "127.0.0.1", 9199,
                    delegate (string exitComment) { Direct3D9Manager.Instance.Exit(); });
                SceneManager.Instance.ChangeScene("Lobby");
            };
        }

        public override void FrameUpdate()
        {
            GameObjectManager.Instance.UpdateObjects();
        }

        public override void FrameRender()
        {
            GameObjectManager.Instance.RenderObjects();
        }

        public override void Release()
        {
            GameObjectManager.Instance.ReleaseObjects();
            Direct3D9Manager.Instance.FontDispose();
            Direct3D9Manager.Instance.TextureDispose();
        }
    }
}

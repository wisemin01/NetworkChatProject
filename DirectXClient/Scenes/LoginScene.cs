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
using GameFramework.Structure;

namespace DirectXClient
{
    class LoginScene : Scene
    {
        public override void Initialize()
        {
            D3D9Manager.Instance.CreateTexture("LoginButton", "./Resource/LoginButton.png");
            D3D9Manager.Instance.CreateTexture("NameInput", "./Resource/NameInput.png");

            D3D9Manager.Instance.CreateFont("InputField", "메이플스토리 Bold", 35, false);

            var textInput = GameObjectManager.Instance.AddObject(new TextInputField("InputField")
            {
                Position        = new Vector3(ClientWindow.Width / 2, 300, 0),
                FieldTexture    = D3D9Manager.Instance.FindTexture("NameInput"),
                MaxLength       = 20,
                StringColor     = new Color(127, 127, 127),
                StringOffset    = new Vector3(10, 3, 0)
            });

            var loginButton = GameObjectManager.Instance.AddObject(new Button()
            {
                ButtonTexture   = D3D9Manager.Instance.FindTexture("LoginButton"),
                Position        = new Vector3(ClientWindow.Width / 2, 370, 0),
                Scale           = new Vector3(1.0f, 1.0f, 1.0f),
                IsMouseOverResize = true
            });

            loginButton.OnButtonClick += textInput.EnterText;

            //textInput.OnEnter += delegate { MessageBox.Show("asdasdas", "Error"); };

            textInput.OnEnter += delegate (object sender, string userName)
            {
                NetworkClientManager.Instance.Initialize(userName, "127.0.0.1", 9199,
                    delegate (string exitComment) { D3D9Manager.Instance.Exit(); });
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
            D3D9Manager.Instance.FontDispose();
            D3D9Manager.Instance.TextureDispose();
        }
    }
}

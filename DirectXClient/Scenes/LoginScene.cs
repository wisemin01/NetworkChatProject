using GameFramework;
using GameFramework.Manager;
using SharpDX;

using ChattingNetwork.Client;

namespace DirectXClient
{
    class LoginScene : Scene
    {
        public override void Initialize()
        {
            D3D9Manager.Instance.CreateTexture("LoginButton", "./Resource/LoginButton.png");
            D3D9Manager.Instance.CreateTexture("NameInput", "./Resource/NameInput.png");

            D3D9Manager.Instance.CreateFont("InputField", "메이플스토리 Light", 35, false);

            TextInputField idInput = GameObjectManager.Instance.AddObject(new TextInputField("InputField")
            {
                Position = new Vector3(ClientWindow.Width / 2, 200, 0),
                FieldTexture = D3D9Manager.Instance.FindTexture("NameInput"),
                MaxLength = 15,
                StringColor = new Color(127, 127, 127),
                StringOffset = new Vector3(30, 6, 0)
            });

            TextInputField passwordInput = GameObjectManager.Instance.AddObject(new TextInputField("InputField")
            {
                Position = new Vector3(ClientWindow.Width / 2, 275, 0),
                FieldTexture = D3D9Manager.Instance.FindTexture("NameInput"),
                MaxLength = 15,
                StringColor = new Color(127, 127, 127),
                StringOffset = new Vector3(30, 6, 0)
            });

            Button loginButton = GameObjectManager.Instance.AddObject(new Button()
            {
                ButtonTexture = D3D9Manager.Instance.FindTexture("LoginButton"),
                Position = new Vector3(ClientWindow.Width / 2, 370, 0),
                Scale = new Vector3(1.0f, 1.0f, 1.0f),
                IsMouseOverResize = true
            });

            loginButton.OnButtonClick += delegate
            {
                bool result = ClientManager.Instance.SignIn(idInput.Text, passwordInput.Text);

                if (result == true)
                {
                    ClientManager.Instance.OnSignIn += OnSignIn;

                    idInput.Clear();
                    passwordInput.Clear();
                }
            };

        }

        public override void FrameUpdate()
        {
            //if (NetworkClientManager.Instance.IsConnection)
            //{
            //    SceneManager.Instance.ChangeScene("Lobby");
            //}
        }

        public override void FrameRender()
        {
        }

        public override void Release()
        {
            D3D9Manager.Instance.FontDispose();
            D3D9Manager.Instance.TextureDispose();
        }

        public void OnSignIn(object sender, bool value)
        {
            if (value == true)
            {
                // 로그인 성공
                SceneManager.Instance.ChangeScene("Lobby");
            }
            else
            {
                // 로그인 실패
                MessageBox.Show("아이디와 비밀번호를 다시 확인해주세요.", "[!] Login Failed");
            }

            ClientManager.Instance.OnSignIn -= OnSignIn;
        }
    }
}

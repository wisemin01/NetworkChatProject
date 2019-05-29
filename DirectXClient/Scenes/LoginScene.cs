﻿
using GameFramework;
using GameFramework.Manager;
using SharpDX;
using TCPNetwork.Client;

namespace DirectXClient
{
    class LoginScene : Scene
    {
        public override void Initialize()
        {
            D3D9Manager.Instance.CreateTexture("LoginButton", "./Resource/LoginButton.png");
            D3D9Manager.Instance.CreateTexture("NameInput", "./Resource/NameInput.png");

            D3D9Manager.Instance.CreateFont("InputField", "메이플스토리 Light", 35, false);

            TextInputField textInput = GameObjectManager.Instance.AddObject(new TextInputField("InputField")
            {
                Position = new Vector3(ClientWindow.Width / 2, 300, 0),
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

            loginButton.OnButtonClick += textInput.EnterText;

            textInput.OnEnter += delegate (object sender, string userName)
            {
                if (string.IsNullOrWhiteSpace(userName))
                {
                    MessageBox.Show("이름에는 공백 문자가\n포함될 수 없습니다.", "알림");
                    return;
                }

                NetworkClientManager.Instance.Initialize(userName, "127.0.0.1", 9199,
                    delegate (string exitComment)
                    {
                        D3D9Manager.Instance.Exit();
                    });
                NetworkClientManager.Instance.ConnectToServer();

                TextInputField input = sender as TextInputField;
                input.Clear();
            };
        }

        public override void FrameUpdate()
        {
            if (NetworkClientManager.Instance.IsConnection)
            {
                SceneManager.Instance.ChangeScene("Lobby");
            }
        }

        public override void FrameRender()
        {
        }

        public override void Release()
        {
            D3D9Manager.Instance.FontDispose();
            D3D9Manager.Instance.TextureDispose();
        }
    }
}

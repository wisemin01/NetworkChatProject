using GameFramework;
using GameFramework.Manager;
using SharpDX;

using ChattingNetwork.Client;
using System;

namespace DirectXClient
{
    internal partial class SignInScene
    {
        private TextureObject background = null;
        private TextInputField idInput = null;
        private TextInputField passwordInput = null;
        private Button loginButton = null;
        private Button toSignUpButton = null;

        private void InitializeComponent()
        {
            #region UI RESOURCE INITIALIZE SOURCE CODE

            /// <summary>
            /// background UI Resource Initialize
            /// </summary>
            background = new TextureObject();

            background.Position = new Vector3(ClientWindow.Width / 2, ClientWindow.Height / 2, 0);
            background.Scale = new Vector3(1, 1, 1);
            background.Texture = D3D9Manager.Instance.FindTexture("Background");

            /// <summary>
            /// idInput UI Resource Initialize
            /// </summary>
            idInput = new TextInputField("InputField");

            idInput.Position = new Vector3(619, 415, 0);
            idInput.FieldTexture = D3D9Manager.Instance.FindTexture("NameInput");
            idInput.MaxLength = 15;
            idInput.StringColor = new Color(127, 127, 127);
            idInput.StringOffset = new Vector3(20, 6, 0);

            /// <summary>
            /// passwordInput UI Resource Initialize
            /// </summary>
            passwordInput = new TextInputField("InputField");

            passwordInput.Position = new Vector3(619, 472, 0);
            passwordInput.FieldTexture = D3D9Manager.Instance.FindTexture("NameInput");
            passwordInput.MaxLength = 15;
            passwordInput.StringColor = new Color(127, 127, 127);
            passwordInput.StringOffset = new Vector3(20, 6, 0);
            passwordInput.PasswordChar = '●';

            /// <summary>
            /// loginButton UI Resource Initialize
            /// </summary>
            loginButton = new Button();

            loginButton.ButtonTexture = D3D9Manager.Instance.FindTexture("LoginButton");
            loginButton.Position = new Vector3(850, 443, 0);
            loginButton.Scale = new Vector3(1.0f, 1.0f, 1.0f);
            loginButton.IsMouseOverResize = true;
            loginButton.OnButtonClick += LoginButton_OnClick;

            /// <summary>
            /// toSignUpButton UI Resource Initialize
            /// </summary>
            toSignUpButton = new Button();

            toSignUpButton.ButtonTexture = D3D9Manager.Instance.FindTexture("ToSignupButton");
            toSignUpButton.Position = new Vector3(100, 670, 0);
            toSignUpButton.Scale = new Vector3(1.0f, 1.0f, 1.0f);
            toSignUpButton.IsMouseOverResize = true;

            toSignUpButton.OnButtonClick += ToSignUpButton_OnClick;


            GameObjectManager.Instance.AddObject(background);
            GameObjectManager.Instance.AddObject(idInput);
            GameObjectManager.Instance.AddObject(passwordInput);
            GameObjectManager.Instance.AddObject(loginButton);
            GameObjectManager.Instance.AddObject(toSignUpButton);

            #endregion
        }

        private void Dispose()
        {
            background = null;
            idInput = null;
            passwordInput = null;
            loginButton = null;
            toSignUpButton = null;
        }

    }
}

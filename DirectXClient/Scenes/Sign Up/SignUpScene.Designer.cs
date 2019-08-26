using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFramework;
using GameFramework.Manager;
using GameFramework.Structure;
using SharpDX;
using ChattingNetwork;
using ChattingNetwork.Client;


namespace DirectXClient
{
    internal partial class SignUpScene
    {
        private void InitializeComponent()
        {
            #region UI RESOURCE INITIALIZE SOURCE CODE

            /// <summary>
            /// idInput UI Resource Initialize
            /// </summary>
            idInput = new TextInputField("InputField");

            idInput.Position = new Vector3(705, 177, 0);
            idInput.FieldTexture = D3D9Manager.Instance.FindTexture("NameInput");
            idInput.MaxLength = 15;
            idInput.StringColor = new Color(127, 127, 127);
            idInput.StringOffset = new Vector3(30, 6, 0);


            /// <summary>
            /// passwordInput UI Resource Initialize
            /// </summary>
            passwordInput = new TextInputField("InputField");

            passwordInput.Position = new Vector3(705, 241, 0);
            passwordInput.FieldTexture = D3D9Manager.Instance.FindTexture("NameInput");
            passwordInput.MaxLength = 15;
            passwordInput.StringColor = new Color(127, 127, 127);
            passwordInput.StringOffset = new Vector3(30, 6, 0);


            /// <summary>
            /// userNameInput UI Resource Initialize
            /// </summary>
            userNameInput = new TextInputField("InputField");

            userNameInput.Position = new Vector3(705, 305, 0);
            userNameInput.FieldTexture = D3D9Manager.Instance.FindTexture("NameInput");
            userNameInput.MaxLength = 15;
            userNameInput.StringColor = new Color(127, 127, 127);
            userNameInput.StringOffset = new Vector3(30, 6, 0);


            /// <summary>
            /// signUpButton UI Resource Initialize
            /// </summary>
            signUpButton = new Button();

            signUpButton.ButtonTexture = D3D9Manager.Instance.FindTexture("SignupButton");
            signUpButton.Position = new Vector3(637, 381, 0);
            signUpButton.Scale = new Vector3(1.0f, 1.0f, 1.0f);
            signUpButton.IsMouseOverResize = true;
            signUpButton.OnButtonClick += SignUpButton_OnClick;


            /// <summary>
            /// signUpButton UI Resource Initialize
            /// </summary>
            toSignInButton = new Button();

            toSignInButton.ButtonTexture = D3D9Manager.Instance.FindTexture("BackButton");
            toSignInButton.Position = new Vector3(48, 48, 0);
            toSignInButton.Scale = new Vector3(1.0f, 1.0f, 1.0f);
            toSignInButton.IsMouseOverResize = true;
            toSignInButton.OnButtonClick += ToSignInButton_OnClick;

            /// <summary>
            /// signUpButton UI Resource Initialize
            /// </summary>
            background = new TextureObject();

            background.Position = new Vector3(ClientWindow.Width / 2, ClientWindow.Height / 2, 0);
            background.Scale = new Vector3(1, 1, 1);
            background.Texture = D3D9Manager.Instance.FindTexture("Background");


            /// <summary>
            /// Add To GameObject Manager
            /// </summary>
            GameObjectManager.Instance.AddObject(background);
            GameObjectManager.Instance.AddObject(idInput);
            GameObjectManager.Instance.AddObject(passwordInput);
            GameObjectManager.Instance.AddObject(userNameInput);
            GameObjectManager.Instance.AddObject(signUpButton);
            GameObjectManager.Instance.AddObject(toSignInButton);


            #endregion
        }

        private void Dispose()
        {
            idInput = null;
            passwordInput = null;
            userNameInput = null;
            signUpButton = null;
            toSignInButton = null;
            background = null;
        }

        private TextInputField idInput = null;
        private TextInputField passwordInput = null;
        private TextInputField userNameInput = null;
        private Button signUpButton = null;
        private Button toSignInButton = null;
        private TextureObject background = null;
    }
}

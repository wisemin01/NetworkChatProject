﻿using GameFramework;
using GameFramework.Manager;
using SharpDX;

using ChattingNetwork.Client;
using System;

namespace DirectXClient
{
    internal partial class SignInScene
    {
        private TextInputField idInput = null;
        private TextInputField passwordInput = null;
        private Button loginButton = null;
        private Button toSignUpButton = null;

        private void InitializeComponent()
        {
            #region UI RESOURCE INITIALIZE SOURCE CODE

            /// <summary>
            /// idInput UI Resource Initialize
            /// </summary>
            idInput = new TextInputField("InputField");

            idInput.Position = new Vector3(ClientWindow.Width / 2, 200, 0);
            idInput.FieldTexture = D3D9Manager.Instance.FindTexture("NameInput");
            idInput.MaxLength = 15;
            idInput.StringColor = new Color(127, 127, 127);
            idInput.StringOffset = new Vector3(30, 6, 0);

            /// <summary>
            /// passwordInput UI Resource Initialize
            /// </summary>
            passwordInput = new TextInputField("InputField");

            passwordInput.Position = new Vector3(ClientWindow.Width / 2, 275, 0);
            passwordInput.FieldTexture = D3D9Manager.Instance.FindTexture("NameInput");
            passwordInput.MaxLength = 15;
            passwordInput.StringColor = new Color(127, 127, 127);
            passwordInput.StringOffset = new Vector3(30, 6, 0);
            passwordInput.PasswordChar = '●';

            /// <summary>
            /// loginButton UI Resource Initialize
            /// </summary>
            loginButton = new Button();

            loginButton.ButtonTexture = D3D9Manager.Instance.FindTexture("LoginButton");
            loginButton.Position = new Vector3(ClientWindow.Width / 2, 370, 0);
            loginButton.Scale = new Vector3(1.0f, 1.0f, 1.0f);
            loginButton.IsMouseOverResize = true;
            loginButton.OnButtonClick += LoginButton_OnClick;

            /// <summary>
            /// toSignUpButton UI Resource Initialize
            /// </summary>
            toSignUpButton = new Button();

            toSignUpButton.ButtonTexture = D3D9Manager.Instance.FindTexture("SignupButton");
            toSignUpButton.Position = new Vector3(100, 100, 0);
            toSignUpButton.Scale = new Vector3(1.0f, 1.0f, 1.0f);
            toSignUpButton.IsMouseOverResize = true;

            toSignUpButton.OnButtonClick += ToSignUpButton_OnClick;


            GameObjectManager.Instance.AddObject(idInput);
            GameObjectManager.Instance.AddObject(passwordInput);
            GameObjectManager.Instance.AddObject(loginButton);
            GameObjectManager.Instance.AddObject(toSignUpButton);

            #endregion
        }

        private void Dispose()
        {
            idInput = null;
            passwordInput = null;
            loginButton = null;
            toSignUpButton = null;
        }

    }
}

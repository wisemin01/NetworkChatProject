﻿using GameFramework;
using GameFramework.Manager;
using SharpDX;

using ChattingNetwork.Client;
using System;

namespace DirectXClient
{
    internal partial class SignInScene : Scene
    {
        public override void Initialize()
        {
            D3D9Manager.Instance.CreateTexture("Background", "./Resource/Background.png");
            D3D9Manager.Instance.CreateTexture("LoginButton", "./Resource/LoginButton.png");
            D3D9Manager.Instance.CreateTexture("ToSignupButton", "./Resource/ToSignupButton.png");
            D3D9Manager.Instance.CreateTexture("NameInput", "./Resource/NameInput.png");

            D3D9Manager.Instance.CreateFont("InputField", "메이플스토리 Light", 35, false);

            ClientManager.Instance.OnSignIn += OnSignIn;

            InitializeComponent();
        }

        public override void FrameUpdate()
        {
        }

        public override void FrameRender()
        {
        }

        public override void Release()
        {
            Dispose();

            D3D9Manager.Instance.FontDispose();
            D3D9Manager.Instance.TextureDispose();

            ClientManager.Instance.OnSignIn -= OnSignIn;
        }

        public void LoginButton_OnClick(object sender, EventArgs e)
        {
            bool result = ClientManager.Instance.SignIn(idInput.Text, passwordInput.Text);

            if (result == true)
            {
                idInput.Clear();
                passwordInput.Clear();
            }
        }

        public void ToSignUpButton_OnClick(object sender, EventArgs e)
        {
            SceneManager.Instance.ChangeScene("SignUp");
        }

        public void OnSignIn(object sender, Tuple<string, bool> value)
        {
            if (value.Item2 == true)
            {
                // 로그인 성공
                SceneManager.Instance.ChangeScene("Lobby");
            }
            else
            {
                // 로그인 실패
                MessageBox.Show(value.Item1, "[!] Login Failed");
            }
        }
    }
}

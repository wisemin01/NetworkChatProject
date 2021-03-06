﻿using GameFramework;
using GameFramework.Manager;
using GameFramework.Structure;
using SharpDX;
using System;

namespace DirectXClient
{
    class MessageBox : GameObject
    {
        private GameTexture messageBoxTex;
        private GameTexture quitButtonTex;

        private Button quitButton;

        private readonly string Text = string.Empty;
        private readonly string Caption = string.Empty;

        public string FontKey { get; set; } = string.Empty;
        public Color StringColor { get; set; } = Color.White;

        public event EventHandler OnClosing;

        public MessageBox(string text)
        {
            Text = text;
        }

        public MessageBox(string text, string caption)
        {
            Text = text;
            Caption = caption;
        }

        public override void Initialize()
        {
            D3D9Manager.Instance.CreateFont("Default", "Segoe UI", 25, false);

            messageBoxTex = D3D9Manager.Instance.CreateTexture(
                "MessageBox", "./Resource/MessageBox.png");
            quitButtonTex = D3D9Manager.Instance.CreateTexture(
                "MessageBoxQuitButton", "./Resource/MessageBoxQuitButton.png");

            quitButton = new Button()
            {
                ButtonTexture = quitButtonTex,
                IsMouseOverResize = true,
                Position = Position + new Vector3(0, 70, 0),
                Scale = new Vector3(1, 1, 1),
                IsAllowDuplicateClick = true
            };
            quitButton.Initialize();
            quitButton.OnButtonClick += QuitButton_OnButtonClick;

            D3D9Manager.Instance.OnMouseClickToMessageBoxEvent += quitButton.OnClick;
            quitButton.OnButtonClick += OnQuitButtonClick;
        }

        private void QuitButton_OnButtonClick(object sender, EventArgs e)
        {
            OnClosing?.Invoke(this, e);
        }

        public override void FrameUpdate()
        {
            if (quitButton != null)
            {
                quitButton.FrameUpdate();
            }
        }

        public override void FrameRender()
        {
            D3D9Manager.Instance.DrawTexture(messageBoxTex, Position, new Vector3(1, 1, 1));
            D3D9Manager.Instance.DrawFont_NotSetTransform(FontKey, new Vector3(-170, -95, 0), Caption, StringColor);
            D3D9Manager.Instance.DrawFont_NotSetTransform(FontKey, new Vector3(-170, -50, 0), Text, StringColor);

            if (quitButton != null)
            {
                quitButton.FrameRender();
            }
        }

        public override void Release()
        {
            D3D9Manager.Instance.OnMouseClickToMessageBoxEvent -= quitButton.OnClick;
            if (quitButton != null)
            {
                quitButton.Release();
            }
        }

        public void OnQuitButtonClick(object sender, EventArgs e)
        {
            Destroy(this);

            if (quitButton != null)
            {
                Destroy(quitButton);
            }
        }

        public static MessageBox Show(string text, string caption)
        {
            return GameObjectManager.Instance.AddMessageBox(new MessageBox(text, caption)
            {
                FontKey = "Default",
                Position = new Vector3(ClientWindow.Width / 2, ClientWindow.Height / 2, 0),
                StringColor = new Color(125, 125, 125)
            }) as MessageBox;
        }
    }
}

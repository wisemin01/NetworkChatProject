﻿using GameFramework;
using GameFramework.Manager;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPNetwork.Client;

namespace DirectXClient
{
    class LobbyScene : Scene
    {
        public override void Initialize()
        {
            D3D9Manager.Instance.CreateTexture("RoomNameInput", "./Resource/RoomNameInput.png");
            D3D9Manager.Instance.CreateTexture("RoomCreateButton", "./Resource/CreateRoomButton.png");

            D3D9Manager.Instance.CreateFont("ChatInputFont", "메이플스토리 Light", 35, false);

            if (NetworkClientManager.Instance.IsConnection == false)
            {
                _ = NetworkClientManager.Instance.ConnectToServer();
            }

            TextInputField textInput = GameObjectManager.Instance.AddObject(new TextInputField("ChatInputFont")
            {
                Position = new Vector3(275, 50, 0),
                FieldTexture = D3D9Manager.Instance.FindTexture("RoomNameInput"),
                MaxLength = 10,
                StringColor = new Color(127, 127, 127),
                StringOffset = new Vector3(25, 3, 0)
            });

            Button loginButton = GameObjectManager.Instance.AddObject(new Button()
            {
                ButtonTexture = D3D9Manager.Instance.FindTexture("RoomCreateButton"),
                Position = new Vector3(650, 50, 0),
                Scale = new Vector3(1.0f, 1.0f, 1.0f),
                IsMouseOverResize = true
            });

            var roomList = GameObjectManager.Instance.AddObject(new NetworkRoomListViewer()
            {
                Position = new Vector3(-5, 80, 0)
            });

            loginButton.OnButtonClick += textInput.EnterText;
            textInput.OnEnter += delegate (object sender, string s)
            {
                if (string.IsNullOrWhiteSpace(s))
                {
                    MessageBox.Show("방 제목에는 공백 문자가\n포함될 수 없습니다.", "알림");
                    return;
                }

                NetworkClientManager.Instance.SendMessageToServer($"/CreateRoom/{s}");
                TextInputField input = sender as TextInputField;
                input.Clear();
            };
            textInput.OnEnter += delegate { roomList.RefreshList(0); };
        }

        public override void FrameUpdate()
        {
            if (NetworkClientManager.Instance.IsConnection == false)
            {
                SceneManager.Instance.ChangeScene("Login");
            }
        }

        public override void FrameRender()
        {
        }

        public override void Release()
        {
        }
    }
}
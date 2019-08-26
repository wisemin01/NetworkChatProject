using ChattingNetwork.Client;
using GameFramework;
using GameFramework.Manager;
using SharpDX;
using System;

namespace DirectXClient
{
    internal partial class LobbyScene : Scene
    {
        public override void Initialize()
        {
            D3D9Manager.Instance.CreateTexture("RoomNameInput", "./Resource/RoomNameInput.png");
            D3D9Manager.Instance.CreateTexture("RoomCreateButton", "./Resource/CreateRoomButton.png");

            D3D9Manager.Instance.CreateFont("ChatInputFont", "메이플스토리 Light", 35, false);

            InitializeComponent();

            ClientManager.Instance.OnJoinRoom += OnJoinRoom;
        }

        private void OnJoinRoom(object sender, Tuple<string, bool> e)
        {
            if (e.Item2 == true)
            {
                SceneManager.Instance.ChangeScene("Chat");
                ClientManager.Instance.CurrentChatRoom = e.Item1;
            }
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

            ClientManager.Instance.OnJoinRoom -= OnJoinRoom;
        }

        private void RoomNameInput_OnEnter(object sender, string roomName)
        {
            if (string.IsNullOrWhiteSpace(roomName))
            {
                MessageBox.Show("방 제목에는 공백 문자가\n포함될 수 없습니다.", "알림");
                return;
            }

            ClientManager.Instance.CreateRoom(roomName);
            ClientManager.Instance.JoinRoom(roomName);

            TextInputField input = sender as TextInputField;
            input.Clear();
        }
    }
}

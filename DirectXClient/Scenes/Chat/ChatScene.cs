using ChattingNetwork.Client;
using GameFramework;
using GameFramework.Manager;
using SharpDX;
using System;

namespace DirectXClient
{
    internal partial class ChatScene : Scene
    {
        public ChatScene() : base() { }

        public override void Initialize()
        {
            D3D9Manager.Instance.CreateTexture("ChatInput", "./Resource/ChatInput.png");
            D3D9Manager.Instance.CreateTexture("ToLobbyButton", "./Resource/ToLobby.png");
            D3D9Manager.Instance.CreateFont("ChatListFont", "메이플스토리 Light", 25, false);
            D3D9Manager.Instance.CreateFont("ChatInputFont", "메이플스토리 Light", 35, false);
            D3D9Manager.Instance.CreateFont("RoomTitleFont", "메이플스토리 Bold", 75, false);

            InitializeComponent();

            ClientManager.Instance.OnJoinRoom += OnJoinRoom;
            ClientManager.Instance.OnExitRoom += OnExitRoom;
            ClientManager.Instance.OnChatting += OnChatting;
        }

        private void OnChatting(object sender, string e)
        {
            chattingView.DrawText(e);
        }

        private void OnExitRoom(object sender, bool e)
        {
            if (e == true)
            {
                SceneManager.Instance.ChangeScene("Lobby");
            }
        }

        private void OnJoinRoom(object sender, Tuple<string, bool> e)
        {
            if (e.Item2 == true)
            {
                ClientManager.Instance.CurrentChatRoom = e.Item1;

                SceneManager.Instance.ChangeScene("Chat");
            }
        }

        public override void FrameRender()
        {

        }

        public override void FrameUpdate()
        {

        }

        public override void Release()
        {
            Dispose();

            D3D9Manager.Instance.FontDispose();
            D3D9Manager.Instance.TextureDispose();

            ClientManager.Instance.OnJoinRoom -= OnJoinRoom;
            ClientManager.Instance.OnExitRoom -= OnExitRoom;
            ClientManager.Instance.OnChatting -= OnChatting;
        }

        private void ToLobbyButton_OnClick(object sender, EventArgs e)
        {
            ClientManager.Instance.JoinRoom("Lobby");
            ClientManager.Instance.ExitRoom(ClientManager.Instance.CurrentChatRoom);
        }

        private void ChattingInput_OnEnter(object sender, string s)
        {
            ClientManager.Instance.SendChat(s);
            TextInputField input = sender as TextInputField;
            input.Clear();
        }
    }
}

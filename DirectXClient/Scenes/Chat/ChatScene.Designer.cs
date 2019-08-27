using GameFramework;
using GameFramework.Manager;
using SharpDX;

using ChattingNetwork.Client;
using System;

namespace DirectXClient
{
    internal partial class ChatScene
    {
        TextureObject background = null;
        TextureObject roomNameBG = null;
        TextList chattingView = null;
        TextInputField chattingInput = null;
        Button roomChangeToggle = null;
        TextBox roomNameView = null;
        NetworkRoomListViewer roomListViewer = null;

        private void InitializeComponent()
        {
            #region UI RESOURCE INITIALIZE SOURCE CODE

            /// <summary>
            /// background UI Resource Initialize
            /// </summary>
            background = new TextureObject();

            background.Position = new Vector3(ClientWindow.Width / 2, ClientWindow.Height / 2, 0);
            background.Scale = new Vector3(1, 1, 1);
            background.Texture = D3D9Manager.Instance.CreateTexture("Background", "./Resource/Background3.png");

            /// <summary>
            /// roomNameBG UI Resource Initialize
            /// </summary>
            roomNameBG = new TextureObject();

            roomNameBG.Position = new Vector3(320, 56, 0);
            roomNameBG.Scale = new Vector3(1, 1, 1);
            roomNameBG.Texture = D3D9Manager.Instance.CreateTexture("RoomNameBG", "./Resource/RoomName.png");

            /// <summary>
            /// roomChangeToggle UI Resource Initialize
            /// </summary>
            roomChangeToggle = new Button();

            roomChangeToggle.ButtonTexture = D3D9Manager.Instance.CreateTexture("RoomChange", "./Resource/RoomChange.png");
            roomChangeToggle.Position = new Vector3(1087, 58, 0);
            roomChangeToggle.Scale = new Vector3(1.0f, 1.0f, 1.0f);
            roomChangeToggle.IsMouseOverResize = true;
            roomChangeToggle.OnButtonClick += RoomChangeToggle_OnClick;


            /// <summary>
            /// chattingView UI Resource Initialize
            /// </summary>
            chattingView = new TextList("ChatListFont");

            chattingView.TextDepth = 25;
            chattingView.Position = new Vector3(30, D3D9Manager.Instance.WindowHeight - 160, 0);

            /// <summary>
            /// chattingInput UI Resource Initialize
            /// </summary>
            chattingInput = new TextInputField("ChatInputFont");

            chattingInput.Position = new Vector3(549, D3D9Manager.Instance.WindowHeight - 100, 0);
            chattingInput.FieldTexture = D3D9Manager.Instance.FindTexture("ChatInput");
            chattingInput.MaxLength = 60;
            chattingInput.StringColor = new Color(127, 127, 127);
            chattingInput.StringOffset = new Vector3(16, 3, 0);
            chattingInput.OnEnter += ChattingInput_OnEnter;

            /// <summary>
            /// roomNameView UI Resource Initialize
            /// </summary>
            roomNameView = new TextBox();

            roomNameView.Position = new Vector3(50, 20, 0);
            roomNameView.Text = ClientManager.Instance.CurrentChatRoom;
            roomNameView.StringColor = new Color(255, 255, 255, 255);
            roomNameView.FontKey = "RoomTitleFont";

            /// <summary>
            /// roomListViewer UI Resource Initialize
            /// </summary>

            roomListViewer = new NetworkRoomListViewer();

            roomListViewer.Position = new Vector3(1087, 300, 0);
            roomListViewer.IsActive = false;

            GameObjectManager.Instance.AddObject(background);
            GameObjectManager.Instance.AddObject(roomNameBG);
            GameObjectManager.Instance.AddObject(chattingView);
            GameObjectManager.Instance.AddObject(chattingInput);
            GameObjectManager.Instance.AddObject(roomChangeToggle);
            GameObjectManager.Instance.AddObject(roomNameView);
            GameObjectManager.Instance.AddObject(roomListViewer);

            #endregion
        }

        private void RoomChangeToggle_OnClick(object sender, EventArgs e)
        {
            roomListViewer.IsActive = !roomListViewer.IsActive;
        }

        private void Dispose()
        {
            background = null;
            roomNameBG = null;
            chattingView = null;
            chattingInput = null;
            roomChangeToggle = null;
            roomNameView = null;
            roomListViewer = null;
        }
    }
}

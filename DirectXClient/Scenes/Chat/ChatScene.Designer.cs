using GameFramework;
using GameFramework.Manager;
using SharpDX;

using ChattingNetwork.Client;
using System;

namespace DirectXClient
{
    internal partial class ChatScene
    {
        TextList chattingView = null;
        TextInputField chattingInput = null;
        Button toLobbyButton = null;
        TextBox roomNameView = null;

        private void InitializeComponent()
        {
            #region UI RESOURCE INITIALIZE SOURCE CODE

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

            chattingInput.Position = new Vector3(404, D3D9Manager.Instance.WindowHeight - 120, 0);
            chattingInput.FieldTexture = D3D9Manager.Instance.FindTexture("ChatInput");
            chattingInput.MaxLength = 35;
            chattingInput.StringColor = new Color(127, 127, 127);
            chattingInput.StringOffset = new Vector3(16, 3, 0);
            chattingInput.OnEnter += ChattingInput_OnEnter;

            /// <summary>
            /// toLobbyButton UI Resource Initialize
            /// </summary>
            toLobbyButton = new Button();

            toLobbyButton.ButtonTexture = D3D9Manager.Instance.FindTexture("ToLobbyButton");
            toLobbyButton.Position = new Vector3(1000, D3D9Manager.Instance.WindowHeight - 120, 0);
            toLobbyButton.Scale = new Vector3(1.0f, 1.0f, 1.0f);
            toLobbyButton.IsMouseOverResize = true;
            toLobbyButton.OnButtonClick += ToLobbyButton_OnClick;

            /// <summary>
            /// roomNameView UI Resource Initialize
            /// </summary>
            roomNameView = new TextBox();

            roomNameView.Position = new Vector3(20, 20, 0);
            roomNameView.Text = ClientManager.Instance.CurrentChatRoom;
            roomNameView.StringColor = new Color(255, 255, 255, 255);
            roomNameView.FontKey = "RoomTitleFont";


            GameObjectManager.Instance.AddObject(chattingView);
            GameObjectManager.Instance.AddObject(chattingInput);
            GameObjectManager.Instance.AddObject(toLobbyButton);
            GameObjectManager.Instance.AddObject(roomNameView);

            #endregion
        }

        private void Dispose()
        {
            chattingView = null;
            chattingInput = null;
            toLobbyButton = null;
            roomNameView = null;
        }
    }
}

using GameFramework;
using GameFramework.Manager;
using SharpDX;

using ChattingNetwork.Client;
using System;

namespace DirectXClient
{
    internal partial class LobbyScene
    {
        TextInputField roomNameInput = null;
        Button createButton = null;
        NetworkRoomListViewer roomListViewer = null;

        private void InitializeComponent()
        {
            #region UI RESOURCE INITIALIZE SOURCE CODE

            /// <summary>
            /// roomNameInput UI Resource Initialize
            /// </summary>
            roomNameInput = new TextInputField("ChatInputFont");

            roomNameInput.Position = new Vector3(275, 50, 0);
            roomNameInput.FieldTexture = D3D9Manager.Instance.FindTexture("RoomNameInput");
            roomNameInput.MaxLength = 10;
            roomNameInput.StringColor = new Color(127, 127, 127);
            roomNameInput.StringOffset = new Vector3(25, 8, 0);
            roomNameInput.OnEnter += RoomNameInput_OnEnter;

            /// <summary>
            /// createButton UI Resource Initialize
            /// </summary>
            createButton = new Button();

            createButton.ButtonTexture = D3D9Manager.Instance.FindTexture("RoomCreateButton");
            createButton.Position = new Vector3(650, 50, 0);
            createButton.Scale = new Vector3(1.0f, 1.0f, 1.0f);
            createButton.IsMouseOverResize = true;
            createButton.OnButtonClick += roomNameInput.EnterText;

            /// <summary>
            /// roomListViewer UI Resource Initialize
            /// </summary>
            roomListViewer = new NetworkRoomListViewer();

            roomListViewer.Position = new Vector3(-5, 80, 0);


            GameObjectManager.Instance.AddObject(roomNameInput);
            GameObjectManager.Instance.AddObject(createButton);
            GameObjectManager.Instance.AddObject(roomListViewer);

            #endregion
        }

        private void Dispose()
        {
            roomNameInput = null;
            createButton = null;
            roomListViewer = null;
        }
    }
}

using GameFramework;
using GameFramework.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using ChattingNetwork.Client;
using GameFramework.Structure;

namespace DirectXClient
{
    internal class CreateRoomWindow : GameObject
    {
        Button createButton = null;
        Button cancelButton = null;

        TextInputField roomNameInput = null;

        GameTexture background = null;

        public override void FrameRender()
        {
            D3D9Manager.Instance.DrawTexture(background, GetWorldMatrix());
        }

        public override void FrameUpdate()
        {
        }

        public override void Initialize()
        {
            D3D9Manager.Instance.CreateFont("CreateWindowInputFont", "Segoe UI", 50, false);

            background = D3D9Manager.Instance.CreateTexture("CreateRoomWindowBG", "./Resource/CreateRoomWindow.png");

            createButton = new Button();

            createButton.ButtonTexture = D3D9Manager.Instance.CreateTexture("CreateButton", "./Resource/Create.png");
            createButton.IsMouseOverResize = true;
            createButton.Parent = this;
            createButton.Position = new Vector3(-187, 130, 0);
            createButton.UseParentMatrix = true;
            createButton.OnButtonClick += CreateButton_OnButtonClick;

            cancelButton = new Button();

            cancelButton.ButtonTexture = D3D9Manager.Instance.CreateTexture("CancelButton", "./Resource/Cancel.png");
            cancelButton.IsMouseOverResize = true;
            cancelButton.Parent = this;
            cancelButton.Position = new Vector3(198, 134, 0);
            cancelButton.UseParentMatrix = true;
            cancelButton.OnButtonClick += CancelButton_OnButtonClick;

            roomNameInput = new TextInputField("CreateWindowInputFont");

            roomNameInput.Position = new Vector3(0, 0, 0);
            roomNameInput.FieldTexture = D3D9Manager.Instance.CreateTexture("RoomNameInput", "./Resource/RoomNameInput.png");
            roomNameInput.StringOffset = new Vector3(20, 0, 0);
            roomNameInput.Text = "New Room";
            roomNameInput.MaxLength = 15;
            roomNameInput.Parent = this;

            GameObjectManager.Instance.AddObject(createButton);
            GameObjectManager.Instance.AddObject(cancelButton);
            GameObjectManager.Instance.AddObject(roomNameInput);
        }

        private void CancelButton_OnButtonClick(object sender, EventArgs e)
        {
            Destroy(this);
        }

        private void CreateButton_OnButtonClick(object sender, EventArgs e)
        {
            ClientManager.Instance.CreateAndJoinRoom(roomNameInput.Text);
            Destroy(this);
        }

        public override void OnDisable()
        {
        }

        public override void OnEnable()
        {
        }

        public override void Release()
        {
            Destroy(createButton);
            Destroy(cancelButton);
            Destroy(roomNameInput);

            createButton = null;
            cancelButton = null;
            roomNameInput = null;
        }

        public override Matrix GetWorldMatrix()
        {
            return Matrix.Scaling(Scale) * Matrix.Translation(Position);
        }
    }
}

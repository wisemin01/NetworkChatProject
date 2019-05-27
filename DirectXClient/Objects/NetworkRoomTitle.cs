using GameFramework;
using GameFramework.Manager;
using GameFramework.Structure;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectXClient
{
    class NetworkRoomTitle : GameObject
    {
        public string RoomTitle { get; set; } = string.Empty;
        private GameTexture TitleTexture { get; set; } = null;

        public Vector3 Position { get; set; } = default;

        public NetworkRoomTitle(string title)
        {
            RoomTitle = title;
        }

        public override void Initialize()
        {
            TitleTexture = D3D9Manager.Instance.CreateTexture(
                "RoomTitle", "./Resource/RoomTitle.png");
            D3D9Manager.Instance.CreateFont("RoomTitle", "메이플스토리 Bold", 35, false);
        }

        public override void FrameUpdate()
        {

        }

        public override void FrameRender()
        {
            D3D9Manager.Instance.DrawTexture(TitleTexture, Position, new Vector3(1, 1, 1));
            D3D9Manager.Instance.DrawFont_NotSetTransform(
                "RoomTitle", new Vector3(-50, -15, 0), RoomTitle, new Color(127, 127, 127, 255));
        }

        public override void Release()
        {
        }
    }
}

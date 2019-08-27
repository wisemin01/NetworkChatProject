using GameFramework;
using GameFramework.Manager;
using GameFramework.Structure;
using SharpDX;

namespace DirectXClient
{
    class NetworkRoomTitle : GameObject
    {
        public string RoomTitle { get; set; } = string.Empty;

        public NetworkRoomTitle(string title)
        {
            RoomTitle = title;
        }

        public override void Initialize()
        {
            D3D9Manager.Instance.CreateFont("RoomTitle", "Segoe UI", 35, false);
        }

        public override void FrameUpdate()
        {

        }

        public override void FrameRender()
        {
            D3D9Manager.Instance.SetTransform(D3D9Manager.TransformState.Sprite, Parent.GetWorldMatrix());
            D3D9Manager.Instance.DrawFont_NotSetTransform(
                "RoomTitle", Position, RoomTitle, new Color(127, 127, 127, 255));
        }

        public override void Release()
        {
        }
    }
}

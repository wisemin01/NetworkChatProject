using GameFramework;
using GameFramework.Manager;
using GameFramework.Structure;
using SharpDX;

namespace DirectXClient
{
    class TextureObject : GameObject
    {
        public GameTexture Texture { get; set; } = null;
        public float Rotation { get; set; } = 0;
        public override void FrameRender()
        {
            D3D9Manager.Instance.DrawTexture(Texture, Position, Scale, Rotation);
        }

        public override void FrameUpdate()
        {

        }

        public override void Initialize()
        {
        }

        public override void Release()
        {
        }
    }
}

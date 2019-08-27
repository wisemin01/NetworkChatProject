using SharpDX;
using System;

namespace GameFramework.Structure
{
    using D3D9 = SharpDX.Direct3D9;

    public class GameTexture : IDisposable
    {
        public GameTexture(D3D9.Texture texture, D3D9.ImageInformation info)
        {
            Texture = texture;
            Info = info;
        }

        public D3D9.Texture Texture { get; set; }
        public D3D9.ImageInformation Info { get; set; }

        public void Dispose()
        {
            Texture.Dispose();
        }

        public int Width { get => Info.Width; }
        public int Height { get => Info.Height; }

        public int HalfWidth { get => Info.Width / 2; }
        public int HalfHeight { get => Info.Height / 2; }

        public Vector3 Center { get => new Vector3(HalfWidth, HalfHeight, 0); }
    }
}

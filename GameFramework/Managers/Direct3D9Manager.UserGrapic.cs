using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;

namespace GameFramework.Manager
{
    using BGRAColor = SharpDX.Mathematics.Interop.RawColorBGRA;

    public partial class Direct3D9Manager
    {
        private Dictionary<string, Font>    fonts    = new Dictionary<string, Font>();
        private Dictionary<string, Texture> textures = new Dictionary<string, Texture>();

        public void CreateFont(string key, string faceName, int size, bool isItalic)
        {
            Font d3dFont = new Font(d3d9Device,
            size,
            0,
            FontWeight.Normal,
            0,
            isItalic,
            FontCharacterSet.Hangul,
            FontPrecision.Default,
            FontQuality.Default,
            FontPitchAndFamily.Default,
            faceName);

            fonts.Add(key, d3dFont);
        }

        public void CreateTexture(string key, string path)
        {
            Texture texture = Texture.FromFile(d3d9Device, path);

            textures.Add(key, texture);
        }

        public void DrawFont(string fontKey, Vector3 position, string text, Color color)
        {
            Font d3dFont = fonts[fontKey];

            if (d3dFont != null)
            {
                try
                {
                    d3dFont.DrawText(d3d9Sprite, text, (int)position.X, (int)position.Y, color);
                }
                catch (SharpDXException)
                {
                    
                }
            }
        }

        public void DrawTexture(Texture texture, Vector3 position, Vector3 scale, float rot)
        {
            if (texture != null)
            {
                Matrix mat = Matrix.Scaling(scale) * Matrix.RotationZ(rot) * Matrix.Translation(position);

                d3d9Device.SetTransform(0, mat);

                d3d9Sprite.Draw(texture, new Color(255, 255, 255, 255));
            }
        }

        private void FontDispose()
        {
            foreach (var Iter in fonts)
            {
                Iter.Value.Dispose();
            }
            fonts.Clear();
        }

        private void TextureDispose()
        {
            foreach (var Iter in textures)
            {
                Iter.Value.Dispose();
            }
            textures.Clear();
        }
    }
}

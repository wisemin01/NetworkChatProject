using SharpDX;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GameFramework.Structure;


namespace GameFramework.Manager
{
    using D3D9 = SharpDX.Direct3D9;
    public partial class Direct3D9Manager
    {
        private Dictionary<string, D3D9.Font> fonts    = new Dictionary<string, D3D9.Font>();
        private Dictionary<string, GameTexture>   textures = new Dictionary<string, GameTexture>();

        public void CreateFont(string key, string faceName, int size, bool isItalic)
        {
            D3D9.Font d3dFont = new D3D9.Font(d3d9Device,
            size,
            0,
            D3D9.FontWeight.Normal,
            0,
            isItalic,
            D3D9.FontCharacterSet.Hangul,
            D3D9.FontPrecision.Default,
            D3D9.FontQuality.Default,
            D3D9.FontPitchAndFamily.Default,
            faceName);

            fonts.Add(key, d3dFont);
        }

        public void CreateTexture(string key, string path)
        {
            try
            {
                D3D9.ImageInformation info = new D3D9.ImageInformation();

                D3D9.Texture d3d9Texture = D3D9.Texture.FromFile(d3d9Device, path, D3D9.D3DX.DefaultNonPowerOf2, D3D9.D3DX.DefaultNonPowerOf2, 0,
                    D3D9.Usage.None, D3D9.Format.Unknown, D3D9.Pool.Managed, D3D9.Filter.None, D3D9.Filter.None, 0, out info);

                GameTexture texture = new GameTexture(d3d9Texture, info);

                textures.Add(key, texture);
            }
            catch (SharpDXException)
            {
                MessageBox.Show($"이미지 로딩에 실패했습니다. \n {path} 경로를 다시 확인해주세요.", "Texture Load Failed");
            }
        }

        public void DrawFont(string fontKey, Vector3 position, string text, Color color)
        {
            D3D9.Font d3dFont = fonts[fontKey];

            if (d3dFont != null)
            {
                try
                {
                    d3d9Sprite.Transform = Matrix.Identity;
                    d3dFont.DrawText(d3d9Sprite, text, (int)position.X, (int)position.Y, color);
                }
                catch (SharpDXException)
                {
                    
                }
            }
        }

        public void DrawTexture(GameTexture texture, Vector3 position, Vector3 scale, float rot = 0.0f)
        {
            if (texture.Texture != null)
            {
                Matrix mat = Matrix.Scaling(scale) * Matrix.RotationZ(rot) * Matrix.Translation(position);

                Vector3 center = new Vector3(texture.HalfWidth, texture.HalfHeight, 0);

                d3d9Sprite.Transform = mat;
                d3d9Sprite.Draw(
                    textureRef:  texture.Texture,
                    centerRef:   center,
                    color:       new Color(255, 255, 255, 255)
                    );
            }
        }

        public void FontDispose()
        {
            foreach (var Iter in fonts)
            {
                Iter.Value.Dispose();
            }
            fonts.Clear();
        }

        public void TextureDispose()
        {
            foreach (var Iter in textures)
            {
                Iter.Value.Dispose();
            }
            textures.Clear();
        }

        public GameTexture FindTexture(string key)
        {
            try
            {
                return textures[key];
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}

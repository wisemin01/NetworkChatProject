using GameFramework.Structure;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace GameFramework.Manager
{
    using D3D9 = SharpDX.Direct3D9;
    public partial class D3D9Manager
    {
        [Flags]
        public enum TransformState
        {
            World,
            Sprite
        }

        private readonly Dictionary<string, D3D9.Font> fonts = new Dictionary<string, D3D9.Font>();
        private readonly Dictionary<string, GameTexture> textures = new Dictionary<string, GameTexture>();

        public void SetTransform(TransformState types, Matrix mat)
        {
            switch (types)
            {
                case TransformState.World:
                    D3D9Device.SetTransform(D3D9.TransformState.World, mat);
                    break;
                case TransformState.Sprite:
                    d3d9Sprite.Transform = mat;
                    break;
            }
        }

        public void CreateFont(string key, string faceName, int size, bool isItalic)
        {
            lock (locker)
            {
                if (fonts.ContainsKey(key) == false)
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
            }
        }

        public GameTexture CreateTexture(string key, string path)
        {
            lock (locker)
            {
                if (textures.ContainsKey(key))
                    return textures[key];

                try
                {
                    D3D9.ImageInformation info = new D3D9.ImageInformation();

                    D3D9.Texture d3d9Texture = D3D9.Texture.FromFile(d3d9Device, path, D3D9.D3DX.DefaultNonPowerOf2, D3D9.D3DX.DefaultNonPowerOf2, 0,
                        D3D9.Usage.None, D3D9.Format.Unknown, D3D9.Pool.Managed, D3D9.Filter.None, D3D9.Filter.None, 0, out info);

                    GameTexture texture = new GameTexture(d3d9Texture, info);

                    textures.Add(key, texture);
                    return texture;
                }
                catch (SharpDXException)
                {
                    MessageBox.Show($"이미지 로딩에 실패했습니다. \n {path} 경로를 다시 확인해주세요.", "Texture Load Failed");
                    return null;
                }
            }
        }

        public void DrawFont(string fontKey, Vector3 position, string text, Color color)
        {
            if (string.IsNullOrEmpty(text))
                return;

            D3D9.Font d3dFont = fonts[fontKey];

            if (d3dFont != null)
            {
                d3d9Sprite.Transform = Matrix.Identity;
                d3dFont.DrawText(d3d9Sprite, text, (int)position.X, (int)position.Y, color);
            }
        }

        public void DrawFont_NotSetTransform(string fontKey, Vector3 position, string text, Color color)
        {
            if (string.IsNullOrEmpty(text))
                return;

            D3D9.Font d3dFont = fonts[fontKey];

            if (d3dFont != null)
            {
                d3dFont.DrawText(d3d9Sprite, text, (int)position.X, (int)position.Y, color);
            }
        }

        public void DrawTexture(GameTexture texture,
            Vector3 position, Vector3 scale, float rot = 0.0f)
        {
            if (texture != null && texture.Texture != null)
            {
                Matrix mat = Matrix.Scaling(scale) * Matrix.RotationZ(rot) * Matrix.Translation(position);

                Vector3 center = new Vector3(texture.HalfWidth, texture.HalfHeight, 0);

                d3d9Sprite.Transform = mat;
                d3d9Sprite.Draw(
                    textureRef: texture.Texture,
                    centerRef: center,
                    color: new Color(255, 255, 255, 255)
                    );
            }
        }

        public void DrawTexture(GameTexture texture,
            Matrix mat)
        {
            if (texture != null && texture.Texture != null)
            {
                Vector3 center = new Vector3(texture.HalfWidth, texture.HalfHeight, 0);

                d3d9Sprite.Transform = mat;
                d3d9Sprite.Draw(
                    textureRef: texture.Texture,
                    centerRef: center,
                    color: new Color(255, 255, 255, 255)
                    );
            }
        }

        public void DrawTexture(GameTexture texture,
            Matrix mat, Color color)
        {
            if (texture != null && texture.Texture != null)
            {
                Vector3 center = new Vector3(texture.HalfWidth, texture.HalfHeight, 0);

                d3d9Sprite.Transform = mat;
                d3d9Sprite.Draw(
                    textureRef: texture.Texture,
                    centerRef: center,
                    color: color
                    );
            }
        }

        public void DrawTextureWithCenter(GameTexture texture,
            Vector3 position, Vector3 scale, Vector3 center, float rot = 0.0f)
        {
            if (texture != null && texture.Texture != null)
            {
                Matrix mat = Matrix.Scaling(scale) * Matrix.RotationZ(rot) * Matrix.Translation(position);

                d3d9Sprite.Transform = mat;
                d3d9Sprite.Draw(
                    textureRef: texture.Texture,
                    centerRef: center,
                    color: new Color(255, 255, 255, 255)
                    );
            }
        }

        public void FontDispose()
        {
            lock (locker)
            {
                foreach (KeyValuePair<string, D3D9.Font> Iter in fonts)
                {
                    Iter.Value.Dispose();
                }
                fonts.Clear();
            }
        }

        public void TextureDispose()
        {
            lock (locker)
            {
                foreach (KeyValuePair<string, GameTexture> Iter in textures)
                {
                    Iter.Value.Dispose();
                }
                textures.Clear();
            }
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

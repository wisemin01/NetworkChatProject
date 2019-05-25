using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;

namespace GameFramework.Manager
{
    using BGRAColor = SharpDX.Mathematics.Interop.RawColorBGRA;

    public partial class Direct3D9Manager
    {
        private Dictionary<string, Font> fonts = new Dictionary<string, Font>();

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

        private void FontDispose()
        {
            foreach (var Iter in fonts)
            {
                Iter.Value.Dispose();
            }
            fonts.Clear();
        }
    }
}

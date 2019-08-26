using GameFramework;
using GameFramework.Manager;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectXClient
{
    public class TextBox : GameObject
    {
        public Vector3 Position { get; set; } = new Vector3(0, 0, 0);
        public string FontKey { get; set; } = string.Empty;
        public Color StringColor { get; set; } = Color.White;

        public string Text { get; set; } = string.Empty;

        public override void FrameRender()
        {
            D3D9Manager.Instance.DrawFont(FontKey, Position, Text, StringColor);
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

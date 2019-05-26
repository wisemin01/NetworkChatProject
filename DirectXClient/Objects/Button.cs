using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameFramework;
using GameFramework.Manager;
using SharpDX;
using SharpDX.Direct3D9;
using SharpDX.DirectInput;
using GameFramework.Structure;

namespace DirectXClient
{
    class Button : GameObject
    {
        public event EventHandler OnButtonClick;

        public GameTexture  ButtonTexture    { get; set; } = null;
        public Vector3      Position         { get; set; } = default;
        public Vector3      Scale            { get; set; } = default;
        public RectCollider rectCollider     { get; set; } = null;
        
        private readonly Vector3 MouseOverSize      = new Vector3(1.1f, 1.1f, 1.1f);
        private readonly Vector3 MouseNoneOverSize  = new Vector3(1.0f, 1.0f, 1.0f);


        public Button()
        {

        }

        public override void Initialize()
        {
            Direct3D9Manager.Instance.OnMouseClickEvent += OnClick;

            if (rectCollider == null)
            {
                rectCollider = new RectCollider()
                {
                    Range = new Rectangle(0, 0, ButtonTexture.Width, ButtonTexture.Height)
                };
            }
        }

        public override void FrameUpdate()
        {
            bool isMouseInside = rectCollider.IsMouseOver(Position, ButtonTexture);

            if (isMouseInside)
                Scale = Vector3.Lerp(Scale, MouseOverSize, 0.15f);
            else
                Scale = Vector3.Lerp(Scale, MouseNoneOverSize, 0.15f);
        }

        public override void FrameRender()
        {
            Direct3D9Manager.Instance.DrawTexture(ButtonTexture, Position, Scale, 0);
        }

        public override void Release()
        {

        }
        
        public void OnClick(object sender, EventArgs e)
        {
            if (rectCollider.IsMouseOver(Position, ButtonTexture))
            {
                OnButtonClick?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}

using GameFramework;
using GameFramework.Manager;
using GameFramework.Structure;
using SharpDX;
using System;

namespace DirectXClient
{
    class Button : GameObject
    {
        public event EventHandler OnButtonClick;

        public GameTexture ButtonTexture { get; set; } = null;
        public Vector3 Position { get; set; } = default;
        public Vector3 Scale { get; set; } = default;
        public RectCollider rectCollider { get; set; } = null;
        public bool IsMouseOverResize { get; set; } = true;
        public bool IsAllowDuplicateClick { get; set; } = false;

        private readonly Vector3 MouseOverSize = new Vector3(1.1f, 1.1f, 1.1f);
        private readonly Vector3 MouseNoneOverSize = new Vector3(1.0f, 1.0f, 1.0f);

        public Button()
        {

        }

        public override void Initialize()
        {
            D3D9Manager.Instance.OnMouseClickEvent += OnClick;

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
            try
            {
                bool isMouseInside = rectCollider.IsMouseOver(Position, ButtonTexture);

                if (IsMouseOverResize)
                {
                    if (isMouseInside)
                        Scale = Vector3.Lerp(Scale, MouseOverSize, 0.15f);
                    else
                        Scale = Vector3.Lerp(Scale, MouseNoneOverSize, 0.15f);
                }
            }
            catch (NullReferenceException)
            {

            }
        }

        public override void FrameRender()
        {
            D3D9Manager.Instance.DrawTexture(ButtonTexture, Position, Scale, 0);
        }

        public override void Release()
        {
            D3D9Manager.Instance.OnMouseClickEvent -= OnClick;
            OnButtonClick = null;
        }

        public void OnClick(object sender, ClickChecker checker)
        {
            if (checker.IsEndCheck == true)
            {
                if (IsAllowDuplicateClick == false)
                    return;
            }

            if (rectCollider.IsMouseOver(Position, ButtonTexture))
            {
                checker.IsEndCheck = true;
                OnButtonClick?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}

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
        public RectCollider rectCollider { get; set; } = null;
        public bool IsMouseOverResize { get; set; } = true;
        public bool IsAllowDuplicateClick { get; set; } = false;

        private readonly Vector3 MouseOverSize = new Vector3(1.05f, 1.05f, 1.05f);
        private readonly Vector3 MouseNoneOverSize = new Vector3(1.0f, 1.0f, 1.0f);

        public bool UseParentMatrix { get; set; } = false;

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
                bool isMouseInside = rectCollider.IsMouseOver(Vector3.TransformCoordinate(-ButtonTexture.Center, GetWorldMatrix()));

                if (IsMouseOverResize)
                {
                    if (isMouseInside)
                        Scale = Vector3.Lerp(Scale, MouseOverSize, 0.15f);
                    else
                        Scale = Vector3.Lerp(Scale, MouseNoneOverSize, 0.15f);
                }
            }
            finally
            {

            }
        }

        public override void FrameRender()
        {
            D3D9Manager.Instance.DrawTexture(ButtonTexture, GetWorldMatrix());
        }

        public override void Release()
        {
            EventDisconnect();
        }

        public void EventDisconnect()
        {
            OnButtonClick = null;
            D3D9Manager.Instance.OnMouseClickEvent -= OnClick;
        }

        public void OnClick(object sender, ClickChecker checker)
        {
            if (IsActive == false)
                return;

            if (checker.IsEndCheck == true)
            {
                if (IsAllowDuplicateClick == false)
                    return;
            }

            bool isMouseInside = 
                rectCollider.IsMouseOver(Vector3.TransformCoordinate(-ButtonTexture.Center, GetWorldMatrix()));

            if (isMouseInside)
            {
                checker.IsEndCheck = true;
                OnButtonClick?.Invoke(this, EventArgs.Empty);
            }
        }

        public override Matrix GetWorldMatrix()
        {
            if (UseParentMatrix == true)
                return base.GetWorldMatrix();
            else
                return Matrix.Scaling(Scale) * Matrix.Translation(Position);
        }
    }
}

using GameFramework.Manager;
using SharpDX;

namespace GameFramework.Structure
{
    public class RectCollider
    {
        public Rectangle Range { get; set; } = default;

        public RectCollider() { }
        public RectCollider(Rectangle rect)
        {
            Range = rect;
        }
        public bool IsMouseOver(Vector3 position, GameTexture texture = null)
        {
            Rectangle localRect = Range;
            localRect.Offset((int)position.X, (int)position.Y);

            if (texture != null)
                localRect.Offset(-texture.HalfWidth, -texture.HalfHeight);

            Vector2 mousePos = D3D9Manager.Instance.MousePosition;

            if (localRect.Contains(mousePos.X, mousePos.Y))
            {
                return true;
            }
            else
                return false;
        }
    }
}

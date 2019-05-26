using GameFramework.Structure;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFramework.Manager;

namespace GameFramework.Structure
{
    public class RectCollider
    {
        public Rectangle Range { get; set; } = default;

        public bool IsMouseOver(Vector3 position, GameTexture texture = null)
        {
            Rectangle localRect = Range;
            localRect.Offset((int)position.X, (int)position.Y);

            if (texture != null)
                localRect.Offset(-texture.HalfWidth, -texture.HalfHeight);

            Vector2 mousePos = Direct3D9Manager.Instance.MousePosition;

            if (localRect.Contains(mousePos.X, mousePos.Y))
            {
                return true;
            }
            else
                return false;
        }
    }
}

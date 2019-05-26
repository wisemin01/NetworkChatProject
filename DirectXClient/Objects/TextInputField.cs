using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using GameFramework;
using GameFramework.Manager;
using GameFramework.Structure;
using SharpDX;

namespace DirectXClient
{
    public class TextInputField : GameObject
    {
        readonly StringBuilder inputString = new StringBuilder();

        public Vector3      Position        { get; set; }       = new Vector3(0, 0, 0);
        public Vector3      StringOffset    { get; set; }       = new Vector3(0, 0, 0);
        public string       FontKey         { get; set; }       = string.Empty;
        public bool         IsSelected      { get; private set; } = true;
        public int          MaxLength       { get; set; }       = 15;
        public Color        StringColor     { get; set; }       = Color.White;
        public RectCollider Range           { get; set; }       = null;
        public GameTexture  FieldTexture    { get; set; }       = null;

        public event EventHandler<string> OnEnter;

        public TextInputField(string fontKey)
        {
            FontKey = fontKey;

            D3D9Manager.Instance.OnMouseClickEvent += OnMouseClick;
        }

        private void OnMouseClick(object sender, EventArgs e)
        {
            if (Range.IsMouseOver(Position, FieldTexture))
            {
                IsSelected = true;
            }
            else
                IsSelected = false;
        }

        public void EnterText(object sender, EventArgs e)
        {
            OnEnter?.Invoke(this, inputString.ToString());
            inputString.Clear();
        }

        public override void Initialize()
        {
            D3D9Manager.Instance.AddMessageHandler(MsgProc);

            if (Range == null)
            {
                if (FieldTexture != null)
                {
                    Range = new RectCollider(new Rectangle(0, 0, FieldTexture.Width, FieldTexture.Height));
                }
                else
                {
                    Range = new RectCollider(new Rectangle(0, 0, 50, 50 * MaxLength));
                }
            }
        }

        public override void FrameUpdate()
        {
        }

        public override void FrameRender()
        {
            int alpha = IsSelected ? 255 : 180;

            Vector3 fontDrawPosition = Position;
            Color   textColor        = StringColor;

            textColor.A = (byte)alpha;

            if (FieldTexture != null)
            {
                D3D9Manager.Instance.DrawTextureWithColor(FieldTexture, Position,
                    new Vector3(1, 1, 1), new Color(255, 255, 255, alpha));

                fontDrawPosition -= new Vector3(FieldTexture.HalfWidth, FieldTexture.HalfHeight, 0) - StringOffset;
            }

            D3D9Manager.Instance.DrawFont(FontKey, fontDrawPosition,
                inputString.ToString(), textColor);
        }

        public override void Release()
        {
            D3D9Manager.Instance.RemoveMessageHandler(MsgProc);
        }

        private void MsgProc(object sender, Message m)
        {
            if (IsSelected == false)
                return;

            switch (m.Msg)
            {
                case MsgType.Wm_Char:
                    AddChar((char)m.WParam);
                    break;
            }
        }

        private void AddChar(char ch)
        {
            if(ch == (char)13)
            {
                EnterText(this, EventArgs.Empty);
                return;
            }

            if (ch == (char)8)
            {
                if (inputString.Length > 0)
                    inputString.Remove(inputString.Length - 1, 1);
            }
            else if (inputString.Length <= MaxLength)
            {
                inputString.Append(ch);
            }
        }
    }
}

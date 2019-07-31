using GameFramework;
using GameFramework.Manager;
using GameFramework.Structure;
using SharpDX;
using System;
using System.Text;
using System.Windows.Forms;

namespace DirectXClient
{
    public class TextInputField : GameObject
    {
        readonly StringBuilder inputString = new StringBuilder();

        public Vector3 Position { get; set; } = new Vector3(0, 0, 0);
        public Vector3 StringOffset { get; set; } = new Vector3(0, 0, 0);
        public string FontKey { get; set; } = string.Empty;
        public bool IsSelected { get; private set; } = true;
        public int MaxLength { get; set; } = 15;
        public Color StringColor { get; set; } = Color.White;
        public RectCollider Range { get; set; } = null;
        public GameTexture FieldTexture { get; set; } = null;
        public char? PasswordChar { get; set; } = null;

        private string outputString = string.Empty;

        public event EventHandler<string> OnEnter;

        public string Text {
            get
            {
                return inputString.ToString();
            }
            set
            {
                inputString.Clear();
                inputString.Append(value);
            }
        }

        public TextInputField(string fontKey)
        {
            FontKey = fontKey;
            IsSelected = false;
        }

        private void OnMouseClick(object sender, ClickChecker checker)
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
        }

        public void Clear()
        {
            inputString.Clear();
        }

        public override void Initialize()
        {
            D3D9Manager.Instance.AddMessageHandler(MsgProc);
            D3D9Manager.Instance.OnMouseClickEvent += OnMouseClick;

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
            if (PasswordChar != null)
            {
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < inputString.Length; i++)
                {
                    builder.Append(PasswordChar);
                }
                outputString = builder.ToString();
            }
            else
            {
                outputString = inputString.ToString();
            }

        }

        public override void FrameRender()
        {
            int alpha = IsSelected ? 255 : 180;

            Vector3 fontDrawPosition = Position;
            Color textColor = StringColor;

            textColor.A = (byte)alpha;

            if (FieldTexture != null)
            {
                D3D9Manager.Instance.DrawTextureWithColor(FieldTexture, Position,
                    new Vector3(1, 1, 1), new Color(255, 255, 255, alpha));

                fontDrawPosition -= new Vector3(FieldTexture.HalfWidth, FieldTexture.HalfHeight, 0) - StringOffset;
            }

            D3D9Manager.Instance.DrawFont(FontKey, fontDrawPosition,
                outputString, textColor);
        }

        public override void Release()
        {
            D3D9Manager.Instance.OnMouseClickEvent -= OnMouseClick;
            D3D9Manager.Instance.RemoveMessageHandler(MsgProc);
            OnEnter = null;
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
            if (ch == (char)KeyType.Enter)
            {
                EnterText(this, EventArgs.Empty);
                return;
            }

            if (ch == (char)KeyType.Back)
            {
                if (inputString.Length > 0)
                    inputString.Remove(inputString.Length - 1, 1);
                return;
            }

            if (inputString.Length <= MaxLength)
            {
                inputString.Append(ch);
            }
        }
    }
}

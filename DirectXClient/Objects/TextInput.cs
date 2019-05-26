using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using GameFramework;
using GameFramework.Manager;
using SharpDX;

namespace DirectXClient
{
    class TextInput : GameObject
    {
        StringBuilder   inputString                 = new StringBuilder();

        public Vector3  Position    { get; set; }   = new Vector3(0, 0, 0);
        public string   FontKey     { get; set; }   = string.Empty;
        public bool     IsSelected  { get; private set; } = true;

        public event EventHandler<string> OnEnter;

        public TextInput(string fontKey)
        {
            FontKey = fontKey;
        }

        public override void FrameRender()
        {
            Direct3D9Manager.Instance.DrawFont(FontKey, Position, inputString.ToString(), Color.White);
        }

        public override void FrameUpdate()
        {
        }

        public override void Initialize()
        {
            Direct3D9Manager.Instance.AddMessageHandler(MsgProc);
        }

        public override void Release()
        {
            Direct3D9Manager.Instance.RemoveMessageHandler(MsgProc);
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
                OnEnter?.Invoke(this, inputString.ToString());
                inputString.Clear();
                return;
            }

            if (ch == (char)8)
            {
                if (inputString.Length > 0)
                    inputString.Remove(inputString.Length - 1, 1);
            }
            else
                inputString.Append(ch);
        }
    }
}

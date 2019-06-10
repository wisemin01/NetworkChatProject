using SharpDX.Windows;
using System;
using System.Security.Permissions;
using System.Windows.Forms;

namespace GameFramework
{
    public class MsgType
    {
        public const int Wm_Char = 0x0102;
    }

    public class KeyType
    {
        public const int Enter = 13;
        public const int Back = 8;
    }
}

namespace GameFramework
{
    class RenderMsgForm : RenderForm
    {
        public event EventHandler<Message> OnMessage;

        public RenderMsgForm()
        {

        }

        public RenderMsgForm(string text) : base(text)
        {

        }


        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            OnMessage?.Invoke(this, m);

            base.WndProc(ref m);
        }

    }
}
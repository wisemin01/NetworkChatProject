using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Windows;
using SharpDX.Direct3D9;
using System.Security.Permissions;
using System.Windows.Forms;

namespace GameFramework
{
    public class MsgType
    {
        public const int Wm_Char = 0x0102;

    }

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

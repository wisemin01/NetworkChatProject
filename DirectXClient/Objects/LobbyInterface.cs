using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPNetwork;
using GameFramework;

namespace DirectXClient
{
    public class LobbyInterface : ITextDraw
    {
        public void ClearText()
        {
        }

        public void DrawColorText(string text, int r, int g, int b, int a)
        {
        }

        public void DrawText(string text)
        {
        }

        public void ShowMessageBox(string text, string caption)
        {
            MessageBox.Show(text, caption);
        }
    }
}

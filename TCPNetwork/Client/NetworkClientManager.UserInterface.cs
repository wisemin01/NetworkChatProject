﻿namespace TCPNetwork.Client
{
    public partial class NetworkClientManager
    {
        public void DrawText(string text)
        {
            if (TextDraw != null)
            {
                TextDraw.DrawText(text);
            }
        }

        public void ClearText()
        {
            if (TextDraw != null)
            {
                TextDraw.ClearText();
            }
        }

        public void ShowMessageBox(string text, string caption)
        {
            if (TextDraw != null)
            {
                TextDraw.ShowMessageBox(text, caption);
            }
        }
    }
}

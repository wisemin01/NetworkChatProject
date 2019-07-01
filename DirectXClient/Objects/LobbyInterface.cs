namespace DirectXClient
{
    public class LobbyInterface
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

namespace TCPNetwork
{
    /*
     * 텍스트를 출력할 수 있는 객체의
     * 기본 인터페이스
     */
    public interface ITextDraw
    {
        void DrawText(string text);
        void DrawColorText(string text, int r, int g, int b, int a);
        void ClearText();
        void ShowMessageBox(string text, string caption);
    }
}

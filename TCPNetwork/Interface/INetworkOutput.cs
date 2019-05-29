namespace TCPNetwork
{
    /*
     * 네트워크 상의 출력 담당 인터페이스
     */
    public interface INetworkOutput
    {
        void AddRoomToListBox(string roomName);
        void RemoveRoomToListBox(string roomName);
    }
}

using System.Text;
using TCPNetwork.Interface.Packet;
using TCPNetwork.Packet.Chatting;

namespace TCPNetwork.Server
{
    public partial class NetworkServerManager : IPacketHandleCallback
    {
        /* 
         * ITextDraw 인터페이스를 구현한 객체로
         * 텍스트 출력
         */
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

        // 사용자 이름과 메시지를 결합한 기본 메시지 구조를 리턴
        public static string GetDefaultMessageFormat(string userName, string message)
        {
            StringBuilder finalMessage = new StringBuilder();

            finalMessage.
                Append("[").
                Append(userName).
                Append("] : ").
                Append(message);

            return finalMessage.ToString();
        }

        public void OnChatting(ChattingPacket packet)
        {
        }

        public void OnCreateRoom(CreateRoomPacket packet)
        {
        }

        public void OnDestroyRoom(DestroyRoomPacket packet)
        {
        }

        public void OnRoomCount(RoomCountPacket packet)
        {
        }

        public void OnRoomList(RoomListPacket packet)
        {
        }

        public void OnLeave(LeavePacket packet)
        {
        }

        public void OnMoveToOtherRoom(MoveToOtherRoomPacket packet)
        {
        }

        public void OnWhisper(WhisperPacket packet)
        {
        }

        public void OnLogin(LoginPacket packet)
        {
            // 로비에 추가
            networkLobby.Add(clientAcceptSocket, packet.Name);
        }
    }
}

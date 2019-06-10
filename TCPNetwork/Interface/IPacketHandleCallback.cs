using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TCPNetwork.Packet.Chatting;

namespace TCPNetwork.Interface.Packet
{
    public interface IPacketHandleCallback
    {
        void OnChatting(ChattingPacket packet);
        void OnCreateRoom(CreateRoomPacket packet);
        void OnDestroyRoom(DestroyRoomPacket packet);
        void OnRoomCount(RoomCountPacket packet);
        void OnRoomList(RoomListPacket packet);
        void OnLeave(LeavePacket packet);
        void OnMoveToOtherRoom(MoveToOtherRoomPacket packet);
        void OnWhisper(WhisperPacket packet);
        void OnLogin(LoginPacket packet);
    }

}

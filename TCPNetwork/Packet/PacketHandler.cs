using Google.Protobuf;
using System;
using System.Linq;
using System.Net.Sockets;
using TCPNetwork.Interface.Packet;

namespace TCPNetwork.Packet.Chatting
{
    public class PacketHandler
    {
        public IPacketHandleCallback FunctionHandler { get; set; } = null;

        static public void Send<T>(MessageType type, T packet, TcpClient client) where T : IMessage<T>
        {
            byte[] message = packet.ToByteArray();

            byte[] message_info = BitConverter.GetBytes((int)type);
            byte[] message_size = BitConverter.GetBytes(message.Length);

            byte[] buffer = new byte[message.Length + sizeof(int) * 2];

            message_info.CopyTo(buffer, 0);
            message_size.CopyTo(buffer, sizeof(int));
            message.CopyTo(buffer, sizeof(int) * 2);

            NetworkStream stream = client.GetStream();

            stream.Write(buffer, 0, buffer.Count());
            stream.Flush();
        }

        public void Receive(byte[] buffer)
        {
            MessageType type = (MessageType)BitConverter.ToInt32(buffer, 0);
            int size = BitConverter.ToInt32(buffer, sizeof(int));

            byte[] message = new byte[size];

            Buffer.BlockCopy(buffer, sizeof(int) * 2, message, 0, size);
            
            switch (type)
            {
                case MessageType.Chatting:
                    FunctionHandler.OnChatting(ChattingPacket.Parser.ParseFrom(message));
                    break;

                case MessageType.CreateRoom:
                    FunctionHandler.OnCreateRoom(CreateRoomPacket.Parser.ParseFrom(message));
                    break;

                case MessageType.DestroyRoom:
                    FunctionHandler.OnDestroyRoom(DestroyRoomPacket.Parser.ParseFrom(message));
                    break;

                case MessageType.GetRoomCount:
                    FunctionHandler.OnRoomCount(RoomCountPacket.Parser.ParseFrom(message));
                    break;

                case MessageType.GetRoomList:
                    FunctionHandler.OnRoomList(RoomListPacket.Parser.ParseFrom(message));
                    break;

                case MessageType.Leave:
                    FunctionHandler.OnLeave(LeavePacket.Parser.ParseFrom(message));
                    break;

                case MessageType.MoveToOtherRoom:
                    FunctionHandler.OnMoveToOtherRoom(MoveToOtherRoomPacket.Parser.ParseFrom(message));
                    break;

                case MessageType.Whisper:
                    FunctionHandler.OnWhisper(WhisperPacket.Parser.ParseFrom(message));
                    break;

                case MessageType.Login:
                    FunctionHandler.OnLogin(LoginPacket.Parser.ParseFrom(message));
                    break;
            }

            return;
        }
    }
}

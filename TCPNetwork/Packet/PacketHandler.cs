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

            byte[] message_size = BitConverter.GetBytes(message.Length);
            byte[] message_info = BitConverter.GetBytes((int)type);
            byte[] message_process_type = BitConverter.GetBytes(0x00000008);

            byte[] buffer = new byte[message.Length + sizeof(int) * 3];

            message_size.CopyTo(buffer, 0);
            message_info.CopyTo(buffer, sizeof(int));
            message_process_type.CopyTo(buffer, sizeof(int) * 2);
            message.CopyTo(buffer, sizeof(int) * 3);

            NetworkStream stream = client.GetStream();

            stream.Write(buffer, 0, buffer.Count());
            stream.Flush();
        }

        public void Receive(byte[] buffer)
        {
            int             size = BitConverter.ToInt32(buffer, 0);
            MessageType     type = (MessageType)BitConverter.ToInt32(buffer, sizeof(int) * 1);
            _                    = BitConverter.ToInt32(buffer, sizeof(int) * 2);

            byte[] message = new byte[size];

            Buffer.BlockCopy(buffer, sizeof(int) * 3, message, 0, size);

            switch (type)
            {
                case MessageType.Chatting:
                    {
                        ChattingPacket packet = ChattingPacket.Parser.ParseFrom(message);
                        FunctionHandler.OnChatting(packet);
                        // Console.WriteLine($"ChattingPacket : {packet.Name} {packet.Text}");
                    }
                    break;

                case MessageType.CreateRoom:
                    {
                        CreateRoomPacket packet = CreateRoomPacket.Parser.ParseFrom(message);
                        FunctionHandler.OnCreateRoom(packet);
                        // Console.WriteLine($"CreateRoomPacket : {packet.RoomName}");
                    }
                    break;

                case MessageType.DestroyRoom:
                    {
                        DestroyRoomPacket packet = DestroyRoomPacket.Parser.ParseFrom(message);
                        FunctionHandler.OnDestroyRoom(packet);
                        // Console.WriteLine($"DestroyRoomPacket : {packet.RoomName}");
                    }
                    break;

                case MessageType.GetRoomCount:
                    {
                        RoomCountPacket packet = RoomCountPacket.Parser.ParseFrom(message);
                        FunctionHandler.OnRoomCount(packet);
                        // Console.WriteLine($"RoomCountPacket : {packet.Count}");
                    }
                    break;

                case MessageType.GetRoomList:
                    {
                        RoomListPacket packet = RoomListPacket.Parser.ParseFrom(message);
                        FunctionHandler.OnRoomList(packet);
                        // Console.Write($"RoomListPacket : ");
                        // foreach (var Iter in packet.RoomNames)
                        // {
                        //     Console.Write($" {Iter}");
                        // }
                        // Console.WriteLine("");
                    }
                    break;

                case MessageType.Leave:
                    {
                        LeavePacket packet = LeavePacket.Parser.ParseFrom(message);
                        FunctionHandler.OnLeave(packet);
                        // Console.WriteLine($"LeavePacket : {packet.Text}");
                    }
                    break;

                case MessageType.MoveToOtherRoom:
                    {
                        MoveToOtherRoomPacket packet = MoveToOtherRoomPacket.Parser.ParseFrom(message);
                        FunctionHandler.OnMoveToOtherRoom(packet);
                        // Console.WriteLine($"MoveToOtherRoomPacket : {packet.Name} {packet.RoomName}");
                    }
                    break;

                case MessageType.Whisper:
                    {
                        WhisperPacket packet = WhisperPacket.Parser.ParseFrom(message);
                        FunctionHandler.OnWhisper(packet);
                        // Console.WriteLine($"WhisperPacket : {packet.Sender} {packet.Listener} {packet.Text}");
                    }
                    break;

                case MessageType.Login:
                    {
                        LoginPacket packet = LoginPacket.Parser.ParseFrom(message);
                        FunctionHandler.OnLogin(packet);
                        // Console.WriteLine($"LoginPacket : {packet.Name}");
                    }
                    break;
            }

            return;
        }
    }
}

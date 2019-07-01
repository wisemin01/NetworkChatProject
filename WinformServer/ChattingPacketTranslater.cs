using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MNetwork.Packet;
using ChattingPacket;

namespace ServerHost
{
    internal class ChattingPacketTranslater : PacketTranslater
    {
        public override BasePacket Distinction(byte[] buffer)
        {
            PacketHeader header = PacketHeader.ParseFrom(buffer);

            MessageType type = (MessageType)header.packetType;

            switch (type)
            {
                case MessageType.LoginRequest:
                    return new ProtobufPacket<LoginRequestPacket>(buffer);
                case MessageType.LoginAnswer:
                    return new ProtobufPacket<LoginAnswerPacket>(buffer);
                case MessageType.JoinRoomRequest:
                    return new ProtobufPacket<JoinRoomRequestPacket>(buffer);
                case MessageType.JoinRoomAnswer:
                    return new ProtobufPacket<JoinRoomAnswerPacket>(buffer);
                case MessageType.ExitRoomRequest:
                    return new ProtobufPacket<ExitRoomRequestPacket>(buffer);
                case MessageType.ExitRoomAnswer:
                    return new ProtobufPacket<ExitRoomAnswerPacket>(buffer);
                case MessageType.CreateRoomRequest:
                    return new ProtobufPacket<CreateRoomRequestPacket>(buffer);
                case MessageType.CreateRoomAnswer:
                    return new ProtobufPacket<CreateRoomAnswerPacket>(buffer);
                case MessageType.RoomListRequest:
                    return new ProtobufPacket<RoomListRequestPacket>(buffer);
                case MessageType.RoomListAnswer:
                    return new ProtobufPacket<RoomListAnswerPacket>(buffer);
                case MessageType.WhisperRequest:
                    return new ProtobufPacket<WhisperRequestPacket>(buffer);
                case MessageType.WhisperAnswer:
                    return new ProtobufPacket<WhisperAnswerPacket>(buffer);
                case MessageType.SignUpRequest:
                    return new ProtobufPacket<SignUpRequestPacket>(buffer);
                case MessageType.SignUpAnswer:
                    return new ProtobufPacket<SignUpAnswerPacket>(buffer);
                case MessageType.ChattingRequest:
                    return new ProtobufPacket<ChattingRequestPacket>(buffer);
                case MessageType.ChattingAnswer:
                    return new ProtobufPacket<ChattingAnswerPacket>(buffer);
                default:
                    return null;
            }
        }
    }
}

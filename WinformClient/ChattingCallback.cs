using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChattingPacket;
using MNetwork.Callback;
using MNetwork.Packet;
using MNetwork.Debuging;

namespace ClientHost
{
    internal class ChattingCallback : NetworkCallback
    {
        public override bool HandleNetworkMessage(BasePacket packet)
        {
            PacketHeader header = PacketHeader.ParseFrom(packet.Buffer);

            MessageType type = (MessageType)header.packetType;

            switch (type)
            {
                case MessageType.LoginAnswer:
                    OnLoginAnswer(packet as ProtobufPacket<LoginAnswerPacket>);
                    return true;
                case MessageType.JoinRoomAnswer:
                    OnJoinRoomAnswer(packet as ProtobufPacket<JoinRoomAnswerPacket>);
                    return true;
                case MessageType.ExitRoomAnswer:
                    OnExitRoomAnswer(packet as ProtobufPacket<ExitRoomAnswerPacket>);
                    return true;
                case MessageType.CreateRoomAnswer:
                    OnCreateRoomAnswer(packet as ProtobufPacket<CreateRoomAnswerPacket>);
                    return true;
                case MessageType.RoomListAnswer:
                    OnRoomListAnswer(packet as ProtobufPacket<RoomListAnswerPacket>);
                    return true;
                case MessageType.WhisperAnswer:
                    OnWhisperAnswer(packet as ProtobufPacket<WhisperAnswerPacket>);
                    return true;
                case MessageType.SignUpAnswer:
                    OnSignUpAnswer(packet as ProtobufPacket<SignUpAnswerPacket>);
                    return true;
                case MessageType.ChattingAnswer:
                    OnChattingAnswer(packet as ProtobufPacket<ChattingAnswerPacket>);
                    return true;
            }

            return false;
        }
        
        public void OnLoginAnswer(ProtobufPacket<LoginAnswerPacket> packet)
        {
            LoginAnswerPacket answer = packet.ProtobufMessage;

            if (answer.Success == true)
            {
                Debug.Log($"{answer.UserName} 님이 접속하셨습니다.");
            }
            else
            {
                Debug.Log($"로그인 실패.\n 사유 : {answer.Context}");
            }
        }

        public void OnJoinRoomAnswer(ProtobufPacket<JoinRoomAnswerPacket> packet)
        {

        }

        public void OnExitRoomAnswer(ProtobufPacket<ExitRoomAnswerPacket> packet)
        {

        }
        public void OnCreateRoomAnswer(ProtobufPacket<CreateRoomAnswerPacket> packet)
        {

        }

        public void OnRoomListAnswer(ProtobufPacket<RoomListAnswerPacket> packet)
        {

        }

        public void OnWhisperAnswer(ProtobufPacket<WhisperAnswerPacket> packet)
        {

        }
        public void OnSignUpAnswer(ProtobufPacket<SignUpAnswerPacket> packet)
        {
            SignUpAnswerPacket answer = packet.ProtobufMessage;

            Debug.Log(answer.Success ? "회원가입 성공" : "회원가입 실패");
            Debug.Log(answer.Context);
        }
        public void OnChattingAnswer(ProtobufPacket<ChattingAnswerPacket> packet)
        {

        }
    }
}

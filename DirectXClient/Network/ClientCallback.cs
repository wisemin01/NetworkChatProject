using MNetwork.Callback;
using MNetwork.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChattingPacket;
using MNetwork.Debuging;
using MNetwork.Rooms;


namespace DirectXClient
{
    internal class ClientCallback : NetworkCallback
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

                ChatClientManager.userName = answer.UserName;
                ChatClientManager.userState = MNetworkPlayer.MPlayerState.LoginSuccess;
            }
            else
            {
                Debug.Log($"로그인 실패. [사유 : {answer.Context}]");
            }
        }

        public void OnJoinRoomAnswer(ProtobufPacket<JoinRoomAnswerPacket> packet)
        {
            JoinRoomAnswerPacket answer = packet.ProtobufMessage;

            string s = answer.Success ? "성공했습니다." : "실패했습니다.";

            Debug.Log($"{answer.RoomName} 방 입장에 {s}");
        }

        public void OnExitRoomAnswer(ProtobufPacket<ExitRoomAnswerPacket> packet)
        {
            ExitRoomAnswerPacket answer = packet.ProtobufMessage;
        }

        public void OnCreateRoomAnswer(ProtobufPacket<CreateRoomAnswerPacket> packet)
        {
            CreateRoomAnswerPacket answer = packet.ProtobufMessage;

            if (answer.Success == true)
            {
                Debug.Log("방 생성 성공");
            }
            else
            {
                Debug.Log("방 생성 실패");
            }
        }

        public void OnRoomListAnswer(ProtobufPacket<RoomListAnswerPacket> packet)
        {
            RoomListAnswerPacket answer = packet.ProtobufMessage;
        }

        public void OnWhisperAnswer(ProtobufPacket<WhisperAnswerPacket> packet)
        {
            WhisperAnswerPacket answer = packet.ProtobufMessage;

            Debug.Log($"{answer.Sender} >> {answer.Text}");
        }

        public void OnSignUpAnswer(ProtobufPacket<SignUpAnswerPacket> packet)
        {
            SignUpAnswerPacket answer = packet.ProtobufMessage;

            Debug.Log(answer.Success ? "회원가입 성공" : "회원가입 실패");
            Debug.Log(answer.Context);
        }

        public void OnChattingAnswer(ProtobufPacket<ChattingAnswerPacket> packet)
        {
            ChattingAnswerPacket answer = packet.ProtobufMessage;

            Debug.Log(answer.Text);
        }
    }
}

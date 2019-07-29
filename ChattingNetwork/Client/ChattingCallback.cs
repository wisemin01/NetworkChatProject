using ChattingPacket;
using MNetwork.Callback;
using MNetwork.Packet;
using MNetwork.Debuging;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ChattingNetwork.Client
{
    internal class ChattingCallback : NetworkCallback
    {
        public event EventHandler<bool> OnSignIn;
        public event EventHandler<bool> OnSignUp;
        public event EventHandler<string> OnChatting;
        public event EventHandler<bool> OnJoinRoom;
        public event EventHandler<bool> OnExitRoom;
        public event EventHandler<bool> OnCreateRoom;
        public event EventHandler<List<string>> OnRoomListRefresh;
        public event EventHandler<string> OnWhisper;

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

                UserInfoManager.userName = answer.UserName;
                UserInfoManager.userState = MNetwork.Rooms.MNetworkPlayer.MPlayerState.LoginSuccess;
            }
            else
            {
                Debug.Log($"로그인 실패. [사유 : {answer.Context}]");
            }

            OnSignIn?.Invoke(this, answer.Success);
        }

        public void OnJoinRoomAnswer(ProtobufPacket<JoinRoomAnswerPacket> packet)
        {
            JoinRoomAnswerPacket answer = packet.ProtobufMessage;

            string s = answer.Success ? "성공했습니다." : "실패했습니다.";

            Debug.Log($"{answer.RoomName} 방 입장에 {s}");

            OnJoinRoom?.Invoke(this, answer.Success);
        }

        public void OnExitRoomAnswer(ProtobufPacket<ExitRoomAnswerPacket> packet)
        {
            ExitRoomAnswerPacket answer = packet.ProtobufMessage;

            OnExitRoom?.Invoke(this, answer.Success);
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

            OnCreateRoom?.Invoke(this, answer.Success);
        }

        public void OnRoomListAnswer(ProtobufPacket<RoomListAnswerPacket> packet)
        {
            RoomListAnswerPacket answer = packet.ProtobufMessage;

            List<string> names = new List<string>();

            foreach (var s in answer.RoomNames)
            {
                names.Add(s);
            }

            OnRoomListRefresh?.Invoke(this, names);
        }

        public void OnWhisperAnswer(ProtobufPacket<WhisperAnswerPacket> packet)
        {
            WhisperAnswerPacket answer = packet.ProtobufMessage;

            string ret = $"{answer.Sender} >> {answer.Text}";

            Debug.Log(ret);

            OnWhisper?.Invoke(this, ret);
        }

        public void OnSignUpAnswer(ProtobufPacket<SignUpAnswerPacket> packet)
        {
            SignUpAnswerPacket answer = packet.ProtobufMessage;

            Debug.Log(answer.Success ? "회원가입 성공" : "회원가입 실패");
            Debug.Log(answer.Context);

            OnSignUp?.Invoke(this, answer.Success);
        }

        public void OnChattingAnswer(ProtobufPacket<ChattingAnswerPacket> packet)
        {
            ChattingAnswerPacket answer = packet.ProtobufMessage;
            
            Debug.Log(answer.Text);

            OnChatting?.Invoke(this, answer.Text);
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MNetwork.Logic;
using MNetwork.Engine;
using MNetwork.Packet;
using MNetwork.Time;

using ChattingPacket;
using MNetwork.Rooms;

namespace ServerHost
{
    internal partial class ChattingLogic : MLogicEntry
    {
        private void OnLoginRequest(ProtobufPacket<LoginRequestPacket> packet)
        {
            LoginRequestPacket request = packet.ProtobufMessage;
            LoginAnswerPacket send = new LoginAnswerPacket();

            // Packet Data Set

            // 로그인 핸들러에게 로그인이 성공했는지를 알아온다.
            bool success = loginHandler.SignIn(request.ID, request.Password, out string userName);

            // 보낼 패킷의 정보 설정
            send.UserName = userName;
            send.Success = success;
            send.Context = "로그인에 성공했습니다.";

            MNetworkPlayer player = NetworkLobby.FindPlayer(packet.Serial);

            if (send.Success == false)
            {
                send.Context = "아이디 또는 비밀번호를 확인해주세요.";
            }
            else if (player != null)
            {
                if (player.PlayerState >= MNetworkPlayer.MPlayerState.LoginSuccess)
                {
                    // 이미 접속중인 클라이언트 처리

                    send.Success = false;
                    send.Context = "이미 접속된 아이디입니다.";
                }
                else
                {
                    MNetworkPlayer find = NetworkLobby.FindPlayer(userName);

                    if (find != null)
                    {
                        // 이미 해당 닉네임을 가진 유저가 접속중임을 처리
                        send.Success = false;
                        send.Context = "해당 아이디는 이미 다른 컴퓨터에서 사용 중입니다.";
                    }
                    else
                    {
                        // 없다면 네임 리스트에 추가해준다.

                        player.ID = request.ID;
                        player.Password = request.Password;
                        player.PlayerState = MNetworkPlayer.MPlayerState.LoginSuccess;
                        player.UserName = userName;

                        NetworkLobby.AddPlayerToNameList(player);

                        send.Success = true;
                    }
                }
            }

            SendPacket(new ProtobufPacket<LoginAnswerPacket>(packet.Serial, PacketEnum.ProcessType.Data,
                (int)MessageType.LoginAnswer, send));
        }

        private void OnJoinRoomRequest(ProtobufPacket<JoinRoomRequestPacket> packet)
        {
            JoinRoomRequestPacket request = packet.ProtobufMessage;
            JoinRoomAnswerPacket send = new JoinRoomAnswerPacket();

            // Packet Data Set

            SendPacket(new ProtobufPacket<JoinRoomAnswerPacket>(packet.Serial, PacketEnum.ProcessType.Data,
                (int)MessageType.JoinRoomAnswer, send));
        }

        private void OnExitRoomRequest(ProtobufPacket<ExitRoomRequestPacket> packet)
        {
            ExitRoomRequestPacket request = packet.ProtobufMessage;
            ExitRoomAnswerPacket send = new ExitRoomAnswerPacket();

            // Packet Data Set

            SendPacket(new ProtobufPacket<ExitRoomAnswerPacket>(packet.Serial, PacketEnum.ProcessType.Data,
                (int)MessageType.ExitRoomAnswer, send));
        }

        private void OnCreateRoomRequest(ProtobufPacket<CreateRoomRequestPacket> packet)
        {
            CreateRoomRequestPacket request = packet.ProtobufMessage;
            CreateRoomAnswerPacket send = new CreateRoomAnswerPacket();

            // Packet Data Set
            bool success = NetworkLobby.AddRoom(request.RoomName, new MNetwork.Rooms.MNetworkRoom(request.RoomName));

            send.Success = success;

            SendPacket(new ProtobufPacket<CreateRoomAnswerPacket>(packet.Serial, PacketEnum.ProcessType.Data,
                (int)MessageType.CreateRoomAnswer, send));
        }

        private void OnRoomListRequest(ProtobufPacket<RoomListRequestPacket> packet)
        {
            RoomListRequestPacket request = packet.ProtobufMessage;
            RoomListAnswerPacket send = new RoomListAnswerPacket();

            // Packet Data Set

            SendPacket(new ProtobufPacket<RoomListAnswerPacket>(packet.Serial, PacketEnum.ProcessType.Data,
                (int)MessageType.RoomListAnswer, send));
        }

        private void OnWhisperRequest(ProtobufPacket<WhisperRequestPacket> packet)
        {
            WhisperRequestPacket request = packet.ProtobufMessage;
            WhisperAnswerPacket send = new WhisperAnswerPacket();

            // Packet Data Set

            MNetworkPlayer listener = NetworkLobby.FindPlayer(request.Listener);

            if (listener == null)
                return;

            send.Sender = request.Sender;
            send.Text = $"From {request.Sender} : {request.Text}";

            SendPacket(new ProtobufPacket<WhisperAnswerPacket>(listener.Serial, PacketEnum.ProcessType.Data,
                (int)MessageType.WhisperAnswer, send));
        }

        private void OnSignUpRequest(ProtobufPacket<SignUpRequestPacket> packet)
        {
            SignUpRequestPacket request = packet.ProtobufMessage;
            SignUpAnswerPacket send = new SignUpAnswerPacket();

            // Packet Data Set

            bool success = loginHandler.SignUp(request.ID, request.Password, request.UserName, out string context);

            send.Success = success;
            send.Context = context;

            SendPacket(new ProtobufPacket<SignUpAnswerPacket>(packet.Serial, PacketEnum.ProcessType.Data,
                (int)MessageType.SignUpAnswer, send));
        }

        private void OnChattingRequest(ProtobufPacket<ChattingRequestPacket> packet)
        {
            ChattingRequestPacket request = packet.ProtobufMessage;
            ChattingAnswerPacket send = new ChattingAnswerPacket();

            // Packet Data Set

            MNetworkPlayer sender = NetworkLobby.FindPlayer(request.Sender);

            if (sender == null)
                return;

            MNetworkRoom room = NetworkLobby.FindRoom(sender.RoomKey);

            if (room == null)
                return;

            send.Text = $"[{Time.TimeLogHMS}] {request.Sender} : {request.Text}";

            SendPacket(new ProtobufPacket<ChattingAnswerPacket>(packet.Serial, PacketEnum.ProcessType.Data,
                (int)MessageType.ChattingAnswer, send), room.SerialList);
        }
    }
}
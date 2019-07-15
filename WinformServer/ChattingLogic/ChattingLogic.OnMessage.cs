using System;
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

            while (true)
            {
                MNetworkPlayer player;

                // 받은 패킷의 시리얼로 플레이어를 찾아서
                player = NetworkLobby.FindPlayer(packet.Serial);

                // 로그인이 아직 안된 경우 플레이어 정보 설정 후
                // 로그인 상태로 변경
                if (player.PlayerState != MNetworkPlayer.MPlayerState.LoginSuccess)
                {
                    player.PlayerState = MNetworkPlayer.MPlayerState.LoginSuccess;
                    player.UserName = userName;
                    player.ID = request.ID;
                    player.Password = request.Password;

                    // 이름으로 플레이어를 빠르게 찾기 위해서 
                    // 이름 리스트에 저장한다.
                    NetworkLobby.AddPlayerToNameList(player);
                }
                // 이미 로그인 된 클라이언트라면
                // 로그인이 실패라는걸 알려준다.
                else
                {
                    send.Success = false;
                    send.Context = "이미 접속중입니다.";
                    break;
                }

                // 이름으로 플레이어를 찾아서
                // 만약 존재한다면 이미 접속중인 아이디이므로 접속 불가 설정
                player = NetworkLobby.FindPlayer(userName);

                if (player != null)
                {
                    send.Success = false;
                    send.Context = "이미 접속중인 아이디입니다.";
                    break;
                }

                break;
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

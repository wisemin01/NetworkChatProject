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
    internal partial class ChattingLogic : LogicEntry
    {
        private void OnLoginRequest(ProtobufPacket<LoginRequestPacket> packet)
        {
            LoginRequestPacket request = packet.ProtobufMessage;
            LoginAnswerPacket send = new LoginAnswerPacket();

            // Packet Data Set

            bool success = loginHandler.Login(request.ID, request.Password, out string userName);

            send.UserName = userName;
            send.Success = success;

            MEngine.Instance.SendRequest(new ProtobufPacket<LoginAnswerPacket>(packet.Serial, PacketEnum.ProcessType.Data,
                (int)MessageType.LoginAnswer, send));
        }

        private void OnJoinRoomRequest(ProtobufPacket<JoinRoomRequestPacket> packet)
        {
            JoinRoomRequestPacket request = packet.ProtobufMessage;
            JoinRoomAnswerPacket send = new JoinRoomAnswerPacket();

            // Packet Data Set

            MEngine.Instance.SendRequest(new ProtobufPacket<JoinRoomAnswerPacket>(packet.Serial, PacketEnum.ProcessType.Data,
                (int)MessageType.JoinRoomAnswer, send));
        }

        private void OnExitRoomRequest(ProtobufPacket<ExitRoomRequestPacket> packet)
        {
            ExitRoomRequestPacket request = packet.ProtobufMessage;
            ExitRoomAnswerPacket send = new ExitRoomAnswerPacket();

            // Packet Data Set

            MEngine.Instance.SendRequest(new ProtobufPacket<ExitRoomAnswerPacket>(packet.Serial, PacketEnum.ProcessType.Data,
                (int)MessageType.ExitRoomAnswer, send));
        }

        private void OnCreateRoomRequest(ProtobufPacket<CreateRoomRequestPacket> packet)
        {
            CreateRoomRequestPacket request = packet.ProtobufMessage;
            CreateRoomAnswerPacket send = new CreateRoomAnswerPacket();

            // Packet Data Set
            bool success = NetworkLobby.AddRoom(request.RoomName, new MNetwork.Rooms.MNetworkRoom(request.RoomName));

            send.Success = success;

            MEngine.Instance.SendRequest(new ProtobufPacket<CreateRoomAnswerPacket>(packet.Serial, PacketEnum.ProcessType.Data,
                (int)MessageType.CreateRoomAnswer, send));
        }

        private void OnRoomListRequest(ProtobufPacket<RoomListRequestPacket> packet)
        {
            RoomListRequestPacket request = packet.ProtobufMessage;
            RoomListAnswerPacket send = new RoomListAnswerPacket();

            // Packet Data Set

            MEngine.Instance.SendRequest(new ProtobufPacket<RoomListAnswerPacket>(packet.Serial, PacketEnum.ProcessType.Data,
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

            MEngine.Instance.SendRequest(new ProtobufPacket<WhisperAnswerPacket>(listener.Serial, PacketEnum.ProcessType.Data,
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

            MEngine.Instance.SendRequest(new ProtobufPacket<SignUpAnswerPacket>(packet.Serial, PacketEnum.ProcessType.Data,
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

            MEngine.Instance.SendRequest(new ProtobufPacket<ChattingAnswerPacket>(packet.Serial, PacketEnum.ProcessType.Data,
                (int)MessageType.ChattingAnswer, send), room.SerialList);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using MNetwork.Logic;
using MNetwork.Packet;
using MNetwork.Rooms;
using MNetwork.Debuging;

using ChattingPacket;

namespace ChattingNetwork.Server
{
    internal partial class ChattingLogic : MLogicEntry
    {
        public MNetworkLobby NetworkLobby { get; private set; } = null;

        public LoginHandler loginHandler = new LoginHandler();

        public override LogicEntry Clone()
        {
            return null;
        }

        public override bool Initialize()
        {
            NetworkLobby = new MNetworkLobby();

            NetworkLobby.CreateLobbyRoom();
            NetworkLobby.Initialize("ROOM", 5);

            return true;
        }

        public override void ProcessPacket(BasePacket packet)
        {
            PacketEnum.ProcessType processType = packet.ProcessType;

            switch (processType)
            {
                case PacketEnum.ProcessType.Connect:
                    {
                        MNetworkPlayer player = new MNetworkPlayer(packet.Serial);
                        NetworkLobby.AddPlayer(player);

                        player.PlayerState = MNetworkPlayer.MPlayerState.Connected;

                        Debug.Log($"S:[{packet.Serial}] Connected");
                        break;
                    }

                case PacketEnum.ProcessType.Disconnect:
                    {
                        NetworkLobby.DeletePlayer(packet.Serial);
                        Debug.Log($"S:[{packet.Serial}] Disconnected");
                        break;
                    }

                case PacketEnum.ProcessType.Data:
                    {
                        OnMessage(packet);
                        break;
                    }
            }
        }

        public void OnMessage(BasePacket packet)
        {
            MessageType type = (MessageType)packet.Type;

            switch (type)
            {
                case MessageType.LoginRequest:
                    OnLoginRequest(packet as ProtobufPacket<LoginRequestPacket>);
                    break;
                case MessageType.JoinRoomRequest:
                    OnJoinRoomRequest(packet as ProtobufPacket<JoinRoomRequestPacket>);
                    break;
                case MessageType.ExitRoomRequest:
                    OnExitRoomRequest(packet as ProtobufPacket<ExitRoomRequestPacket>);
                    break;
                case MessageType.CreateRoomRequest:
                    OnCreateRoomRequest(packet as ProtobufPacket<CreateRoomRequestPacket>);
                    break;
                case MessageType.RoomListRequest:
                    OnRoomListRequest(packet as ProtobufPacket<RoomListRequestPacket>);
                    break;
                case MessageType.WhisperRequest:
                    OnWhisperRequest(packet as ProtobufPacket<WhisperRequestPacket>);
                    break;
                case MessageType.SignUpRequest:
                    OnSignUpRequest(packet as ProtobufPacket<SignUpRequestPacket>);
                    break;
                case MessageType.ChattingRequest:
                    OnChattingRequest(packet as ProtobufPacket<ChattingRequestPacket>);
                    break;
                case MessageType.CreateAndJoinRoomRequest:
                    OnCreateAndJoinRoomRequest(packet as ProtobufPacket<CreateAndJoinRoomRequestPacket>);
                    break;

                default:
                    break;
            }
        }
    }
}
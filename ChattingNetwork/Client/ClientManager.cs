using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using MNetwork.Debuging;
using MNetwork.Time;
using MNetwork.Engine;
using MNetwork.Packet;

using ChattingPacket;
using ChattingNetwork;

namespace ChattingNetwork.Client
{
    public partial class ClientManager
    {
        private static ClientManager instance = null;

        public static ClientManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ClientManager();

                return instance;
            }
        }

        private readonly ChattingCallback 
            callback = new ChattingCallback();

        private readonly ChattingPacketTranslater 
            translater = new ChattingPacketTranslater();

        private bool isConnected = false;

        public ClientManager()
        {
            Debug.LogPath = $"./Log/Client[{Time.TimeLogYMD}].txt";
        }

        ~ClientManager()
        {
            Debug.Flush();
        }

        public bool Connect(string IP, ushort port)
        {
            if (string.IsNullOrWhiteSpace(IP))
            {
                Debug.WarningLog("IP 입력이 비어 있습니다.");
                return false;
            }

            try
            {
                MNetworkEntry.Instance.Initialize(callback, translater);
                MNetworkEntry.Instance.Run("127.0.0.1", 9199);

                isConnected = true;

                return true;
            }
            catch (FormatException)
            {
                Debug.WarningLog("입력 형식이 맞지 않습니다.");
            }
            catch (OverflowException)
            {
                Debug.WarningLog("최대 입력 범위를 벗어났습니다.");
            }
            catch (Exception)
            {
                Debug.WarningLog("오류가 발생했습니다.");
            }

            return false;
        }

        public bool Disconnect()
        {
            if (MNetworkEntry.Instance.Shutdown() == true)
            {
                isConnected = false;
                return true;
            }

            return false;
        }

        public void Update()
        {
            MNetworkEntry.Instance.Update();
        }

        public bool UpdateAsync()
        {
            Task.Factory.StartNew(
                delegate {
                    while (isConnected == true)
                    {
                        Update();
                    }
                });

            return true;
        }

        public delegate void LogCallback(object sender, string e);
        public void AddDebuger(LogCallback debuger)
        {
            Debug.OnLog += delegate (object sender, string e) { debuger(sender, e); };
        }

        // CHATTING INTERFACE

        public bool SendChat(string context)
        {
            if (isConnected == false)
                return false;

            ChattingRequestPacket packet = new ChattingRequestPacket
            {
                Sender = UserInfoManager.userName,
                Text = context
            };

            return MNetworkEntry.Instance.Send(new ProtobufPacket<ChattingRequestPacket>(0, PacketEnum.ProcessType.Data,
                (int)MessageType.ChattingRequest, packet));
        }

        public bool CreateRoom(string roomName)
        {
            if (isConnected == false)
                return false;

            CreateRoomRequestPacket packet = new CreateRoomRequestPacket()
            {
                RoomName = roomName
            };

            return MNetworkEntry.Instance.Send(new ProtobufPacket<CreateRoomRequestPacket>(0, PacketEnum.ProcessType.Data,
                (int)MessageType.CreateRoomRequest, packet));
        }

        public bool JoinRoom(string roomName)
        {
            if (isConnected == false)
                return false;

            JoinRoomRequestPacket packet = new JoinRoomRequestPacket()
            {
                UserName = UserInfoManager.userName,
                RoomName = roomName
            };

            return MNetworkEntry.Instance.Send(new ProtobufPacket<JoinRoomRequestPacket>(0, PacketEnum.ProcessType.Data,
                (int)MessageType.JoinRoomRequest, packet));
        }

        public bool ExitRoom(string roomName)
        {
            if (isConnected == false)
                return false;

            ExitRoomRequestPacket packet = new ExitRoomRequestPacket()
            {
                UserName = UserInfoManager.userName,
                RoomName = roomName
            };

            return MNetworkEntry.Instance.Send(new ProtobufPacket<ExitRoomRequestPacket>(0, PacketEnum.ProcessType.Data,
                (int)MessageType.ExitRoomRequest, packet));
        }

        public bool SignIn(string id, string password)
        {
            if (isConnected == false)
                return false;

            LoginRequestPacket packet = new LoginRequestPacket()
            {
                ID = id,
                Password = password
            };

            return MNetworkEntry.Instance.Send(new ProtobufPacket<LoginRequestPacket>(0, PacketEnum.ProcessType.Data,
                (int)MessageType.LoginRequest, packet));
        }

        public bool SignUp(string id, string password, string userName)
        {
            if (isConnected == false)
                return false;

            SignUpRequestPacket packet = new SignUpRequestPacket()
            {
                ID = id,
                Password = password,
                UserName = userName
            };

            return MNetworkEntry.Instance.Send(new ProtobufPacket<SignUpRequestPacket>(0, PacketEnum.ProcessType.Data,
                (int)MessageType.SignUpRequest, packet));
        }

        public bool Whisper(string targetUser, string context)
        {
            if (isConnected == false)
                return false;

            WhisperRequestPacket packet = new WhisperRequestPacket()
            {
                Sender = UserInfoManager.userName,
                Listener = targetUser,
                Text = context
            };

            return MNetworkEntry.Instance.Send(new ProtobufPacket<WhisperRequestPacket>(0, PacketEnum.ProcessType.Data, 
                (int)MessageType.WhisperRequest, packet));
        }
    }
}

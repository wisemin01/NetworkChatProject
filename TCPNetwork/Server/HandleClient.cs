using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using TCPNetwork.Packet.Chatting;
using TCPNetwork.Interface.Packet;

namespace TCPNetwork.Server
{
    /*
     * 클라이언트의 연결 요청 수락 후
     * 해당 클라이언트를 담당해 통신하는 클래스
     */

    public class HandleClient : IPacketHandleCallback
    {
        public delegate void MessageDisplayHandler(string userName, string message, bool flag);
        public delegate void MessageDispatcher(string userName, string message, TcpClient client);
        public delegate void DisconnectedHandler(TcpClient client);

        public TcpClient Client { get; private set; } = null;
        public NetworkRoom NetworkRoom { get; set; } = null;

        public MessageDisplayHandler OnReceived { get; set; } = null;
        public DisconnectedHandler OnDisconnected { get; set; } = null;
        public MessageDispatcher OnMessageSended { get; set; } = null;

        private PacketHandler packetHandler = null;

        public void StartClientHandling(TcpClient client, NetworkRoom room)
        {
            // 맴버 초기화
            Client = client;
            NetworkRoom = room;

            // 패킷 핸들러 초기화
            packetHandler = new PacketHandler
            {
                FunctionHandler = this
            };

            // 스레드 생성
            Thread thread = new Thread(ClientLoop)
            {
                IsBackground = true
            };
            thread.Start();
        }

        private void ClientLoop()
        {
            NetworkStream stream = null;

            try
            {
                byte[] buffer = new byte[1024];
                string message = string.Empty;
                int bytes = 0;

                while (true)
                {
                    if (Client.Connected == false)
                    {
                        throw new Exception();
                    }

                    stream = Client.GetStream();

                    // 메시지를 읽어온다
                    bytes = stream.Read(buffer, 0, buffer.Length);
                    packetHandler.Receive(buffer);
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                if (Client != null)
                {
                    OnDisconnected?.Invoke(Client);
                    Client.Close();
                    stream.Close();
                }
            }
        }

        public void OnChatting(ChattingPacket packet)
        {
            OnReceived(packet.Name, packet.Text, true);
        }
        public void OnCreateRoom(CreateRoomPacket packet)
        {
            NetworkRoom room = NetworkServerManager.Instance.CreateNetworkRoom(packet.RoomName);

            if (room != null)
            {
                OnReceived("System", $"{packet.RoomName} 방이 생성되었습니다.", false);
            }
        }
        public void OnDestroyRoom(DestroyRoomPacket packet)
        {
            NetworkRoom room = NetworkServerManager.Instance.FindNetworkRoom(packet.RoomName);

            if (room == null)
                return;

            if (NetworkRoom.RoomName == packet.RoomName)
                return;

            if (room.ClientList.Count != 0)
                return;

            bool result = NetworkServerManager.Instance.DestroyNetworkRoom(packet.RoomName);

            OnReceived("System", $"{packet.RoomName} 방이 파괴되었습니다.", false);
        }
        public void OnRoomCount(RoomCountPacket packet)
        {
            PacketHandler.Send(MessageType.GetRoomCount, new RoomCountPacket()
            {
                Count = NetworkServerManager.Instance.RoomCount
            }, Client);
        }
        public void OnRoomList(RoomListPacket packet)
        {
            RoomListPacket receive = new RoomListPacket();

            foreach (var Iter in NetworkServerManager.Instance.GetNetworkRoomKeys())
            {
                receive.RoomNames.Add(Iter);
            }

            PacketHandler.Send(MessageType.GetRoomList, receive, Client);
        }
        public void OnLeave(LeavePacket packet)
        {
            PacketHandler.Send(MessageType.Leave, new LeavePacket() { Text = string.Empty }, Client);
        }
        public void OnMoveToOtherRoom(MoveToOtherRoomPacket packet)
        {
            if (packet.RoomName == NetworkRoom.RoomName)
                return;

            bool result = NetworkServerManager.Instance.MoveToOtherRoom(this,
                NetworkServerManager.Instance.FindNetworkRoom(packet.RoomName));

            if (result == false)
            {
                PacketHandler.Send(MessageType.Chatting,
                    new ChattingPacket()
                    {
                        Name = "System",
                        Text = "지정된 방을 찾을 수 없습니다."
                    },
                Client);
            }
            else
            {
                PacketHandler.Send(MessageType.MoveToOtherRoom,
                    new MoveToOtherRoomPacket()
                    {
                        Name = "System",
                        RoomName = packet.RoomName
                    },
                Client);
            }
        }
        public void OnWhisper(WhisperPacket packet)
        {
            if (packet.Sender == packet.Listener)
            {
                PacketHandler.Send(MessageType.Chatting, new ChattingPacket() {
                    Name = "System",
                    Text = "자기 자신에게는 귓속말을 보낼 수 없습니다."
                }, Client);
                return;
            }

            TcpClient targetUser = NetworkServerManager.Instance.FindUserClient(packet.Listener);

            if (targetUser == null)
            {
                PacketHandler.Send(MessageType.Chatting,
                    new ChattingPacket()
                    {
                        Name = "System",
                        Text = "상대를 찾지 못했습니다."
                    }, Client);
                return;
            }

            PacketHandler.Send(MessageType.Chatting,
                new ChattingPacket()
                {
                    Name = "System",
                    Text = $"To[{packet.Listener}]: {packet.Text}"
                },
                Client);

            PacketHandler.Send(MessageType.Chatting,
               new ChattingPacket()
               {
                   Name = packet.Sender,
                   Text = $" >> [귓속말] : {packet.Text}"
               },
               targetUser);
        }
        public void OnLogin(LoginPacket packet)
        {
            return;
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TCPNetwork.Interface.Packet;
using TCPNetwork.Packet.Chatting;

namespace TCPNetwork.Client
{
    // Client Version Class

    /*
     * 채팅 프로그램의 서버를 담당하는 클래스
     * Initialize후 ConnectToServer 를 이용해 서버와 연결
     */

    public partial class NetworkClientManager : IPacketHandleCallback
    {
        // Singleton
        private static NetworkClientManager instance = null;

        public static NetworkClientManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new NetworkClientManager();

                return instance;
            }
        }

        public delegate void OnButtonDown(object sender, EventArgs e);
        public delegate void OnClientExit(string text);

        public event EventHandler<string> OnChangeRoomEvent;
        public event EventHandler<List<string>> OnUpdateRoomListEvent;

        // Member
        private TcpClient clientSocket = null;           // 클라이언트 소켓
        private NetworkStream stream = default;        // 메시지 입출력 스트림

        private string message = string.Empty;   // 메시지
        private string userName = string.Empty;   // 유저 이름
        private string serverIP = string.Empty;   // 서버 IP
        private string roomName = string.Empty;   // 속해있는 방 이름

        List<string> networkRoomTitles = new List<string>();

        PacketHandler packetHandler;

        private int serverPort = 0;              // 서버 포트

        private OnClientExit onClientExit = null;           // 클라이언트를 종료시켜줄 콜백

        public bool IsConnection { get; private set; } = false;
        public int RoomListCount { get { return networkRoomTitles.Count; } }
        public ITextDraw TextDraw { get; set; } = null;

        public void Initialize(
            string userName,
            string serverIP,
            int serverPort,
            OnClientExit exitFunc)
        {
            this.userName = userName;
            this.serverPort = serverPort;
            this.serverIP = serverIP;
            onClientExit = exitFunc;
            IsConnection = false;

            clientSocket = new TcpClient();
            packetHandler = new PacketHandler()
            {
                FunctionHandler = this
            };
        }

        public bool ConnectToServer()
        {
            // 서버 연결 시도
            try
            {
                clientSocket.Connect(serverIP, serverPort);
                stream = clientSocket.GetStream();
            }
            catch (Exception)
            {
                // 서버 연결 실패시 클라이언트 종료
                ShowMessageBox("현재 서버가 실행중이 아니거나 입력값이 잘못되었습니다.\n" +
                    "포트, IP 주소를 다시 확인해보세요.", "Connection Failed");
                return false;
            }
            finally
            {

            }

            IsConnection = true;

            message = "서버에 연결되었습니다.";
            DrawText(message);

            // 서버에 유저 이름 전송
            PacketHandler.Send(MessageType.Login, new LoginPacket() { Name = userName }, clientSocket);

            // 메시지 처리 루프 비동기 실행
            Thread messageThread = new Thread(MessageHandling)
            {
                IsBackground = true
            };
            messageThread.Start();

            return true;
        }

        public void SendMessageToServer(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return;

            PacketHandler.Send(MessageType.Chatting,
                new ChattingPacket()
                {
                    Name = userName,
                    Text = message
                },
                clientSocket);
        }

        public void RoomListUpdateRequest()
        {
            PacketHandler.Send(MessageType.GetRoomList, new RoomListPacket(), clientSocket);
        }

        public void CreateRoomRequest(string roomName)
        {
            PacketHandler.Send(MessageType.CreateRoom,
                new CreateRoomPacket()
                {
                    RoomName = roomName
                },
            clientSocket);
        }

        public void DestroyRoomRequest(string roomName)
        {
            PacketHandler.Send(MessageType.DestroyRoom,
                new DestroyRoomPacket()
                {
                    RoomName = roomName
                },
            clientSocket);
        }

        public void JoinRoomRequest(string roomName)
        {
            PacketHandler.Send(MessageType.MoveToOtherRoom,
                   new MoveToOtherRoomPacket()
                   {
                       RoomName = roomName
                   },
               clientSocket);
        }

        public void WhisperRequest(string targetUserName, string text)
        {
            PacketHandler.Send(MessageType.Whisper,
                   new WhisperPacket()
                   {
                       Sender = userName,
                       Listener = targetUserName,
                       Text = text
                   },
               clientSocket);
        }

        private void MessageHandling()
        {
            try
            {
                while (true)
                {
                    // 예외 처리
                    if (clientSocket.Connected == false)
                    {
                        throw new IOException();
                    }

                    // 소켓에서 스트림을 얻어와서
                    stream = clientSocket.GetStream();

                    // 메시지 Read
                    int bufferSize = clientSocket.ReceiveBufferSize;
                    byte[] buffer = new byte[bufferSize];
                    int bytes = stream.Read(buffer, 0, buffer.Length);

                    packetHandler.Receive(buffer);
                }

            }
            catch (Exception)
            {
            }
            finally
            {
                ShowMessageBox("서버와의 연결이 끊겼습니다.", "Disconnected");
                onClientExit("서버와의 연결이 끊겼습니다.");

                IsConnection = false;
            }
        }

        public void OnChatting(ChattingPacket packet)
        {
            DrawText(packet.Text);
        }

        public void OnCreateRoom(CreateRoomPacket packet)
        {
            return;
        }

        public void OnDestroyRoom(DestroyRoomPacket packet)
        {
            return;
        }

        public void OnRoomCount(RoomCountPacket packet)
        {
            return;
        }

        public void OnRoomList(RoomListPacket packet)
        {
            networkRoomTitles.Clear();

            foreach (var Iter in packet.RoomNames)
            {
                networkRoomTitles.Add(Iter);
            }

            OnUpdateRoomListEvent?.Invoke(this, networkRoomTitles);
        }

        public void OnLeave(LeavePacket packet)
        {
            onClientExit("서버에서 종료 요청을 받았습니다.");
        }

        public void OnMoveToOtherRoom(MoveToOtherRoomPacket packet)
        {
            roomName = packet.RoomName;
            OnChangeRoomEvent?.Invoke(this, packet.RoomName);
        }

        public void OnWhisper(WhisperPacket packet)
        {
            return;
        }

        public void OnLogin(LoginPacket packet)
        {
            throw new NotImplementedException();
        }
    }
}

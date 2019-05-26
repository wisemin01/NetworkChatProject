using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading;


namespace TCPNetwork.Server
{
    /*
     * 채팅 프로그램의 서버를 담당하는 클래스
     * Initialize후 StartServer를 이용해 서버 시작
     */
    public partial class NetworkServerManager
    {
        // Singleton

        private static NetworkServerManager instance = null;

        public static NetworkServerManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new NetworkServerManager();

                return instance;
            }
        }

        public ITextDraw textDraw = null;
        public ITextDraw TextDraw
        {
            get => textDraw;
            set {
                textDraw = value;

                networkLobby.TextDraw = value;

                foreach (var Room in networkRooms)
                {
                    Room.Value.TextDraw = value;
                }
            }
        }
        public INetworkOutput   NetworkOutput   { get; set; } = null;

        // Member

        private TcpListener         tcpServer           = null; // 서버
        private TcpClient           clientAcceptSocket  = null; // 접속받기 위한 소켓

        private NetworkRoom         networkLobby        = null; // 처음 사용자가 접속할 방 (로비)

        private Dictionary<string, NetworkRoom> networkRooms = null; // 네트워크 룸 컨테이너

        /*
         * 초기화
         */
        public void Initialize(int serverPort)
        {
            if (serverPort < 0 || serverPort > 65535)
                throw new OverflowException("The port number is out of range");

            tcpServer           = new TcpListener(IPAddress.Any, serverPort);
            networkRooms        = new Dictionary<string, NetworkRoom>();
            clientAcceptSocket  = default;

            networkLobby        = new NetworkRoom("Lobby", TextDraw);
        }

        /*
         * 서버 시작
         */
        public void StartServer()
        {
            // 접속자를 받기 위한 스레드 생성
            Thread serverThread = new Thread(ServerLoop)
            {
                IsBackground = true
            };
            serverThread.Start();
        }

        /*
         * 메인 서버 루프입니다
         */
        private void ServerLoop()
        {
            if (tcpServer == null)
            {
                throw new Exception("tcpServer 가 null입니다.");
            }

            // 서버 시작
            tcpServer.Start();
            DrawText("[System] Server is started");

            if (NetworkOutput != null)
                NetworkOutput.AddRoomToListBox("Lobby");


            while (true)
            {
                try
                {
                    // 최대 버퍼 사이즈
                    const int bufferSize = 1024;

                    // 연결 요청 수락
                    clientAcceptSocket = tcpServer.AcceptTcpClient();

                    // 소켓 스트림에서 버퍼를 읽어와
                    NetworkStream   stream  = clientAcceptSocket.GetStream();
                    byte[]          buffer  = new byte[bufferSize];
                    int             bytes   = stream.Read(buffer, 0, buffer.Length);
                    
                    // 유저 이름으로 저장
                    string userName = Encoding.Unicode.GetString(buffer, 0, bytes);
                    userName        = userName.Substring(0, userName.IndexOf("$"));

                    // 로비에 추가
                    networkLobby.Add(clientAcceptSocket, userName);
                }
                catch (SocketException)
                {
                    break;
                }
                catch (Exception)
                {
                    break;
                }
            }

            clientAcceptSocket.Close();
            tcpServer.Stop();
        }

        /*
         * 유저를 다른 방으로 이동
         */
        public bool MoveToOtherRoom(HandleClient handleClient, NetworkRoom otherRoom)
        {
            if (otherRoom == null || handleClient == null)
                return false;

            // 현재 연결된 방으로부터 연결 해제
            string userName = handleClient.NetworkRoom.Remove(handleClient.Client);
            // 다른 방 연결
            handleClient.NetworkRoom = otherRoom;

            handleClient.NetworkRoom.AddFromOtherRoom(handleClient, userName);

            return true;
        }

        /*
         * 네트워크 룸을 찾아 반환
         */
        public NetworkRoom FindNetworkRoom(string roomName)
        {
            if (roomName == "Lobby")
                return networkLobby;
            else
            {
                NetworkRoom ret;

                if (networkRooms.TryGetValue(roomName, out ret))
                    return ret;

                else return null;
            }
        }

        /*
         * 네트워크 룸 생성
         */
        public NetworkRoom CreateNetworkRoom(string roomName)
        {
            if (networkRooms.ContainsKey(roomName))
            {
                return null;
            }

            NetworkRoom newRoom = new NetworkRoom(roomName, TextDraw);

            networkRooms.Add(roomName, newRoom);

            if (NetworkOutput != null)
                NetworkOutput.AddRoomToListBox(roomName);

            return newRoom;
        }

        /*
         * 네트워크 룸 파괴
         */
        public bool DestroyNetworkRoom(string roomName)
        {
            if (networkRooms.ContainsKey(roomName))
            {
                NetworkRoom room = networkRooms[roomName];

                networkRooms.Remove(roomName);

                if (NetworkOutput != null)
                    NetworkOutput.RemoveRoomToListBox(roomName);

                return true;
            }

            return false;
        }

        /* 
         * 모든 방을 순회하며 해당 유저를 찾습니다.
         * 비용 큼
         */
        public TcpClient FindUserClient(string userName)
        {
            foreach (var Iter in networkLobby.ClientList)
            {
                if (Iter.Value == userName)
                    return Iter.Key;
            }

            foreach (var Iter in networkRooms)
            {
                foreach (var Iter2 in Iter.Value.ClientList)
                {
                    if (Iter2.Value == userName)
                    {
                        return Iter2.Key;
                    }
                }
            }

            return null;
        }

        /*
         * 해당 방의 유저들을 반환
         */
        public List<Tuple<TcpClient, string>> GetUserClientList(string roomName)
        {
            if (roomName == null)
                return null;

            List<Tuple<TcpClient, string>> list = new List<Tuple<TcpClient, string>>();

            NetworkRoom networkRoom = NetworkServerManager.Instance.FindNetworkRoom(roomName);

            if (networkRoom == null)
                return null;

            foreach (var Iter in networkRoom.ClientList)
            {
                list.Add(new Tuple<TcpClient, string>(Iter.Key, Iter.Value));
            }

            return list;
        }
    }
}

using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Collections.Generic;

namespace TCPNetwork.Client
{
    // Client Version Class

    /*
     * 채팅 프로그램의 서버를 담당하는 클래스
     * Initialize후 ConnectToServer 를 이용해 서버와 연결
     */

    public partial class NetworkClientManager
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

        public event EventHandler<string>       OnChangeRoomEvent;
        public event EventHandler<List<string>> OnUpdateRoomListEvent;

        // Member
        private TcpClient       clientSocket  = null;           // 클라이언트 소켓
        private NetworkStream   stream        = default;        // 메시지 입출력 스트림
        
        private string          message       = string.Empty;   // 메시지
        private string          userName      = string.Empty;   // 유저 이름
        private string          serverIP      = string.Empty;   // 서버 IP
        private string          roomName      = string.Empty;   // 속해있는 방 이름

        List<string>            networkRoomTitles = new List<string>();

        private int             serverPort    = 0;              // 서버 포트

        private OnClientExit    onClientExit  = null;           // 클라이언트를 종료시켜줄 콜백

        public bool         IsConnection    { get; private set; }   = false;
        public ITextDraw    TextDraw        { get; set; }           = null;

        public void Initialize(
            string userName,
            string serverIP,
            int serverPort,
            OnClientExit exitFunc)
        {
            this.userName   = userName;
            this.serverPort = serverPort;
            this.serverIP   = serverIP;
            onClientExit    = exitFunc;
            IsConnection    = false;

            clientSocket    = new TcpClient();
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

            IsConnection = true;

            message = "서버에 연결되었습니다.";
            DrawText(message);

            // 서버에 유저 이름 전송
            byte[] buffer = Encoding.Unicode.GetBytes(userName + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();

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
            {
                return;
            }
            if (IsConnection)
            {
                if (clientSocket.Connected)
                {
                    NetworkStream stream = clientSocket.GetStream();
                    byte[] buffer = Encoding.Unicode.GetBytes(message + "$");

                    stream.Write(buffer, 0, buffer.Length);
                    stream.Flush();
                }
            }
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
                        IsConnection = false;
                        break;
                    }

                    // 소켓에서 스트림을 얻어와
                    stream = clientSocket.GetStream();

                    // 메시지 Read
                    int bufferSize  = clientSocket.ReceiveBufferSize;
                    byte[] buffer   = new byte[bufferSize];
                    int bytes       = stream.Read(buffer, 0, buffer.Length);

                    string message  = Encoding.Unicode.GetString(buffer, 0, bytes);

                    Console.WriteLine(message);

                    string[] messageArray = message.Split(new char[] { '$' });

                    OnMessage(messageArray, 0);
                }

                IsConnection = false;
            }
            catch (System.IO.IOException)
            {
                ShowMessageBox("서버와의 연결이 끊겼습니다.", "Disconnected");
                onClientExit("서버와의 연결이 끊겼습니다.");

                IsConnection = false;
            }
        }

        public List<string> GetNetworkRooms()
        {
            return networkRoomTitles;
        }

        private void OnMessage(string[] messageArray, int nowIndex = 0)
        {
            if (messageArray.Length == 0)
                return;

            if (nowIndex >= messageArray.Length)
                return;

            OnMessage(messageArray, nowIndex + 1);

            string message = messageArray[nowIndex];

            if (string.IsNullOrEmpty(message))
            {

            }
            // 명령어 해석 구문
            else if (message[0] == '/')
            {
                string[] result = message.Split(new char[] { '/' });

                MessageCommandType commandType = CommandPaser.Parse(result[1]);

                switch (commandType)
                {
                    case MessageCommandType.Leave:
                        onClientExit("서버에서 종료 요청을 받았습니다.");
                        break;

                    case MessageCommandType.ChangeRoom:
                        roomName = result[2];
                        OnChangeRoomEvent?.Invoke(this, result[2]);
                        break;

                    case MessageCommandType.ShowMessageBox:
                        ShowMessageBox(result[2], result[3]);
                        break;

                    case MessageCommandType.ReturnRoomList:
                        networkRoomTitles.Clear();

                        for (int i = 2; i < result.Length; i++)
                        {
                            networkRoomTitles.Add(result[i]);
                        }
                        OnUpdateRoomListEvent?.Invoke(this, networkRoomTitles);

                        break;

                    case MessageCommandType.None:
                        break;
                }
            }
            else
            {
                // 출력
                DrawText(message);
            }
        }
    }
}

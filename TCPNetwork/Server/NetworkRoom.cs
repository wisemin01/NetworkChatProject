using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading;

namespace TCPNetwork.Server
{
    public class NetworkRoom
    {
        private ITextDraw   textDraw    = null;         // 그래픽 출력 인터페이스
        private string      roomName    = string.Empty; // 방 이름

        private Dictionary<TcpClient, string> clientList = null; // 클라이언트 리스트

        public Dictionary<TcpClient, string> ClientList { get => clientList; }
        public string RoomName { get => roomName; }

        public NetworkRoom(string roomName, ITextDraw draw)
        {
            this.roomName   = roomName;
            textDraw        = draw;
            clientList      = new Dictionary<TcpClient, string>();
        }

        public void Add(TcpClient client, string userName)
        {
            clientList.Add(client, userName);

            DrawText(string.Format("[System] {0} join the room ({1})", userName, roomName));
            SendMessageToClients(string.Format("{0} 님이 {1}에 입장하셨습니다.", userName, roomName));

            // 해당 클라이언트와의 통신을 담당하는 객체 생성
            HandleClient handleClient = new HandleClient
            {
                OnReceived      = OnReceived,
                OnDisconnected  = OnDisconnected,
                OnMessageSended = SendMessageToClient
            };

            // 스레드로 관리
            handleClient.StartClientHandling(client, this);
        }

        public void AddFromOtherRoom(HandleClient handleClient, string userName)
        {
            clientList.Add(handleClient.Client, userName);

            handleClient.OnDisconnected = OnDisconnected;
            handleClient.OnReceived     = OnReceived;

            DrawText(string.Format("[System] {0} join the room ({1})", userName, roomName));
            SendMessageToClients(string.Format("{0} 님이 {1} 방에 입장하셨습니다.", userName, roomName));
        }

        public string Remove(TcpClient client)
        {
            string userName = clientList[client].ToString();

            OnDisconnected(client);

            return userName;
        }

        public void RoomClear()
        {
            foreach (var Iter in clientList)
            {
                DrawText(string.Format("[System] {0} left the room ({1})", Iter.Value, roomName));
                SendMessageToClients(string.Format("{0} 님이 {1} 방에서 퇴장하셨습니다.", Iter.Value, roomName));
                clientList.Remove(Iter.Key);
            }
        }

        // 연결이 해제되었을때 호출되는 함수
        private void OnDisconnected(TcpClient clientSocket)
        {
            // 컨테이너에서 검색해
            if (clientList.ContainsKey(clientSocket))
            {
                string userName = clientList[clientSocket].ToString();

                // 로그 출력 후 컨테이너에서 제거
                DrawText(string.Format("[System] {0} left the room ({1})", userName, roomName));
                SendMessageToClients(string.Format("{0} 님이 {1} 방에서 퇴장하셨습니다.", userName, roomName));
                clientList.Remove(clientSocket);
            }
        }

        /*
         * 클라이언트에게 받은 메시지 처리
        */
        private void OnReceived(string userName, string message, bool roomNameFlag = true)
        {
            string log = NetworkServerManager.GetDefaultMessageFormat(userName, message);

            if (roomNameFlag)
                DrawText(string.Format("[{0}]{1}", roomName, log));
            else
                DrawText(log);

            SendMessageToClients(log);
        }


        private void SendMessageToClients(string message)
        {
            /* 
             * 컨테이너에 저장된 클라이언트를 순회하며
             * 각 클라이언트에 메시지 전송
            */
            foreach (var pair in clientList)
            {
                TcpClient client = pair.Key as TcpClient;

                // 클라이언트 접속 상태 확인
                if (client.Connected == true)
                {
                    NetworkStream stream = client.GetStream();
                    byte[] buffer = null;

                    buffer = Encoding.Unicode.GetBytes(message);

                    // 해당 클라이언트에 버퍼 전송
                    stream.Write(buffer, 0, buffer.Length);
                    stream.Flush();
                }
            }
        }

        // 대상 클라이언트에게 메시지 전송
        private void SendMessageToClient(string message, TcpClient client)
        {
            if (client.Connected == true)
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = null;

                buffer = Encoding.Unicode.GetBytes(message);

                // 해당 클라이언트에 버퍼 전송
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
            }
        }

        private void DrawText(string text)
        {
            if (textDraw != null)
            {
                textDraw.DrawText(text);
            }
        }

    }
}

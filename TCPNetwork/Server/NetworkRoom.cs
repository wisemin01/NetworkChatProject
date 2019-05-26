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

        public Dictionary<TcpClient, string>    ClientList { get; } = null;
        public string                           RoomName   { get; } = string.Empty;
        public ITextDraw TextDraw { get => textDraw; set => textDraw = value; }

        public NetworkRoom(string roomName, ITextDraw draw)
        {
            this.RoomName   = roomName;
            textDraw        = draw;
            ClientList      = new Dictionary<TcpClient, string>();
        }

        public void Add(TcpClient client, string userName)
        {
            ClientList.Add(client, userName);

            DrawText(string.Format("[System] {0} join the room ({1})", userName, RoomName));
            SendMessageToClients(string.Format("{0} 님이 {1}에 입장하셨습니다.", userName, RoomName));

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

        // 스레드 분기는 이전의 방에서 했기 때문에
        // 리스트에 추가 후 콜백만 재설정
        public void AddFromOtherRoom(HandleClient handleClient, string userName)
        {
            ClientList.Add(handleClient.Client, userName);

            handleClient.OnDisconnected = OnDisconnected;
            handleClient.OnReceived     = OnReceived;

            DrawText(string.Format("[System] {0} join the room ({1})", userName, RoomName));
            SendMessageToClients(string.Format("{0} 님이 {1} 방에 입장하셨습니다.", userName, RoomName));
        }

        public string Remove(TcpClient client)
        {
            string userName = ClientList[client].ToString();

            OnDisconnected(client);

            return userName;
        }

        public void RoomClear()
        {
            foreach (var Iter in ClientList)
            {
                DrawText(string.Format("[System] {0} left the room ({1})", Iter.Value, RoomName));
                SendMessageToClients(string.Format("{0} 님이 {1} 방에서 퇴장하셨습니다.", Iter.Value, RoomName));
                ClientList.Remove(Iter.Key);
            }
        }

        // 연결이 해제되었을때 호출되는 함수
        private void OnDisconnected(TcpClient clientSocket)
        {
            // 컨테이너에서 검색해
            if (ClientList.ContainsKey(clientSocket))
            {
                string userName = ClientList[clientSocket].ToString();

                // 로그 출력 후 컨테이너에서 제거
                DrawText(string.Format("[System] {0} left the room ({1})", userName, RoomName));
                SendMessageToClients(string.Format("{0} 님이 {1} 방에서 퇴장하셨습니다.", userName, RoomName));
                ClientList.Remove(clientSocket);
            }
        }

        /*
         * 클라이언트에게 받은 메시지 처리
        */
        private void OnReceived(string userName, string message, bool roomNameFlag = true)
        {
            string log = NetworkServerManager.GetDefaultMessageFormat(userName, message);

            if (roomNameFlag)
                DrawText($"[{RoomName}]{log}");
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
            foreach (var pair in ClientList)
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

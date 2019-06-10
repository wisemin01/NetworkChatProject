using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

using TCPNetwork.Packet.Chatting;

namespace TCPNetwork.Server
{
    public class NetworkRoom
    {
        public Dictionary<TcpClient, string> ClientList { get; } = null;
        public string RoomName { get; } = string.Empty;
        public ITextDraw TextDraw { get; set; } = null;

        public NetworkRoom(string roomName, ITextDraw draw)
        {
            RoomName        = roomName;
            TextDraw        = draw;

            ClientList      = new Dictionary<TcpClient, string>();
            
        }

        public void Add(TcpClient client, string userName)
        {
            ClientList.Add(client, userName);

            DrawText($"[System] {userName} join the room ({RoomName})");
            SendMessageToClients($"{userName} 님이 {RoomName} 방에 입장하셨습니다.");

            // 해당 클라이언트와의 통신을 담당하는 객체 생성
            HandleClient handleClient = new HandleClient
            {
                OnReceived = OnReceived,
                OnDisconnected = OnDisconnected,
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
            handleClient.OnReceived = OnReceived;

            DrawText($"[System] {userName} join the room ({RoomName})");
            SendMessageToClients($"{userName} 님이 {RoomName} 방에 입장하셨습니다.");
        }

        public string Remove(TcpClient client)
        {
            if (ClientList.ContainsKey(client))
            {
                string userName = ClientList[client].ToString();

                OnDisconnected(client);

                return userName;
            }
            return string.Empty;

        }

        public void RoomClear()
        {
            foreach (KeyValuePair<TcpClient, string> Iter in ClientList)
            {
                DrawText($"[System] {Iter.Value} left the room ({RoomName})");
                SendMessageToClients($"{Iter.Value} 님이 {RoomName} 방에서 퇴장하셨습니다.");
                ClientList.Remove(Iter.Key);
            }
        }

        // 연결이 해제되었을때 호출되는 함수
        private void OnDisconnected(TcpClient clientSocket)
        {
            string userName = ClientList[clientSocket].ToString();

            // 로그 출력 후 컨테이너에서 제거
            DrawText($"[System] {userName} left the room ({RoomName})");
            SendMessageToClients($"{userName} 님이 {RoomName} 방에서 퇴장하셨습니다.");
            ClientList.Remove(clientSocket);
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
                    PacketHandler.Send(MessageType.Chatting,
                        new ChattingPacket()
                        {
                            Name = string.Empty,
                            Text = message
                        }, client);
                }
            }
        }

        // 대상 클라이언트에게 메시지 전송
        private void SendMessageToClient(string sender, string message, TcpClient client)
        {
            if (client.Connected == true)
            {
                PacketHandler.Send(MessageType.Chatting,
                        new ChattingPacket()
                        {
                            Name = sender,
                            Text = message
                        }, client);
            }
        }

        private void DrawText(string text)
        {
            if (TextDraw != null)
            {
                TextDraw.DrawText(text);
            }
        }
    }
}

using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TCPNetwork.Server
{
    /*
     * 클라이언트의 연결 요청 수락 후
     * 해당 클라이언트를 담당해 통신하는 클래스
     */

    public class HandleClient
    {
        public delegate void MessageDisplayHandler(string userName, string message, bool flag);
        public delegate void MessageDispatcher(string message, TcpClient client);
        public delegate void DisconnectedHandler(TcpClient client);

        public TcpClient Client { get; private set; } = null;
        public NetworkRoom NetworkRoom { get; set; } = null;

        public MessageDisplayHandler OnReceived { get; set; } = null;
        public DisconnectedHandler OnDisconnected { get; set; } = null;
        public MessageDispatcher OnMessageSended { get; set; } = null;

        public void StartClientHandling(TcpClient client, NetworkRoom room)
        {
            // 맴버 초기화
            Client = client;
            NetworkRoom = room;

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
                    message = Encoding.Unicode.GetString(buffer, 0, bytes);
                    message = message.Substring(0, message.IndexOf("$"));

                    Console.WriteLine("READ << " + message);

                    if (message[0] == '/')
                    {
                        string[] result = message.Split(new char[] { '/' });

                        MessageCommandType commandType = CommandPaser.Parse(result[1]);

                        if (commandType != MessageCommandType.None)
                        {
                            CommandSwitch(commandType, result);
                        }
                    }
                    else
                    {
                        // OnReceived 함수에 전달
                        OnReceived?.Invoke(NetworkRoom.ClientList[Client].ToString(), message, true);
                    }
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

        private void CommandSwitch(MessageCommandType type, string[] splitResult)
        {
            switch (type)
            {
                case MessageCommandType.Leave:
                    {
                        OnMessageSended?.Invoke("/Leave", Client);
                    }
                    break;

                case MessageCommandType.MoveToOtherRoom:
                    {
                        if (splitResult[2] == NetworkRoom.RoomName)
                            break;

                        bool result = NetworkServerManager.Instance.MoveToOtherRoom(this,
                            NetworkServerManager.Instance.FindNetworkRoom(splitResult[2]));

                        if (result == false)
                        {
                            OnMessageSended?.Invoke("지정된 방을 찾을 수 없습니다.", Client);
                        }
                        else
                        {
                            OnMessageSended?.Invoke($"/ChangeRoom/{splitResult[2]}", Client);
                        }
                    }
                    break;

                case MessageCommandType.CreateRoom:
                    {
                        NetworkRoom room = NetworkServerManager.Instance.CreateNetworkRoom(splitResult[2]);

                        if (room != null)
                        {
                            OnMessageSended?.Invoke("[System] : 방을 생성했습니다.", Client);
                            OnReceived("System", $"{splitResult[2]} 방이 생성되었습니다.", false);
                        }
                        else
                        {
                            OnMessageSended?.Invoke("[System] : 방 생성에 실패했습니다.", Client);
                        }

                    }
                    break;

                case MessageCommandType.DestroyRoom:
                    {
                        NetworkRoom room = NetworkServerManager.Instance.FindNetworkRoom(splitResult[2]);

                        if (room == null)
                            break;

                        if (NetworkRoom.RoomName == splitResult[2])
                        {
                            OnMessageSended?.Invoke("[System] : 자신이 속한 방은 파괴할 수 없습니다.", Client);
                            break;
                        }

                        if (room.ClientList.Count != 0)
                        {
                            OnMessageSended?.Invoke("[System] : 플레이어가 접속 중인 방은 파괴할 수 없습니다.", Client);
                            break;
                        }

                        bool result = NetworkServerManager.Instance.DestroyNetworkRoom(splitResult[2]);

                        string log = $"[System] : {splitResult[2]} 방을 {(result ? "파괴했습니다." : "파괴하지 못했습니다.")}";

                        OnMessageSended?.Invoke(log, Client);
                        NetworkServerManager.Instance.DrawText($"[System] : {splitResult[2]} 방이 파괴되었습니다.");
                    }
                    break;

                case MessageCommandType.Whisper:
                    {
                        TcpClient targetUser = NetworkServerManager.Instance.FindUserClient(splitResult[2]);

                        if (targetUser == Client)
                        {
                            OnMessageSended?.Invoke("자기 자신에게는 귓속말을 보낼 수 없습니다.", Client);
                            break;
                        }
                        if (targetUser != null)
                        {
                            OnMessageSended?.Invoke(
                               string.Format("To [{0}]: {1}",
                               splitResult[2], splitResult[3]
                               ), Client);
                            OnMessageSended?.Invoke(
                                string.Format("[{0}][{1}] <귓속말> : {2}",
                                NetworkRoom.RoomName, NetworkRoom.ClientList[Client].ToString(), splitResult[3]
                                ), targetUser);
                        }
                        else
                        {
                            OnMessageSended?.Invoke("귓속말을 전할 상대를 찾지 못했습니다.", Client);
                        }
                    }
                    break;
                case MessageCommandType.ReturnRoomList:
                    {
                        StringBuilder stringBuilder = new StringBuilder(64);
                        stringBuilder.Append("/ReturnRoomList");

                        foreach (string Iter in NetworkServerManager.Instance.GetNetworkRoomKeys())
                        {
                            stringBuilder.Append('/' + Iter);
                        }

                        OnMessageSended?.Invoke(stringBuilder.ToString(), Client);
                    }
                    break;
            }
        }
    }
}

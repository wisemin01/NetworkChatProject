using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
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

        private MessageDisplayHandler onReceived = null;
        private DisconnectedHandler onDisconnected = null;
        private MessageDispatcher onSendMessage = null;

        private NetworkRoom networkRoom = null;
        private TcpClient client = null;

        public TcpClient Client { get => client; }
        public NetworkRoom NetworkRoom { get => networkRoom; set => networkRoom = value; }

        public MessageDisplayHandler OnReceived
        {
            get => onReceived; set => onReceived = value;
        }
        public DisconnectedHandler OnDisconnected
        {
            get => onDisconnected; set => onDisconnected = value;
        }
        public MessageDispatcher OnMessageSended
        {
            get => onSendMessage; set => onSendMessage = value;
        }

        public void StartClientHandling(TcpClient client, NetworkRoom room)
        {
            // 맴버 초기화
            this.client = client;
            networkRoom = room;

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
                    if (client.Connected == false)
                        break;

                    stream = client.GetStream();

                    // 메시지를 읽어온다
                    bytes   = stream.Read(buffer, 0, buffer.Length);
                    message = Encoding.Unicode.GetString(buffer, 0, bytes);
                    message = message.Substring(0, message.IndexOf("$"));

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
                        OnReceived?.Invoke(networkRoom.ClientList[client].ToString(), message, true);
                    }
                }
            }
            // 예외 발생시 해당 소켓과 스레드 Close
            catch (SocketException)
            {
                if (client != null)
                {
                    OnDisconnected?.Invoke(client);
                    client.Close();
                    stream.Close();
                }
            }
            catch (ArgumentOutOfRangeException argumentException)
            {
                NetworkServerManager.Instance.ShowMessageBox(argumentException.Message, "Error");
            }
            catch (Exception)
            {
                if (client != null)
                {
                    OnDisconnected?.Invoke(client);
                    client.Close();
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
                        OnMessageSended?.Invoke("/Leave", client);
                    }
                    break;

                case MessageCommandType.MoveToOtherRoom:
                    {
                        bool result = NetworkServerManager.Instance.MoveToOtherRoom(this,
                            NetworkServerManager.Instance.FindNetworkRoom(splitResult[2]));

                        if (result == false)
                        {
                            OnMessageSended?.Invoke("지정된 방을 찾을 수 없습니다.", client);
                        }
                    }
                    break;

                case MessageCommandType.CreateRoom:
                    {
                        NetworkRoom room = NetworkServerManager.Instance.CreateNetworkRoom(splitResult[2]);

                        if (room != null)
                            OnMessageSended?.Invoke("[System] 방을 생성했습니다.", client);
                        else
                            OnMessageSended?.Invoke("[System] 방 생성에 실패했습니다.", client);
                    }
                    break;

                case MessageCommandType.DestroyRoom:
                    {
                        NetworkRoom room = NetworkServerManager.Instance.FindNetworkRoom(splitResult[2]);

                        if (networkRoom.RoomName == splitResult[2])
                        {
                            OnReceived?.Invoke("System", "자신이 속한 방은 파괴할 수 없습니다.", false);
                            break;
                        }

                        if (room.ClientList.Count != 0)
                        {
                            OnReceived?.Invoke("System", "플레이어가 접속 중인 방은 파괴할 수 없습니다.", false);
                            break;
                        }

                        bool result = NetworkServerManager.Instance.DestroyNetworkRoom(splitResult[2]);

                        OnReceived?.Invoke("System", string.Format("{0} 방을 {1}", splitResult[2],
                            result ? "파괴했습니다." : "파괴하지 못했습니다."), false);
                    }
                    break;

                case MessageCommandType.Whisper:
                    {
                        TcpClient targetUser = NetworkServerManager.Instance.FindUserClient(splitResult[2]);
                        if (targetUser != null)
                        {
                            OnMessageSended?.Invoke(
                               string.Format("To [{0}]: {1}",
                               splitResult[2], splitResult[3]
                               ), client);
                            OnMessageSended?.Invoke(
                                string.Format("[{0}][{1}] <귓속말> : {2}",
                                networkRoom.RoomName, networkRoom.ClientList[client].ToString(), splitResult[3]
                                ), targetUser);
                        }
                        else
                        {
                            OnMessageSended?.Invoke("귓속말을 전할 상대를 찾지 못했습니다.", client);
                        }
                    }
                    break;

            }
        }
    }
}

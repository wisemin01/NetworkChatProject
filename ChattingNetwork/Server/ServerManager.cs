using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MNetwork.Debuging;
using MNetwork.Time;
using MNetwork.Engine;

namespace ChattingNetwork.Server
{
    public class ServerManager
    {
        private static ServerManager instance = null;

        public static ServerManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ServerManager();

                return instance;
            }
        }

        private readonly ChattingLogic logic = new ChattingLogic();
        private readonly ChattingPacketTranslater translater = new ChattingPacketTranslater();

        public MNetwork.Rooms.MNetworkLobby Lobby { get => logic.NetworkLobby; }

        public ServerManager()
        {
            Debug.LogPath = $"./Log/Server[{Time.TimeLogYMD}].log";
        }

        ~ServerManager()
        {
            Debug.Flush();
        }

        public bool Start(string IP, ushort port)
        {
            if (string.IsNullOrWhiteSpace(IP))
            {
                Debug.WarningLog("IP 입력이 비어 있습니다.");
                return false;
            }

            try
            {
                MEngine.Instance.Intialize(logic, translater);
                MEngine.Instance.Start(IP, port);

                return true;
            }
            catch (FormatException)
            {
                Debug.WarningLog("입력 형식이 맞지 않습니다.");
            }
            catch (OverflowException e)
            {
                Debug.WarningLog($"최대 입력 범위를 벗어났습니다.\n Error Message : {e.Message}");
            }

            return false;
        }

        public void Stop()
        {
            MEngine.Instance.ShutDown();
        }

        public delegate void LogCallback(object sender, string e);

        public void AddDebuger(LogCallback debuger)
        {
            Debug.OnLog += delegate (object sender, string e) { debuger(sender, e); };
        }
    }
}

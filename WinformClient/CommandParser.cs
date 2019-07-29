using ChattingPacket;
using MNetwork.Engine;
using MNetwork.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChattingNetwork.Client;

namespace ClientHost
{
    public class CommandParser
    {
        public static bool Parse(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
                return false;

            if (command[0] != '/')
                return false;

            string[] strArr = command.Split(new char[] { '/' });

            if (strArr.Length <= 1)
                return false;

            switch (strArr[1])
            {
                case "SignUp":
                    {
                        ClientManager.Instance.SignUp(strArr[2], strArr[3], strArr[4]);
                        return true;
                    }
                case "SignIn":
                    {
                        ClientManager.Instance.SignIn(strArr[2], strArr[3]);
                        return true;
                    }
                case "CreateRoom":
                    {
                        ClientManager.Instance.CreateRoom(strArr[2]);
                        return true;
                    }
                case "Join":
                    {
                        ClientManager.Instance.JoinRoom(strArr[2]);
                        return true;
                    }
                case "Whisper":
                    {
                        ClientManager.Instance.Whisper(strArr[2], strArr[3]);
                        return true;
                    }
                case "r":
                    {
                        ClientManager.Instance.Whisper(strArr[2], strArr[3]);
                        return true;
                    }
            }

            return false;
        }
    }
}

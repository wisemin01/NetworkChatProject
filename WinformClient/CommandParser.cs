using ChattingPacket;
using MNetwork.Engine;
using MNetwork.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientHost
{
    public class CommandParser
    {
        public static bool Parse(string command)
        {
            if (command[0] != '/')
                return false;

            string[] strArr = command.Split(new char[] { '/' });

            if (strArr.Length <= 1)
                return false;

            switch (strArr[1])
            {
                case "SignUp":
                    {
                        SignUpRequestPacket packet = new SignUpRequestPacket()
                        {
                            ID = strArr[2],
                            Password = strArr[3],
                            UserName = strArr[4]
                        };

                        MNetworkEntry.Instance.Send(new ProtobufPacket<SignUpRequestPacket>(0, PacketEnum.ProcessType.Data,
                            (int)MessageType.SignUpRequest, packet));
                        return true;
                    }
                case "SignIn":
                    {
                        LoginRequestPacket packet = new LoginRequestPacket()
                        {
                            ID = strArr[2],
                            Password = strArr[3]
                        };

                        MNetworkEntry.Instance.Send(new ProtobufPacket<LoginRequestPacket>(0, PacketEnum.ProcessType.Data,
                            (int)MessageType.LoginRequest, packet));
                        return true;
                    }
                case "CreateRoom":
                    return true;
                case "DestroyRoom":
                    return true;
                case "Join":
                    return true;
                case "Whisper":
                    return true;
                case "r":
                    return true;
            }

            return false;
        }
    }
}

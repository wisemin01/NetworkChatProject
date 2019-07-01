using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientHost.WinForm
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

            //switch (strArr[1])
            //{
            //    case "CreateRoom":
            //        ServerForm.Client.NetworkClientManager.Instance.CreateRoomRequest(strArr[2]);
            //        return true;
            //    case "DestroyRoom":
            //        ServerForm.Client.NetworkClientManager.Instance.DestroyRoomRequest(strArr[2]);
            //        return true;
            //    case "Join":
            //        ServerForm.Client.NetworkClientManager.Instance.JoinRoomRequest(strArr[2]);
            //        return true;
            //    case "Whisper":
            //        ServerForm.Client.NetworkClientManager.Instance.WhisperRequest(strArr[2], strArr[3]);
            //        return true;
            //    case "r":
            //        ServerForm.Client.NetworkClientManager.Instance.WhisperRequest(strArr[2], strArr[3]);
            //        return true;
            //}

            return false;
        }
    }
}

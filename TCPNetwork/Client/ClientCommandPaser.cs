using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPNetwork.Client
{
    public enum MessageCommandType
    {
        None,
        Leave,
        ChangeRoom,
        ReturnRoomList,
        ShowMessageBox
    }

    class CommandPaser
    {
        static public MessageCommandType Parse(string message)
        {
            switch (message)
            {
                case "Leave":
                    return MessageCommandType.Leave;
                case "ChangeRoom":
                    return MessageCommandType.ChangeRoom;
                case "ReturnRoomList":
                    return MessageCommandType.ReturnRoomList;
                case "ShowMessageBox":
                    return MessageCommandType.ShowMessageBox;
                default:
                    return MessageCommandType.None;
            }
        }
    }
}

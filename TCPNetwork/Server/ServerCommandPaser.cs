using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPNetwork.Server
{
    public enum MessageCommandType
    {
        None,
        Leave,
        MoveToOtherRoom,
        CreateRoom,
        DestroyRoom,
        Whisper
    }

    class CommandPaser
    {
        static public MessageCommandType Parse(string message)
        {
            switch (message)
            {
                case "Leave":
                    return MessageCommandType.Leave;
                case "Exit":
                    return MessageCommandType.Leave;
                case "MoveToOtherRoom":
                    return MessageCommandType.MoveToOtherRoom;
                case "Join":
                    return MessageCommandType.MoveToOtherRoom;
                case "CreateRoom":
                    return MessageCommandType.CreateRoom;
                case "DestroyRoom":
                    return MessageCommandType.DestroyRoom;
                case "Whisper":
                    return MessageCommandType.Whisper;
                case "r":
                    return MessageCommandType.Whisper;
                default:
                    return MessageCommandType.None;
            }
        }
    }
}
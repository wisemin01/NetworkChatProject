namespace TCPNetwork.Server
{
    public enum MessageCommandType
    {
        None,
        Leave,
        MoveToOtherRoom,
        CreateRoom,
        DestroyRoom,
        Whisper,
        ReturnRoomList
    }

    class CommandParser
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
                case "ReturnRoomList":
                    return MessageCommandType.ReturnRoomList;
                case "GetRoomList":
                    return MessageCommandType.ReturnRoomList;
                default:
                    return MessageCommandType.None;
            }
        }
    }
}
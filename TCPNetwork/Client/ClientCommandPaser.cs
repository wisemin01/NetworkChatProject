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
        Leave
    }

    class CommandPaser
    {
        static public MessageCommandType Parse(string message)
        {
            switch (message)
            {
                case "Leave":
                    return MessageCommandType.Leave;
                default:
                    return MessageCommandType.None;
            }
        }
    }
}

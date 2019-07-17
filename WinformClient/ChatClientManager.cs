using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MNetwork.Rooms;

namespace ClientHost
{
    using PlayerState = MNetworkPlayer.MPlayerState;

    public static class ChatClientManager
    {
        public static string        userName    = string.Empty;
        public static PlayerState   userState   = PlayerState.None;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MNetwork.Rooms;

namespace ChattingNetwork.Client
{
    using PlayerState = MNetworkPlayer.MPlayerState;

    internal static class UserInfoManager
    {
        public static string userName = string.Empty;
        public static PlayerState userState = PlayerState.None;
    }
}

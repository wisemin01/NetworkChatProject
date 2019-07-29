using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingNetwork.Client
{
    public partial class ClientManager
    {
        public event EventHandler<bool> OnSignIn
        {
            add => callback.OnSignIn += value;
            remove => callback.OnSignIn -= value;
        }
        public event EventHandler<bool> OnSignUp
        {
            add => callback.OnSignUp += value;
            remove => callback.OnSignUp -= value;
        }
        public event EventHandler<string> OnChatting
        {
            add => callback.OnChatting += value;
            remove => callback.OnChatting -= value;
        }
        public event EventHandler<bool> OnJoinRoom
        {
            add => callback.OnJoinRoom += value;
            remove => callback.OnJoinRoom -= value;
        }
        public event EventHandler<bool> OnExitRoom
        {
            add => callback.OnExitRoom += value;
            remove => callback.OnExitRoom -= value;
        }
        public event EventHandler<bool> OnCreateRoom
        {
            add => callback.OnCreateRoom += value;
            remove => callback.OnCreateRoom -= value;
        }
        public event EventHandler<List<string>> OnRoomListRefresh
        {
            add => callback.OnRoomListRefresh += value;
            remove => callback.OnRoomListRefresh -= value;
        }
        public event EventHandler<string> OnWhisper
        {
            add => callback.OnWhisper += value;
            remove => callback.OnWhisper -= value;
        }
    }
}

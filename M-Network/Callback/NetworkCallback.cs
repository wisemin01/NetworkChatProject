using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MNetwork.Packet;

namespace MNetwork.Callback
{
    public abstract class NetworkCallback
    {
        public bool IsConnected { get; set; } = false;

        /// <summary>
        /// 메시지를 처리합니다.<para/>
        /// ProcessType.RPC 타입은 HandleRPC 함수에서 처리하기 때문에 여기서 처리하지 않습니다.
        /// </summary>
        /// <param name="packet"> 처리할 패킷입니다.</param>
        /// <returns> 메시지 처리에 성공했는지를 반환합니다. </returns>
        public abstract bool HandleNetworkMessage(BasePacket packet);

        /// <summary>
        /// RPC 메시지를 처리합니다.
        /// </summary>
        /// <param name="packet"> 처리할 패킷입니다. </param>
        /// <returns> RPC 처리에 성공했는지를 반환합니다. </returns>
        public virtual bool HandleRPC(BasePacket packet) { return false; }
        public virtual void HandleConnect()
        {
            IsConnected = true;
        }
        public virtual void HandleDisconnect()
        {
            IsConnected = false;
        }
    }
}

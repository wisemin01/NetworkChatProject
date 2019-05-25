using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPNetwork
{
    /*
     * 네트워크 상의 출력 담당 인터페이스
     */
    public interface INetworkDebugger
    {
        void AddRoomToListBox(string roomName);
        void RemoveRoomToListBox(string roomName);
    }
}

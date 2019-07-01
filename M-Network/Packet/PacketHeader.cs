using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNetwork.Packet
{
    public class PacketEnum
    {
        [Flags]
        public enum ProcessType : System.Int32
        {
            None = 0x00000001,
            Connect = 0x00000002,
            Disconnect = 0x00000004,
            Data = 0x00000008,
            RPC = 0x00000010,
            Timer = 0x00000020,
            ServerShutdown = 0x00000040
        }
    }

    public class PacketHeader
    {
        public int packetSize;  // 총 패킷의 사이즈
        public int packetType;  // 데이터 패킷의 종류 구별

        public PacketEnum.ProcessType processType; // 패킷 동작 타입

        // buffer       : 받은 버퍼
        // protoBuffer  : Google Protobuf 를 이용해서 파싱할 데이터 ( out )
        public static PacketHeader ParseFrom(byte[] buffer)
        {
            PacketHeader header = new PacketHeader
            {
                packetSize = BitConverter.ToInt32(buffer, sizeof(int) * 0),
                packetType = BitConverter.ToInt32(buffer, sizeof(int) * 1),
                processType = (PacketEnum.ProcessType)BitConverter.ToInt32(buffer, sizeof(int) * 2),
            };

            return header;
        }
    }
}

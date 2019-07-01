using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Reflection;

using Google.Protobuf;

namespace MNetwork.Packet
{
    public class ProtobufPacketImpl : BasePacket
    {
        protected IMessage message;

        public ProtobufPacketImpl(byte[] buffer)
        {
            Buffer = buffer;
        }

        public ProtobufPacketImpl(int serial, PacketEnum.ProcessType processType, int type = -1) : base(serial, processType, type)
        {
        }

        public override BasePacket Clone()
        {
            return base.Clone();
        }

        public override bool Decode(byte[] buffer)
        {
            if (buffer == null)
                return false;

            try
            {
                PacketHeader header = PacketHeader.ParseFrom(buffer);

                int packetSize  = header.packetSize;
                Type            = header.packetType;
                ProcessType     = header.processType;

                byte[] messageBuf = new byte[packetSize];
                System.Buffer.BlockCopy(buffer, sizeof(int) * 3, messageBuf, 0, packetSize);

                message = message.Descriptor.Parser.ParseFrom(messageBuf);
            }
            catch (Exception e)
            {
                Debuging.Debug.ErrorLog(MethodBase.GetCurrentMethod(), e.Message);
                return false;
            }

            return true;
        }

        public override bool Encode()
        {
            try
            {
                int packetSize = message.CalculateSize();
                Buffer = new byte[sizeof(int) * 3 + packetSize];

                BitConverter.GetBytes((int)packetSize).CopyTo(Buffer, sizeof(int) * 0);
                BitConverter.GetBytes((int)Type).CopyTo(Buffer, sizeof(int) * 1);
                BitConverter.GetBytes((int)ProcessType).CopyTo(Buffer, sizeof(int) * 2);

                message.ToByteArray().CopyTo(Buffer, sizeof(int) * 3);
            }
            catch(Exception)
            {
                return false;
            }

            return true;
        }
    }
}

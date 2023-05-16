using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Net.IO
{
    class PacketReader : BinaryReader
    {
        private NetworkStream _ns;

        public PacketReader(NetworkStream ns) : base(ns)        //GETS PASSED INTO THE BASE CLASS
        {
            _ns = ns;
        }

        public string ReadMessage()
        {
            byte[] messageBuffer;

            var length = ReadInt32();

            messageBuffer = new byte[length];

            _ns.Read(messageBuffer, 0, length);

            var msg = Encoding.ASCII.GetString(messageBuffer);

            return msg;
        }
    }
}
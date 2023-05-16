using System;
using ChatServer.Net.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using ChatServer;

namespace ClientServer
{
    class Client
    {
        public string Username { get; set;}
        public string IPHost { get; set; }

        public Guid UID { get; set; }

        public TcpClient ClientSocket { get; set; }

        PacketReader _packetReader;
        

        public Client(TcpClient client)
        {
           
            ClientSocket = client;
            UID = Guid.NewGuid();
            _packetReader = new PacketReader(ClientSocket.GetStream());

            var opcode = _packetReader.ReadByte();                              // we need to run validations on the op code, if false we need to drop the connection

            Username = _packetReader.ReadMessage();

            //IPHost = _packetReader.ReadMessage();

            Console.WriteLine($"[{DateTime.Now}]:A user has joined as:{Username}");

            Task.Run(() => Process());
        }

        void Process()
        {
            while (true)
            {
                try
                {
                    var opcode = _packetReader.ReadByte();
                    switch(opcode)
                    {
                        case 5:
                            var msg = _packetReader.ReadMessage();
                            Console.WriteLine($"[{DateTime.Now}]: Message received! {msg}");
                            Program.BroadcastMessage($"[{DateTime.Now}]: [{Username}]: {msg}");
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"[{UID.ToString()}]: Disconnected!");
                    Program.BroadcastDisconnect(UID.ToString());
                    ClientSocket.Close();
                    break;
                }
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using ClientServer.Net.IO;
using ChatServer.Net.IO;
using System.Threading.Tasks;
using System.Net;
using Texting_Program;

namespace Client.Net
{
    class Server
    {
        TcpClient _client;
    
        public PacketReader PacketReader;

        public event Action connectedEvent;
        public event Action msgReceivedEvent;
        public event Action userDisconnectedEvent;

        public Server()
        {
            _client = new TcpClient();
        }
        public static string ip;



        public void ConnectToServer(string username, string IPHost)
        {
           
            String strHostName = string.Empty;
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] addr = ipEntry.AddressList;

            for (int i = 1; i < addr.Length; i++)
            {
                 addr[i].ToString();
            }


            if (!_client.Connected)
            {
                _client.Connect((IPHost), 1434); // (create our client)

                PacketReader = new PacketReader(_client.GetStream());

                if (!string.IsNullOrEmpty(username))
                {
                    var connectPacket = new PacketBuilder();

                    connectPacket.WriteOpCode(0);

                    connectPacket.WriteMessage(username);

                    _client.Client.Send(connectPacket.GetPacketBytes());
                }

                 ReadPackets();
              /*var connectPacket = new PacketBuilder();

                connectPacket.WriteOpCode(0);

                connectPacket.WriteMessage(username);

                _client.Client.Send(connectPacket.GetPacketBytes()); */
            }
        }


        private void ReadPackets()
        {
            Task.Run(() =>
            {
                while (true) 
                {
                    var opcode = PacketReader.ReadByte();
                    switch (opcode)
                    {//we wont use 0 for our cases since it is already being handled
                        case 1:
                            connectedEvent?.Invoke();
                        break;

                        case 5:
                            msgReceivedEvent?.Invoke();
                            break;

                        case 10:
                            userDisconnectedEvent?.Invoke();
                            break;

                        default:
                            Console.WriteLine();
                            break;
                    }
                }
            });
        }

        public void SendMessageToServer(string message)
        {
            var messagePacket = new PacketBuilder();
            messagePacket.WriteOpCode(5);
            messagePacket.WriteMessage(message);
            _client.Client.Send(messagePacket.GetPacketBytes());
        }

    }
}

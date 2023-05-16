using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using ClientServer;
using ClientServer.Net.IO;
using System.Linq;
//using ChatServer.Net.;
using ClientServer;




namespace ChatServer
{
    class Program
    {
        static List<Client> _users;

        static TcpListener _listener;

        public static string ip;
        

        static void Main(string[] args)
        {
            String strHostName = string.Empty;
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] addr = ipEntry.AddressList;

            for (int i = 1; i < addr.Length; i++)
            {
               ip = addr[i].ToString();
            }

            _users = new List<Client>();
            _listener = new TcpListener(IPAddress.Parse(ip), 1434);// IP adress
            _listener.Start();


            while (true)
            {
                var client = new Client(_listener.AcceptTcpClient());         //returns a tcpClient, that we are using as the parameter.
                _users.Add(client);                                           // The connection will be anounced to everyone on the server.

                BroadcastConnection();           // coded the on connect broadcast method below
            }


            /* String strHostName = string.Empty;
         IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
         IPAddress[] addr = ipEntry.AddressList;

         for (int i = 1; i < addr.Length; i++)
         {
             addr[i].ToString();
         }*/


            // _users.Add();
            //var client = _listener.AcceptTcpClient();
            //Console.WriteLine("User has joined.");
        }

        static void BroadcastConnection() 
        {
            foreach(var user in _users) 
            {
                foreach(var usr in _users) 
                {
                    var broadcastPacket = new PacketBuilder();
                    broadcastPacket.WriteOpCode(1);                    // the one inbetween the brackets is to differentiate between packets
                    broadcastPacket.WriteMessage(usr.Username);         // had to fix writeString into WriteMessage.
                    broadcastPacket.WriteMessage(usr.UID.ToString());
                    user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
                    

                }
            }
        }

        public static void BroadcastMessage(string message)
        {
            foreach( var user in _users)
            {
                var msgPacket = new PacketBuilder();
                msgPacket.WriteOpCode(5);
                msgPacket.WriteMessage(message);
                user.ClientSocket.Client.Send(msgPacket.GetPacketBytes());
            }
        }

        public static void BroadcastDisconnect(string uid)
        {
            var disconnectedUser = _users.Where(x => x.UID.ToString() == uid).FirstOrDefault();
            _users.Remove(disconnectedUser);

            foreach (var user in _users)
            {
                var broadcastPacket = new PacketBuilder();
                broadcastPacket.WriteOpCode(10);
                broadcastPacket.WriteMessage(uid);
                user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
            }
            BroadcastMessage($"[{disconnectedUser.Username}] Disconnected!");
        }
    }
}

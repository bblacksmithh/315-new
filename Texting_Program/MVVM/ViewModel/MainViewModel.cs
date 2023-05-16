using Client.MVVM.Core;
using ClientServer.MVVM.Model;
using Client.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;


namespace Client.MVVM.ViewModel
{
    class MainViewModel
    {
        public ObservableCollection<UserModel> Users { get; set; }
        public ObservableCollection<string> Messages { get; set; }

        public ObservableCollection<string> Host { get; set; }

        public RelayCommand ConnectToServerCommand { get; set; }

        public RelayCommand ConnectToHost { get; set; }

        public RelayCommand SendMessageCommand { get; set; }

        public string Username { get; set; }

        public string IPHost { get; set; }

        public string Message { get; set; }

        private Server _server;

        public MainViewModel()
        {
            Users = new ObservableCollection<UserModel>();
            Messages = new ObservableCollection<string>();
            Host = new ObservableCollection<string>();
            _server = new Server();

            _server.connectedEvent += UserConnected;
            _server.msgReceivedEvent += MessageReceived;
            _server.userDisconnectedEvent += RemoveUser;

            ConnectToServerCommand = new RelayCommand(o=> _server.ConnectToServer(Username, IPHost), o=> !string.IsNullOrEmpty(Username));
           // ConnectToHost = new RelayCommand(o => _server.ConnectToServer(IPHost), o => !string.IsNullOrEmpty(IPHost));//This line of cod e assigns the username from the text box to the username variable essentially

            SendMessageCommand = new RelayCommand(o => _server.SendMessageToServer(Message), o => !string.IsNullOrEmpty(Message));
        }

        private void MessageReceived()
        {
            var msg = _server.PacketReader.ReadMessage();
            Application.Current.Dispatcher.Invoke(() => Messages.Add(msg));
        }


        private void UserConnected() //will be invoked when we recieve a packet with he curent op code.
        {
            var user = new UserModel
            {
                Username = _server.PacketReader.ReadMessage(),
                UID = _server.PacketReader.ReadMessage(),
                IPHost = _server.PacketReader.ReadMessage(),
            };

          if(!Users.Any(x=> x.UID == user.UID))// we still need to check for duplicate users
            {
                Application.Current.Dispatcher.Invoke(() => Users.Add(user));
            }
        }

        private void RemoveUser()
        {
            var uid = _server.PacketReader.ReadMessage();
            var user = Users.Where(x =>x.UID == uid).FirstOrDefault();
            Application.Current.Dispatcher.Invoke(() => Users.Remove(user));
            
        }
    }
}

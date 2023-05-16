using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChatServer.Net.IO;
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

namespace Texting_Program
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        public static string IPHost;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String strHostName = string.Empty;
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] addr = ipEntry.AddressList;

            for (int i = 1; i < addr.Length; i++)
            {
                addr[i].ToString();
            }
            IPHost = txtHost.Text;
            MessageBox.Show(addr[1].ToString());
            ChatServer chatServer = new ChatServer();
            //this.Close();
        }


    private void Conect_Click(object sender, RoutedEventArgs e)
        { 

            //MainWindow..Properties[IPhost] = txtHost.Text;
            
            
        }
    }

    public partial class ChatServer
    {
        public ChatServer()
        {
            
        }
    }
}

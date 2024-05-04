using Serveeer.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using Serveeer.View;
using System.Net.Sockets;
using Serveeer.Model;
using TcpClient = Serveeer.Model.TcpClient;
using System.Net.Http;

namespace Serveeer.ViewModel
{
    public class MainViewModel : BindingHelper
    {
        public BindableCommand CreateChatCommand { get; set; }
        public BindableCommand ConnectionCommand { get; set; }
        public BindableCommand SendCommand { get; set; }

        private TcpClient tcpClient;
        private TcpServer tcpServer;
        public MainWindow StartWindow { get; set; }
        public MainWindow Window { get; set; }
        public MainViewModel(MainWindow Window) 
        {
            CreateChatCommand = new BindableCommand(_ => CreateChat());
            ConnectionCommand = new BindableCommand(_ => Connection());
            SendCommand = new BindableCommand(_ => SendText());
            this.Window = Window;
        }
        private void CreateChat()
        {
            var name = NameTextProperty;
            if (name == null)
            {
                MessageBox.Show("Введите имя пользователя!");
            }
            else if (name != null)
            {
                tcpServer = new TcpServer();
                ChatWindow1xaml windowStart = new ChatWindow1xaml(this);
                windowStart.Show();
                Window.Close();

                MessageTextProperty = name;
                SendText();
            }
        }
        private void Connection()
        {
            var ip = IpTextProperty;
            var name = NameTextProperty;
            if (ip == null || name == null || ip == null && name == null)
            {
                MessageBox.Show("Проверьте заполненность имени или введенный IP-адрес");
            }
            else if (ip != null || name != null || ip != null && name != null)
            {
                tcpClient = new TcpClient();
                ChatWindow1xaml windowStart = new ChatWindow1xaml(this);
                windowStart.Show();
                Window.Close();

                MessageTextProperty = name;
                SendText();
            }
        }
        private async Task SendText()
        {
            if (tcpClient != null)
            {
                tcpClient.SendMessage(MessageTextProperty);
            }
        }

        private ObservableCollection<string> messageList;
        public ObservableCollection<string> MessageList
        {
            get { return messageList; }
            set
            {
                messageList = value;
                OnPropertyChanged(nameof(MessageList));
            }
        }

        private string ipTextProperty;
        public string IpTextProperty
        {
            get { return ipTextProperty; }
            set
            {
                ipTextProperty = value;
                OnPropertyChanged(nameof(IpTextProperty));
            }
        }
        private string nameTextProperty;
        public string NameTextProperty
        {
            get { return nameTextProperty; }
            set
            {
                nameTextProperty = value;
                OnPropertyChanged(nameof(NameTextProperty));
            }
        }

        private string messageTextProperty;
        public string MessageTextProperty
        {
            get { return messageTextProperty; }
            set
            {
                messageTextProperty = value;
                OnPropertyChanged(nameof(MessageTextProperty));
            }
        }

    }
}
    
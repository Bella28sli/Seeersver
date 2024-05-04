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

namespace Serveeer.ViewModel
{
    public class MainViewModel : BindingHelper
    {
        public BindableCommand CreateChatCommand { get; set; }
        public BindableCommand ConnectionCommand { get; set; }
        public BindableCommand SendCommand { get; set; }

        public MainWindow StartWindow { get; set; }
        public Window Window { get; set; }
        public TcpClient tcpClient;
        public TcpServer tcpServer;
        public MainViewModel(MainWindow Window) 
        {
            CreateChatCommand = new BindableCommand(_ => CreateChat());
            ConnectionCommand = new BindableCommand(_ => Connection());
            SendCommand = new BindableCommand(_ => SendText());
            this.Window = Window;

        }
        public MainViewModel(ChatWindow1xaml ChatWin)
        {
            CreateChatCommand = new BindableCommand(_ => CreateChat());
            ConnectionCommand = new BindableCommand(_ => Connection());
            SendCommand = new BindableCommand(_ => SendText());
            this.Window = ChatWin;

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
                ChatWindow1xaml windowStart = new ChatWindow1xaml(true, this);
                MessageTextProperty = name;
                tcpClient = new TcpClient(this, MessageTextProperty);
                windowStart.Show();
                Window.Close();

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
                MessageTextProperty = name;
                ChatWindow1xaml windowStart = new ChatWindow1xaml(false, this);
                tcpClient = new TcpClient(this, MessageTextProperty);
                windowStart.Show();
                Window.Close();
                MessageTextProperty = "";


            }
        }
        private async Task SendText()
        {
            tcpClient.SendMessage(MessageTextProperty);
        }

        private ObservableCollection<string> messageList = new ObservableCollection<string>();
        public ObservableCollection<string> MessageList
        {
            get { return messageList; }
            set
            {
                messageList = value;
                OnPropertyChanged(nameof(MessageList));
            }
        }

        private ObservableCollection<string> userList = new ObservableCollection<string>();
        public ObservableCollection<string> UserList
        {
            get { return userList; }
            set
            {
                userList = value;
                OnPropertyChanged(nameof(UserList));
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
    
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
using System.ComponentModel;

namespace Serveeer.ViewModel
{
    public class MainViewModel : BindingHelper
    {
        public BindableCommand CreateChatCommand { get; set; }
        public BindableCommand ConnectionCommand { get; set; }
        public BindableCommand SendCommand { get; set; }
        public BindableCommand ExitCommand { get; set; }
        public BindableCommand CheckLogsCommand { get; set; }


        public MainWindow StartWindow { get; set; }
        ChatWindow1xaml currentWin;
        static bool needToClose = true;

        public Window Window { get; set; }
        public TcpClient tcpClient;
        public TcpServer tcpServer;
        public MainViewModel(MainWindow Window) 
        {
            CreateChatCommand = new BindableCommand(_ => CreateChat());
            ConnectionCommand = new BindableCommand(_ => Connection());
            SendCommand = new BindableCommand(_ => SendText());
            ExitCommand = new BindableCommand(_ => ExitText());

            CheckLogsCommand = new BindableCommand(_ => CheckLogs());

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
                currentWin = new ChatWindow1xaml(true, this);
                MessageTextProperty = name;
                tcpClient = new TcpClient(this, MessageTextProperty);
                currentWin.Show();
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
                currentWin = new ChatWindow1xaml(false, this);
                tcpClient = new TcpClient(this, MessageTextProperty);

                currentWin.Show();
                Window.Close();
                MessageTextProperty = "";


            }
        }
        private async Task SendText()
        {
            if(MessageTextProperty == "/disconnect")
            {
                needToClose = false;
                tcpClient.SendMessage("/disconnect");
                MainWindow newMain = new MainWindow();
                newMain.Show();
                currentWin.Close();
                return;
            }
            tcpClient.SendMessage(MessageTextProperty);
        }
        private async Task ExitText()
        {
            MessageTextProperty = "/disconnect";
            SendText();

        }
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if (needToClose)
            {
                ExitText();
            }
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


        private void CheckLogs()
        {

        }
    }
}
    
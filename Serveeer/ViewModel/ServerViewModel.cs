using Serveeer.Model;
using Serveeer.View;
using Serveeer.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TcpClient = Serveeer.Model.TcpClient;

namespace Serveeer.ViewModel
{
    public class ServerViewModel : BindingHelper
    {
        public BindableCommand SendCommand { get; set; }
        public BindableCommand CheckLogsCommand { get; set; }
        public BindableCommand ExitCommand { get; set; }


        public Window Window { get; set; }
        TcpServer tcpServer;
        TcpClient tcpClient;
        MainViewModel mainViewModel;
        ChatWindow1xaml chatWindow;
        public ServerViewModel(ChatWindow1xaml ChatWin, MainViewModel mainView)
        {
            mainViewModel = mainView;
            chatWindow = ChatWin;

            tcpServer = new TcpServer(this, mainView);
            tcpServer.clients.Add(tcpServer.socket);
            SendCommand = new BindableCommand(_ => SendText());
            CheckLogsCommand = new BindableCommand(_ => CheckLogs());
            ExitCommand = new BindableCommand(_ => ExitText());


            this.Window = ChatWin;

        }
        private async Task SendText()
        {
            if (MessageTextProperty == "/disconnect")
            {

                tcpClient.SendMessage("/disconnect");
                MainWindow newMain = new MainWindow();
                newMain.Show();
                Window.Close();
                return;
            }
            tcpClient.SendMessage(MessageTextProperty);
        }
        private async Task ExitText()
        {
            MessageTextProperty = "/disconnect";
            SendText();
        }
        private void CheckLogs()
        {
            ObservableCollection<string> backUp = new ObservableCollection<string>();
            backUp = UserList;
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
    }
}

using Serveeer.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Serveeer.Model
{
    public class TcpServer
    {
        public Socket socket; 
        public List<Socket> clients = new List<Socket>();
        private ServerViewModel viewModel;
        private MainViewModel mainView;

        public List<User> users = new List<User>();
        public ObservableCollection<string> backUp = new ObservableCollection<string>();

        public TcpServer(ServerViewModel viewModel, MainViewModel mainView)
        {
            this.viewModel = viewModel;
            this.mainView = mainView;

            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 8888);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipPoint);
            socket.Listen(1000);
            ListenToClients();
        }
        private async Task ListenToClients()
        {
            while (true)
            {
                var client = await socket.AcceptAsync();
                clients.Add(client);

                ReceiveMessage(client, new CancellationToken());
            }
        }
        private async Task ReceiveMessage(Socket client, CancellationToken shitfuck)
        {
            bool isNewUser = true;
            string receivedMessage = "";

            while (!shitfuck.IsCancellationRequested)
            {
                byte[] bytes = new byte[1024];
                int bytesRead = await client.ReceiveAsync(bytes, SocketFlags.None);
                string text = Encoding.Unicode.GetString(bytes, 0, bytesRead);

                receivedMessage += text;

                if (isNewUser)
                {
                    User newUser = new User();
                    newUser.socket = client;
                    newUser.name = receivedMessage;
                    newUser.logTime = DateTime.Now;
                    newUser.tokenSource = new CancellationTokenSource();
                    shitfuck = newUser.tokenSource.Token;

                    users.Add(newUser);
                    viewModel.UserList.Add(receivedMessage);
                    backUp.Add($"{newUser.name} - подключился\n {DateTime.Now}");
                    string names = "/log\n" + string.Join("\n", viewModel.UserList);
                    foreach (User user in users)
                    {
                        SendMessage(user.socket, names);
                    }

                    isNewUser = false;
                }
                else if (text == "/disconnect")
                {
                    User userToRemove = users.FirstOrDefault(u => u.socket == client);
                    backUp.Add($"{userToRemove.name} - отключился\n {DateTime.Now}");

                    viewModel.UserList.Remove(userToRemove.name); //обновление пользователей
                    users.Remove(userToRemove);
                    SendMessage(userToRemove.socket, "/disconnect");

                    userToRemove.tokenSource.Cancel();

                    string names = "/log\n" + string.Join("\n", viewModel.UserList);
                    foreach (User user in users)
                    {
                        SendMessage(user.socket, names);
                    }
                }
                else
                {
                    string messageText = receivedMessage;
                    if (messageText.StartsWith("/log"))
                    {
                        viewModel.UserList.Clear();
                        viewModel.UserList = new ObservableCollection<string>(messageText.Split('\n'));
                        viewModel.UserList.RemoveAt(0);

                    }
                    else
                    {
                        string messageSender = users.FirstOrDefault(u => u.socket == client)?.name;
                        string messageSendDate = DateTime.Now.ToString();
                        string fulltext = $"[ {messageSendDate} ] {messageSender}: {messageText}";


                        foreach (User user in users)
                        {
                            if (user.socket != client)
                            {
                                SendMessage(user.socket, fulltext);
                            }
                        }
                        viewModel.MessageList.Add(fulltext);
                        SendMessage(client, fulltext);
                        fulltext = "";
                    }

                }
                receivedMessage = ""; // Сбрасываем сообщение после обработки
            }
        }
        public async Task SendMessage(Socket client, string text)
        {
            if (text == "/disconnect")
            {
                foreach (User user in users)
                {
                    user.tokenSource.Cancel();
                    
                }
            }
            byte[] bytes = Encoding.Unicode.GetBytes(text);
            await client.SendAsync(bytes, SocketFlags.None);        
        }
    }
}

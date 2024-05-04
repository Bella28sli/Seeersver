using Serveeer.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Serveeer.Model
{
    public class TcpServer
    {
        private Socket socket; 
        private List<Socket> clients = new List<Socket>();
        private MainViewModel viewModel;
        private List<User> users = new List<User>();

        public TcpServer()
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 8888);//точка для подключения клиентов
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //сокет внутри сети
            socket.Bind(ipPoint);//привязка точки к сокету
            socket.Listen(1000);//максимум 1000 чел могут подключиться 
            ListenToClients();
        }
        private async Task ListenToClients()//интерфейс работает параллельно с интерфейсом
        {
            while (true)
            {
                var client = await socket.AcceptAsync();//подключившийся юзер сохраняется в переменную
                clients.Add(client);//добавление сокета-клиента в лист

                ReceiveMessage(client);
            }
        }
        private async Task ReceiveMessage(Socket client)
        {
            while (true)
            {
                bool isNew = true;
                byte[] bytes = new byte[1024];
                await client.ReceiveAsync(bytes, SocketFlags.None);//
                string text = Encoding.UTF8.GetString(bytes);

                viewModel.MessageList.Add($"[Сообщение от {client.RemoteEndPoint}]: {text}");
                foreach (User user in users)
                {
                    if(user.socket == client)
                    {
                        if(text == "/disconnect")
                        {
                            user.tokenSource.Cancel();
                        }
                        else
                        {
                            string messageText = text;
                            string messageSender = user.name;
                            string messageSendDate = DateTime.Now.ToString();
                            string fulltext = $"[ {messageSendDate} ] {messageSender}: {messageText}";
                            viewModel.MessageList.Add(fulltext);
                            foreach (var item in clients)
                            {
                                SendMessage(client, fulltext);
                            }
                        }
                        isNew = false;
                    }
                }
                if (isNew)
                {
                    User newuser = new User();
                    newuser.socket = client;
                    newuser.name = text;
                    newuser.logTime = DateTime.Now;
                    users.Add(newuser);
                    isNew = true;
                }
            }
        }
        private async Task SendMessage(Socket client, string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            await client.SendAsync(bytes, SocketFlags.None);
        }
    }
}

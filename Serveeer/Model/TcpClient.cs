using Serveeer.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Serveeer.Model
{
    public class TcpClient
    {
        private Socket server;
        private MainViewModel viewModel;
        private CancellationTokenSource isFUCKINGSHIT;
        public TcpClient(MainViewModel viewModel, string name) 
        { 
            this.viewModel = viewModel;
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Connect("127.0.0.1", 8888);
            isFUCKINGSHIT = new CancellationTokenSource();
            SendMessage(name);
            ReceiveMessage(isFUCKINGSHIT.Token);

        }
        private async Task ReceiveMessage(CancellationToken shitfuck)
        {
            while (!shitfuck.IsCancellationRequested)
            {
                
                byte[] bytes = new byte[1024];
                await server.ReceiveAsync(bytes, SocketFlags.None);
                string text = Encoding.Unicode.GetString(bytes);

                viewModel.MessageList.Add(text);

            }
        }
        public async Task SendMessage(string text)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(text);
            await server.SendAsync(bytes, SocketFlags.None);
        }
    }
}

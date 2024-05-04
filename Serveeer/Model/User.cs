using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Serveeer.Model
{
    public class User
    {
        public string name {  get; set; }
        public Socket socket { get; set; }
        public CancellationTokenSource tokenSource { get; set; }
        public DateTime logTime { get; set; }

    }
}

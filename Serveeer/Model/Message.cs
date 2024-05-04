using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serveeer.Model
{
    internal class Message
    {
        public string text {  get; set; }
        public DateTime sendDate { get; set; }
        public User sender { get; set; }
    }
}

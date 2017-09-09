using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketSupport.Models
{
    public class Message
    {
        public string Text { get; set; }
        public string DateStr { get; set; }
        public int AuthorId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TicketSupport.ViewModels;

namespace TicketSupport.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public bool NewMessage { get; set; }
        public Priority Priority { get; set; }
        public string DateIncident { get; set; }
        public string CharName { get; set; }
        public string Title { get; set; }
        public List<Message> Messages { get; set; } = new List<Message>();
        public string Category { get; set; }
        
    }
}

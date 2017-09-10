using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TicketSupport.Records;
using TicketSupport.ViewModels;

namespace TicketSupport.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public Author Author { get; set; }
        public string ServerName { get; set; }
        public bool NewMessage { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }
        public string DateIncident { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string CharName { get; set; }
        public string Title { get; set; }
        public List<Message> Messages { get; set; } = new List<Message>();
        public int CategoryId { get; set; }

        public Ticket(TicketRecord ticketsRecord)
        {
            Id = ticketsRecord.Id;
            Author = new Author(ticketsRecord.Author);
            ServerName = ticketsRecord.GameServer.Name;
            CreateDate = Convert.ToDateTime(ticketsRecord.CreateDate);
            DateIncident = ticketsRecord.DateIncident;
            UpdateDate = Convert.ToDateTime(ticketsRecord.UpdateDate);
            CharName = ticketsRecord.CharName;
            Title = ticketsRecord.Title;
            CategoryId = ticketsRecord.CategoryId;
            foreach (var messageRecord in ticketsRecord.Answers)
            {
                Messages.Add(new Message(messageRecord));
            } 
        }

    }

}

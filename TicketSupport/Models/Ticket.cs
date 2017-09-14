using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Threading;
using TicketSupport.Records;
using TicketSupport.ViewModels;

namespace TicketSupport.Models
{
    [Serializable]
    public class Ticket
    {
        [NonSerialized]
        private bool _haveNewMessage;
        public bool HaveNewMessage
        {
            get { return _haveNewMessage; }
            set { _haveNewMessage = value; }
        }

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
        public ObservableCollection<Message> Messages { get; set; }
        public string Category { get; set; }

        public Ticket(TicketRecord ticketsRecord)
        {
            Messages = new ObservableCollection<Message>();
            SetFromticketsRecord(ticketsRecord);
        }

        public void SetFromticketsRecord(TicketRecord ticketsRecord)
        {
            Id = ticketsRecord.Id;
            Author = new Author(ticketsRecord.Author);
            ServerName = ticketsRecord.GameServer.Name;
            CreateDate = Convert.ToDateTime(ticketsRecord.CreateDate);
            DateIncident = ticketsRecord.DateIncident;
            UpdateDate = Convert.ToDateTime(ticketsRecord.UpdateDate);
            CharName = ticketsRecord.CharName;
            Title = ticketsRecord.Title;
            Category = ticketsRecord.Category.Title;
            Priority = (Priority)ticketsRecord.Priority;
            Status = (Status)ticketsRecord.Status;
            foreach (var messageRecord in ticketsRecord.Answers)
            {
                if (Messages.All(c => c.Id != messageRecord.Id))
                {
                    var isUser = messageRecord.AuthorId == Author.UserId;
                    Messages.Add(new Message(messageRecord, isUser));
                    if(isUser) HaveNewMessage = true;
                }
                   
            }
        }
    }

}

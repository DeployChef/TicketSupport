using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketSupport.Records;

namespace TicketSupport.Models
{
    public class Message
    {
        public Message(AnswerRecord messageRecord, bool isUser)
        {
            Text = messageRecord.Text;
            IsUser = isUser;
            Id = messageRecord.Id;
            DateStr = Convert.ToDateTime(messageRecord.DateStr);
        }

        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime DateStr { get; set; }
        public bool IsUser { get; set; }

        public string Time => DateStr.ToShortTimeString();

        public string Date => DateStr.ToShortDateString();
    }
}

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
        
        public Message(AnswerRecord messageRecord)
        {
            Text = messageRecord.Text;
            AuthorId = messageRecord.AuthorId;
            DateStr = Convert.ToDateTime(messageRecord.DateStr);
        }

        public string Text { get; set; }
        public DateTime DateStr { get; set; }
        public int AuthorId { get; set; }
    }
}

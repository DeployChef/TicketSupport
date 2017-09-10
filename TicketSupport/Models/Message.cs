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
        public Message(AnswerRecord messageRecord, string authorName)
        {
            Text = messageRecord.Text;
            AuthorName = authorName;
            DateStr = Convert.ToDateTime(messageRecord.DateStr);
        }

        public string Text { get; set; }
        public DateTime DateStr { get; set; }
        public string AuthorName { get; set; }
    }
}

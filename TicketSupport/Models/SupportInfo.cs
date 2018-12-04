using System.Collections.Generic;
using TicketSupport.Records;

namespace TicketSupport.Models
{
    public class SupportInfo
    {
        public SupportInfo(string token)
        {
            Token = token;
        }

        public string Token { get; }
        public int? UserId { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public Dictionary<int,string> Categories { get; } = new Dictionary<int, string>();

        public void SetRecord(SupportRecord record)
        {
            UserId = record.UserId;
            Login = record.Login;
            Email = record.Email;
            foreach (var recordCategory in record.Categories)
            {
                Categories.Add(recordCategory.Id, recordCategory.Title);
            }
        }
    }
}
using System.Collections.Generic;

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
    }
}
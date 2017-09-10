using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketSupport.Records;

namespace TicketSupport.Models
{
    public class Author
    {

        public Author(AuthorRecord author)
        {
            UserId = author.UserId;
            Login = author.Login;
            Email = author.Email;
        }

        public string UserId { get; set; }

        public string Login { get; set; }

        public string Email { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TicketSupport.Records
{
    [DataContract(Name = "user", Namespace = "")]
    public class SupportRecord
    {
        [DataMember(Name = "user_id", IsRequired = true, Order = 0)]
        public int? UserId { get; set; }

        [DataMember(Name = "login", IsRequired = true, Order = 1)]
        public string Login { get; set; }

        [DataMember(Name = "email", IsRequired = true, Order = 2)]
        public string Email { get; set; }

        [DataMember(Name = "categories", IsRequired = true, Order = 3)]
        public Categories Categories { get; set; }
    }


    [CollectionDataContract(Name = "categories", Namespace = "")]
    public class Categories : List<CategoryRecord>
    {
        
    }

    [DataContract(Name = "category", Namespace = "")]
    public class CategoryRecord
    {
        [DataMember(Name = "id", IsRequired = true, Order = 1)]
        public int Id { get; set; }

        [DataMember(Name = "title", IsRequired = true, Order = 2)]
        public string Title { get; set; }

    }
}

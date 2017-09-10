
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TicketSupport.Records
{
    [DataContract(Name = "ticket", Namespace = "")]
    public class TicketRecord
    {
        [DataMember(Name = "id", IsRequired = true, Order = 0)]
        public int Id { get; set; }

        [DataMember(Name = "category_id", IsRequired = true, Order = 1)]
        public int CategoryId { get; set; }

        [DataMember(Name = "priority", IsRequired = true, Order = 2)]
        public int Priority { get; set; }

        [DataMember(Name = "date_incident", IsRequired = true, Order = 3)]
        public string DateIncident { get; set; }

        [DataMember(Name = "char_name", IsRequired = true, Order = 4)]
        public string CharName { get; set; }

        [DataMember(Name = "title", IsRequired = true, Order = 5)]
        public string Title { get; set; }

        [DataMember(Name = "status", IsRequired = true, Order = 6)]
        public int Status { get; set; }

        [DataMember(Name = "new_message_for_admin", IsRequired = true, Order = 7)]
        public int NewMessage { get; set; }

        [DataMember(Name = "created_at", IsRequired = true, Order = 8)]
        public string CreateDate { get; set; }

        [DataMember(Name = "updated_at", IsRequired = true, Order = 9)]
        public string UpdateDate { get; set; }

        [DataMember(Name = "gs", IsRequired = true, Order = 10)]
        public ServerRecord GameServer { get; set; }

        [DataMember(Name = "category", IsRequired = true, Order = 11)]
        public CategoryRecord Category { get; set; }
        
        [DataMember(Name = "answers", IsRequired = true, Order = 12)]
        public Answers Answers { get; set; }

        [DataMember(Name = "author", IsRequired = true, Order = 13)]
        public AuthorRecord Author { get; set; }
    }

    [CollectionDataContract(Name = "tickets", Namespace = "")]
    public class TicketsRecord : List<TicketRecord>
    {

    }

    [CollectionDataContract(Name = "answers", Namespace = "")]
    public class Answers : List<AnswerRecord>
    {

    }

    [DataContract(Name = "answer", Namespace = "")]
    public class AnswerRecord
    {
        [DataMember(Name = "id", IsRequired = true, Order = 0)]
        public int Id { get; set; }

        [DataMember(Name = "text", IsRequired = true, Order = 1)]
        public string Text { get; set; }

        [DataMember(Name = "user_id", IsRequired = true, Order = 2)]
        public int AuthorId { get; set; }

        [DataMember(Name = "created_at", IsRequired = true, Order = 3)]
        public string DateStr { get; set; }
    }

    [DataContract(Name = "author", Namespace = "")]
    public class AuthorRecord
    {
        [DataMember(Name = "user_id", IsRequired = true, Order = 0)]
        public int UserId { get; set; }

        [DataMember(Name = "login", IsRequired = true, Order = 1)]
        public string Login { get; set; }

        [DataMember(Name = "email", IsRequired = true, Order = 2)]
        public string Email { get; set; }
    }

    [DataContract(Name = "gs", Namespace = "")]
    public class ServerRecord
    {
        [DataMember(Name = "id", IsRequired = true, Order = 0)]
        public string Id { get; set; }

        [DataMember(Name = "name", IsRequired = true, Order = 1)]
        public string Name { get; set; }

    }
}

using System.ComponentModel;

namespace TicketSupport.Models
{
    public enum Priority
    {
        [Description("Любой")]
        All = 3,
        [Description("Высокий")]
        High = 2,
        [Description("Средний")]
        Mid = 1,
        [Description("Низкий")]
        Low = 0
    }
}

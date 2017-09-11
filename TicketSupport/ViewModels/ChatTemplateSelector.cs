using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TicketSupport.Models;

namespace TicketSupport.ViewModels
{
    public class ChatTemplateSelector : DataTemplateSelector
    {
        public DataTemplate LeftTemplate { get; set; }
        public DataTemplate RightTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var message = (Message)item;
            var dt = message != null && message.IsUser ? this.LeftTemplate : this.RightTemplate;
            return dt;
        }
    }
}

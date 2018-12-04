using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TicketSupport.Records;

namespace TicketSupport.Models
{
    [Serializable]
    public class Tickets: ObservableCollection<Ticket>
    {
        public string Owner { get; }
        [field: NonSerialized]
        public event EventHandler<EventArgs> HaveChanges; 
        private readonly object _locker = new object();

        public void SyncTickets(List<TicketRecord> newTickets)
        {
            lock (_locker)
            {
                var removeList = new List<Ticket>();
                foreach (var ticket in Items)
                {
                    if (newTickets.All(t => t.Id != ticket.Id))
                    {
                        removeList.Add(ticket);
                        continue;
                    }
                    if(ticket.Status == Status.Close)
                        removeList.Add(ticket);
                }
                foreach (var removeTicket in removeList)
                {
                    Items.Remove(removeTicket);
                }
                foreach (var newTicket in newTickets)
                {
                    if (Items.All(ticket => newTicket.Id != ticket.Id))
                    {
                        Add(new Ticket(newTicket));
                        OnHaveChanges();
                        continue;
                    }
                    var currentTicket = this.FirstOrDefault(t => t.Id == newTicket.Id);
                    if(currentTicket == null || currentTicket.UpdateDate.CompareTo(Convert.ToDateTime(newTicket.UpdateDate)) >= 0)
                        continue;
                    currentTicket.SetFromticketsRecord(newTicket);
                    OnHaveChanges();
                }
            }
        }

        public Tickets(string owner)
        {
            Owner = owner;
        }
       
        protected virtual void OnHaveChanges()
        {
            HaveChanges?.Invoke(this, EventArgs.Empty);
        }
    }
}

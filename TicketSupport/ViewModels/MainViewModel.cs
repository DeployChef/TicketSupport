using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TicketSupport.Models;
using TicketSupport.Views;

namespace TicketSupport.ViewModels
{
    public class MainViewModel : ViewModelBase
    {

        private Ticket _selectedTicket;
        private string _status;
        private string _searchText;
        private List<KeyValuePair<int, DateTime>> ticketsIdsAndUpdateDates = new List<KeyValuePair<int, DateTime>>();

        public ObservableCollection<Ticket> Tickets { get; set; } = new ObservableCollection<Ticket>();
        public SupportInfo SupportInfo { get; }

        public Ticket SelectedTicket
        {
            get { return _selectedTicket; }
            set
            {
                _selectedTicket = value;
                RaisePropertyChanged(() => SelectedTicket);
                RaisePropertyChanged(() => MessageText);
            }
        }

        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                RaisePropertyChanged(() => Status);
            }
        }

        public string MessageText
        {
            get
            {
                if (SelectedTicket == null)
                    return " Ничего не выбрано";
                if (!SelectedTicket.Messages.Any())
                    return " Сообщений нет";

                return CreateMessagesText();
            }
        }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                RaisePropertyChanged(() => SearchText);
                RaisePropertyChanged(()=> FiltredTickets);
            }
        }

        public List<Ticket> FiltredTickets
        {
            get
            {
                return SearchText.IsNullOrEmpty() ? Tickets.ToList() : Tickets.Where(c => c.Title.Contains(SearchText)).ToList();
            }
        }

        public RelayCommand ClearSearchTextCommand => new RelayCommand(ClearSearchText);

        private void ClearSearchText(object obj)
        {
            SearchText = string.Empty;
        }

        private string CreateMessagesText()
        {
            var textBuilder = new StringBuilder();
            foreach (var message in SelectedTicket.Messages)
            {
               
                textBuilder.Append(message.DateStr.ToShortTimeString() + " | ");
                textBuilder.Append(message.AuthorName + " : ");
                textBuilder.Append(message.Text);
                textBuilder.Append(Environment.NewLine);
                textBuilder.Append(Environment.NewLine);
            }
            return textBuilder.ToString();
        }

        public MainViewModel(SupportInfo supInfo)
        {
            SupportInfo = supInfo;
            //Task.Factory.StartNew(ParceTiket);
            Timer t = new Timer(o => {
                ParceTiket();
            }, null, 0, 10000);
        }

        private void LoadTickets(List<Ticket> tikets)
        {
            Tickets = new ObservableCollection<Ticket>(tikets);
            RaisePropertyChanged(() => FiltredTickets);
            Status = "Данные успешно получены";
            IsBusy = false;
        }

        private void ParceTiket()
        {
            IsBusy = true;
            var tikets = TicketParcer.GetTikets(SupportInfo);
            if (tikets == null)
            {
                Status = "Не удалось получить тикеты";
                IsBusy = false;
            }

            if (ticketsIdsAndUpdateDates.Count == 0)
                LoadTickets(tikets);

            foreach (var ticket in tikets)
            {
                KeyValuePair<int, DateTime> keyValueTicketIdDateTime = ticketsIdsAndUpdateDates.SingleOrDefault(x => x.Key == ticket.Id);
                if (keyValueTicketIdDateTime.IsDefault())
                    ticketsIdsAndUpdateDates.Add(new KeyValuePair<int, DateTime>(ticket.Id, ticket.UpdateDate));
                if (keyValueTicketIdDateTime.Value < ticket.UpdateDate)
                    LoadTickets(tikets);
            }
            Status = "Обновлений нет.";
        }

    }
}
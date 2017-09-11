using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mime;
using System.Text;
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

        public ObservableCollection<Ticket> Tickets { get; set; } = new ObservableCollection<Ticket>();
        public SupportInfo SupportInfo { get; }

        public Ticket SelectedTicket
        {
            get { return _selectedTicket; }
            set
            {
                _selectedTicket = value;
                RaisePropertyChanged(() => SelectedTicket);
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

        public MainViewModel(SupportInfo supInfo)
        {
            SupportInfo = supInfo;
            Task.Factory.StartNew(ParceTiket);
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
            else
            {
                Tickets = new ObservableCollection<Ticket>(tikets);
                RaisePropertyChanged(()=>FiltredTickets);
                Status = "Данные успешно получены";
                IsBusy = false;
            }

        }
    }
}
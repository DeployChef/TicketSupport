using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using TicketSupport.Models;
using TicketSupport.Views;

namespace TicketSupport.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private Ticket _selectedTicket;
        private string _status;
        private string _searchText;
        private string _newMessageText = string.Empty;
        private bool _isUpdating;
        private CancellationTokenSource _cts;
        private bool _isChatChanged;
        private readonly Timer _timer;
        public string SyncDate => DateTime.Now.ToLongTimeString();
        private FlashHelper _flashHelper;
        public bool IsUpdating
        {
            get { return _isUpdating; }
            set
            {
                _isUpdating = value;
                RaisePropertyChanged(() => IsUpdating);
            }
        }

        public bool IsChatChanged
        {
            get { return _isChatChanged; }
            set
            {
                _isChatChanged = value;
                RaisePropertyChanged(()=>IsChatChanged);
            }
        }
        public string NewMessageText
        {
            get { return _newMessageText; }
            set
            {
                _newMessageText = value;
                RaisePropertyChanged(()=>NewMessageText);
                RaisePropertyChanged(()=>SendMessageCommand);
            }
        }

        public Tickets Tickets { get; set; }
        public SupportInfo SupportInfo { get; }
        public RelayCommand SendMessageCommand { get; }
        public RelayCommand SyncCommand { get; }
       

        public Ticket SelectedTicket
        {
            get { return _selectedTicket; }
            set
            {
                _selectedTicket = value;
                _selectedTicket.HaveNewMessage = false;
                IsChatChanged = false;
                IsChatChanged = true;
                NewMessageText = string.Empty;
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
            _flashHelper = new FlashHelper(Application.Current);
            _timer = new Timer(SyncTicketsAsync, null, 0, 10000);
            Tickets = new Tickets();
            Tickets.HaveChanges += (sender, args) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _flashHelper.FlashApplicationWindow();
                });
            };
            SendMessageCommand = new RelayCommand(SendMessage, can => SelectedTicket != null && NewMessageText.Length > 5);
            SyncCommand = new RelayCommand(SyncTicketsAsync);
            SupportInfo = supInfo;
            IsBusy = true;
            Task.Factory.StartNew(SyncTickets);
        }

        private async void SyncTicketsAsync(object obj)
        {
            IsUpdating = true;
            await Task.Factory.StartNew(SyncTickets);
        }

        private void SyncTickets()
        {
            Status = "Получаем обновление";
            var tiketsRecord = TicketParcer.GetTiketsRecord(SupportInfo.Token);
            if (tiketsRecord == null)
            {
                Status = "Не удалось получить тикеты";
                return;
            }
            Tickets.SyncTickets(tiketsRecord);
            RaisePropertyChanged(()=>FiltredTickets);
            Status = "Данные успешно получены";
            RaisePropertyChanged(()=>SyncDate);
            IsBusy = false;
            IsUpdating = false;
        }

        private async void SendMessage(object obj)
        {
            Status = "Отправка";
            IsBusy = true;
            _cts?.Cancel(true);
            _cts = new CancellationTokenSource();
            var token = _cts.Token;
            var sendTask = TicketParcer.SendMessageAsync(NewMessageText, SupportInfo.Token, SelectedTicket.Id, token);
            await sendTask;
            if (sendTask.Result)
            {
                Status = "Успешно отправлено";
                NewMessageText = string.Empty;
                IsUpdating = true;
                SyncTickets();
            }
            else
            {
                Status = "Отправка не вышла";
            }
            IsBusy = false;
            _cts = null;
        }
    }
}
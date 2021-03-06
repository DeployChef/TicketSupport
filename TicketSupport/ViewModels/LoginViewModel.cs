﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TicketSupport.Models;

namespace TicketSupport.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public event EventHandler<SupportInfo> LoginSuccess;
        protected virtual void OnLoginSuccess(SupportInfo e)
        {
            LoginSuccess?.Invoke(this, e);
        }


        private CancellationTokenSource _cts;
        private string _status = "Добро пожаловать";
       
        public Action CloseAction { get; set; }
        public string SupportToken { get; set; } = Properties.Settings.Default.LastToken;

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

        public RelayCommand EnterCommand { get; }

        public LoginViewModel()
        {
            EnterCommand = new RelayCommand(Login, can => !SupportToken.IsNullOrEmpty() && !IsBusy);
        }

        public async void Login(object obj)
        {
            Status = "Подключаемся";
            IsBusy = true;
            _cts?.Cancel(true);
            _cts = new CancellationTokenSource();
            var token = _cts.Token;
            var loginTask = TicketParcer.GetSupportInfoAsync(SupportToken, token);
            await loginTask;
            if (!token.IsCancellationRequested && loginTask.Result != null)
            {

                Status = "Успешно";
                Properties.Settings.Default.LastToken = SupportToken;
                OnLoginSuccess(loginTask.Result);
                CloseAction();
            }

            if (loginTask.Result == null)
            {
                Status = "Нет доступа";
            }
            IsBusy = false;
            _cts = null;
        }

       
    }
}

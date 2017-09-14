using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using TicketSupport.Models;
using TicketSupport.ViewModels;
using TicketSupport.Views;

namespace TicketSupport
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        
        private void OnStartup(object sender, StartupEventArgs e)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            VersionHelper.CheckUpdate();
            var context = new LoginViewModel();
            context.LoginSuccess += ContextOnLoginSuccess;
            var loginview = new LoginWindow()
            {
                DataContext = context
            };
            context.CloseAction = loginview.Close;
            loginview.Show();
            
        }

     

        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            // Тут нужно смотреть какую библиотеку грузить, название библиотеки
            // написано в свойстве args.Name

            if (args.Name.Contains("System.Windows.Interactivity"))
            {
                return Assembly.Load(TicketSupport.Properties.Resources.System_Windows_Interactivity);
            }
            else
                return null;
        }
        private void ContextOnLoginSuccess(object sender, SupportInfo supportInfo)
        {
           
            var view = new MainWindow();
            Application.Current.MainWindow = view;
            var viewModel = new MainViewModel(supportInfo);
            view.DataContext = viewModel;
            view.Show();
        }
    }
}

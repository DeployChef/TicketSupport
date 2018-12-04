using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TicketSupport.Models;

namespace TicketSupport
{
    public static class HistoryHelper
    {
        private const string FILE_NAME = "ticket.hist";

        public static Tickets LoadHistory(string token)
        {
            if (!File.Exists(FILE_NAME)) return new Tickets(token);

            var bf = new BinaryFormatter();
            var fsin = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read, FileShare.None);
            try
            {
                using (fsin)
                {
                    var hist = (Tickets)bf.Deserialize(fsin);
                    return hist.Owner == token ? hist : new Tickets(token);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Не удалось pзагрузить историю тикетов по причине [{e.Message}]");
                if(File.Exists(FILE_NAME))
                File.Delete(FILE_NAME);
                return new Tickets(token);
            }
        }


        public static void SaveHistory(Tickets tickets)
        {
            if(tickets == null) return;

            var bf = new BinaryFormatter();
            var historyStream = new FileStream(FILE_NAME, FileMode.Create, FileAccess.Write, FileShare.None);
            try
            {
                using (historyStream)
                {
                    bf.Serialize(historyStream, tickets);
                }
            }
            catch(Exception e)
            {
                MessageBox.Show($"Не удалось сохранить историю тикетов по причине [{e.Message}]");
            }
        }
    }
}

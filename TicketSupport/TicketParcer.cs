using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using TicketSupport.Models;
using TicketSupport.Records;

namespace TicketSupport
{
    public class TicketParcer
    {
        private const string AUTH_URL = "https://hunger-games.ws/api/auth/";

        private const string GET_TICKETS_URL = "https://hunger-games.ws/api/tickets/";

        public static async Task<SupportInfo> GetSupportInfoAsync(String supportToken, CancellationToken token)
        {
            return await Task.Run(() => GetSupportInfo(supportToken, token), token);
        }

        private static SupportInfo GetSupportInfo(string supportToken, CancellationToken token)
        {
            try
            {
                var doc = GetXmlFromUrl(AUTH_URL, supportToken); 
                var supInfo = new SupportInfo(supportToken);

                var serializer = new DataContractSerializer(typeof(SupportRecord));

                using (var strReader = new StringReader(doc.InnerXml))
                using (var reader = XmlReader.Create(strReader))
                {
                    var record = (SupportRecord)serializer.ReadObject(reader);
                    supInfo.SetRecord(record);
                }
                token.ThrowIfCancellationRequested();
                return supInfo.UserId == null ? null : supInfo;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Не удалось, причина [{e.Message}]");
                return null;
            }
            
        }

        public static List<Ticket> GetTikets(SupportInfo supInfo)
        {
            try
            {
                var doc = GetXmlFromUrl(GET_TICKETS_URL, supInfo.Token);
                var serializer = new DataContractSerializer(typeof(TicketsRecord));
                TicketsRecord record;
                using (var strReader = new StringReader(doc.InnerXml))
                using (var reader = XmlReader.Create(strReader))
                {
                    record = (TicketsRecord)serializer.ReadObject(reader);
                }
                return record?.Select(ticketsRecord => new Ticket(ticketsRecord)).ToList();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Не удалось, причина [{e.Message}]");
                return null;
            }
        }

        private static XmlDocument GetXmlFromUrl(string url, string token)
        {
            var xdoc = new XmlDocument();
            var request = (HttpWebRequest)WebRequest.Create(url);
           
            request.Headers.Add("token", token);

            request.Method = "GET";
            request.Accept = "application/json";

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                xdoc.Load(response.GetResponseStream());
            }

            return xdoc;
        }

       
    }
}

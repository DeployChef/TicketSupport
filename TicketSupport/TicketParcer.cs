using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

        private const string TICKETS_URL = "https://hunger-games.ws/api/tickets/";

        public static async Task<SupportInfo> GetSupportInfoAsync(String supportToken, CancellationToken token)
        {
            return await Task.Run(() => GetSupportInfo(supportToken, token), token);
        }

        private static SupportInfo GetSupportInfo(string supportToken, CancellationToken token)
        {
            XmlDocument doc = null;
            try
            {
                doc = GetXmlFromUrl(AUTH_URL, supportToken);

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
                if (doc!=null && doc.InnerText.Contains("Token not found"))
                {
                    MessageBox.Show($"Не удалось, такой токен не найден");
                    return null;
                }
                MessageBox.Show($"Не удалось, причина [{e.Message}]");
                return null;
            }
            
        }

        private static TicketsRecord GetTicketsRecords(string supInfo)
        {
            var doc = GetXmlFromUrl(TICKETS_URL, supInfo);
            var serializer = new DataContractSerializer(typeof(TicketsRecord));
            TicketsRecord record;
            using (var strReader = new StringReader(doc.InnerXml))
            using (var reader = XmlReader.Create(strReader))
            {
                record = (TicketsRecord) serializer.ReadObject(reader);
            }
            return record;
        }

        public static List<TicketRecord> GetTiketsRecord(string supInfo)
        {
            try
            {
                return GetTicketsRecords(supInfo); ;
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

        private static XmlDocument GetXmlFromUrl2(string url, string token)
        {
            var xdoc = new XmlDocument();
            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Headers.Add("token", token);

            request.Method = "GET";
            request.Accept = "application/json";

            using (var response = (HttpWebResponse) request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var l = reader.ReadToEnd();
                }
            }


            return xdoc;
        }

        public static async Task<bool> SendMessageAsync(string newMessageText, string supportToken, int ticketId, CancellationToken token)
        {
            return await Task.Run(() => SendMessage(newMessageText, supportToken, ticketId, token), token);
        }

        private static bool SendMessage(string newMessageText, string supportToken, int ticketId, CancellationToken token)
        {
            try
            {
                var link = CreateTicketLink(ticketId,"answer");
                // Создаём коллекцию параметров
                var pars = new NameValueCollection {{"answer", newMessageText}};

                XmlDocument doc = new XmlDocument();
                using (var webClient = new WebClient())
                {
                    webClient.Headers.Add("token", supportToken);
                    var response = webClient.UploadValues(link, pars);
                    string xml = Encoding.UTF8.GetString(response);
                    doc.LoadXml(xml);
                }
                token.ThrowIfCancellationRequested();
                var node = doc.SelectNodes("result");
                return node?.Count!=0;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Не удалось, причина [{e.Message}]");
                return false;
            }
        }

        private static string CreateTicketLink(int ticketId, string action)
        {
            return TICKETS_URL + ticketId + "/"+action;
        }

        public static async Task<bool> CloseTicketAsync(string supportToken, int ticketId, CancellationToken token)
        {
            return await Task.Run(() => CloseTicket(supportToken, ticketId, token), token);
        }

        private static bool CloseTicket(string supportToken, int ticketId, CancellationToken token)
        {
            try
            {
                var link = CreateTicketLink(ticketId,"close");

                XmlDocument doc = GetXmlFromUrl(link, supportToken);

                token.ThrowIfCancellationRequested();
                var res = doc.SelectNodes("res");
                return res!=null && res.Cast<XmlNode>().Any(node => node.Name == "status" && node.InnerText == "OK");
            }
            catch (Exception e)
            {
                MessageBox.Show($"Не удалось, причина [{e.Message}]");
                return false;
            }
        }
    }
}

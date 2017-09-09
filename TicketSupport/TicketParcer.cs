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
                //HIODSgue32QjM89tltHk
                var doc = GetXmlFromUrl(AUTH_URL, supportToken); 
                SupportInfo supInfo = new SupportInfo(supportToken);

                var serializer = new DataContractSerializer(typeof(SupportRecord));

                using (var strReader = new StringReader(doc.InnerXml))
                using (var reader = XmlReader.Create(strReader))
                {
                    var record = (SupportRecord)serializer.ReadObject(reader);
                }



                foreach (XmlNode node in doc.SelectNodes("user"))
                {
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        if (child.Name == "user_id")
                            supInfo.UserId = Convert.ToInt32(child.InnerText);
                        if (child.Name == "login")
                            supInfo.Login = child.InnerText;
                        if (child.Name == "email")
                            supInfo.Email = child.InnerText;
                    }
                    foreach (XmlNode child in node.SelectNodes("categories"))
                    {
                        foreach (XmlNode category in child.ChildNodes)
                        {
                            int? id = null;
                            var title = string.Empty;
                            foreach (XmlNode categoryFirld in category.ChildNodes)
                            {     
                                if (categoryFirld.Name == "id")
                                    id = Convert.ToInt32(categoryFirld.InnerText);
                                if (categoryFirld.Name == "title")
                                    title = categoryFirld.InnerText;
                            }
                            if (id != null)
                                supInfo.Categories.Add(id.Value, title);
                        }
                    }
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

        public static List<Ticket> GetTikets(String supportToken)
        {
            try
            {
                var doc = GetXmlFromUrl(GET_TICKETS_URL, supportToken);
                foreach (XmlNode node in doc.SelectNodes("user"))
                {
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        if (child.Name == "user_id")
                            supInfo.UserId = Convert.ToInt32(child.InnerText);
                        if (child.Name == "login")
                            supInfo.Login = child.InnerText;
                        if (child.Name == "email")
                            supInfo.Email = child.InnerText;
                    }
                    foreach (XmlNode child in node.SelectNodes("categories"))
                    {
                        foreach (XmlNode category in child.ChildNodes)
                        {
                            int? id = null;
                            var title = string.Empty;
                            foreach (XmlNode categoryFirld in category.ChildNodes)
                            {
                                if (categoryFirld.Name == "id")
                                    id = Convert.ToInt32(categoryFirld.InnerText);
                                if (categoryFirld.Name == "title")
                                    title = categoryFirld.InnerText;
                            }
                            if (id != null)
                                supInfo.Categories.Add(id.Value, title);
                        }
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Не удалось, причина [{e.Message}]");
                return null;
            }
        }

        private static XmlDocument GetXmlFromUrl(string url, string token)
        {
            XmlDocument xdoc = new XmlDocument();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Cookie", "ANTIDDOS=2d36534375e3a66621432173a0dccc0f");
            request.Headers.Add("token", token);

            request.Method = "GET";
            request.Accept = "application/json";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                xdoc.Load(response.GetResponseStream());
            }

            return xdoc;
        }

       
    }
}

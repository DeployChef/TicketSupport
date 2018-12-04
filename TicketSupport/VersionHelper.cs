using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace TicketSupport
{
    public static class VersionHelper
    {
        private const string UPDATER_FILE_NAME = "UpNovaUpd.exe";
        private const string VERSION_FILE_NAME = "version.xml";
        private static readonly string MainDirSavePath = System.AppDomain.CurrentDomain.BaseDirectory;
        private static readonly double CurrentVersion = Convert.ToInt32(Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", ""));
        public static void CheckUpdate()
        {
            Task.Run(() =>
            {
                var remoteVersion = GetVersion();
                if (remoteVersion > CurrentVersion)
                {
                    Downdload(Properties.Settings.Default.UpdateUrl + UPDATER_FILE_NAME, MainDirSavePath + UPDATER_FILE_NAME);
                    if (File.Exists(UPDATER_FILE_NAME))
                    {
                       MessageBox.Show("Обнаружена новая версия (" + remoteVersion + ")" + Environment.NewLine +
				"Приложение будет автоматически обновлено и перезапущено.", Assembly.GetExecutingAssembly().ManifestModule.Name + " v" + CurrentVersion, MessageBoxButton.OK, MessageBoxImage.Information);
                        Process.Start(UPDATER_FILE_NAME, Assembly.GetExecutingAssembly().ManifestModule.Name);
                        Application.Current.Shutdown();
                    }
                }
            });
        }

        private static void Downdload(string url, string saveFilePath)
        {
            try
            {
                var webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Credentials = CredentialCache.DefaultCredentials;
                var webResponse = (HttpWebResponse)webRequest.GetResponse();

                var strResponse = webResponse.GetResponseStream();
               
                var strLocal = new FileStream(saveFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
              
                int bytesSize = 0;
                byte[] downBuffer = new byte[2048];

                while ((bytesSize = strResponse.Read(downBuffer, 0, downBuffer.Length)) > 0)
                {
                    strLocal.Write(downBuffer, 0, bytesSize);
                }
                strResponse.Close();
                strLocal.Close();
            }
            catch
            {
                // ignored
            }
        }

        private static int GetVersion()
        {
            try
            {
                ClearFiles();
                var xdoc = new XmlDocument();
                var request = WebRequest.Create(Properties.Settings.Default.UpdateUrl + VERSION_FILE_NAME) as HttpWebRequest;
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    xdoc.Load(response.GetResponseStream());
                }
                return Convert.ToInt32(xdoc.GetElementsByTagName("TicketSupport")[0].InnerText.Replace(".", ""));
            }
            catch
            {
                return 0;
            }
        }

        private static void ClearFiles()
        {
            if (File.Exists(MainDirSavePath + UPDATER_FILE_NAME))
            {
                File.Delete(MainDirSavePath + UPDATER_FILE_NAME);
            }
            if (File.Exists(MainDirSavePath + VERSION_FILE_NAME))
            {
                File.Delete(MainDirSavePath + VERSION_FILE_NAME);
            }
        }
    }
}

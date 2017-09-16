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

namespace TicketSupport
{
    public static class VersionHelper
    {
        private const string UPDATER_FILE_NAME = "autoupd.exe";
        private const string VERSION_FILE_NAME = "ticketsup.version";
        private static readonly string MainDirSavePath = System.AppDomain.CurrentDomain.BaseDirectory;
        public static void CheckUpdate()
        {
            Task.Run(() => {
                if (GetVersion() > Convert.ToInt32(Properties.Settings.Default.Version))
                {
                    Downdload(Properties.Settings.Default.UpdateUrl + UPDATER_FILE_NAME, MainDirSavePath + UPDATER_FILE_NAME);
                    if (File.Exists(UPDATER_FILE_NAME))
                    {
                        MessageBox.Show("New version exists. Click OK to download a new version.");
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
                var request = WebRequest.Create(Properties.Settings.Default.UpdateUrl + VERSION_FILE_NAME) as HttpWebRequest;
                var response = request.GetResponse() as HttpWebResponse;
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    return Convert.ToInt32(reader.ReadLine());
                }
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

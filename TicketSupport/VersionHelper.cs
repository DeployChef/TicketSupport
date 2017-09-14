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
        private const string UPDATER_FILE_NAME = "upd.exe";
        private const string VERSION_FILE_NAME = "version.txt";
        private static readonly string MainDirSavePath = System.AppDomain.CurrentDomain.BaseDirectory;
        public static void CheckUpdate()
        {
            if (GetVersion() > Convert.ToInt32(Properties.Settings.Default.Version))
            {
                Downdload(Properties.Settings.Default.UpdateUrl + UPDATER_FILE_NAME, MainDirSavePath + "upd.exe");
                if (File.Exists("upd.exe"))
                {
                    MessageBox.Show("New version exists. Click OK to download a new version.");
                    Process.Start("upd.exe", Assembly.GetExecutingAssembly().ManifestModule.Name);
                    Application.Current.Shutdown();
                }
            }
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
            if (File.Exists(MainDirSavePath + "upd.exe"))
            {
                File.Delete(MainDirSavePath + "upd.exe");
            }
            if (File.Exists(MainDirSavePath + "updaterver.txt"))
            {
                File.Delete(MainDirSavePath + "updaterver.txt");
            }
        }
    }
}

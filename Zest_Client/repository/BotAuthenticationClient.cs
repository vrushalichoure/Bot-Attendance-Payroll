using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Zest_Client.repository
{
    public class BotAuthenticationClient
    {

        public async Task<ServiceResponse> BotAuthentication(string loginID, string password)
        {
            string url = ConfigurationManager.AppSettings["url"];
            HttpClient cons = new HttpClient();
            cons.BaseAddress = new Uri(url);
            cons.DefaultRequestHeaders.Accept.Clear();
            cons.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var tag = new BotAuthenticationModal { loginID = loginID, password = password };

            try
            {
                string req = JsonConvert.SerializeObject(tag);
                HttpContent content = new StringContent(tag.ToString(), Encoding.UTF8, "application/json");

                HttpResponseMessage res = cons.PostAsync(url + "api/BotAuthentication/BotAuthenticationDetails", new StringContent(@"{""RequestJSON"":" + req + "}", Encoding.Default, "application/json")).Result;
                var data = await res.Content.ReadAsAsync<ServiceResponse>();
                //var login = data.ResponseJSON;
                return data;
            }
            catch (Exception ex)
            {
                WriteLogFile(new StringBuilder(ex.Message + " " + ex.InnerException != null ? ex.InnerException.InnerException.Message : ""));
                return new ServiceResponse();
            }
        }
        public class ServiceResponse
        {
            public string Status { get; set; }
            public string ServerDateTime { get; set; }
            public string ErrorList { get; set; }
            public BotAuthenticationModal ResponseJSON { get; set; }
        }
        public class BotAuthenticationModal
        {
            public string loginID { get; set; }
            public string password { get; set; }
            public int empID { get; set; }
            public BotAuthenticationModal ResponseJSON { get; set; }
        }
        public static string GetLogPath
        {
            get
            {
                if (!Directory.Exists(Environment.CurrentDirectory + "\\Log"))
                {
                    Directory.CreateDirectory(Environment.CurrentDirectory + "\\Log");
                }
                var _directory = new DirectoryInfo(Environment.CurrentDirectory + "\\Log");

                if (!String.IsNullOrEmpty(_directory.FullName))
                {

                    return _directory.FullName;
                }
                else
                {
                    return "";
                }
            }
        }
        public static void WriteLogFile(StringBuilder strLog)
        {
            // Log generation and Event entry  after complete process

            string strFileName = GetLogPath + "\\Log_" + System.DateTime.Now.ToString("dd_MM_yyyy").ToString() + ".txt";
            if (System.IO.File.Exists(strFileName))
            {
                System.IO.File.Delete(strFileName);
            }
            if (strLog.ToString().Length > 0)
                System.IO.File.WriteAllText(strFileName, strLog.ToString());
        }
    }
}
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Zest_Client.repository
{
    public class InOutTimeWorkingHrsClient
    {
        public async Task<InOutTimeServiceResponse> InOutTimeWorkingHrsDetails(string token, int emp,DateTime date)
        {
            string url = ConfigurationManager.AppSettings["url"];
            HttpClient cons = new HttpClient();
            cons.BaseAddress = new Uri(url);
            cons.DefaultRequestHeaders.Accept.Clear();
            cons.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            cons.DefaultRequestHeaders.Add("Authorization", token);
            var emp_id = new InOutTimeWorkingHrsModel { EmployeeID = emp,AttendanceDate=date.ToString("yyyy-MM-dd") };
            string apireq = JsonConvert.SerializeObject(emp_id);
            HttpContent apicontent = new StringContent(emp_id.ToString(), Encoding.UTF8, "application/json");
            HttpResponseMessage apires = cons.PostAsync(url+"api/InOutTimeWorkingHrs/GetInOutTimeWorkingHrsDetails", new StringContent(@"{""RequestJSON"":" + apireq + "}", Encoding.Default, "application/json")).Result;
            var data = await apires.Content.ReadAsAsync<InOutTimeServiceResponse>();
           return data;
         
            
        }


    }


    public class InOutTimeWorkingHrsModel
    {
        public int EmployeeID { get; set; }
        public TimeSpan WorkingHours { get; set; }
        public TimeSpan BreakTime { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public TimeSpan TotalHours { get; set; }
        public string AttendanceDate { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
        public InOutTimeWorkingHrsModel ResponseJSON { get; set; }

    }
    public class InOutTimeServiceResponse
    {
        public string Status { get; set; }
        public string ServerDateTime { get; set; }
        public string ErrorList { get; set; }
        public InOutTimeWorkingHrsModel ResponseJSON { get; set; }
    }

   
}

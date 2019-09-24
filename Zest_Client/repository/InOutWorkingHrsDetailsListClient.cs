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
    public class InOutWorkingHrsDetailsListClient
    {
        public async Task<InOutTimeDetailsServiceResponse> InOutTimeWorkingHrsDetailsRangeList(string token, int emp, DateTime start_date,DateTime end_date)
        {
            string url = ConfigurationManager.AppSettings["url"];
            HttpClient cons = new HttpClient();
            cons.BaseAddress = new Uri(url);
            cons.DefaultRequestHeaders.Accept.Clear();
            cons.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            cons.DefaultRequestHeaders.Add("Authorization", token);
            var emp_details = new InOutWorkingHrsListDetailsModel { EmployeeID = emp, Start_Date = start_date,End_Date=end_date };
            string api_parameter_passing = JsonConvert.SerializeObject(emp_details);
            HttpContent api_content = new StringContent(emp_details.ToString(), Encoding.UTF8, "application/json");
            HttpResponseMessage apires = cons.PostAsync(url+"api/InOutTimeListDetails/GetInTimeOutTimeListDetails", new StringContent(@"{""RequestJSON"":" + api_parameter_passing + "}", Encoding.Default, "application/json")).Result;
            var data = await apires.Content.ReadAsAsync<InOutTimeDetailsServiceResponse>();
            return data;
        }
    }
    public class InOutWorkingHrsListDetailsModel
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
        public List<InOutWorkingHrsListDetailsModel> ResponseJSON { get; set; }
        
    }
    public class InOutTimeDetailsServiceResponse
    {
        public string Status { get; set; }
        public string ServerDateTime { get; set; }
        public string ErrorList { get; set; }
        public List<InOutWorkingHrsListDetailsModel> ResponseJSON { get; set; }
    }

}

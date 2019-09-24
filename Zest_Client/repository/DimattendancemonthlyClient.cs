using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Zest_Client.repository
{
     public class DimattendancemonthlyClient
    {
        public async Task<ServiceResponseDim> DimattendancemonthlyDetails(string token, int emp,string month,string year)
        {
            string url = ConfigurationManager.AppSettings["url"];
            HttpClient cons = new HttpClient();
            cons.BaseAddress = new Uri(url);
            cons.DefaultRequestHeaders.Accept.Clear();
            cons.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            cons.DefaultRequestHeaders.Add("Authorization", token);
            var emp_id = new DimattendancemonthlyModel { EmpID = emp ,month=month,year=year};
            string proreq = JsonConvert.SerializeObject(emp_id);
            HttpContent procontent = new StringContent(emp_id.ToString(), Encoding.UTF8, "application/json");
            HttpResponseMessage prores = cons.PostAsync(url+"api/Dimattendancemonthly/DimattendancemonthlyDetails", new StringContent(@"{""RequestJSON"":" + proreq + "}", Encoding.Default, "application/json")).Result;
            var prodata = await prores.Content.ReadAsAsync<ServiceResponseDim>();
            var proname = prodata;
            // string name = proname.ToString();
            return proname;

        }

    }
    public class ServiceResponseDim
    {
        public string Status { get; set; }
        public string ServerDateTime { get; set; }
        public string ErrorList { get; set; }
        public DimattendancemonthlyModel ResponseJSON { get; set; }
    }
    public class DimattendancemonthlyModel
    {
        public int EmpID { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public string PayrollMonth { get; set; }
        public int CalenderDays { get; set; }
        public int WorkingDays { get; set; }
        public int FullDay { get; set; }
        public int HalfDay { get; set; }
        public decimal Absent { get; set; }
        public int LeavesFullDay { get; set; }
        public decimal LeavesHalfDay { get; set; }
        public int SandwichLeave { get; set; }
        public string TotalGrossHours { get; set; }
        public string TotalNetHours { get; set; }
        public string TotalTimesheetHours { get; set; }
        public string TotalWOHHours { get; set; }
        public string TotalBreakHours { get; set; }
        public string AvgGrossHours { get; set; }
        public string AvgNetHours { get; set; }
        public string AvgTimesheetHours { get; set; }
        public int PenaltyCount { get; set; }
        public decimal LateComing { get; set; }
        public decimal EarlyLeaving { get; set; }
        public int MissPunch { get; set; }
        public int WFHRequest { get; set; }
        public int WOHRequest { get; set; }
        public int TOURRequest { get; set; }
        public decimal LOPRequest { get; set; } 
        public decimal LOP_Absent { get; set; }
        public decimal LOPPenalty { get; set; }
        public decimal WOHTimeSheetHrs { get; set; }
        public decimal WeekOff { get; set; }
        public decimal PresentDays { get; set; }
    }

}

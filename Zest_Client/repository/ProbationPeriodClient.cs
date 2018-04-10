using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Zest_Client.repository
{
    public class ProbationPeriodClient
    {

        //public async Task<string> ProbationPeroid(string token, int emp)
        //{
        //    HttpClient cons = new HttpClient();
        //    cons.BaseAddress = new Uri("http://localhost:57144/");
        //    cons.DefaultRequestHeaders.Accept.Clear();
        //    cons.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        //    cons.DefaultRequestHeaders.Add("Authorization", token);
        //    var emp_id = new EmployeeDetails { EmpID = emp };
        //    string proreq = JsonConvert.SerializeObject(emp_id);
        //    HttpContent procontent = new StringContent(emp_id.ToString(), Encoding.UTF8, "application/json");
        //    HttpResponseMessage prores = cons.PostAsync("http://localhost:57144/api/ProbationPeriod/GetProbationPeriodDetails", new StringContent(@"{""RequestJSON"":" + proreq + "}", Encoding.Default, "application/json")).Result;
        //    var prodata = await prores.Content.ReadAsAsync<EmployeeDetails>();
        //    var proname = prodata.ResponseJSON.EmployeeName;
        //    string name = proname.ToString();
        //    return name;
        //}
        public async Task<ServiceResponse> ProbationPeroid(string token, int emp)
        {
            HttpClient cons = new HttpClient();
            cons.BaseAddress = new Uri("http://localhost:57144/");
            cons.DefaultRequestHeaders.Accept.Clear();
            cons.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            cons.DefaultRequestHeaders.Add("Authorization", token);
            var emp_id = new EmployeeDetails { EmpID = emp };
            string proreq = JsonConvert.SerializeObject(emp_id);
            HttpContent procontent = new StringContent(emp_id.ToString(), Encoding.UTF8, "application/json");
            HttpResponseMessage prores = cons.PostAsync("http://localhost:57144/api/Probation_Period/Probation_period_details", new StringContent(@"{""RequestJSON"":" + proreq + "}", Encoding.Default, "application/json")).Result;
            var prodata = await prores.Content.ReadAsAsync<ServiceResponse>();
            var proname = prodata;
           // string name = proname.ToString();
            return prodata;

        }

    }
    public class ServiceResponse
    {
        public string Status { get; set; }
        public string ServerDateTime { get; set; }
        public string ErrorList { get; set; }
        public Probation_periodModal ResponseJSON { get; set; }
    }
    public class Probation_periodModal
    {
        public int empID { get; set; }
        public int ProbationPeriod { get; set; }
        public DateTime JoinDate { get; set; }
        public int Experience { get; set; }
        public int empCode { get; set; }
        public string bloodGroup { get; set; }
        public string passportNumber { get; set; }
        public int incrementperiod { get; set; }
        public int DepartmentID { get; set; }
        public string BankName { get; set; }
        public Probation_periodModal ResponseJSON { get; set; }
    }
    public class EmployeeDetails
    {
        public int EmpID { get; set; }
        public string EmployeeName { get; set; }
        public string Email { get; set; }
        public EmployeeDetails ResponseJSON { get; set; }
    }

    
}

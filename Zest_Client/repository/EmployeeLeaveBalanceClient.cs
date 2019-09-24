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
    public class EmployeeLeaveBalanceClient
    {
        public async Task<EmployeeLeaveBalanceResponse> EmployeeLeaveBalanceDetails(string token, int emp)
        {
            string url = ConfigurationManager.AppSettings["url"];
            HttpClient cons = new HttpClient();
            cons.BaseAddress = new Uri(url);
            cons.DefaultRequestHeaders.Accept.Clear();
            cons.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            cons.DefaultRequestHeaders.Add("Authorization", token);
            var emp_id = new EmployeeLeaveBalance { EmployeeID = emp };
            string apireq = JsonConvert.SerializeObject(emp_id);
            HttpContent apicontent = new StringContent(emp_id.ToString(), Encoding.UTF8, "application/json");
            HttpResponseMessage apires = cons.PostAsync(url+"api/EmployeeLeaveBalance/GetEmployeeLeaveBalanceListDetails", new StringContent(@"{""RequestJSON"":" + apireq + "}", Encoding.Default, "application/json")).Result;
            var data = await apires.Content.ReadAsAsync<EmployeeLeaveBalanceResponse>();
            return data;
        }

    }
}

public class EmployeeLeaveBalance
{
    public int EmployeeID { get; set; }
    public decimal ClosingBalance { get; set; }
    public decimal OpeningBalance { get; set; }
    public string LeaveCategoryName { get; set; }

}
public class EmployeeLeaveBalanceResponse
{
    public string Status { get; set; }
    public string ServerDateTime { get; set; }
    public string ErrorList { get; set; }
    public List<EmployeeLeaveBalance> ResponseJSON { get; set; }
}


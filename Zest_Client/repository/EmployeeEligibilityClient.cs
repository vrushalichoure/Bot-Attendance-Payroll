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

    public class EmployeeEligibilityClient
    {
        public async Task<EmployeeEligibilityServiceResponse> EmployeeEligibilityDetails(string token, int emp, string LkpCode)
        {
            string url = ConfigurationManager.AppSettings["url"];
            HttpClient cons = new HttpClient();
            cons.BaseAddress = new Uri(url);
            cons.DefaultRequestHeaders.Accept.Clear();
            cons.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            cons.DefaultRequestHeaders.Add("Authorization", token);
            var emp_id = new EmployeeEligibilityModel {empID = emp, LkpCode = LkpCode };
            string apireq = JsonConvert.SerializeObject(emp_id);
            HttpContent apicontent = new StringContent(emp_id.ToString(), Encoding.UTF8, "application/json");
            HttpResponseMessage apires = cons.PostAsync(url+"api/EmployeeEligibility/EmployeeEligibilityDetails", new StringContent(@"{""RequestJSON"":" + apireq + "}", Encoding.Default, "application/json")).Result;
            var data = await apires.Content.ReadAsAsync<EmployeeEligibilityServiceResponse>();
            return data;

        }
    }
}

public class EmployeeEligibilityModel
{
    public int empID { get; set; }
    public int PolicyID { get; set; }
    public string PolicyName { get; set; }
    public int EligibilityTypeID { get; set; }
    public string LkpCode { get; set; }
    public EmployeeEligibilityModel ResponseJSON { get; set; }

}
public class EmployeeEligibilityServiceResponse
{
    public string Status { get; set; }
    public string ServerDateTime { get; set; }
    public string ErrorList { get; set; }
    public EmployeeEligibilityModel ResponseJSON { get; set; }
}


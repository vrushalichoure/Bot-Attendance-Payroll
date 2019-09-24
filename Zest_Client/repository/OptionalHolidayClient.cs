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
    public class OptionalHolidaysClient
    {
        public async Task<OptionalHolidayResponse> OptionalHolidaysDetails(string token, int emp)
        {
            string url = ConfigurationManager.AppSettings["url"];
            HttpClient cons = new HttpClient();
            cons.BaseAddress = new Uri(url);
            cons.DefaultRequestHeaders.Accept.Clear();
            cons.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            cons.DefaultRequestHeaders.Add("Authorization", token);
            var emp_id = new OptionalHolidayList { EmpID = emp };
            string apireq = JsonConvert.SerializeObject(emp_id);
            HttpContent apicontent = new StringContent(emp_id.ToString(), Encoding.UTF8, "application/json");
            HttpResponseMessage apires = cons.PostAsync(url + "api/OptionalHoliday/GetOptionalHolidayListDetails", new StringContent(@"{""RequestJSON"":" + apireq + "}", Encoding.Default, "application/json")).Result;
            var apidata = await apires.Content.ReadAsAsync<OptionalHolidayResponse>();
            return apidata;

        }

    }

    public class OptionalHolidayResponse
    {
        public List<OptionalHolidayList> ResponseJSON { get; set; }
    }

    public class ServiceResponseOptionalHoliday
    {
        public string Status { get; set; }
        public string ServerDateTime { get; set; }
        public string ErrorList { get; set; }
        public OptionalHolidayList ResponseJSON { get; set; }
    }

    public class OptionalHolidayList
    {
        public int EmpID { get; set; }
        public string HolidayName { get; set; }
        public DateTime ObservingDate { get; set; }
        public OptionalHolidayList ResponseJSON { get; set; }
    }
}

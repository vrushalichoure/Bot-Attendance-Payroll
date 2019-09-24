using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using Newtonsoft.Json.Linq;
using Zest_Client.repository;

namespace Bot_Attendance_Payroll.Dialogs
{
    [Serializable]

    public class Holidays : LuisDialog<object>
    {
        protected int employee_id { get; set; }
        public string details { get; set; }

        public async Task DisplayAllHoliday(IDialogContext context, IAwaitable<object> result)
        {
            var token = "ok";
            var empID = context.UserData.GetValue<string>("empID");
            var activity = await result as Activity;
            try
            {
                var all_holiday_details = new AllHolidaysClient();
                var holiday_response = await all_holiday_details.AllHolidaysDetails(token, Convert.ToInt32(empID));
                if (holiday_response != null && holiday_response.ResponseJSON != null)
                {
                    List<HolidayList> data = holiday_response.ResponseJSON;
                    List<string> values = new List<string>();

                    foreach (var dataresp in data)
                    {
                        values.Add("**Holiday Name**" + ":::" + dataresp.HolidayName);
                        values.Add("**Holiday Date**" + ":::" + dataresp.ObservingDate.ToLongDateString());
                        values.Add("------------------------------------------");
                        values.Add("------------------------------------------");
                    }
                    string all_holiday_list_value = string.Join("<br/>\r\n", values);
                    await context.PostAsync(all_holiday_list_value);
                   
                }
                 context.Done(true);
            }
            catch (Exception ex)
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory;
                StringBuilder sb = new StringBuilder();
                sb.Append("InnerException : " + ex.InnerException);
                sb.Append("All_Holidays");
                sb.Append(Environment.NewLine);
                sb.Append("Message : " + ex.Message);
                sb.Append(Environment.NewLine);
                System.IO.File.AppendAllText(System.IO.Path.Combine(filePath, "Exception_log.txt"), sb.ToString());
                sb.Clear();
                await context.PostAsync("Data not found");
                context.Done(true);
            }

        }

       
    }
}

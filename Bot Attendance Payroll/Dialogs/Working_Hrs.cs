using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Zest_Client.repository;

namespace Bot_Attendance_Payroll.Dialogs
{

    [Serializable]
    public class Working_Hrs : IDialog<object>
    {

        [Required]
        [DataType(DataType.Date, ErrorMessage = "Not a valid format")]
        public DateTime date { get; set; }
        string msg = "not valid";

        [Required]
        [DataType(DataType.Date, ErrorMessage = "Not a valid format")]
        protected DateTime start_date { get; set; }

        [Required]
        [DataType(DataType.Date, ErrorMessage = "Not a valid format")]
        protected DateTime end_date { get; set; }

        public async Task StartAsync(IDialogContext context)
        {
            var selection_form = FormDialog.FromForm(Working_Hrs_FormFlow.WorkingHrsForm, FormOptions.PromptInStart);
            context.Call(selection_form, Working_Hrs_Display_Selection);
        }

        private async Task Working_Hrs_Display_Selection(IDialogContext context, IAwaitable<Working_Hrs_FormFlow> result)
        {
            var form_flow_selection = await result;
            if (form_flow_selection.worktimeTypes.ToString().Equals("ForAParticularDate"))
            {
                await context.PostAsync("Which date details you want??");
                await context.PostAsync(" I understand this format ::**yyyy-mm-dd**");
                context.Wait(Working_Hrs_Details_For_Particular_Date);
            }
            if (form_flow_selection.worktimeTypes.ToString().Equals("ForADetailedList"))
            {
                await context.PostAsync("Please tell me start date:");
                await context.PostAsync(" I understand this format ::**yyyy-mm-dd**");
                context.Wait(Get_End_Date);
            }

        }

        private async Task Get_End_Date(IDialogContext context, IAwaitable<object> result)
        {
            var get_start_date = await result as Activity;
            start_date = DateTime.Parse(get_start_date.Text);
            var todays_date = DateTime.Today;
            if (start_date < todays_date)
            {
                await context.PostAsync("Please tell me end date:");
                await context.PostAsync(" I understand this format ::**yyyy-mm-dd**");
                context.Wait(Working_Hrs_Details_For_Detail_List);
            }
            else
            {
                context.PostAsync("Date is yet to come");
                context.Done(true);
            }

        }

        private async Task Working_Hrs_Details_For_Detail_List(IDialogContext context, IAwaitable<object> result)
        {
            var get_end_date = await result as Activity;
            end_date = DateTime.Parse(get_end_date.Text);
            var todays_date = DateTime.Today;

            if (end_date < todays_date)

            {
                var activity = await result as Activity;
                var token = "ok";
                var empID = context.UserData.GetValue<int>("empID");
                if (empID != null && empID != null)
                {
                    if (token != null)
                    {
                        
                        var in_out_details = new InOutWorkingHrsDetailsListClient();
                        var in_out_response = await in_out_details.InOutTimeWorkingHrsDetailsRangeList(token, empID, start_date, end_date);
                        if (in_out_response.ResponseJSON!=null && in_out_response!=null && in_out_response.ResponseJSON.Count != 0)
                        {
                            List<InOutWorkingHrsListDetailsModel> data = in_out_response.ResponseJSON;
                            List<string> values = new List<string>();
                            foreach (var dataresp in data)
                            {
                                values.Add("**Date**" + ":::" + DateTime.Parse(dataresp.AttendanceDate).ToLongDateString());
                                values.Add("**Net-Hrs**" + ":::" + dataresp.WorkingHours);
                                values.Add("**Break-Time**" + ":::" + dataresp.BreakTime);
                                values.Add("**Gross-Hrs**" + ":::" + dataresp.TotalHours);
                                values.Add("------------------------------------------");
                                values.Add("------------------------------------------");
                            }
                            string in_out_time_list_value = string.Join("<br/>\r\n", values);
                            await context.PostAsync(in_out_time_list_value);
                        }
                        else
                        {
                            await context.PostAsync("Data not found");
                        }

                    }

                    else
                    {
                        await context.PostAsync("Need to Login to access data");
                        await context.PostAsync("Please Type **'Hello'** to Login ");

                    }


                }

                else
                {
                    await context.PostAsync("Please Type **'Hello'** to Login ");
                }
                context.Done(true);
            }
            else
            {
                context.PostAsync("Date is yet to come");

            }

        }


        public async Task Working_Hrs_Details_For_Particular_Date(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var get_date = await result as Activity;
                date = DateTime.Parse(get_date.Text);
                var activity = await result as Activity;
                var token = "ok";
                var empID = context.UserData.GetValue<int>("empID");
                var in_out_details = new InOutTimeWorkingHrsClient();
                var in_out_response = await in_out_details.InOutTimeWorkingHrsDetails(token, empID, date);
                    if (in_out_response.ResponseJSON != null &&  in_out_response.ResponseJSON.AttendanceDate != null)
                    {
                    var data = new InOutTimeWorkingHrsModel();
                    data.ResponseJSON= in_out_response.ResponseJSON;
                    await context.PostAsync(
                        $"**DATE**:{DateTime.Parse(data.ResponseJSON.AttendanceDate).ToLongDateString()}<br>" +
                        $"**Net-Working-Hrs**:{data.ResponseJSON.WorkingHours}\n\n" +
                        $"**Break-Time**:{data.ResponseJSON.BreakTime}\n\n" +
                        $"**Gross-Hrs**:{data.ResponseJSON.TotalHours}\n\n");
                    }

                

                else
                {
                    await context.PostAsync("Data Not found");
                }

            }
            catch (Exception ex)
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory;
                StringBuilder sb = new StringBuilder();
                sb.Append("InnerException : " + ex.InnerException);
                sb.Append("Total gross Hours");
                sb.Append(Environment.NewLine);
                sb.Append("Message : " + ex.Message);
                sb.Append(Environment.NewLine);
                System.IO.File.AppendAllText(System.IO.Path.Combine(filePath, "Exception_log.txt"), sb.ToString());
                sb.Clear();
                await context.PostAsync("Data not found");
                context.Done(true);
            }
            context.Done(true);
        }       
    }
}





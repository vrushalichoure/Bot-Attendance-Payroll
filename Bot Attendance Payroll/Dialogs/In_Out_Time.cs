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
    public class In_Out_Time : IDialog<object>
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
            var selection_in_out_time = FormDialog.FromForm(InOutTimeFormFlow.InOutForm, FormOptions.PromptInStart);
            context.Call(selection_in_out_time, In_Out_Time_Display_Selection);
        }

        private async Task In_Out_Time_Display_Selection(IDialogContext context, IAwaitable<InOutTimeFormFlow> result)
        {
            var form_flow_selection = await result;
            if (form_flow_selection.selectionType.ToString().Equals("ForAParticularDate"))
            {
                await context.PostAsync("Which date details you want??");
                await context.PostAsync(" I understand this format ::**yyyy-mm-dd**");
                context.Wait(In_Out_Details_For_Particular_Date);
            }
            if (form_flow_selection.selectionType.ToString().Equals("ForADetailedList"))
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
            await context.PostAsync("Please tell me end date:");
            await context.PostAsync(" I understand this format ::**yyyy-mm-dd**");
            context.Wait(In_Out_Details_For_Detail_List);
        }

        private async Task In_Out_Details_For_Detail_List(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var get_end_date = await result as Activity;
                end_date = DateTime.Parse(get_end_date.Text);

                var activity = await result as Activity;         
                var token = "ok";
                var empID = context.UserData.GetValue<string>("empID");
                var in_out_details = new InOutWorkingHrsDetailsListClient();
                var in_out_response = await in_out_details.InOutTimeWorkingHrsDetailsRangeList(token, Convert.ToInt32(empID), start_date, end_date);
                if (in_out_response.ResponseJSON.Count != 0)
                {
                    List<InOutWorkingHrsListDetailsModel> data = in_out_response.ResponseJSON;
                    List<string> values = new List<string>();
                    foreach (var dataresp in data)
                    {
                        values.Add("**Date**" + ":::" + DateTime.Parse(dataresp.AttendanceDate).ToLongDateString());
                        values.Add("**In-Time**" + ":::" + DateTime.Parse(dataresp.InTime).ToLongTimeString());
                        values.Add("**Out-Time**" + ":::" + DateTime.Parse(dataresp.OutTime).ToLongTimeString());
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
            catch (Exception ex)
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory;
                StringBuilder sb = new StringBuilder();
                sb.Append("InnerException : " + ex.InnerException);
                sb.Append(Environment.NewLine);
                sb.Append("Message : " + ex.Message);
                sb.Append(Environment.NewLine);
                System.IO.File.AppendAllText(System.IO.Path.Combine(filePath, "Exception_log.txt"), sb.ToString());
                sb.Clear();
                await context.PostAsync("Data not found");
                context.Done(true);
            }
            //else
            //{
            //    await context.PostAsync("Need to Login to access data, Please type hello to login");
            //    //context.Call(new UserLogin(), ResumeAfteNullToken);
            //    await context.PostAsync("Please Type **'Hello'** to Login ");

            //}


        }


        public async Task In_Out_Details_For_Particular_Date(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var get_date = await result as Activity;
                date = DateTime.Parse(get_date.Text);
                var activity = await result as Activity;
                // StateClient stateClient = activity.GetStateClient();
                // BotData userData = await stateClient.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
                // if (userData != null && userData.Data != null)
                // {
                // var obj = JObject.Parse(userData.Data.ToString());
                //  var token = (string)obj["Authorization_Token_Attendance"];
                var token = "ok";

                //  if (token != null)
                //   {
                // StateClient empCode1 = activity.GetStateClient();
                //BotData empCodeu = await empCode1.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
                // var ob = JObject.Parse(empCodeu.Data.ToString());
                // var empID = (int)obj["empID"];
                var empID = context.UserData.GetValue<string>("empID");
                var in_out_details = new InOutTimeWorkingHrsClient();
                var in_out_response = await in_out_details.InOutTimeWorkingHrsDetails(token, Convert.ToInt32(empID), date);
                if (in_out_response.ResponseJSON != null && !string.IsNullOrWhiteSpace(in_out_response.ResponseJSON.AttendanceDate))
                {
                    if (!string.IsNullOrWhiteSpace(in_out_response.ResponseJSON.InTime) && !string.IsNullOrWhiteSpace(in_out_response.ResponseJSON.OutTime))
                    {
                        var data = new InOutTimeWorkingHrsModel();
                        data.ResponseJSON = in_out_response.ResponseJSON;

                        await context.PostAsync($"**DATE**:{DateTime.Parse(data.ResponseJSON.AttendanceDate).ToLongDateString()}<br>" +
                        $"**In-Time**:{DateTime.Parse(data.ResponseJSON.InTime).ToLongTimeString()}<br>" +
                        $"**Out-Time**:{DateTime.Parse(data.ResponseJSON.OutTime).ToLongTimeString()}<br>");
                        context.Done(true);
                    }
                    else
                    {
                        await context.PostAsync($"{DateTime.Parse(in_out_response.ResponseJSON.AttendanceDate).ToLongTimeString()}<br>" + "This day was holiday or leave taken");
                        context.Done(true);
                    }
                }

                else
                {
                    await context.PostAsync("Data Not found");
                }

                //    }
                //    else
                //    {
                //        await context.PostAsync("Need to Login to access data");
                //        //context.Call(new UserLogin(), ResumeAfteNullToken);
                //        await context.PostAsync("Please Type **'Hello'** to Login ");

                //    }


                //}
                //else
                //{
                //    await context.PostAsync("Please Type **'Hello'** to Login ");
                //}

               
            }
            catch (Exception ex)
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory;
                StringBuilder sb = new StringBuilder();
                sb.Append("InnerException : " + ex.InnerException);
                sb.Append(Environment.NewLine);
                sb.Append("Message : " + ex.Message);
                sb.Append(Environment.NewLine);
                System.IO.File.AppendAllText(System.IO.Path.Combine(filePath, "Exception_log.txt"), sb.ToString());
                sb.Clear();
                await context.PostAsync("Data Not found");
                context.Done(true);
            }
        }

    }
}





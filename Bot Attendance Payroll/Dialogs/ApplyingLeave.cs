using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Bot_Attendance_Payroll.Dialogs
{
    [Serializable]
    public class ApplyingLeave : IDialog<object>
    {
        protected DateTime start_date { get; set; }

        protected DateTime end_date { get; set; }

        protected DateTime todays_date { get; set; }


        public async Task StartAsync(IDialogContext context)
        {
            var Type = FormDialog.FromForm(ApplyLeaveFormFlow.ApplyLeaveForm, FormOptions.PromptInStart);
            context.Call(Type, LeaveSelection);

        }

        private async Task LeaveSelection(IDialogContext context, IAwaitable<ApplyLeaveFormFlow> result)
        {
            var selection = await result;
            if ((selection.leaveTypes.ToString().Equals("Apply_for_Sick_Leave_sl")) &&
                (selection.halfDayorfullDay.ToString().Equals("Apply_for_FullDay")))
            {
                await context.PostAsync("Please tell me from which date shall I apply???");
                await context.PostAsync(" I understand this format ::**yyyy-mm-dd**");
                context.Wait(Get_End_Date);

            }
            else if((selection.leaveTypes.ToString().Equals("Apply_for_Paid_Leave_pl")) &&
                 (selection.halfDayorfullDay.ToString().Equals("Apply_for_FullDay")))
            {
                await context.PostAsync("Please tell me from which date shall I apply???");
                await context.PostAsync(" I understand this format ::**yyyy-mm-dd**");
                context.Wait(Get_End_Date);

            }
            else if ((selection.leaveTypes.ToString().Equals("Apply_for_Vacation_Days_VD"))
               && (selection.halfDayorfullDay.ToString().Equals("Apply_for_FullDay")))
            {
                await context.PostAsync("Please tell me from which date shall I apply???");
                await context.PostAsync(" I understand this format ::**yyyy-mm-dd**");
                context.Wait(Get_End_Date);
            }
            else if ((selection.leaveTypes.ToString().Equals("Apply_for_Maternity_Leave_MTL")) && (selection.halfDayorfullDay.ToString().Equals("Apply_for_FullDay")))
            {
                await context.PostAsync("Please tell me from which date shall I apply???");
                await context.PostAsync(" I understand this format ::**yyyy-mm-dd**");
                context.Wait(Get_End_Date);

            }
            else if ((selection.leaveTypes.ToString().Equals("Apply_for_Paternity_Leave_PTL"))
               && (selection.halfDayorfullDay.ToString().Equals("Apply_for_FullDay")))
            {

                await context.PostAsync("Please tell me from which date shall I apply???");
                await context.PostAsync(" I understand this format ::**yyyy-mm-dd**");
                context.Wait(Get_End_Date);
            }
            else if ((selection.leaveTypes.ToString().Equals("Apply_for_Study_Leave_STL")) &&(selection.halfDayorfullDay.ToString().Equals("Apply_for_FullDay")))
            {
                await context.PostAsync("Please tell me from which date shall I apply???");
                await context.PostAsync(" I understand this format ::**yyyy-mm-dd**");
                context.Wait(Get_End_Date);

            }
            else if (selection.halfDayorfullDay.ToString().Equals("Apply_for_HalfDay"))
            {
                await context.PostAsync("Please tell me from which date shall I apply???");
                context.Wait(Half_Day_Request);
            }
        }
        public async Task Half_Day_Request(IDialogContext context, IAwaitable<object> result)
        {
            var get_start_date = await result as Activity;
            start_date = DateTime.Parse(get_start_date.Text);
            var todays_date = DateTime.Today;
            try
            {
                if (start_date > todays_date)
                {
                    var activity = await result as Activity;
                    var empID = context.UserData.GetValue<string>("empID");
                    await context.PostAsync($"Your request for half day for {start_date.ToShortDateString()} is forwarded to project manager");
                    context.Done(true);
                }

                else
                {
                    context.PostAsync("Date has already passed");
                    context.Done(true);
                }

            } catch(Exception ex)
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory;
                StringBuilder sb = new StringBuilder();
                sb.Append("InnerException : " + ex.InnerException);
                sb.Append("Apply Leave");
                sb.Append(Environment.NewLine);
                sb.Append("Message : " + ex.Message);
                sb.Append(Environment.NewLine);
                System.IO.File.AppendAllText(System.IO.Path.Combine(filePath, "Exception_log.txt"), sb.ToString());
                sb.Clear();
                await context.PostAsync("Data not found");
                context.Done(true);
            }
           
        }
        public async Task Get_End_Date(IDialogContext context, IAwaitable<object> result)
        {
            var get_start_date = await result as Activity;
            start_date = DateTime.Parse(get_start_date.Text);
            var todays_date = DateTime.Today;

            if (start_date > todays_date)
            {
                await context.PostAsync("Please tell me till which date shall I apply???");
                context.Wait(Apply_leave_Details_For_Detail_List);
            }

            else
            {
                context.PostAsync("Date has already passed");
                context.Done(true);
            }
        }

        public async Task Apply_leave_Details_For_Detail_List(IDialogContext context, IAwaitable<object> result)
        {
            var get_end_date = await result as Activity;
            end_date = DateTime.Parse(get_end_date.Text);
            var todays_date = DateTime.Today;
            try
            {
                if (end_date > todays_date)
                {
                    var activity = await result as Activity;
                    var token = "ok";
                    var empID = context.UserData.GetValue<string>("empID");
                    await context.PostAsync($"request for leave from {start_date.ToShortDateString()} till {end_date.ToShortDateString()} forwarded to project manager");
                    context.Done(true);
                }
                else
                {
                    context.PostAsync("Date has already passed");
                    context.Done(true);
                }
            }
            catch (Exception ex)
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory;
                StringBuilder sb = new StringBuilder();
                sb.Append("InnerException : " + ex.InnerException);
                sb.Append("Apply Leave");
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



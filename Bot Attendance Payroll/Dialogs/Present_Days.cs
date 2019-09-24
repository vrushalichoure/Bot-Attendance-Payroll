using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using Zest_Client.repository;

namespace Bot_Attendance_Payroll.Dialogs
{
    [Serializable]
    public class Present_Days : IDialog<object>
    {
        protected string month { get; set; }
        protected string year { get; set; }

        public async Task StartAsync(IDialogContext context)
        {
            context.PostAsync("Please enter payroll month number.");
            context.Wait(Enter_month);
        }
        private async Task Enter_month(IDialogContext context, IAwaitable<object> result)
        {
            var mon = await result as Activity;
            month = mon.Text;
            await context.PostAsync("Enter year:");
            context.Wait(Halfdays);
        }
        private async Task Halfdays(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            year = activity.Text;
          
            var empID = context.UserData.GetValue<int>("empID");
            if (empID != null && empID != null)
            {
               
                var token = "ok";


                if (token == null)
                {
                    await context.PostAsync("Need to Login to access data");
                    // context.Call(new UserLogin(), ResumeAfteNullToken);
                    await context.PostAsync("Please Type **'Hello'** to Login ");
                    context.Done(true);

                }
                if (token != null)
                {

                   
                    var Dimattendancemonthly = new DimattendancemonthlyClient();
                    var DimattendancemonthlyDetails = await Dimattendancemonthly.DimattendancemonthlyDetails(token, empID, month, year);
                    if (DimattendancemonthlyDetails != null && DimattendancemonthlyDetails.ResponseJSON != null
                        && DimattendancemonthlyDetails.ResponseJSON.PayrollMonth != null)
                    {
                        await context.PostAsync($"Your present days in payroll month {DimattendancemonthlyDetails.ResponseJSON.PayrollMonth}" +
                        $" are {DimattendancemonthlyDetails.ResponseJSON.MissPunch} ");
                      
                        context.Done(true);
                    }
                    else
                    {
                        await context.PostAsync("Data not found ");
                        context.Done(true);
                    }
                }
            }
            else
            {
                await context.PostAsync("Need to Login to access data");
           
                await context.PostAsync("Please Type **'Hello'** to Login ");
                context.Done(true);
            }
        }
    }
}
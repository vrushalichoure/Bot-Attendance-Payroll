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
    public class HalfDay : IDialog<object>
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
            await context.PostAsync("Enter year");
            context.Wait(Halfdays);
        }
        private async Task Halfdays(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var activity = await result as Activity;
                year = activity.Text;
                // StateClient stateClient = activity.GetStateClient();
                // BotData userData = await stateClient.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
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

                        // StateClient empCode1 = activity.GetStateClient();
                        //  BotData empCodeu = await empCode1.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
                        //  var ob = JObject.Parse(empCodeu.Data.ToString());
                        //  var empID = (int)obj["empID"];
                        var empID = context.UserData.GetValue<int>("empID");
                        var Dimattendancemonthly = new DimattendancemonthlyClient();
                        var DimattendancemonthlyDetails = await Dimattendancemonthly.DimattendancemonthlyDetails(token, empID, month, year);
                        if (DimattendancemonthlyDetails != null && DimattendancemonthlyDetails.ResponseJSON != null
                            && DimattendancemonthlyDetails.ResponseJSON.PayrollMonth != null)
                        {
                            await context.PostAsync($"Your half days in payroll month {DimattendancemonthlyDetails.ResponseJSON.PayrollMonth}" +
                            $" are {DimattendancemonthlyDetails.ResponseJSON.HalfDay} ");
                            //context.Call(new ResumeAfter(), this.ResumeAfterTaskDialog);
                            context.Done(true);
                        }
                        else
                        {
                            await context.PostAsync("Data not found ");
                            context.Done(true);
                        }
                    }
                }

            }
            catch(Exception ex)
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("InnerException : " + ex.InnerException);
                sb.Append("Halfdays");
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
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Zest_Client.repository;

namespace Bot_Attendance_Payroll.Dialogs
{
    [Serializable]
    public class Probation_Period : IDialog<object>
    {
         protected int employee_id { get; set; }

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Do you want know your probation duration?<br>"+"*Please enter employee id*");
            context.Wait(this.abc);
        }

        private async Task abc(IDialogContext context, IAwaitable<object> result)
        {
          //  var empid= await result as Activity;
            var activity = await result as Activity;
           // employee_id = int.Parse(empid.Text);
            StateClient stateClient = activity.GetStateClient();
            BotData userData = await stateClient.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
            var obj = JObject.Parse(userData.Data.ToString());
            var token = (string)obj["token"];

            StateClient empCode1 = activity.GetStateClient();
            BotData empCodeu = await empCode1.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
            var ob = JObject.Parse(empCodeu.Data.ToString());
            var empID = (int)obj["empID"];

            if (token == null)
            {
                await context.PostAsync("Need to Login to access data");
                context.Call(new UserLogin(), ResumeAfteNullToken);

            }
            if (token != null)
            {
                var probation_period = new ProbationPeriodClient();
                var probation_period_response = await probation_period.ProbationPeroid(token,empID);
                await context.PostAsync($"Your Probation is of  {probation_period_response.ResponseJSON.ProbationPeriod} months");
                context.Done(true);
            }
        }
        private async Task ResumeAfteNullToken(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Login Successful!!!");
        }
    }
}
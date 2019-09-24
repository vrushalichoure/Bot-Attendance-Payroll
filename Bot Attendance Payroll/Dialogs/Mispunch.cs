using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Bot_Attendance_Payroll.Dialogs
{
    [Serializable]
    public class Misspunch : IDialog<string>
    {

        protected DateTime start_date { get; set; }

        protected DateTime end_date { get; set; }

        protected DateTime todays_date { get; set; }

        protected string ans { get; set; }

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Do you want to apply for mispunch??");
            context.Wait(User_Response);
        }
        public async Task User_Response(IDialogContext context, IAwaitable<object> result)
        {
            var message = await result as Activity;
            ans = message.Text;
            if (ans.Equals("yes", StringComparison.InvariantCultureIgnoreCase))
            {
                await context.PostAsync("Please tell me for which date shall I apply???");
                await context.PostAsync(" I understand this format ::**yyyy-mm-dd**");
                context.Wait(Apply_For_Mispunch);
            }
            else
            {
                context.PostAsync("ok");
                context.Done(true);
            }
        }
        public async Task Response_After_Passed_Start_Date(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Please tell me from which date shall I apply???");
            await context.PostAsync(" I understand this format ::**yyyy-mm-dd**");
            context.Wait(Apply_For_Mispunch);

        }



        public async Task Apply_For_Mispunch(IDialogContext context, IAwaitable<object> result)
        {
            var get_end_date = await result as Activity;
            end_date = DateTime.Parse(get_end_date.Text);
            var todays_date = DateTime.Today;

            if (end_date > todays_date)
            {
                await context.PostAsync($"request for mispunch on {end_date.ToShortDateString()}  forwarded to project manager");
                context.Done(true);

            }
            else
            {
                context.PostAsync("Date has already passed");
                context.PostAsync("Please Re-Enter your details");
                context.Wait(Response_After_Passed_Start_Date);

            }

        }

    }

}

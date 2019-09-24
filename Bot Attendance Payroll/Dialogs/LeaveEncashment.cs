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
    public class LeaveEncashment : IDialog<object>
    {
        public string ans { get; set; }
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Do you want to encash your remaning leaves for this assignmnet cycle?");
            context.Wait(this.user_answer);
        }

        private async Task user_answer(IDialogContext context, IAwaitable<object> result)
        {
            var message = await result as Activity;
            ans = message.Text;
            if(ans.Equals("yes",StringComparison.InvariantCultureIgnoreCase))
            {

                await context.PostAsync("your request is forwarded to project manager");
                context.Done(true);

            }
            else
            {
                await context.PostAsync("ok");
                context.Done(true);
            }
            }
    }
}
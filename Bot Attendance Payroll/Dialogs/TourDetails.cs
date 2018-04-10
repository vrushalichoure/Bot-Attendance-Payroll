using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Bot_Attendance_Payroll.Dialogs
{
   
    [Serializable]
    public class TourDetails : IDialog<object>
    {   //public String s = "session";
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("tour det");
            context.Done(true);
            //context.Wait(MessageReceived);
        }

        private async Task MessageReceived(IDialogContext context, IAwaitable<object> result)
        {
            var message = await result;
            await context.PostAsync("Tour Details");
            context.Done(true);
        }
    }
}
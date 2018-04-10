using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Bot_Attendance_Payroll.Dialogs
{
    [Serializable]
    public class Gross_Hrs : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("You want see gross_hrs details..");
            context.Wait(this.Gross_Hrs_Details);
        }

        private async Task Gross_Hrs_Details(IDialogContext context, IAwaitable<object> result)
        {
            var message = await result;
            await context.PostAsync("your gross hrs are 8hrs 30 min");
            context.Done(true);
        }
    }
}
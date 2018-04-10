using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Bot_Attendance_Payroll.Dialogs
{
    [Serializable]
    public class Section80D : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Do you want see decalaration under 80/d");
            context.Wait(this.Declaration_under_80D);
        }

        private async Task Declaration_under_80D(IDialogContext context, IAwaitable<object> result)
        {
            var message = await result;
            await context.PostAsync("Your declaration under 80/D is..");
            context.Done(true);
        }
    }
}
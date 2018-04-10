using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Bot_Attendance_Payroll.Dialogs
{
    [Serializable]
    public class LopDetails : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Do You want see Loss of Pay details..");
            context.Wait(this.Lop_Details);
        }

        private async Task Lop_Details(IDialogContext context, IAwaitable<object> result)
        {
            var message = await result;
            await context.PostAsync("your loss of pay details are as follows");
            context.Done(true);
        }
    }
}
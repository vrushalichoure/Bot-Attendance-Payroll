using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Threading.Tasks;

namespace Bot_Attendance_Payroll.Dialogs
{
    [Serializable]
    public class Late_Comings : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {

            await context.PostAsync("Late coming report");
            context.Done(true);
        }

    }
}
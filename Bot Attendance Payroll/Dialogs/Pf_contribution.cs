using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Bot_Attendance_Payroll.Dialogs
{
    [Serializable]
    public class Pf_contribution : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Its 95634 rs");
            context.Done(true);
        }

    }
}
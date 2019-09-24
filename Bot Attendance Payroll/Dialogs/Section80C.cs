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
    public class Section80C : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("90152 rs");
            context.Done(true);
        }
    }
}
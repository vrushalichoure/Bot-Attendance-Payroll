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
    public class Base_pay : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Its 59045 rs");
            context.Done(true);
        }

     
    }
}
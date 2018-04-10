using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Bot_Attendance_Payroll.Dialogs
{
    [Serializable]
    public class OutdoorDuty : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.PostAsync("Yes you can Apply for Outdoor duty");
            context.Done(true);
        }

        
        
    }
}
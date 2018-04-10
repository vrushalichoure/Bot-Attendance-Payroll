using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Bot_Attendance_Payroll.Dialogs
{

    [Serializable]
    public class ResumeAfter : IDialog<object>
    {   //public String s = "session";
        public async Task StartAsync(IDialogContext context)
        {
           // await context.PostAsync("tour det");
            context.Done(true);
            //context.Wait(MessageReceived);
        }

       
    }
}
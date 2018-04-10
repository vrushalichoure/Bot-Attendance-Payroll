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
    public class Profile : IDialog
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("You can get information about:<br>"+ "1.join date<br>"+" 2.exp<br>"+"3.pf number<br>"+"Type any of them to get details");

            context.Done(true);
        }
       
    }
}
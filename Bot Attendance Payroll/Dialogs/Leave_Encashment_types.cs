using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot_Attendance_Payroll.Dialogs
{
    [Serializable]
    public class Leave_Encashment_types : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Leaves you can encashed are<br>"+"**sl<br>**"+"**cl<br>**"+"**pl<br>**");
            context.Done(true);
        }
    }
}

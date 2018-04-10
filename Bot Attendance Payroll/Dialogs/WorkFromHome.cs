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
    public class WorkFromHome : IDialog<string>
    {

        public async Task StartAsync(IDialogContext context)
        {
             await context.PostAsync("Yes you can apply for work from home");

            //var workform = FormDialog.FromForm(WorkFromHomeFormFlow.WorkFromHomeForm, FormOptions.PromptInStart);
            //context.Call(workform, ResumeAfterCallingWorkFromHome);
            context.Done(true);
        }

        private async Task ResumeAfterCallingWorkFromHome(IDialogContext context, IAwaitable<WorkFromHomeFormFlow> result)
        {
            await context.PostAsync("Request done");
            context.Done(true);
        }
    }
}
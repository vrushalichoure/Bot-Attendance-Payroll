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
    public class Payslip : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Which month payslip?");
            context.Wait(this.payslip);
        }

        private async Task payslip(IDialogContext context, IAwaitable<object> result)
        {
            var message = await result;
            var msg = context.MakeMessage();
            await context.PostAsync("Payslip Generated..");



            Attachment attachment1 = new Attachment();
            attachment1.ContentType = "image/png";
            attachment1.ContentUrl = "https://documentation.thesaurussoftware.com/images/tpm/feature-payslipb1.PNG";
            msg.Attachments.Add(attachment1);
            await context.PostAsync(msg);
            context.Done(message);

        }
    }
}
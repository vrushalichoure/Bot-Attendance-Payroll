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
            await context.PostAsync("Do you want see decalaration under 80/c");
            context.Wait(this.Declaration_under_80C);
        }

        private async Task Declaration_under_80C(IDialogContext context, IAwaitable<object> result)
        {

            var message = await result;
            var msg = context.MakeMessage();
            await context.PostAsync("Base Pay Details..");
            Attachment attachment1 = new Attachment();
            attachment1.ContentType = "image/png";
            attachment1.ContentUrl = "C:/Users/Vrushali/source/repos/Bot Attendance Payroll/Bot Attendance Payroll/Images/declaration.png";
            msg.Attachments.Add(attachment1);
            await context.PostAsync(msg);
            context.Done(message);

        }
    }
}
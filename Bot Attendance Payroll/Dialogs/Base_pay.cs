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
            await context.PostAsync("Do you want to see salary details..");
            context.Wait(Base_Pay);
        }

        private async Task Base_Pay(IDialogContext context, IAwaitable<object> result)
        {
            var message = await result;
            var msg = context.MakeMessage();
            await context.PostAsync("Base Pay Details..");
            Attachment attachment1 = new Attachment();
            attachment1.ContentType = "image/png";
            attachment1.ContentUrl = "C:/Users/Vrushali/source/repos/Bot Attendance Payroll/Bot Attendance Payroll/Images/base_pay.png";
            msg.Attachments.Add(attachment1);
            await context.PostAsync(msg);
            context.Done(message);

        }
    }
}
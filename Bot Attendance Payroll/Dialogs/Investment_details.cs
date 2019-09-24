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
    public class Investment_details : IDialog<object>
    {
        protected string answer { get; set; }
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("1.Public Provident Fund: 2,00,000\n\n" +
                                    "2.NSC Certificate: 20,000\n\n" +
                                    "3.Mutula Fund: 10,000\n\n" +
                                    "4.Pension Plan: 2000\n\n");
            await context.PostAsync("Would you like to know complete details?");
            context.Wait(Investment_Slip);
        }

        private async Task Investment_Slip(IDialogContext context, IAwaitable<object> result)
        {
            var ans = await result as Activity;
            answer = ans.Text;
            if (answer.Equals("yes", StringComparison.InvariantCultureIgnoreCase))
            {
                var message = await result;
                var msg = context.MakeMessage();
                await context.PostAsync("Investment details..");

                Attachment attachment1 = new Attachment();
                attachment1.ContentType = "image/png";
                attachment1.ContentUrl = "http://im.rediff.com/getahead/2011/jul/12tax7.gif";
                msg.Attachments.Add(attachment1);
                await context.PostAsync(msg);
                context.Done(message);

            }

            else
            {
                context.PostAsync("ok");
                context.Done(true);
            }

        }
    }
}
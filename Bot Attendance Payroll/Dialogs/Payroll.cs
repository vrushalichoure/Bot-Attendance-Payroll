using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Zest_Client.repository;

namespace Bot_Attendance_Payroll.Dialogs
{
    [Serializable]
    public class Payroll : IDialog<object>
    {



        public async Task StartAsync(IDialogContext context)
        {
            var Type = FormDialog.FromForm(PayrollFormFlow.PayrollForm, FormOptions.PromptInStart);
            context.Call(Type, PayrollSelection);

        }

        private async Task PayrollSelection(IDialogContext context, IAwaitable<PayrollFormFlow> result)
        {
            try
            {
                var selection = await result;
                if (selection.parollType.ToString().Equals("Payslip"))
                {
                    context.Call(new Payslip(), this.ResumeAfterTaskDialog);
                }
                else if (selection.parollType.ToString().Equals("Investment_Details"))
                {
                    context.Call(new Investment_details(), this.ResumeAfterTaskDialog);
                }
                else if (selection.parollType.ToString().Equals("Tds_deductions"))
                {
                    context.Call(new Tds_deduction(), this.ResumeAfterTaskDialog);
                }

            }
            catch (Exception ex)
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory;
                StringBuilder sb = new StringBuilder();
                sb.Append("InnerException : " + ex.InnerException);
                sb.Append("Apply Leave");
                sb.Append(Environment.NewLine);
                sb.Append("Message : " + ex.Message);
                sb.Append(Environment.NewLine);
                System.IO.File.AppendAllText(System.IO.Path.Combine(filePath, "Exception_log.txt"), sb.ToString());
                sb.Clear();
                await context.PostAsync("Data not found");
                context.Done(true);
            }

        }
        private async Task ResumeAfterTaskDialog(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("how can i help you ?");
            context.Done(true);
        }
    }


}



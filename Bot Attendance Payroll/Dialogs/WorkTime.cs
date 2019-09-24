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
    public class WorkTime : IDialog<object>
    {



        public async Task StartAsync(IDialogContext context)
        {
            var Type = FormDialog.FromForm(WorkTimeFormFlow.WorkTimeForm, FormOptions.PromptInStart);
            context.Call(Type, WorkTimeSelection);

        }

        private async Task WorkTimeSelection(IDialogContext context, IAwaitable<WorkTimeFormFlow> result)
        {
            try
            {
                var selection = await result;
                if (selection.worktimeTypes.ToString().Equals("Total_Gross_Hrs"))
                {
                    context.Call(new Total_Gross_Hours(), this.ResumeAfterTaskDialog);
                }
                else if (selection.worktimeTypes.ToString().Equals("Totat_Net_Hrs"))
                {
                    context.Call(new Total_Net_Hours(), this.ResumeAfterTaskDialog);
                }
                else if (selection.worktimeTypes.ToString().Equals("Detail_Working_Hrs"))
                {
                    context.Call(new Working_Hrs(), this.ResumeAfterTaskDialog);
                }
                
                else if (selection.worktimeTypes.ToString().Equals("In_Out_Time"))
                {
                    context.Call(new In_Out_Time(), this.ResumeAfterTaskDialog);
                }
            }
            catch (Exception ex)
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory;
                StringBuilder sb = new StringBuilder();
                sb.Append("InnerException : " + ex.InnerException);
                sb.Append("Work Time");
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
            context.Done(true);
        }
    }
  

}



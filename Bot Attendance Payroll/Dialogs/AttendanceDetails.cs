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
    public class AttendanceDetails : IDialog<object>
    {

        public async Task StartAsync(IDialogContext context)
        {
            var Type = FormDialog.FromForm(AttendanceDetailsFormFlow.AttendanceDetailsForm, FormOptions.PromptInStart);
            context.Call(Type, AttendanceDetailSelection);

        }

        private async Task AttendanceDetailSelection(IDialogContext context, IAwaitable<AttendanceDetailsFormFlow> result)
        {
            try
            {
                var selection = await result;
                if (selection.attendenceType.ToString().Equals("Lop"))
                {
                    context.Call(new LopDetails(), this.ResumeAfterTaskDialog);
                }
                else if (selection.attendenceType.ToString().Equals("Early_Leavings"))
                {
                    context.Call(new Early_leavings(), this.ResumeAfterTaskDialog);
                }
                else if (selection.attendenceType.ToString().Equals("Absent_Days"))
                {
                    context.Call(new Absent(), this.ResumeAfterTaskDialog);
                }

                else if (selection.attendenceType.ToString().Equals("Missed_Punch"))
                {
                    context.Call(new Mispunch(), this.ResumeAfterTaskDialog);
                }
                else if (selection.attendenceType.ToString().Equals("Present_Days"))
                {
                    context.Call(new Present_Days(), this.ResumeAfterTaskDialog);
                }
            }
            catch (Exception ex)
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory;
                StringBuilder sb = new StringBuilder();
                sb.Append("InnerException : " + ex.InnerException);
                sb.Append("Attendance Details");
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



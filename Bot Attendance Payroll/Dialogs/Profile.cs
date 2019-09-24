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
    public class Profile : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            var Type = FormDialog.FromForm(ProfileFormFlow.ProfileForm, FormOptions.PromptInStart);
            context.Call(Type, ProfileSelection);

        }

        private async Task ProfileSelection(IDialogContext context, IAwaitable<ProfileFormFlow> result)
        {
            try
            {
                var selection = await result;
                if (selection.profileType.ToString().Equals("Probation_Period"))
                {
                    try
                    {
                        var token = "ok";
                        var empID = context.UserData.GetValue<int>("empID");
                        var probation_period = new ProbationPeriodClient();
                        var probation_period_response = await probation_period.ProbationPeroid(token,empID);
                        
                        {
                            await context.PostAsync($"Your Probation is of  {probation_period_response.ResponseJSON.ProbationPeriod} months");
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        string filePath = AppDomain.CurrentDomain.BaseDirectory;
                        StringBuilder sb = new StringBuilder();
                        sb.Append("InnerException : " + ex.InnerException);
                        sb.Append("Probation_Period");
                        sb.Append(Environment.NewLine);
                        sb.Append("Message : " + ex.Message);
                        sb.Append(Environment.NewLine);
                        System.IO.File.AppendAllText(System.IO.Path.Combine(filePath, "Exception_log.txt"), sb.ToString());
                        sb.Clear();
                        await context.PostAsync("Data not found");
                        context.Done(true);
                    }



                }
                else if (selection.profileType.ToString().Equals("Joining_Date"))
                {
                    try
                    {
                        var token = "ok";
                        var empID = context.UserData.GetValue<int>("empID");
                        var probation_period = new ProbationPeriodClient();
                        var probation_period_response = await probation_period.ProbationPeroid(token, empID);
                        if (probation_period_response != null && probation_period_response.ResponseJSON != null)
                        {
                            await context.PostAsync($"Your Joining date is {probation_period_response.ResponseJSON.JoinDate.Value.ToLongDateString()} ");
                           
                        }
                        else
                        {
                            await context.PostAsync("Data not found ");
                        }

                    }
                    catch (Exception ex)
                    {
                        string filePath = AppDomain.CurrentDomain.BaseDirectory;
                        StringBuilder sb = new StringBuilder();
                        sb.Append("InnerException : " + ex.InnerException);
                        sb.Append("Join_date");
                        sb.Append(Environment.NewLine);
                        sb.Append("Message : " + ex.Message);
                        sb.Append(Environment.NewLine);
                        System.IO.File.AppendAllText(System.IO.Path.Combine(filePath, "Exception_log.txt"), sb.ToString());
                        sb.Clear();
                        await context.PostAsync("Data not found");
                        context.Done(true);
                    }


                }
                else if (selection.profileType.ToString().Equals("Experience"))
                {
                    try
                    {
                        var token = "ok";
                        var empID = context.UserData.GetValue<int>("empID");
                        var probation_period = new ProbationPeriodClient();
                        var probation_period_response = await probation_period.ProbationPeroid(token, empID);
                        if (probation_period_response != null && probation_period_response.ResponseJSON != null)
                        {
                            await context.PostAsync($"Your Experience in Cygnet is {probation_period_response.ResponseJSON.Experience} years");
                           
                        }
                        else
                        {
                            await context.PostAsync("Data not found ");
                        }
                    }
                    catch (Exception ex)
                    {
                        string filePath = AppDomain.CurrentDomain.BaseDirectory;
                        StringBuilder sb = new StringBuilder();
                        sb.Append("InnerException : " + ex.InnerException);
                        sb.Append("Experience");
                        sb.Append(Environment.NewLine);
                        sb.Append("Message : " + ex.Message);
                        sb.Append(Environment.NewLine);
                        System.IO.File.AppendAllText(System.IO.Path.Combine(filePath, "Exception_log.txt"), sb.ToString());
                        sb.Clear();
                        await context.PostAsync("Data not found");
                        context.Done(true);

                    }

                }
                context.Done(true);
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
    }

}



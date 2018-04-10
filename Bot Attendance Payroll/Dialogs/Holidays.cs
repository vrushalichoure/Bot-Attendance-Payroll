using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;

namespace Bot_Attendance_Payroll.Dialogs
{
    [Serializable]
    public class Holidays : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            var Type = FormDialog.FromForm(HolidayFormFlow.HolidayForm, FormOptions.PromptInStart);
            context.Call(Type, HolidaySelection);
        }

        private async  Task HolidaySelection(IDialogContext context, IAwaitable<HolidayFormFlow> result)
        {
            var token = await result;
            if (token.holidayType.ToString().Equals("Optional"))
            {
                await context.PostAsync("Optional Holiday List");
                context.Done(true);
            }
            else if (token.holidayType.ToString().Equals("Mandatory"))
            {
                await context.PostAsync("Mandatory Holiday List");
                context.Done(true);
            }
            else if (token.holidayType.ToString().Equals("All"))
            {
                await context.PostAsync("All Holiday List");
                context.Done(true);
            }

        }

        
            private async Task DisplayHoliday(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("All holidays");

            using (HttpClient client = new HttpClient())
            {
                
                //Assuming that the api takes the user message as a query paramater
                string RequestURI = "http://localhost:62943/api/Holiday";
                HttpResponseMessage responsemMsg = await client.GetAsync(RequestURI);
                if (responsemMsg.IsSuccessStatusCode)
                {
                    var apiResponse = await responsemMsg.Content.ReadAsStringAsync();

                    //Post the API response to bot again
                    await context.PostAsync($"Response is {apiResponse}");

                }

                context.Done<object>(null);


                //context.Wait(MessageReceivedAsync);
            }
        }
    }
}
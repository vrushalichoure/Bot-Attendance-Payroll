using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace Bot_Attendance_Payroll.Dialogs
{
    [Serializable]
    public class Tour:IDialog<object>
    {

        protected DateTime start_date { get; set; }

        protected DateTime end_date { get; set; }

        protected DateTime todays_date { get; set; }

        protected string ans { get; set; }

        public String reply { get; set; }

        public const string AngryMood = "🙄";

        public async Task StartAsync(IDialogContext context)
        {
            var Type = FormDialog.FromForm(YesNoSelection.YesNoForm, FormOptions.PromptInStart);
            context.Call(Type, User_Response);
        }

        public async Task User_Response(IDialogContext context, IAwaitable<YesNoSelection> result)
        {
            var selection = await result;
            if (selection.option.ToString().Equals("Yes"))
            {
                await context.PostAsync("Please tell me from which date shall I apply???");
                await context.PostAsync(" I understand this format ::**yyyy-mm-dd**");
                context.Wait(Get_End_Date);
            }
            else if(selection.option.ToString().Equals("No"))
            {
                context.PostAsync("ok");
                context.Done(true);
            }
        }

        public async Task Response_After_Passed_Start_Date(IDialogContext context, IAwaitable<object> result)
        {
                await context.PostAsync("Please tell me from which date shall I apply???");
                await context.PostAsync(" I understand this format ::**yyyy-mm-dd**");
                context.Wait(Get_End_Date);
                
        }

        public async Task Get_End_Date(IDialogContext context, IAwaitable<object> result)
        {
            var get_start_date = await result as Activity;
            start_date = DateTime.Parse(get_start_date.Text);
            var todays_date = DateTime.Today;

            if (start_date > todays_date)
            {
                await context.PostAsync("Please tell me till which date shall I apply???");
                await context.PostAsync(" I understand this format ::**yyyy-mm-dd**");
                context.Wait(Apply_Outdoor_Duty);
            }

            else
            {
                context.PostAsync("Date has already passed");
                context.PostAsync("Re-Enter Your Details");
                context.Wait(Response_After_Passed_Start_Date);
            }
        }

        public async Task Apply_Outdoor_Duty(IDialogContext context, IAwaitable<object> result)
        {
            var get_end_date = await result as Activity;
            end_date = DateTime.Parse(get_end_date.Text);
            var todays_date = DateTime.Today;

            if (end_date > todays_date)
            {
                await context.PostAsync($"request for tour from {start_date.ToShortDateString()} till {end_date.ToShortDateString()} forwarded to project manager");
                context.PostAsync("*Would you like check ticket availability??*");
                var Type = FormDialog.FromForm(YesNoSelection.YesNoForm, FormOptions.PromptInStart);
                context.Call(Type, MessageReceivedAsync);
            }
            else
            {
                context.PostAsync("Date has already passed");
                context.PostAsync("Re-Enter your details");
                context.Wait(Response_After_Passed_Start_Date);
            }

        }

    



        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<YesNoSelection> result)
        {

            var selection = await result;
            if (selection.option.ToString().Equals("Yes"))
            {
                var selectedCard = await result;
                var message = context.MakeMessage();
                var attachment = GetSelectedCard(selectedCard);
                message.Attachments.Add(attachment);
                await context.PostAsync(message);
                context.Done(true);
            }
            else if(selection.option.ToString().Equals("No"))
            {
                context.PostAsync("I could have helped you better" + AngryMood);
                context.Done(true);
            }
        }
   

        private Attachment GetSelectedCard(object selectedCard)
        {
            var heroCard = new HeroCard
            {
                Title = "I Can Help you to book a Ticket",
                Subtitle = "Lets book ticket for you",
                Text = "You can check flight and train booking from here only ....!!",
                Images = new List<CardImage>
                {
                    new CardImage("https://cdn0.iconfinder.com/data/icons/transportation-13/512/ticket_booking-256.png")
                },
                Buttons = new List<CardAction> {
                    new CardAction(ActionTypes.OpenUrl, "Book a Flight", value: "https://www.google.co.in/flights/#search"),
                    new CardAction(ActionTypes.OpenUrl, "Book a Train", value: "https://www.irctc.co.in/eticketing/loginHome.jsf"),
                    
                }
            };

            return heroCard.ToAttachment();
        }
        

    }
    
}

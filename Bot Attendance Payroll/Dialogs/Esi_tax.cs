using Chronic;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services.Description;
using Zest_Client;

namespace Bot_Attendance_Payroll.Dialogs
{
    [Serializable]
    public class Esi_tax : IDialog<object>
    {
        private string t;

        protected string username { get; set; }
        protected string password { get; set; }
        protected string AuthenticationType { get; set; }

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync(" fa");
            context.Wait(abc);
            
          //  return Task.CompletedTask;
        }
        private async Task abc(IDialogContext context, IAwaitable<object> result)
        {
            var pass = await result as Activity;
            var activity = await result as Activity;
            password =(pass.Text);
            StateClient stateClient = activity.GetStateClient();
            BotData userData = await stateClient.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
            var obj = JObject.Parse(userData.Data.ToString());
            var token = (string)obj["token"];
            if(token==null)
            {
                await context.PostAsync("Need to Login to access data");
                context.Call(new UserLogin(), ResumeAfteNullToken);
            
            }
            if (token != null)
            {
                var pc = new ProductCalling();
                string t = await pc.ProductDetails(token);

                await context.PostAsync($"Response is {t}");
                context.Done(true);
            }
        }

        private async Task ResumeAfteNullToken(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Login Successful!!!");
        }
    }
    public class SessionResponse
    {
        public int Data { get; set; }
        public string Etag { get; set; }
        public SessionResponse ResponseJson { get;set; }
    }

    public class AuthenticationResponse
    {
        public int EmpID { get; set; }
        public string AuthorizationToken { get; set; }
    }
    public class ServiceResponse
    {
        public string Status { get; set; }
        public string ServerDateTime { get; set; }
        public string ErrorList { get; set; }
        public AuthenticationResponse ResponseJSON { get; set; }
    }
    public class AuthenticationRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string AuthenticationType { get; set; }
    }
    public class TestRequest
    {
        public int id { get; set; }
    }

    public class Product
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public Product ResponseJSON { get; set; }
    }
}
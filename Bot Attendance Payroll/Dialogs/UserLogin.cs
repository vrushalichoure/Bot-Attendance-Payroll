using Autofac;
using Chronic;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services.Description;
using Zest_Client;
using Zest_Client.repository;


namespace Bot_Attendance_Payroll.Dialogs
{
    [Serializable]
    public class UserLogin : IDialog<object>
    {
        protected string username { get; set; }
        protected string password { get; set; }
        protected string AuthenticationType { get; set; }

        
        public async Task StartAsync(IDialogContext context)
        {

            context.PostAsync("Please Enter your Username");

            context.Wait(Enter_password);
        }
        private async Task Enter_password(IDialogContext context, IAwaitable<object> result)
        {

            var user = await result as Activity;
            username = (user.Text);
            await context.PostAsync("Enter Password");
            context.Wait(Login);
        }
        private async Task Login(IDialogContext context, IAwaitable<object> result)
        {

            var pass = await result as Activity;
            password = (pass.Text);
            var auth = new BotAuthenticationClient();
            var s = await auth.BotAuthentication(username, password);
           // int id = s.ResponseJSON.empID;
           // string s1 = s.ResponseJSON.loginID;

            if (s.ResponseJSON != null && s.ResponseJSON.loginID!= null )
            {
                int id = s.ResponseJSON.empID;
                string s1 = s.ResponseJSON.loginID;
                context.UserData.SetValue("empID", id);
                var ac = new AuthenticationCalling();
                string t = await ac.TokenCalling(username, password);

                if (t == null)
                {
                    context.PostAsync("you have enterd wrong credentials");
                    await context.PostAsync("Enter your username");
                    context.Wait(Enter_password);
                }

                if (t != null)
                {
                    //await context.PostAsync($"Response is {t}");
                    await context.PostAsync("Welcome " + s.ResponseJSON.loginID+"--"+"You have successfully logged in");
                    //await context.PostAsync("Your EmployeeDetails Code is " + s.ResponseJSON.empID);
                    
                    //await context.PostAsync("UserLogin successfull");
                    context.UserData.SetValue("Authorization_Token_Attendance", t);
                    context.Done(true);
                }
            }
            else
            {
                // await context.PostAsync("Wrong id or password" + " re-login<br>" + "Enter your username");
                context.PostAsync("you have enterd wrong credentials");
                await context.PostAsync("Re-Enter your Username");
                context.Wait(Enter_password);
            }
            /*conversation.updatecontainer(
               builder =>
               {
                   var store = new inmemorydatastore();
                   builder.register(c => store)
                          .keyed<ibotdatastore<botdata>>(t)
                          .asself()
                          .singleinstance();

               });*/

        }
       
    }
   
}
    
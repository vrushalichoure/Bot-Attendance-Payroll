using Autofac;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace Bot_Attendance_Payroll
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            
            AuthBot.Models.AuthSettings.Mode = ConfigurationManager.AppSettings["ActiveDirectory.Mode"];
            AuthBot.Models.AuthSettings.EndpointUrl = ConfigurationManager.AppSettings["ActiveDirectory.EndpointUrl"];
            AuthBot.Models.AuthSettings.Tenant = ConfigurationManager.AppSettings["ActiveDirectory.Tenant"];
            AuthBot.Models.AuthSettings.RedirectUrl = ConfigurationManager.AppSettings["ActiveDirectory.RedirectUrl"];
            AuthBot.Models.AuthSettings.ClientId = ConfigurationManager.AppSettings["ActiveDirectory.ClientId"];
            AuthBot.Models.AuthSettings.ClientSecret = ConfigurationManager.AppSettings["ActiveDirectory.ClientSecret"];

            //Conversation.UpdateContainer(
            //builder =>
            //{
            //    //var store = new InMemoryDataStore();
            //    var store = new TableBotDataStore(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);
            //    builder.Register(c => store)
            //           .Keyed<IBotDataStore<BotData>>(AzureModule.Key_DataStore)
            //           .AsSelf()
            //           .SingleInstance();
            //});

            //var store = new InMemoryDataStore();
            var store = new TableBotDataStore(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);
            Conversation.UpdateContainer(
                       builder =>
                       {
                           builder.Register(c => store)
                                     .Keyed<IBotDataStore<BotData>>(AzureModule.Key_DataStore)
                                     .AsSelf()
                                     .SingleInstance();

                           builder.Register(c => new CachingBotDataStore(store,
                                      CachingBotDataStoreConsistencyPolicy
                                      .ETagBasedConsistency))
                                      .As<IBotDataStore<BotData>>()
                                      .AsSelf()
                                      .InstancePerLifetimeScope();


                       });
        }
    }
}

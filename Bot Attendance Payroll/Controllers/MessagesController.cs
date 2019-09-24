using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Bot_Attendance_Payroll.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Web.Configuration;
using System.Diagnostics;

namespace Bot_Attendance_Payroll
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        /// 

        internal static IDialog<object> MakeRoot()
        {
            return Chain.From(() => new AttendanceDialog());
            //return Chain.ContinueWith(new UserLogin(), AttendanceDialog);

        }

        //internal static IDialog<FormFlowGenralDetails> MakeRoot()
        //{
        //    return Chain.From(() => FormDialog.FromForm(FormFlowGenralDetails.BuildForm));
        //}

       
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)

            {
            if (activity.Type == ActivityTypes.Message)
            {
               

                await Conversation.SendAsync(activity, MakeRoot);
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
              
                
                IConversationUpdateActivity update = message;
                var client = new ConnectorClient(new Uri(message.ServiceUrl), new MicrosoftAppCredentials());
                if (update.MembersAdded != null && update.MembersAdded.Any())
                {
                    foreach (var newMember in update.MembersAdded)
                    {
                        if (newMember.Id != message.Recipient.Id)
                        {
                            var reply = message.CreateReply();
                            var msg = message.CreateReply();
                            //Attachment attachment1 = new Attachment();
                          //  attachment1.ContentType = "image/gif";
                            //attachment1.ContentUrl = "https://cdn.dribbble.com/users/27231/screenshots/2432051/welcome.gif";
                            //msg.Attachments.Add(attachment1);
                           // client.Conversations.ReplyToActivityAsync(msg);
                            reply.Text = "Please type **HELLO** to begin chat";
                            client.Conversations.ReplyToActivityAsync(reply);
                        }

                    }
                }
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
                IConversationUpdateActivity update = message;
                var client = new ConnectorClient(new Uri(message.ServiceUrl), new MicrosoftAppCredentials());  
                if (message.Action == "add")
                {
                    var reply = message.CreateReply();
                    var msg = message.CreateReply();
                    Attachment attachment1 = new Attachment();
                    attachment1.ContentType = "image/gif";
                    attachment1.ContentUrl = "https://cdn.dribbble.com/users/27231/screenshots/2432051/welcome.gif";
                    msg.Attachments.Add(attachment1);
                    client.Conversations.ReplyToActivityAsync(msg);
                    reply.Text = "Please type **HELLO** to begin chat";
                    client.Conversations.ReplyToActivityAsync(reply);
                }
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}
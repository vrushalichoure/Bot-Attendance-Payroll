using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Attendance_Payroll.Dialogs
{
    [Serializable]
    public class UserDetailsFromFlow
    {
     
            [Prompt("Enter Username:")]
            public String UserName { get; set; }

            [Prompt("Enter Password:")]
            public String Password { get; set; }


            public static IForm<UserDetailsFromFlow> UserNamePasswordForm()
            {
                return new FormBuilder<UserDetailsFromFlow>()
                    .Field(nameof(UserName))//calling fileds with priority
                    .Field(nameof(Password))
                    .Build();


            }
        }
}
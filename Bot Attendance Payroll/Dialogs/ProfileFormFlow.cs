using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Attendance_Payroll.Dialogs
{

    [Serializable]
    public class ProfileFormFlow
    {
        public ProfileType profileType;

        public static IForm<ProfileFormFlow> ProfileForm()
        {
            return new FormBuilder<ProfileFormFlow>()
                .Message("Please select any one category from here..")
                  .Field(nameof(profileType))
                    .Build();
        }
        [Serializable]
        public enum ProfileType
        {
            Probation_Period = 1,
            Joining_Date = 2,
            Experience = 3
        }
    }
}
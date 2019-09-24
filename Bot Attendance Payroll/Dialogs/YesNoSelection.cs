using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Attendance_Payroll.Dialogs
{

    [Serializable]
    public class YesNoSelection
    {

        public IsApply option;

        public static IForm<YesNoSelection> YesNoForm()
        {
            return new FormBuilder<YesNoSelection>()
                .Message("Please select one option")//printing msg to bot
                .Field(nameof(option))
                .Build();
        }


       
        [Serializable]
        public enum IsApply
        {
            Yes = 1,
            No = 2
        }
    }

}





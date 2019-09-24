using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Attendance_Payroll.Dialogs
{

    [Serializable]
    public class HelpFormFlow
    {
        public HelpType helpType;

        public static IForm<HelpFormFlow> HelpForm()
        {
            return new FormBuilder<HelpFormFlow>()
                .Message("Please select any one category from here..")
                  .Field(nameof(helpType))
                    .Build();
        }
        [Serializable]
        public enum HelpType
        {
            Profile = 1,
            List_of_Holidays = 2,
            Working_Hours=3,
            Apply_for_leave=4,
            Leave_Balance = 5,
            Attendance_Details = 6,
            Apply_for_tour=7,
            Payslip = 8
        }
    }
}
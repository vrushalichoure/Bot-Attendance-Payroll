using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Attendance_Payroll.Dialogs
{
    [Serializable]
    public class Working_Hrs_FormFlow
    {


        public WorkTimeTypes worktimeTypes;   
        
        public static IForm<Working_Hrs_FormFlow> WorkingHrsForm()
        {

            return new FormBuilder<Working_Hrs_FormFlow>()
                 .Field(nameof(worktimeTypes))
                 .Build();
        }


        [Serializable]
        public enum WorkTimeTypes
        {
            ForAParticularDate = 1,
            ForADetailedList = 2,
        }

        
    }

}
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Attendance_Payroll.Dialogs
{
    [Serializable]
    public class InOutTimeFormFlow
    {
        public SelectionType selectionType;


        public static IForm<InOutTimeFormFlow> InOutForm()
        {
            return new FormBuilder<InOutTimeFormFlow>()
                 .Field(nameof(selectionType))  
                 .Build();
        }


        [Serializable]
        public enum SelectionType
        {
            ForAParticularDate = 1,
            ForADetailedList = 2,
        }


    }
}
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Attendance_Payroll.Dialogs
{
    [Serializable]
    public class WorkTimeFormFlow
    {


        public WorkTimeTypes worktimeTypes;
        
        
        
        public static IForm<WorkTimeFormFlow> WorkTimeForm()
        {

            return new FormBuilder<WorkTimeFormFlow>()
                .Message("Want to check your working hrs????")
                  .Field(nameof(worktimeTypes))
                    .Build();
        }


        [Serializable]
        public enum WorkTimeTypes
        {
            Total_Gross_Hrs =1,
            Totat_Net_Hrs = 2,
            Detail_Working_Hrs=3,
            In_Out_Time = 4
            
        }

        
    }




}
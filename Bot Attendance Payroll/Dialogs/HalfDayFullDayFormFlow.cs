using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System;

namespace Bot_Attendance_Payroll.Dialogs
{
    [Serializable]
    public class HalfDayFullDayFormFlow
    {
        public LeaveSelectionTypes leaveTypes;
        public static IForm<HalfDayFullDayFormFlow> HalfDayFullDayLeaveForm()
        {
            return new FormBuilder<HalfDayFullDayFormFlow>()
                .Message("Please select your leave category from here..")
                .Field(nameof(leaveTypes))
                .Build();


        }


        [Serializable]
        public enum LeaveSelectionTypes
        {
            Apply_for_Half_Day = 1,
            Apply_for_Full_Day = 2
        }


    }
}
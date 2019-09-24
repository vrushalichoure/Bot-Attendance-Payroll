using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Attendance_Payroll.Dialogs
{

    [Serializable]
    public class AttendanceDetailsFormFlow
    {
        public AttendanceType  attendenceType;

        public static IForm<AttendanceDetailsFormFlow> AttendanceDetailsForm()
        {
            return new FormBuilder<AttendanceDetailsFormFlow>()
                .Message("Please select your choice from here..")
                  .Field(nameof(attendenceType))
                    .Build();
        }
        [Serializable]
        public enum AttendanceType
        {
            Lop = 1,
            Early_Leavings = 2,
            Absent_Days = 3,
            Missed_Punch = 4,
            Present_Days = 5
        }
    }
}
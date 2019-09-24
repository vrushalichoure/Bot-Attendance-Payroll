using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Attendance_Payroll.Dialogs
{

    [Serializable]
    public class PayrollFormFlow
    {
        public PayRollType parollType;

        public static IForm<PayrollFormFlow> PayrollForm()
        {
            return new FormBuilder<PayrollFormFlow>()
                .Message("Please select your payroll category from here..")
                  .Field(nameof(parollType))
                    .Build();
        }
        [Serializable]
        public enum PayRollType
        {
            Payslip = 1,
            Investment_Details = 2,
            Tds_deductions = 3
        }
    }
}
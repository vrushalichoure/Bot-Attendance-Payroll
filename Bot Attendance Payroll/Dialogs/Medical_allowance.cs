﻿using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Bot_Attendance_Payroll.Dialogs
{
    [Serializable]
    public class Medical_allowance : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {

            await context.PostAsync("Its 1500rs");
            context.Done(true);
        }

       
    }
}
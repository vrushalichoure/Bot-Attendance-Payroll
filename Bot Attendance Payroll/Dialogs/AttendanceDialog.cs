using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using Microsoft.Bot.Builder.Dialogs.Internals;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Connector;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Zest_Client.repository;

namespace Bot_Attendance_Payroll.Dialogs
{
    [LuisModel("f9a17798-4d13-4310-90a5-19c8bd5582da", "74c7cfca6c3144538ac0f2a21af3b1d3")]
    [Serializable]
    public class AttendanceDialog : LuisDialog<object>
    {
        //**************************************************Greeting*********************************************************************************     
        // Greeting Intent
        [LuisIntent("Greeting")]
        public async Task Greeting(IDialogContext context, IAwaitable<object> activity, LuisResult result)
        {
            var msg = await activity as Activity;
            if (msg.Text.Equals("hello", StringComparison.InvariantCultureIgnoreCase))
            {
                context.PostAsync("Authenticating user...");
                //Authenticating User After User Types Hello.Calling User Login Dialog ask for username & password
                context.Call(new UserLogin(), ResumeAfterTaskDialog);
            }

            else if (msg.Text.Equals("good morning", StringComparison.InvariantCultureIgnoreCase))
            {
                await context.PostAsync("Good Morning Friend");
            }

            else if (msg.Text.Equals("hi", StringComparison.InvariantCultureIgnoreCase))
            {
                await context.PostAsync("hi how can I help you");
            }
        }

        //********************************************************ATTENDANCE***************************************************************************
        //1.Leave Encashment
        //Checks if user can apply for leave encashment.
        [LuisIntent("Leave_Encashment")]
        public async Task LeaveEncashment(IDialogContext context, LuisResult result)
        {
            context.Call(new LeaveEncashment(), this.ResumeAfterTaskDialog);
        }

        //2.Shows which type of leaves user can encash
        [LuisIntent("Leave_Encashment_types")]
        public async Task Leave_Encashment_types(IDialogContext context, LuisResult result)
        {
            context.Call(new Leave_Encashment_types(), this.ResumeAfterTaskDialog);
        }

        //3.Tour Check if user can apply for tour
        [LuisIntent("Tour")]
        public async Task CallingTourMethod(IDialogContext context, LuisResult result)
        {
            context.Call(new Tour(), this.ResumeAfterTaskDialog);
        }

        //4 Tour_Details get tour details
        [LuisIntent("Tour_Details ")]
        public async Task Tour_Details(IDialogContext context, LuisResult result)
        {
            context.Call(new TourDetails(), this.ResumeAfterTaskDialog);
        }

        //5.OutdoorDuty check if user can apply for outdoor duty
        [LuisIntent("Outdoor")]
        public async Task OutdoorApply(IDialogContext context, LuisResult result)
        {
            context.Call(new OutdoorDuty(), this.ResumeAfterTaskDialog);
        }

        //6.Outdoor duty details give users outdoor duty details
        [LuisIntent("Outdoor_Details")]
        public async Task Outdoor_Details(IDialogContext context, LuisResult result)
        {
            context.Call(new OutdoorDutyDetails(), this.ResumeAfterTaskDialog);
        }

        //7 Work From Home Check if user can apply for work from home
        [LuisIntent("WorkFromHome")]
        public async Task CallingWorkFromHome(IDialogContext context, LuisResult result)
        {
            context.Call(new WorkFromHome(), this.ResumeAfterTaskDialog);
        }

        //8 Work From Home Details of a employee
        [LuisIntent("WorkFromHome_Details")]
        public async Task WorkFromHomeDetails(IDialogContext context, LuisResult result)
        {
            context.Call(new WorkFromHomeDetails(), this.ResumeAfterTaskDialog);
        }

        //9.Misspunch details of user for 1 month
        [LuisIntent("Mispunch")]
        private async Task CallingMisspunchMethod(IDialogContext context, LuisResult result)
        {
            context.Call(new Misspunch(), this.ResumeAfterTaskDialog);
        }


        // 10.Compoff Check user can apply for compoff 
        [LuisIntent("Compoff_apply ")]
        private async Task CallingCompOffMethod(IDialogContext context, LuisResult result)
        {
            context.Call(new Compoff(), this.ResumeAfterTaskDialog);
        }
        //11.Apply for a leave employee applying for leave
        [LuisIntent("Apply_leave ")]
        public async Task Apply_Leave(IDialogContext context, LuisResult result)
        {
            context.Call(new ApplyingLeave(), ResumeAfterLeaveApply);
        }

        private async Task ResumeAfterLeaveApply(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Your leave request is applied successfully");
        }

        //12 Loss of Pay details
        [LuisIntent("Lop")]
        public async Task Lop(IDialogContext context, LuisResult result)
        {
            context.Call(new LopDetails(), this.ResumeAfterTaskDialog);
        }

        //13 Show Late Comings details
        [LuisIntent("Late_Comings")]
        public async Task Late_Comings(IDialogContext context, LuisResult result)
        {
            context.Call(new Late_Comings(), this.ResumeAfterTaskDialog);
        }

        //14 Show  Early leaving details
        [LuisIntent("Early_leavings")]
        public async Task Early_leavings(IDialogContext context, LuisResult result)
        {
            context.Call(new Net_Hrs(), this.ResumeAfterTaskDialog);
        }

        //*************************************************PROFILE***********************************************************************************

        // 15.From Flow for Profile
        [LuisIntent("Profile")]
        public async Task CallingProfileMethod(IDialogContext context, LuisResult result)
        {

            context.Call(new Profile(), ResumeAfterTaskDialog);
        }

        //16:Probation_Period
        [LuisIntent("Probation_Period")]
        public async Task Probation_period(IDialogContext context,IAwaitable<object> results, LuisResult result)
        {
            var activity = await results as Activity;
            StateClient stateClient = activity.GetStateClient();
            BotData userData = await stateClient.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
            var obj = JObject.Parse(userData.Data.ToString());
            var token = (string)obj["token"];


            if (token == null)
            {
                await context.PostAsync("Need to Login to access data");
                context.Call(new UserLogin(), ResumeAfteNullToken);

            }
            if (token != null)
            {

                StateClient empCode1 = activity.GetStateClient();
                BotData empCodeu = await empCode1.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
                var ob = JObject.Parse(empCodeu.Data.ToString());
                var empID = (int)obj["empID"];
                var probation_period = new ProbationPeriodClient();
                var probation_period_response = await probation_period.ProbationPeroid(token, empID);
                await context.PostAsync($"Your Probation is of  {probation_period_response.ResponseJSON.ProbationPeriod} months");
                context.Call(new ResumeAfter(), this.ResumeAfterTaskDialog);
            }

            
        }
        private async Task ResumeAfteNullToken(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Login Successful!!!");
        }

        //17:Join_date
        [LuisIntent("Join_date")]
        public async Task Join_date(IDialogContext context, IAwaitable<object> results,LuisResult result)
        {
            
            var activity = await results as Activity;
            StateClient stateClient = activity.GetStateClient();
            BotData userData = await stateClient.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
            var obj = JObject.Parse(userData.Data.ToString());
            var token = (string)obj["token"];


            if (token == null)
            {
                await context.PostAsync("Need to Login to access data");
                context.Call(new UserLogin(), ResumeAfteNullToken);

            }
            if (token != null)
            {

                StateClient empCode1 = activity.GetStateClient();
                BotData empCodeu = await empCode1.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
                var ob = JObject.Parse(empCodeu.Data.ToString());
                var empID = (int)obj["empID"];
                var probation_period = new ProbationPeriodClient();
                var probation_period_response = await probation_period.ProbationPeroid(token, empID);
                await context.PostAsync($"Your Join date is {probation_period_response.ResponseJSON.JoinDate.ToLongDateString()} ");
                context.Call(new ResumeAfter(), this.ResumeAfterTaskDialog);
            }
           // context.Call(new Join_date(), this.ResumeAfterTaskDialog);
        }

        //18:Experience
        [LuisIntent("Experience")]
        public async Task Experience(IDialogContext context, IAwaitable<object> results, LuisResult result)
        {
            var activity = await results as Activity;
            StateClient stateClient = activity.GetStateClient();
            BotData userData = await stateClient.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
            var obj = JObject.Parse(userData.Data.ToString());
            var token = (string)obj["token"];


            if (token == null)
            {
                await context.PostAsync("Need to Login to access data");
                context.Call(new UserLogin(), ResumeAfteNullToken);

            }
            if (token != null)
            {

                StateClient empCode1 = activity.GetStateClient();
                BotData empCodeu = await empCode1.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
                var ob = JObject.Parse(empCodeu.Data.ToString());
                var empID = (int)obj["empID"];
                var probation_period = new ProbationPeriodClient();
                var probation_period_response = await probation_period.ProbationPeroid(token, empID);
                await context.PostAsync($"Your Experience in Cygnet is {probation_period_response.ResponseJSON.Experience} years");
                context.Call(new ResumeAfter(), this.ResumeAfterTaskDialog);
            }
        }

        //19:Bank_Account
        [LuisIntent("Bank_Account")]
        public async Task Bank_Account(IDialogContext context,IAwaitable<object> results, LuisResult result)
        {
            var activity = await results as Activity;
            StateClient stateClient = activity.GetStateClient();
            BotData userData = await stateClient.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
            var obj = JObject.Parse(userData.Data.ToString());
            var token = (string)obj["token"];


            if (token == null)
            {
                await context.PostAsync("Need to Login to access data");
                context.Call(new UserLogin(), ResumeAfteNullToken);

            }
            if (token != null)
            {

                StateClient empCode1 = activity.GetStateClient();
                BotData empCodeu = await empCode1.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
                var ob = JObject.Parse(empCodeu.Data.ToString());
                var empID = (int)obj["empID"];
                var probation_period = new ProbationPeriodClient();
                var probation_period_response = await probation_period.ProbationPeroid(token, empID);
                await context.PostAsync($"Your salary is deposited in {probation_period_response.ResponseJSON.BankName} ");
                context.Call(new ResumeAfter(), this.ResumeAfterTaskDialog);
            }
        }
        //:Increment_Period
        [LuisIntent("Increment_Period")]
        public async Task Increment_Period(IDialogContext context, IAwaitable<object> results,LuisResult result)
        {
            var activity = await results as Activity;
            StateClient stateClient = activity.GetStateClient();
            BotData userData = await stateClient.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
            var obj = JObject.Parse(userData.Data.ToString());
            var token = (string)obj["token"];


            if (token == null)
            {
                await context.PostAsync("Need to Login to access data");
                context.Call(new UserLogin(), ResumeAfteNullToken);

            }
            if (token != null)
            {

                StateClient empCode1 = activity.GetStateClient();
                BotData empCodeu = await empCode1.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
                var ob = JObject.Parse(empCodeu.Data.ToString());
                var empID = (int)obj["empID"];
                var probation_period = new ProbationPeriodClient();
                var probation_period_response = await probation_period.ProbationPeroid(token, empID);
                await context.PostAsync($"Your increment period is {probation_period_response.ResponseJSON.incrementperiod} months");
                context.Call(new ResumeAfter(), this.ResumeAfterTaskDialog);
            }
        }

        //*******************************************Holidays***************************************************************************************
        //20.HOLIDAYS
        [LuisIntent("Holidays")]
        public async Task Holidays(IDialogContext context, LuisResult result)
        {
            context.Call(new Holidays(), this.ResumeAfterTaskDialog);
        }

        //21.Work_on_holiday
        [LuisIntent("Work_on_holiday")]
        public async Task Work_on_holiday(IDialogContext context, LuisResult result)
        {
            context.Call(new Work_on_holiday(), this.ResumeAfterTaskDialog);

        }
        //22.Hrs_work_holiday
        [LuisIntent("Hrs_work_holiday")]
        public async Task Hrs_work_holiday(IDialogContext context, LuisResult result)
        {
            context.Call(new Hrs_work_holiday(), this.ResumeAfterTaskDialog);
        }

        //*********************************************PAYROLL***************************************************************************************

        //23.Calling Payroll
        [LuisIntent("Payroll")]
        public async Task CallingPayrollMethod(IDialogContext context, LuisResult result)
        {
            var payrollform = FormDialog.FromForm(Payroll.PayrollForm, FormOptions.PromptInStart);
            context.Call(payrollform, this.ResumeAfterTaskDialog);
        }

        //24.House_rent_allowance
        [LuisIntent("House_rent_allowance")]
        public async Task House_rent_allowance(IDialogContext context, LuisResult result)
        {
            context.Call(new Hrs_work_holiday(), this.ResumeAfterTaskDialog);
        }
        //25.Dearness_allowance
        [LuisIntent("Dearness_allowance")]
        public async Task Dearness_allowance(IDialogContext context, LuisResult result)
        {
            context.Call(new Dearness_allowance(), this.ResumeAfterTaskDialog);
        }
        //26.Medical_allowance
        [LuisIntent("Medical_allowance")]
        public async Task Medical_allowance(IDialogContext context, LuisResult result)
        {
            context.Call(new Medical_allowance(), this.ResumeAfterTaskDialog);
        }
        //27.Lta_allowance 
        [LuisIntent("Lta_allowance")]
        public async Task Lta_allowance(IDialogContext context, LuisResult result)
        {
            context.Call(new Lta_allowance(), this.ResumeAfterTaskDialog);
        }
        //28.Tax_deductions
        [LuisIntent("Tax_deductions")]
        public async Task Tax_deductions(IDialogContext context, LuisResult result)
        {
            context.Call(new Tax_deductions(), this.ResumeAfterTaskDialog);
        }
        //29.Payslip
        [LuisIntent("Payslip")]
        public async Task Payslip(IDialogContext context, LuisResult result)
        {
            context.Call(new Payslip(), this.ResumeAfterTaskDialog);
        }
        //30.Base_pay
        [LuisIntent("Base_pay")]
        public async Task Base_pay(IDialogContext context, LuisResult result)
        {
            context.Call(new Base_pay(), this.ResumeAfterTaskDialog);
        }
        //31.Gross_pay
        [LuisIntent("Gross_pay")]
        public async Task Gross_pay(IDialogContext context, LuisResult result)
        {
            context.Call(new Gross_pay(), this.ResumeAfterTaskDialog);
        }
        //32.Net_pay
        [LuisIntent("Net_pay")]
        public async Task Net_pay(IDialogContext context, LuisResult result)
        {
            context.Call(new Net_pay(), this.ResumeAfterTaskDialog);
        }
        //33.Pf_contribution
        [LuisIntent("Pf_contribution")]
        public async Task Pf_contribution(IDialogContext context, LuisResult result)
        {
            context.Call(new Pf_contribution(), this.ResumeAfterTaskDialog);
        }

        //34.Pf_number
        [LuisIntent("Pf_number")]
        public async Task Pfnumber(IDialogContext context, LuisResult result)
        {
            context.Call(new Pf_number(), this.ResumeAfterTaskDialog);
        }

        //35.Professional_tax_deducted
        [LuisIntent("Professional_tax_deducted")]
        public async Task Professional_tax_deducted(IDialogContext context, LuisResult result)
        {
            context.Call(new Professional_tax_deducted(), this.ResumeAfterTaskDialog);
        }
        //36.Esi_tax
        [LuisIntent("Esi_tax")]
        public async Task Esi_tax(IDialogContext context, LuisResult result)
        {
            context.Call(new Esi_tax(), this.ResumeAfterTaskDialog);
        }

        //37.Investment_details 
        [LuisIntent("Investment_details")]
        public async Task Investment_details(IDialogContext context, LuisResult result)
        {
            context.Call(new Hrs_work_holiday(), this.ResumeAfterTaskDialog);
        }

        //38.Section 80c
        [LuisIntent("80c")]
        public async Task Section_80_C(IDialogContext context, LuisResult result)
        {
            context.Call(new Section80C(), this.ResumeAfterTaskDialog);
        }

        //39.Section 80d
        [LuisIntent("80d")]
        public async Task Section_80_D(IDialogContext context, LuisResult result)
        {
            context.Call(new Section80D(), this.ResumeAfterTaskDialog);
        }
        //40.Tds_deduction 
        [LuisIntent("Tds_deduction")]
        public async Task Tds_deduction(IDialogContext context, LuisResult result)
        {
            context.Call(new Tds_deduction(), this.ResumeAfterTaskDialog);
        }
        //41.Allowances 
        [LuisIntent("Allowances")]
        public async Task Allowances(IDialogContext context, LuisResult result)
        {
            context.Call(new Allowances(), this.ResumeAfterTaskDialog);
        }



        //******************************************** Work Week *******************WORK TIME*****************************************

        //42:gross ,net and avg hrs FORM FLOW SELECTION
        [LuisIntent("Working_Hrs")]
        public async Task Working_Hrs(IDialogContext context, LuisResult result)
        {
            context.Call(new Working_Hrs(), this.ResumeAfterTaskDialog);
        }

        //43.Gross_Hrs
        [LuisIntent("Gross_Hrs")]
        public async Task Gross_Hrs(IDialogContext context, LuisResult result)
        {
            context.Call(new Gross_Hrs(), this.ResumeAfterTaskDialog);
        }

        //44.Avg_Hrs
        [LuisIntent("Avg_Hrs")]
        public async Task Avg_Hrs(IDialogContext context, LuisResult result)
        {
            context.Call(new Avg_Hrs(), this.ResumeAfterTaskDialog);
        }

        //45.Net_Hrs
        [LuisIntent("Net_Hrs")]
        public async Task Net_Hrs(IDialogContext context, LuisResult result)
        {
            context.Call(new Net_Hrs(), this.ResumeAfterTaskDialog);
        }

        //46.in/out time for month
        [LuisIntent("In_Out_Time")]
        public async Task In_Out_Time(IDialogContext context, LuisResult result)
        {
            context.Call(new In_Out_Time(), this.ResumeAfterTaskDialog);
        }
        //47.HalfDay
        [LuisIntent("Half_day")]
        public async Task Half_day(IDialogContext context, LuisResult result)
        {
            context.Call(new HalfDay(), this.ResumeAfterTaskDialog);
        }

        //**********************************************************NONE*******************************************************************************

        //48 None Intent called no intent is matched
        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Sorry I dont know what you wanted.....");
            await context.PostAsync("### What are you looking for? <br>" +
                "1.Leave Encashment:" + "Try message like *leave encashment*<br>" +
                "2.Tour:" + "Type *tour*<br>" +
                "3.Outdoor Duty:" + "Type *Outdoor Duty*<br>" +
                "4.Work From Home:" + "Type *Work From Home*<br>" +
                "5.Compoff:" + "Type *Compoff*<br>" +
                "6.Mispunch:" + "Type *Mispunch*<br>" +
                "7.Working Hrs:" + "Type *Working Hrs*<br>" +
                "8.Holidays:" + "Type *Holidays*<br>" +
                "9.Payroll:" + "Type *Payroll*<br>" +
                "10.Profile:" + "Type *Profile*<br>" +
                "You can get information about above mention categories:"

                );
            context.Wait(MessageReceived);
        }

        //*******************************************RESUME AFTER DIALOG CALLED AFTER SUCCESSFULL COMPELETION OF INTENT DIALOG************************        

        private async Task ResumeAfterTaskDialog(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("how else can i help you");
            await context.PostAsync("### What are you looking for? <br>" +
                "1.Leave Encashment:" + "Try message like *leave encashment*<br>" +
                "2.Tour:" + "Type *tour*<br>" +
                "3.Outdoor Duty:" + "Type *Outdoor Duty*<br>" +
                "4.Work From Home:" + "Type *Work From Home*<br>" +
                "5.Compoff:" + "Type *Compoff*<br>" +
                "6.Mispunch:" + "Type *Mispunch*<br>" +
                "7.Working Hrs:" + "Type *Working Hrs*<br>" +
                "8.Holidays:" + "Type *Holidays*<br>" +
                "9.Payroll:" + "Type *Payroll*<br>" +
                "10.Profile:" + "Type *Profile*<br>"
                );

            context.Wait(MessageReceived);
        }
    }

   
}

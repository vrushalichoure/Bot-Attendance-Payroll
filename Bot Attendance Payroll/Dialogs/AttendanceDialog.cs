using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using Newtonsoft.Json.Linq;
using Zest_Client.repository;
using System.Text;
using System.Threading;

namespace Bot_Attendance_Payroll.Dialogs
{

    // [LuisModel("f9a17798-4d13-4310-90a5-19c8bd5582da", "74c7cfca6c3144538ac0f2a21af3b1d3")]
    // old working all intent [LuisModel("b78b6a80-cbf1-4b34-bbf2-538db25f9a93", "1e27f9b9ff3d4d9b9e43c830f374514f")]
    [LuisModel("be935b5f-65dc-4a4d-9eab-d7c8652f3429", "afd5de8613574e32884a51182c5e0b50")]
    [Serializable]
    public class AttendanceDialog : LuisDialog<object>
    {
        public const string Smiley = "😃";
        public const string GoodNight = "😪";
        public const string Thumbsup = "👍";
        public const string AngryMood = "🙄";
        public int empID;
        public string token;

        // Global Class to get Employee Id Stored in Context
        public async Task GetEmployeeId(IDialogContext context)
        {
            // var obj = JObject.Parse(context.UserData.ToString());
            // this.token = (string)obj["Authorization_Token_Attendance"];

            empID = context.UserData.GetValue<int>("empID");
            token = "ok";


        }

        // 1.Greeting
        [LuisIntent("Greeting")]
        public async Task Greeting(IDialogContext context, IAwaitable<object> activity, LuisResult result)
        {
            var msg = await activity as Activity;
            if (msg.Text.Equals("hello", StringComparison.InvariantCultureIgnoreCase))
            {
                context.PostAsync("**Login with chatbot to access the details**");
                //Authenticating User After User Types Hello.Calling User Login Dialog ask for username & password
                context.Call(new UserLogin(), ResumeAfterTaskDialog);
            }

            else if (msg.Text.Equals("good morning", StringComparison.InvariantCultureIgnoreCase))
            {
                await context.PostAsync("Good Morning Friend" + Smiley);
            }

            else if (msg.Text.Equals("hi", StringComparison.InvariantCultureIgnoreCase) ||
                msg.Text.Equals("hola", StringComparison.InvariantCultureIgnoreCase) ||
                msg.Text.Equals("how d", StringComparison.InvariantCultureIgnoreCase) ||
                msg.Text.Equals("whats up", StringComparison.InvariantCultureIgnoreCase))
            {
                await context.PostAsync("hi how can I help you");
                await context.PostAsync("### Hi! Here is the list of stuff I can help you with" + Smiley + "\n\n" +
                "Profile :Get details for probation period,joining date,experience,bank account details \n\n" +
                "Holidays:Know your optional, mandatory holidays \n\n" +
                "Working hours:Know your net hours,gross hours,in-out timings,late comings,early leavings \n\n" +
                "also get details of average gross hrs and net hrs and total gross hrs and net hrs \n\n" +
                "Apply for leave: You can apply for sl,cl,pl etc \n\n" +
                "Leave balance:Check your leave balance \n\n" +
                "Attendance Details:check details of lop,mispunch,half days,present days,absent,working days in payroll month \n\n" +
                "Eligibilty:check your are eligible for Tour,Woh,Encashment,Woh,Od,Compoff,Mispunch \n\n" +
                "Payroll:get all your payroll details"

                );


            }
            else if (msg.Text.Equals("ok", StringComparison.InvariantCultureIgnoreCase) ||
                      msg.Text.Equals("done", StringComparison.InvariantCultureIgnoreCase) ||
                      msg.Text.Equals("fine", StringComparison.InvariantCultureIgnoreCase) ||
                      msg.Text.Equals("alright", StringComparison.InvariantCultureIgnoreCase) ||
                      msg.Text.Equals("great", StringComparison.InvariantCultureIgnoreCase) ||
                      msg.Text.Equals("ok", StringComparison.InvariantCultureIgnoreCase))

            {

                await context.PostAsync("Good" + Thumbsup);
            }

            else if (msg.Text.Equals("thanks", StringComparison.InvariantCultureIgnoreCase) ||
                       msg.Text.Equals("thank you", StringComparison.InvariantCultureIgnoreCase) ||
                       msg.Text.Equals("thankyou", StringComparison.InvariantCultureIgnoreCase) ||
                       msg.Text.Equals("thank u", StringComparison.InvariantCultureIgnoreCase) ||
                       msg.Text.Equals("thnx", StringComparison.InvariantCultureIgnoreCase))
            {

                await context.PostAsync("you're welcome. glad to be at your service!" + Thumbsup);
            }
            else if (msg.Text.Equals("get lost", StringComparison.InvariantCultureIgnoreCase) ||
                    msg.Text.Equals("you are mad", StringComparison.InvariantCultureIgnoreCase) ||
                    msg.Text.Equals("I hate you", StringComparison.InvariantCultureIgnoreCase) ||
                    msg.Text.Equals("go away", StringComparison.InvariantCultureIgnoreCase))
            {

                await context.PostAsync("lalalallaal I can't heaaaarr youuuuu." + AngryMood);
            }
            else if (msg.Text.Equals("good night", StringComparison.InvariantCultureIgnoreCase))
            {

                await context.PostAsync("That's all for the day !!! take rest g'night" + GoodNight);
            }
            else if (msg.Text.Equals("name", StringComparison.InvariantCultureIgnoreCase) ||
                msg.Text.Equals("what is your name", StringComparison.InvariantCultureIgnoreCase) ||
                msg.Text.Equals("who are you?", StringComparison.InvariantCultureIgnoreCase))
            {

                await context.PostAsync("Hey!! I am HRMS BOT" + Smiley);

            }

        }

        //2.Profile
        [LuisIntent("Profile")]
        public async Task Profile(IDialogContext context, LuisResult result)
        {
            context.Call(new Profile(), this.ResumeAfterTaskDialog);
        }
        
        //3.Probation_Period
        [LuisIntent("Probation_Period")]
        public async Task Probation_period(IDialogContext context, IAwaitable<object> results, LuisResult result)
        {
            try
            {

                GetEmployeeId(context);
                var activity = await results as Activity;
                var probation_period = new ProbationPeriodClient();
                var probation_period_response = await probation_period.ProbationPeroid(token, empID);
                if (probation_period_response != null && probation_period_response.ResponseJSON != null)
                {
                    await context.PostAsync($"Your Probation is of  {probation_period_response.ResponseJSON.ProbationPeriod} months");
                    context.Call(new ResumeAfter(), this.ResumeAfterTaskDialog);
                }
                else
                {
                    await context.PostAsync("Data not found ");
                }
            }
            catch (Exception ex)
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory;
                StringBuilder sb = new StringBuilder();
                sb.Append("InnerException : " + ex.InnerException);
                sb.Append("Probation_Period");
                sb.Append(Environment.NewLine);
                sb.Append("Message : " + ex.Message);
                sb.Append(Environment.NewLine);
                System.IO.File.AppendAllText(System.IO.Path.Combine(filePath, "Exception_log.txt"), sb.ToString());
                sb.Clear();
                await context.PostAsync("Data not found");
                context.Done(true);
            }





        }

        //4.Join_date
        [LuisIntent("Join_date")]
        public async Task Join_date(IDialogContext context, IAwaitable<object> results, LuisResult result)
        {
            GetEmployeeId(context);
            var activity = await results as Activity;

            try
            {
                var probation_period = new ProbationPeriodClient();
                var probation_period_response = await probation_period.ProbationPeroid(token, empID);
                if (probation_period_response != null && probation_period_response.ResponseJSON != null)
                {
                    await context.PostAsync($"Your Joining date is {probation_period_response.ResponseJSON.JoinDate.Value.ToLongDateString()} ");
                    context.Call(new ResumeAfter(), this.ResumeAfterTaskDialog);
                }
                else
                {
                    await context.PostAsync("Data not found ");
                }

            }
            catch (Exception ex)
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory;
                StringBuilder sb = new StringBuilder();
                sb.Append("InnerException : " + ex.InnerException);
                sb.Append("Join_date");
                sb.Append(Environment.NewLine);
                sb.Append("Message : " + ex.Message);
                sb.Append(Environment.NewLine);
                System.IO.File.AppendAllText(System.IO.Path.Combine(filePath, "Exception_log.txt"), sb.ToString());
                sb.Clear();
                await context.PostAsync("Data not found");
                context.Done(true);
            }


        }

        //5.Experience
        [LuisIntent("Experience")]
        public async Task Experience(IDialogContext context, IAwaitable<object> results, LuisResult result)
        {
            GetEmployeeId(context);
            var activity = await results as Activity;


            try
            {
                var probation_period = new ProbationPeriodClient();
                var probation_period_response = await probation_period.ProbationPeroid(token, empID);
                if (probation_period_response != null && probation_period_response.ResponseJSON != null)
                {
                    await context.PostAsync($"Your Experience in Cygnet is {probation_period_response.ResponseJSON.Experience} years");
                    context.Call(new ResumeAfter(), this.ResumeAfterTaskDialog);
                }
                else
                {
                    await context.PostAsync("Data not found ");
                }
            }
            catch (Exception ex)
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory;
                StringBuilder sb = new StringBuilder();
                sb.Append("InnerException : " + ex.InnerException);
                sb.Append("Experience");
                sb.Append(Environment.NewLine);
                sb.Append("Message : " + ex.Message);
                sb.Append(Environment.NewLine);
                System.IO.File.AppendAllText(System.IO.Path.Combine(filePath, "Exception_log.txt"), sb.ToString());
                sb.Clear();
                await context.PostAsync("Data not found");
                context.Done(true);

            }


        }

        //6.check eligibility for tour
        [LuisIntent("Eligible_Tour")]
        public async Task Eligible_Tour(IDialogContext context, IAwaitable<object> results, LuisResult result)
        {
            try
            {
                var activity = await results as Activity;
                GetEmployeeId(context);
                var Lkp_Code_LeaveEncashment = "TOUR";
                var employee_eligibility_leave_encashment = new EmployeeEligibilityClient();
                var employee_eligibility_response = await employee_eligibility_leave_encashment.EmployeeEligibilityDetails(token, Convert.ToInt32(empID), Lkp_Code_LeaveEncashment);
                if (employee_eligibility_response != null && employee_eligibility_response.ResponseJSON != null && employee_eligibility_response.ResponseJSON.LkpCode != null)
                {
                    await context.PostAsync($"You Are Eligible for {employee_eligibility_response.ResponseJSON.LkpCode} according to policy ' {employee_eligibility_response.ResponseJSON.PolicyName}' ");
                    context.Call(new Tour(), this.ResumeAfterTaskDialog);
                }
                else
                {
                    await context.PostAsync("You are not eligible");
                }


            }
            catch (Exception ex)
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory;
                StringBuilder sb = new StringBuilder();
                sb.Append("InnerException : " + ex.InnerException);
                sb.Append("Apply Leave");
                sb.Append(Environment.NewLine);
                sb.Append("Message : " + ex.Message);
                sb.Append(Environment.NewLine);
                System.IO.File.AppendAllText(System.IO.Path.Combine(filePath, "Exception_log.txt"), sb.ToString());
                sb.Clear();
                await context.PostAsync("Data not found");
                context.Done(true);
            }


        }

        //7. Apply for a leave employee applying for leave
        [LuisIntent("Apply_leave")]
        public async Task Apply_leave(IDialogContext context, LuisResult result)
        {
            context.Call(new ApplyingLeave(), this.ResumeAfterTaskDialog);
        }
        //7. Apply for a leave employee applying for leave
        [LuisIntent("Attendance_Details")]
        public async Task Attendance_Details(IDialogContext context, LuisResult result)
        {
            context.Call(new AttendanceDetails(), this.ResumeAfterTaskDialog);
        }
        //7. Loss of Pay details
        [LuisIntent("Lop")]
        public async Task Lop(IDialogContext context, LuisResult result)
        {
            context.Call(new LopDetails(), this.ResumeAfterTaskDialog);
        }

        //8. Show  Early leaving details
        [LuisIntent("Early_leavings")]
        public async Task Early_leavings(IDialogContext context, LuisResult result)
        {
            context.Call(new Early_leavings(), this.ResumeAfterTaskDialog);

        }

        //9. Absent details
        [LuisIntent("Absent")]
        public async Task Absent(IDialogContext context, LuisResult result)
        {
            context.Call(new Absent(), this.ResumeAfterTaskDialog);
        }
        //10. Leave balance
        [LuisIntent("Leave_Balance")]
        public async Task Leave_Balance(IDialogContext context, IAwaitable<object> results, LuisResult result)
        {
            try
            {
                var activity = await results as Activity;
                GetEmployeeId(context);
                var employee_leave_balance = new EmployeeLeaveBalanceClient();
                var employee_leave_balance_response = await employee_leave_balance.EmployeeLeaveBalanceDetails(token, Convert.ToInt32(empID));
                if (employee_leave_balance_response.ResponseJSON != null && employee_leave_balance_response != null && employee_leave_balance_response.ResponseJSON.Count != 0)
                {
                    List<EmployeeLeaveBalance> data = employee_leave_balance_response.ResponseJSON;
                    List<string> values = new List<string>();
                    foreach (var dataresp in data)
                    {
                        values.Add("**Leave-Category**" + "---" + dataresp.LeaveCategoryName);
                        values.Add("**Balance**" + "---" + dataresp.ClosingBalance);
                        values.Add("------------------------------------------");
                        values.Add("------------------------------------------");
                    }
                    string employee_leave_balance_list_value = string.Join("<br/>\r\n", values);
                    await context.PostAsync(employee_leave_balance_list_value);
                    context.Done(true);
                }
                else
                {
                    await context.PostAsync("Data not found");
                }



            }
            catch (Exception ex)
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory;
                StringBuilder sb = new StringBuilder();
                sb.Append("InnerException : " + ex.InnerException);
                sb.Append("Leave_Balance");
                sb.Append(Environment.NewLine);
                sb.Append("Message : " + ex.Message);
                sb.Append(Environment.NewLine);
                System.IO.File.AppendAllText(System.IO.Path.Combine(filePath, "Exception_log.txt"), sb.ToString());
                sb.Clear();
                await context.PostAsync("Data not found");
                context.Done(true);
            }


        }

        //11.Misspunch details of user for 1 month
        [LuisIntent("Mispunch")]
        private async Task CallingMisspunchMethod(IDialogContext context, LuisResult result)
        {
            context.Call(new Mispunch(), this.ResumeAfterTaskDialog);
        }

        //12.total gross hours of payroll month
        [LuisIntent("Total_Gross_Hours")]
        private async Task Total_Gross_Hours(IDialogContext context, LuisResult result)
        {
            context.Call(new Total_Gross_Hours(), this.ResumeAfterTaskDialog);
        }

        //13.total net hours of payroll month
        [LuisIntent("Total_Net_Hours")]
        private async Task Total_Net_Hours(IDialogContext context, LuisResult result)
        {
            context.Call(new Total_Net_Hours(), this.ResumeAfterTaskDialog);
        }

        //14.total present days of payroll month
        [LuisIntent("Present_Days")]
        private async Task Present_Days(IDialogContext context, LuisResult result)
        {
            context.Call(new Present_Days(), this.ResumeAfterTaskDialog);
        }

        //15:gross ,net and avg hrs FORM FLOW SELECTION
        [LuisIntent("Working_Hrs")]
        public async Task Working_Hrs(IDialogContext context, LuisResult result)
        {
            context.Call(new Working_Hrs(), this.ResumeAfterTaskDialog);
        }
        //16.in/out time for month
        [LuisIntent("In_Out_Time")]
        public async Task In_Out_Time(IDialogContext context, LuisResult result)
        {
            context.Call(new In_Out_Time(), this.ResumeAfterTaskDialog);
        }
        //17.HalfDay
        [LuisIntent("Half_day")]
        public async Task Half_day(IDialogContext context, LuisResult result)
        {
            context.Call(new HalfDay(), this.ResumeAfterTaskDialog);
        }

        //18.HOLIDAYS
        [LuisIntent("All_Holidays")]
        public async Task Holidays(IDialogContext context, IAwaitable<object> results, LuisResult result)
        {
            this.GetEmployeeId(context);
            var activity = await results as Activity;
            try
            {
                var all_holiday_details = new AllHolidaysClient();
                var holiday_response = await all_holiday_details.AllHolidaysDetails(token, Convert.ToInt32(this.empID));
                if (holiday_response != null && holiday_response.ResponseJSON != null)
                {
                    List<HolidayList> data = holiday_response.ResponseJSON;
                    List<string> values = new List<string>();

                    foreach (var dataresp in data)
                    {
                        values.Add("**Holiday Name**" + ":::" + dataresp.HolidayName);
                        values.Add("**Holiday Date**" + ":::" + dataresp.ObservingDate.ToLongDateString());
                        values.Add("------------------------------------------");
                        values.Add("------------------------------------------");
                    }
                    string all_holiday_list_value = string.Join("<br/>\r\n", values);
                    await context.PostAsync(all_holiday_list_value);
                    context.Call(new ResumeAfter(), this.ResumeAfterTaskDialog);
                }
            }
            catch (Exception ex)
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory;
                StringBuilder sb = new StringBuilder();
                sb.Append("InnerException : " + ex.InnerException);
                sb.Append("All_Holidays");
                sb.Append(Environment.NewLine);
                sb.Append("Message : " + ex.Message);
                sb.Append(Environment.NewLine);
                System.IO.File.AppendAllText(System.IO.Path.Combine(filePath, "Exception_log.txt"), sb.ToString());
                sb.Clear();
                await context.PostAsync("Data not found");
                context.Done(true);
            }
        }

        //19.Payslip
        [LuisIntent("Payslip")]
        public async Task Payslip(IDialogContext context, LuisResult result)
        {
            context.Call(new Payslip(), this.ResumeAfterTaskDialog);
        }

        //20.Investment_details 
        [LuisIntent("Investment_details")]
        public async Task Investment_details(IDialogContext context, LuisResult result)
        {
            context.Call(new Investment_details(), this.ResumeAfterTaskDialog);
        }

        //21.Section 80c
        [LuisIntent("80c")]
        public async Task Section_80_C(IDialogContext context, LuisResult result)
        {
            context.Call(new Section80C(), this.ResumeAfterTaskDialog);
        }

        //22.Section 80d
        [LuisIntent("80d")]
        public async Task Section_80_D(IDialogContext context, LuisResult result)
        {
            context.Call(new Section80D(), this.ResumeAfterTaskDialog);
        }

        //23.Tds_deduction 
        [LuisIntent("Tds_deduction")]
        public async Task Tds_deduction(IDialogContext context, LuisResult result)
        {
            context.Call(new Tds_deduction(), this.ResumeAfterTaskDialog);
        }


        //**********************************************************NONE*******************************************************************************

        //24 None Intent called no intent is matched
        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Sorry I dont know what you wanted.....");
            await context.PostAsync("Type 'help' to see what I can do for you" + Smiley);
            context.Wait(MessageReceived);
        }

        //*******************************************RESUME AFTER DIALOG CALLED AFTER SUCCESSFULL COMPELETION OF INTENT DIALOG************************        

        private async Task ResumeAfterTaskDialog(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("how can i help you ?");
            context.Wait(MessageReceived);
        }

        //25 help
        [LuisIntent("Help")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("hi how can I help you");
            await context.PostAsync("### Hi! Here is the list of stuff I can help you with" + Smiley + "\n\n" +
            "Profile :Get details for probation period,joining date,experience \n\n" +
            "Holidays:Know Company holiday list details \n\n" +
            "Working hours:Know your net hours,gross hours,early leavings \n\n" +
            "Apply for leave: You can apply for sl,cl,pl etc \n\n" +
            "Leave balance:Check your leave balance \n\n" +
            "Attendance Details:check details of lop,mispunch,half days,present days,absent in payroll month \n\n" +
            "Apply for Tour:check your are eligible for Tour \n\n" +
            "Payslip:get your payslip details"
            );
            var Type = FormDialog.FromForm(HelpFormFlow.HelpForm, FormOptions.PromptInStart);
            context.Call(Type, this.DisplayHelpSelection);
        }
        private async Task ResumeAfteNullToken(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Login Successful!!!");
        }
        private async Task DisplayHelpSelection(IDialogContext context, IAwaitable<HelpFormFlow> result)
        {
            var selection = await result;

            switch (selection.helpType.ToString())
            {
                case "List_of_Holidays":
                    {

                        this.GetEmployeeId(context);
                        try
                        {
                            var all_holiday_details = new AllHolidaysClient();
                            var holiday_response = await all_holiday_details.AllHolidaysDetails(token, Convert.ToInt32(this.empID));
                            if (holiday_response != null && holiday_response.ResponseJSON != null)
                            {
                                List<HolidayList> data = holiday_response.ResponseJSON;
                                List<string> values = new List<string>();

                                foreach (var dataresp in data)
                                {
                                    values.Add("**Holiday Name**" + ":::" + dataresp.HolidayName);
                                    values.Add("**Holiday Date**" + ":::" + dataresp.ObservingDate.ToLongDateString());
                                    values.Add("------------------------------------------");
                                    values.Add("------------------------------------------");
                                }
                                string all_holiday_list_value = string.Join("<br/>\r\n", values);
                                await context.PostAsync(all_holiday_list_value);
                                context.Call(new ResumeAfter(), this.ResumeAfterTaskDialog);
                            }
                        }
                        catch (Exception ex)
                        {
                            string filePath = AppDomain.CurrentDomain.BaseDirectory;
                            StringBuilder sb = new StringBuilder();
                            sb.Append("InnerException : " + ex.InnerException);
                            sb.Append("All_Holidays");
                            sb.Append(Environment.NewLine);
                            sb.Append("Message : " + ex.Message);
                            sb.Append(Environment.NewLine);
                            System.IO.File.AppendAllText(System.IO.Path.Combine(filePath, "Exception_log.txt"), sb.ToString());
                            sb.Clear();
                            await context.PostAsync("Data not found");
                            context.Done(true);
                        }
                    }
                    break;
                case "Profile":
                    context.Call(new Profile(), this.ResumeAfterTaskDialog);
                    break;
                case "Working_Hours":
                    context.Call(new WorkTime(), this.ResumeAfterTaskDialog);
                    break;
                case "Apply_for_leave":
                    context.Call(new ApplyingLeave(), this.ResumeAfterTaskDialog);
                    break;
                case "Leave_Balance":
                    try
                    {
                        GetEmployeeId(context);
                        var employee_leave_balance = new EmployeeLeaveBalanceClient();
                        var employee_leave_balance_response = await employee_leave_balance.EmployeeLeaveBalanceDetails(token, Convert.ToInt32(empID));
                        if (employee_leave_balance_response.ResponseJSON != null && employee_leave_balance_response != null && employee_leave_balance_response.ResponseJSON.Count != 0)
                        {
                            List<EmployeeLeaveBalance> data = employee_leave_balance_response.ResponseJSON;
                            List<string> values = new List<string>();
                            foreach (var dataresp in data)
                            {
                                values.Add("**Leave-Category**" + "---" + dataresp.LeaveCategoryName);
                                values.Add("**Balance**" + "---" + dataresp.ClosingBalance);
                                values.Add("------------------------------------------");
                                values.Add("------------------------------------------");
                            }
                            string employee_leave_balance_list_value = string.Join("<br/>\r\n", values);
                            await context.PostAsync(employee_leave_balance_list_value);
                            context.Done(true);
                        }
                        else
                        {
                            await context.PostAsync("Data not found");
                        }



                    }
                    catch (Exception ex)
                    {
                        string filePath = AppDomain.CurrentDomain.BaseDirectory;
                        StringBuilder sb = new StringBuilder();
                        sb.Append("InnerException : " + ex.InnerException);
                        sb.Append("Leave_Balance");
                        sb.Append(Environment.NewLine);
                        sb.Append("Message : " + ex.Message);
                        sb.Append(Environment.NewLine);
                        System.IO.File.AppendAllText(System.IO.Path.Combine(filePath, "Exception_log.txt"), sb.ToString());
                        sb.Clear();
                        await context.PostAsync("Data not found");
                        context.Done(true);
                    }
                    break;
                case "Attendance_Details":
                    context.Call(new AttendanceDetails(), this.ResumeAfterTaskDialog);
                    break;
                case "Apply_for_tour":
                    try
                    {
                      
                        GetEmployeeId(context);
                        var Lkp_Code_LeaveEncashment = "TOUR";
                        var employee_eligibility_leave_encashment = new EmployeeEligibilityClient();
                        var employee_eligibility_response = await employee_eligibility_leave_encashment.EmployeeEligibilityDetails(token, Convert.ToInt32(empID), Lkp_Code_LeaveEncashment);
                        if (employee_eligibility_response != null && employee_eligibility_response.ResponseJSON != null && employee_eligibility_response.ResponseJSON.LkpCode != null)
                        {
                            await context.PostAsync($"You Are Eligible for {employee_eligibility_response.ResponseJSON.LkpCode} according to policy ' {employee_eligibility_response.ResponseJSON.PolicyName}' ");
                            context.Call(new Tour(), this.ResumeAfterTaskDialog);
                        }
                        else
                        {
                            await context.PostAsync("You are not eligible");
                        }


                    }
                    catch (Exception ex)
                    {
                        string filePath = AppDomain.CurrentDomain.BaseDirectory;
                        StringBuilder sb = new StringBuilder();
                        sb.Append("InnerException : " + ex.InnerException);
                        sb.Append("Apply Leave");
                        sb.Append(Environment.NewLine);
                        sb.Append("Message : " + ex.Message);
                        sb.Append(Environment.NewLine);
                        System.IO.File.AppendAllText(System.IO.Path.Combine(filePath, "Exception_log.txt"), sb.ToString());
                        sb.Clear();
                        await context.PostAsync("Data not found");
                        context.Done(true);
                    }
                    break;
                case "Payslip":
                    context.Call(new Payroll(), this.ResumeAfterTaskDialog);
                    break;
            }

        }
    }
    

}

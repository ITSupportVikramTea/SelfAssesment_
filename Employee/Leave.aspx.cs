using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_Leave : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            if (Session["EmailId"] == null)
            {
                Response.Redirect("../Default.aspx");
            }
         

            LastLeave();

        }
    }

    protected async void btnaddleave_Click(object sender, EventArgs e)
    {
        User addLeave = new User();

        string fromdate = txtfromdate.Value;
        string todate = txttodate.Value;
        string leaveType = DropDownLeave.SelectedValue;
        string dayTyped = dayType.SelectedValue;
        string reasons = reason.Value;
        string applied = Date1.Value;
        string halfDaySession = SessionDropDown.SelectedValue;
        double totalLeave = Convert.ToDouble(totalCount.Value);
        double leaveBalance = Convert.ToDouble(Lb.Value);

        
        DateTime createdDate = DateTime.UtcNow;
        TimeZoneInfo convertUtc = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime timeZone;

        if (string.IsNullOrEmpty(applied))
        {
            DateTime timeZone1 = TimeZoneInfo.ConvertTimeFromUtc(createdDate, convertUtc);
            timeZone = timeZone1;
        }
        else
        {
            DateTime timeZone2 = Convert.ToDateTime(applied);
            timeZone = timeZone2;
        }

        if(leaveBalance < 0) 
        {
            leaveType = "LWP"; 
        }

        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT EmpCode FROM EmployeeMaster WHERE EmpEmailId='" + Session["EmailId"].ToString() + "'");
        DataTable dtt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT EmpName FROM EmployeeMaster WHERE EmpEmailId='" + Session["EmailId"].ToString() + "'");
        DataTable dttt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT RM FROM EmployeeMaster WHERE EmpEmailId ='" + Session["EmailId"].ToString() + "'");

        string employeeName = dtt.Rows[0]["EmpName"].ToString();
        string empCode = dt.Rows[0]["EmpCode"].ToString();
        string RmName = dttt.Rows[0]["RM"].ToString();

        DataTable dtttt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT FromDate FROM Leaves WHERE employeeName='" + employeeName + "' AND status NOT IN ('Cancelled')");
        DataTable dttttt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT ToDate FROM Leaves WHERE employeeName='" + employeeName + "' AND status NOT IN ('Cancelled')");
        string FDate = "";
        string TDate = "";

        int countRow = Math.Min(dtttt.Rows.Count, dttttt.Rows.Count);

        for (int i = 0; i < countRow; i++)
        {
            FDate = dtttt.Rows[i]["FromDate"].ToString();
            TDate = dttttt.Rows[i]["ToDate"].ToString();
            DateTime date = Convert.ToDateTime(FDate).ToLocalTime();
            DateTime date1 = Convert.ToDateTime(TDate).ToLocalTime();
            DateTime selectedF = Convert.ToDateTime(fromdate).ToLocalTime();
            DateTime selectedT = Convert.ToDateTime(todate).ToLocalTime();

            var dates = new List<DateTime>();
            var dates1 = new List<DateTime>();

            while (date < date1)
            {
                date = date.AddDays(1);
                dates.Add(date);
            }

            while (selectedF < selectedT)
            {
                selectedF = selectedF.AddDays(1);
                dates1.Add(selectedF);


                if (dates.Any(x => dates1.Contains(x)))
                {

                    lblMessage.InnerText = "You have already applied for leave for the selected date range. Please review the leave dates and try again.";
                    txtfromdate.Value = "";
                    txttodate.Value = "";
                    DropDownLeave.SelectedIndex = 0;
                    dayType.SelectedIndex = 0;
                    reason.Value = "";
                    return;
                }
            }
        }

        int result = addLeave.AddLeave(empCode, fromdate.ToString(), todate.ToString(), leaveType, RmName, reasons, timeZone.ToString(), employeeName, dayTyped, halfDaySession, totalLeave, leaveBalance);
       await updateBalance(employeeName, totalLeave, leaveType);
        if (result > 0)
        {
           

            //lblMessage.InnerText = "Leave sent successfully.";
           
            txtfromdate.Value = "";
            txttodate.Value = "";
            DropDownLeave.SelectedIndex = 0;
            dayType.SelectedIndex = 0;
            reason.Value = "";
            totalCount.Value = "";
            Lb.Value = "";

          

          

            DataTable UserName = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT ERPID FROM EmployeeMaster WHERE EmpEmailId='" + Session["EmailId"].ToString() + "'");

            DataTable rmTable = objSQLDataFile.ExecuteDataTableSQL(@"SELECT RM FROM EmployeeMaster WHERE EmpCode='" + empCode + "'");
            string rm = rmTable.Rows[0]["RM"].ToString();

            DataTable rmEmailTable = objSQLDataFile.ExecuteDataTableSQL(@"SELECT EmpEmailId, EmpName FROM EmployeeMaster WHERE EmpName='" + rm + "'");


            if (rmEmailTable.Rows.Count > 0)
            {
                string rmEmailId = rmEmailTable.Rows[0]["EmpEmailId"].ToString();
                string empName = rmEmailTable.Rows[0]["EmpName"].ToString();
                string UserNames = UserName.Rows[0]["ERPID"].ToString();

                if (!string.IsNullOrEmpty(rmEmailId) && empName == rm && !string.IsNullOrEmpty(UserNames))
                {
                    //await SendEmailToRM(rmEmailId, empName, UserNames, timeZone, employeeName);
                }

            }
            string script = @"
<script>
    alert('Leave Sent Successfully');
</script>";

            ClientScript.RegisterStartupScript(this.GetType(), "alert", script);

        }
        else
        {
            lblMessage.InnerText = "Failed to add leave.";
        }

      
    }

   

    public async Task SendEmailToRM(string rmEmailId, string rmName, string UserNames, DateTime timeZone, string employeeName)
    {
        string apiKey = "59d4d40a9429113";
        string apiSecret = "381ce9223819c86";
        string emailSubject = "Leave Application Notification";

        string emailBody = "Hello \n\n" +
                            "A leave application of" + employeeName + "has been submitted for approval. Please review it at your earliest convenience.\n\n" +
                            "Best Regards,\nESS PORTAL";

        //  string apiUrl = "https://vikramtea.erpnext.com/api/resource/ESS";
        string apiUrl = "";
        try
        {
            string reportsTo = null;
            string reportsRM = null;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "token " + apiKey + ":" + apiSecret);
                //string employeeApiUrl = "https://vikramtea.erpnext.com/api/resource/Employee?fields=[\"*\"]&filters=[[\"name\",\"=\",\"" + UserNames + "\"]]";
                string employeeApiUrl = "";
                HttpResponseMessage fetchResponse = await client.GetAsync(employeeApiUrl);
                if (fetchResponse.IsSuccessStatusCode)
                {
                    string fetchResponseBody = await fetchResponse.Content.ReadAsStringAsync();
                    dynamic responseJson = Newtonsoft.Json.JsonConvert.DeserializeObject(fetchResponseBody);
                    if (responseJson.data.Count > 0)
                    {
                        reportsTo = responseJson.data[0].custom_report_to_email;
                        reportsRM = responseJson.data[0].report_manager;
                    }
                }
                else
                {
                    Console.WriteLine("Failed to fetch Employee details.");
                    return;
                }
            }

            if (string.IsNullOrEmpty(reportsTo))
            {
                Console.WriteLine("No reports_to email found for employee: {employeeName}");
                return;
            }

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.ExpectContinue = false;
                client.DefaultRequestHeaders.Add("Authorization", "token " + apiKey + ":" + apiSecret);
                string defaultStatus = "Pending";

                var postData = new Dictionary<string, string>
            {
                { "rm_email", rmEmailId },
                { "subject", emailSubject },
                { "body", emailBody },
                { "employee_id", UserNames },
                { "date", Convert.ToDateTime(timeZone).ToString() },
                { "status", defaultStatus },
                { "employee_ess", employeeName },
                { "rm_email1", reportsTo },
                {"hod_email",reportsRM },
                { "send_me_a_copy", "0" }
            };

                var content = new FormUrlEncodedContent(postData);
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Email sent successfully to RM");
                    DataTable getMob = objSQLDataFile.ExecuteDataTableSQL("SELECT MobileNo FROM EmployeeMaster WHERE ERPID = '" + UserNames + "'");
                    DataTable rmMob = objSQLDataFile.ExecuteDataTableSQL("SELECT MobileNo FROM EmployeeMaster WHERE EmpName = '" + rmName + "'");

                    if (getMob.Rows.Count > 0)
                    {
                        string EmployeeMob = getMob.Rows[0]["MobileNo"].ToString();
                        string rmMobile = rmMob.Rows[0]["MobileNo"].ToString();
                        string message = "Leave application has been sent successfully to " + rmName + " VIKRAM TEA.";
                        string messageRm = "A leave application of" + employeeName + " successfully submitted for approval. Please review it at ess.vikramtea.com VIKRAM TEA.";
                        string username = "gttplpromo";
                        string password = "India@1234";
                        string msgMethod = "simpleMsg";
                        string sendername = "VRMTEA";
                        string msgType = "text";
                        // var result = "https://www.smsgateway.center/SMSApi/rest/send?";
                        var result = "";
                        using (var cliented = new HttpClient())
                        {
                            var payload = new Dictionary<string, string>
                           {
                             { "userId",username },
                             {"password",password },
                             {"sendMethod",msgMethod},
                             {"msgType",msgType},
                             {"senderId",sendername},
                             {"msg",message },
                             {"mobile",EmployeeMob }
                            };

                            var rmPayload = new Dictionary<string, string>
                            {

                              { "userId",username },
                             {"password",password },
                             {"sendMethod",msgMethod},
                             {"msgType",msgType},
                             {"senderId",sendername},
                             {"msg",messageRm },
                             {"mobile",rmMobile }

                            };
                            var contented = new FormUrlEncodedContent(payload);
                            HttpResponseMessage responseMessage = await cliented.PostAsync(result, contented);

                            var contentedRm = new FormUrlEncodedContent(rmPayload);
                            HttpResponseMessage responseMessage1 = await cliented.PostAsync(result, contentedRm);

                            if (responseMessage.IsSuccessStatusCode)
                            {
                                Console.WriteLine("Message sent successfully.");
                            }
                            else
                            {
                                string errorDetails = await responseMessage.Content.ReadAsStringAsync();
                                Console.WriteLine("Failed to send message. Error:" + errorDetails);
                            }

                        }
                    }
                    else
                    {
                        Console.WriteLine("No mobile number found for the given ERPID.");
                    }
                }
                else
                {
                    string errorDetails = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Failed to send email. Error Details: " + errorDetails);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception: " + ex.Message);
        }
    }

    private void LastLeave()
    {

        DataTable EmpName = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT EmpName FROM EmployeeMaster WHERE EmpEmailId='" + Session["EmailId"].ToString() + "'");
        string EmployeeName = EmpName.Rows[0]["EmpName"].ToString();

        DataTable LastL = objSQLDataFile.ExecuteDataTableSQL(@"SELECT TOP 1 * FROM Leaves WHERE employeeName='" + EmployeeName + "' ORDER BY ID ASC");

        if (LastL.Rows.Count > 0)
        {

            DateTime leaveDate = Convert.ToDateTime(LastL.Rows[0]["FromDate"]);
            string Leaved = leaveDate.ToString("yyyy-MM-dd");

            string leaveReason = LastL.Rows[0]["Status"].ToString();

            LastLeaved.InnerText = "Recent Leave : " + Leaved + "-" + leaveReason;
        }
        else
        {
            LastLeaved.InnerText = "No leave records found.";
        }
    }

    [WebMethod]
    public static string LeaveBalance(string selectedLeave)
    {
        try
        {
            SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();

            string emailId = HttpContext.Current.Session["EmailId"].ToString();

            DataTable employeeName = objSQLDataFile.ExecuteDataTableSQL("SELECT EmpName FROM EmployeeMaster WHERE EmpEmailId='" + emailId + "'");

            string empName = employeeName.Rows[0]["EmpName"].ToString();

            DataTable employeeLeave = objSQLDataFile.ExecuteDataTableSQL("SELECT CL, PL, SL , [C/O] FROM LeaveBalance WHERE employeeName='" + empName + "'");

            if (selectedLeave == "CL")
            {
                string CLB = employeeLeave.Rows[0]["CL"].ToString();
                return new JavaScriptSerializer().Serialize(new { success = true, balance = CLB });
            }

            else if (selectedLeave == "PL")
            {
                string PLB = employeeLeave.Rows[0]["PL"].ToString();
                return new JavaScriptSerializer().Serialize(new { success = true, balance = PLB });
            }

            else if (selectedLeave == "SL")
            {
                string SLB = employeeLeave.Rows[0]["SL"].ToString();
                return new JavaScriptSerializer().Serialize(new { success = true, balance = SLB });
            }

            else if (selectedLeave == "C/O")
            {
                string SLB = employeeLeave.Rows[0]["C/O"].ToString(); 
                return new JavaScriptSerializer().Serialize(new { success = true, balance = SLB });
            }

            return "Data retrived";
        }
        catch (Exception ex)
        {
            return "Error: " + ex.Message;
        }
    }

    public async Task updateBalance(string empName, double totalLeave, string leavetype)
    {

        string proce = "updateLeaveBalance";
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(proce, conn))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EmpName", empName);
                    cmd.Parameters.AddWithValue("@Balance", totalLeave);
                    cmd.Parameters.AddWithValue("@Leavetype", leavetype);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}
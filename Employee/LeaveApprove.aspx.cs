using System;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;
using System.Net.Http;
using System.Collections.Generic;
using System.Web.Http;
using System.IdentityModel.Protocols.WSTrust;


public partial class Employee_LeaveApprove : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetLeavesData();
        }
    }

    private void GetLeavesData()
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

        if (Session["EmailId"] == null || string.IsNullOrEmpty(Session["EmailId"].ToString()))
        {
            Response.Write("Session expired. Please log in again.");
            return;
        }

        string emailId = Session["EmailId"].ToString();
        string empCode = GetEmpCodeByEmail(connectionString, emailId);

        if (string.IsNullOrEmpty(empCode))
        {
            Response.Write("No EmpCode found for the logged-in user.");
            return;
        }

        string storedProc = "GetLeavesApproval";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(storedProc, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RM", empCode);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);

                            GridViewLeaves.DataSource = dt;
                            GridViewLeaves.DataBind();
                        }
                        else
                        {
                            //Response.Write("No leave records found for the logged-in user.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("Error: " + ex.Message);
            }
        }
    }

    protected string GetStatusClass(string Status)
    {
        if (Status.Equals("Pending", StringComparison.OrdinalIgnoreCase))
        {
            return "background: #d1ecf1; color: #0c5460; min-width: 50px; font-weight: bold; padding-right: 20px; height: 32px; border-radius: 5px; display: flex; align-items: center; justify-content: center; text-align: center;";
        }
        else if (Status.Equals("Approved", StringComparison.OrdinalIgnoreCase))
        {
            return "background: #d4edda; color: #155724; min-width: 50px; font-weight: bold; padding-right: 20px; height: 32px; border-radius: 5px; display: flex; align-items: center; justify-content: center; text-align: center;";
        }
        else if (Status.Equals("Cancelled", StringComparison.OrdinalIgnoreCase))
        {
            return "background: #f8d7da; color: #721c24; min-width: 50px; font-weight: bold; padding-right: 20px; height: 32px; border-radius: 5px; display: flex; align-items: center; justify-content: center; text-align: center;";
        }
        else
        {
            return string.Empty;
        }
    }

    private string GetEmpCodeByEmail(string connectionString, string emailId)
    {
        string empCode = string.Empty;
        string query = "SELECT EmpName FROM EmployeeMaster WHERE EmpEmailId = @EmailId";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EmailId", emailId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows && reader.Read())
                        {
                            empCode = reader["EmpName"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("Error fetching EmpCode: " + ex.Message);
            }
        }

        return empCode;
    }

    protected void UpdateStatus(object sender, EventArgs e)
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

        foreach (GridViewRow row in GridViewLeaves.Rows)
        {
            DropDownList ddlStatus = row.FindControl("ddlStatus") as DropDownList;
            HiddenField hfLeaveId = row.FindControl("hfLeaveId") as HiddenField;
            TextBox textBox = row.FindControl("txtRemark") as TextBox;

            string Remark = textBox.Text;

            if (ddlStatus != null && hfLeaveId != null)
            {
                int leaveId;
                if (int.TryParse(hfLeaveId.Value, out leaveId))
                {
                    string status = ddlStatus.SelectedValue;


                    if (!string.IsNullOrEmpty(status))
                    {
                        try
                        {
                            DateTime currentUtc = DateTime.UtcNow;
                            TimeZoneInfo zoneInfo = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                            DateTime localTime = TimeZoneInfo.ConvertTime(currentUtc, zoneInfo);


                            string query = @"
                        SELECT 
                            l.employeeName,
                            em.ERPID,
                            em.EmpEmailId,
                            l.LeaveType,
                            l.totalLeaveDay,
                            l.Status AS LeaveStatus 
                        FROM Leaves l
                        INNER JOIN EmployeeMaster em ON l.employeeName = em.EmpName
                        WHERE l.Id = @LeaveId";

                            using (SqlConnection conn = new SqlConnection(connectionString))
                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@LeaveId", leaveId);
                                conn.Open();

                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        string employeeName = reader["employeeName"].ToString();
                                        string erpId = reader["ERPID"].ToString();
                                        string empEmail = reader["EmpEmailId"].ToString();
                                        string currentStatus = reader["LeaveStatus"].ToString();
                                        string LeaveT = reader["LeaveType"].ToString();
                                        string leaveDay = reader["totalLeaveDay"].ToString();
                                        double totalLeave = Convert.ToDouble(leaveDay);


                                        UpdateLeaveStatusInDB(connectionString, leaveId, status, Remark, LeaveT, employeeName, totalLeave);
     
                                        //SendmailToEmployee(localTime, employeeName, erpId, empEmail, status);
                                    }
                                    else
                                    {

                                        approveMsg.InnerHtml = "Error: Leave data not found.";
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                            Console.WriteLine("Error:", ex);
                            approveMsg.InnerHtml = "An error occurred while updating leave status.";
                        }
                        string message = "Leave " + status + " successfully!";
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append("<script type = 'text/javascript'>");
                        sb.Append("window.onload=function(){");
                        sb.Append("alert('");
                        sb.Append(message);
                        sb.Append("')};");
                        sb.Append("</script>");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
                        GetLeavesData();
                    }
                }
            }
        }

        //approveMsg.InnerHtml = "Leave status updated successfully!";

    }

    //protected void UpdateStatus(object sender, EventArgs e)
    //{
    //    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
    //    Button clickedButton = sender as Button;
    //    if (clickedButton != null)
    //    {

    //        GridViewRow row = (GridViewRow)clickedButton.NamingContainer;
    //        HiddenField hfLeaveId = row.FindControl("hfLeaveId") as HiddenField;

    //        if (hfLeaveId != null)
    //        {
    //            int leaveId;
    //            if (int.TryParse(hfLeaveId.Value, out leaveId))
    //            {
    //                string status = "Cancelled";

    //                if (!string.IsNullOrEmpty(status))
    //                {
    //                    try
    //                    {
    //                        DateTime currentUtc = DateTime.UtcNow;
    //                        TimeZoneInfo zoneInfo = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
    //                        DateTime localTime = TimeZoneInfo.ConvertTime(currentUtc, zoneInfo);

    //                        string query = @"
    //                    SELECT 
    //                        l.employeeName,
    //                        em.ERPID,
    //                        em.EmpEmailId,
    //                        l.LeaveType,
    //                        l.totalLeaveDay,
    //                        l.Status AS LeaveStatus 
    //                    FROM Leaves l
    //                    INNER JOIN EmployeeMaster em ON l.employeeName = em.EmpName
    //                    WHERE l.Id = @LeaveId";

    //                        using (SqlConnection conn = new SqlConnection(connectionString))
    //                        using (SqlCommand cmd = new SqlCommand(query, conn))
    //                        {
    //                            cmd.Parameters.AddWithValue("@LeaveId", leaveId);
    //                            conn.Open();

    //                            using (SqlDataReader reader = cmd.ExecuteReader())
    //                            {
    //                                if (reader.Read())
    //                                {
    //                                    string employeeName = reader["employeeName"].ToString();
    //                                    string erpId = reader["ERPID"].ToString();
    //                                    string empEmail = reader["EmpEmailId"].ToString();
    //                                    string currentStatus = reader["LeaveStatus"].ToString();
    //                                    string LeaveT = reader["LeaveType"].ToString();
    //                                    string leaveDay = reader["totalLeaveDay"].ToString();
    //                                    double totalLeave = Convert.ToDouble(leaveDay);

    //                                    UpdateLeaveStatusInDB(connectionString, leaveId, status, LeaveT, employeeName, totalLeave);


    //                                    //SendmailToEmployee(localTime, employeeName, erpId, empEmail, status);
    //                                }
    //                                else
    //                                {
    //                                    approveMsg.InnerHtml = "Error: Leave data not found.";
    //                                }
    //                            }
    //                        }
    //                    }
    //                    catch (Exception ex)
    //                    {
    //                        Console.WriteLine("Error:", ex);
    //                        approveMsg.InnerHtml = "An error occurred while updating leave status.";
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    string message = "Leave cancelled successfully!";
    //    System.Text.StringBuilder sb = new System.Text.StringBuilder();
    //    sb.Append("<script type = 'text/javascript'>");
    //    sb.Append("window.onload=function(){");
    //    sb.Append("alert('");
    //    sb.Append(message);
    //    sb.Append("')};");
    //    sb.Append("</script>");
    //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());

    //    GetLeavesData();
    //}

    public async void SendmailToEmployee(DateTime date, string employee, string ERPID, string EmpMail, string Lstatus)
    {
        string apiKey = "59d4d40a9429113";
        string apiSecret = "381ce9223819c86";
        string apiURL = "https://vikramtea.erpnext.com/api/resource/ESS";

        try
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.ExpectContinue = false;
                client.DefaultRequestHeaders.Add("Authorization", "token " + apiKey + ":" + apiSecret);

                var postData = new Dictionary<string, string>
            {
            {"date", date.ToString("yyyy-MM-ddTHH:mm:ssZ")},
            {"employee_ess", employee},
            {"employee_id", ERPID},
            {"employee_email", EmpMail},
            {"status", Lstatus},
            {"send_me_a_copy", "0"}
             };

                var content = new FormUrlEncodedContent(postData);
                HttpResponseMessage response = client.PostAsync(apiURL, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Data Inserted Successfully");
                    DataTable employeeMob = objSQLDataFile.ExecuteDataTableSQL(@"SELECT MobileNo FROM EmployeeMaster WHERE EmpName='"+employee+"'");
                    DataTable nameRM = objSQLDataFile.ExecuteDataTableSQL(@"SELECT RM FROM EmployeeMaster WHERE EmpName='"+employee+"'" );
                    

                    
                    if (employeeMob.Rows.Count > 0) 
                    {
                        string userName = "gttplpromo";
                        string password = "India@1234";
                        string smsMethod = "simpleMsg";
                        string sender = "VRMTEA";
                        string msgtype = "text";
                        string RM = nameRM.Rows[0]["RM"].ToString();
                        string message = "A leave application has been approved by " + RM + " VIKRAM TEA.";
                        string result = "https://www.smsgateway.center/SMSApi/rest/send?";
                        string mobileEmp = employeeMob.Rows[0]["MobileNo"].ToString();

                        using (var cliented = new HttpClient())
                        {
                            var empSMS = new Dictionary<string, string>()
                            {
                                {"userId",userName },
                                {"password",password },
                                {"sendMethod",smsMethod},
                                {"msgType",msgtype},
                                {"senderId",sender},
                                {"msg",message},
                                {"mobile",mobileEmp}
                            };
                       

                            var post = new FormUrlEncodedContent(empSMS);
                            HttpResponseMessage responseMessage = await cliented.PostAsync(result, post);

                            if (responseMessage.IsSuccessStatusCode) 
                            {
                                Console.WriteLine("SMS Sent Successfully");
                            
                            }
                            else 
                            {
                               string errorMsg = await responseMessage.Content.ReadAsStringAsync();

                            }
                       }
                    }
                }
                else
                {
                    string errorDetails = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("Failed to send email. Error Details: " + errorDetails);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception: " + ex.Message);
            Console.WriteLine("Stack Trace: " + ex.StackTrace);
        }

    }
    protected void GridViewLeaves_RowDataBound(object sender, GridViewRowEventArgs e)
    {
     
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
                 var statusCell = e.Row.Cells[9]; 

            if (statusCell != null)
            {
                string statusValue = statusCell.Text.Trim();

                Button btnSubmit = (Button)e.Row.FindControl("btnSubmit");
                DropDownList dropDownList = (DropDownList)e.Row.FindControl("ddlStatus");
                Label label = (Label)e.Row.FindControl("lblRemark");
                TextBox textBox = (TextBox)e.Row.FindControl("txtRemark");
                if (string.IsNullOrEmpty(label.Text) || textBox.Text == "NULL")
                {
                    label.Visible = false;
                    textBox.Visible = true;
                }

                if (btnSubmit != null && (statusValue == "Approved" || statusValue == "Cancelled"))
                {
                    btnSubmit.Enabled = false;
                    dropDownList.Enabled = false;
                    textBox.Visible = false;
                    label.Visible = true;

                }
            }
        }
    }

    private void UpdateLeaveStatusInDB(string connectionString, int leaveId, string status,string Remark, string leaveT, string empname, double totalLeave)
    {
        string storedProc = "UpdateLeaveStatus";
        string storedProc1 = "updateLeaveBalanceOfCancel";
        string LeaveTy = leaveT;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(storedProc, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", leaveId);
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@Remark", Remark);
                    cmd.ExecuteNonQuery();
                }

                if (LeaveTy != "LWP")
                {
                    using (SqlCommand cmd = new SqlCommand(storedProc1, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EmpName", empname);
                        cmd.Parameters.AddWithValue("@Leavetype", LeaveTy);
                        cmd.Parameters.AddWithValue("@Balance", totalLeave);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("Error updating status: " + ex.Message);
            }
        }
    }
}
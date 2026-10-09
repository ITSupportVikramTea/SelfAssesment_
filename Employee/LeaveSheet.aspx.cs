using System;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Services;
using System.Web;
using System.Web.Script.Serialization;

public partial class Employee_LeaveSheet : System.Web.UI.Page
{
     readonly string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
    SQLDataFileSIS ObjSQLDataFile = new SQLDataFileSIS();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetLeavesData();
        }
    }

    [WebMethod]
    public static string LeaveBalance(string SelectedLeave) 
    {
       SQLDataFileSIS sqlobj = new SQLDataFileSIS();
        string session = HttpContext.Current.Session["Emailid"].ToString();

        DataTable EMP = sqlobj.ExecuteDataTableSQL("SELECT EmpName FROM EMPLOYEEMASTER WHERE EmpEmailId='" + session + "'");
        string empName = EMP.Rows[0]["EmpName"].ToString();

        DataTable getLeaves = sqlobj.ExecuteDataTableSQL("SELECT CL,PL,SL FROM LeaveBalance WHERE employeeName='" + empName + "'");

        if(SelectedLeave == "CL") 
        {
            string CLB = getLeaves.Rows[0]["CL"].ToString();
           return new JavaScriptSerializer().Serialize(new {success = true,balance = CLB });
        }
        else if(SelectedLeave == "SL")
        {
            string SLB = getLeaves.Rows[0]["SL"].ToString();
            return new JavaScriptSerializer().Serialize(new {success = true, balance = SLB});
        }

        else if(SelectedLeave == "PL") 
        {
            string PLB = getLeaves.Rows[0]["PL"].ToString();
            return new JavaScriptSerializer().Serialize(new {success = true,balance = PLB});
        }
        return "Data Retrived";
    }

    private void GetLeavesData()
    {
        
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

        string storedProc = "GetLeaves";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(storedProc, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@EmpCode", empCode);

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
        else if(Status.Equals("Cancelled", StringComparison.OrdinalIgnoreCase)) 
        {
            return "background: #f8d7da; color: #721c24; min-width: 50px; font-weight: bold; padding-right: 20px; height: 32px; border-radius: 5px; display: flex; align-items: center; justify-content: center; text-align: center;";
        }
        else
        {
            return string.Empty;
        }
    }

    protected void UpdateStatus(object sender, EventArgs e)
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        Button clickedButton = sender as Button;
        if (clickedButton != null)
        {
            
            GridViewRow row = (GridViewRow)clickedButton.NamingContainer;
            HiddenField hfLeaveId = row.FindControl("hfLeaveId") as HiddenField;

            if (hfLeaveId != null)
            {
                int leaveId;
                if (int.TryParse(hfLeaveId.Value, out leaveId))
                {
                    string status = "Cancelled";
 
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

                                        UpdateLeaveStatusInDB(connectionString,leaveId, status,LeaveT, employeeName, totalLeave);


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
                    }
                }
            }
        }
    
    string message = "Leave cancelled successfully!";
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

    protected void GridViewLeaves_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var dataItem = e.Row.DataItem as DataRowView;

            if (dataItem != null)
            {
                string status = dataItem["Status"].ToString();
                string leaveType = dataItem["dayType"].ToString();
                DateTime fromDate = Convert.ToDateTime(dataItem["FromDate"]);

                Button cancelButton = e.Row.FindControl("CancelButton") as Button;
                Button editBtn = e.Row.FindControl("Edit") as Button;
                Button editCancel = e.Row.FindControl("EditCancel") as Button;
                Button update = e.Row.FindControl("Update") as Button;

  
                if (e.Row.RowState.HasFlag(DataControlRowState.Edit))
                {
                    if (editCancel != null) editCancel.Visible = true;
                    if (update != null) update.Visible = true;
                    if (editBtn != null) editBtn.Visible = false;
                    if (editBtn != null) cancelButton.Visible = false;
                }

 
                if (editBtn != null)
                {
                    if (leaveType == "Half Day" || status == "Cancelled" || fromDate <= DateTime.Today)
                    {
                        editBtn.Visible = false;
                        editBtn.Enabled = false;
                    }
                }

                if (cancelButton != null)
                {
                    if (status == "Cancelled" || fromDate <= DateTime.Today)
                    {
                        cancelButton.Enabled = false;
                        cancelButton.CssClass += " disabled"; 
                    }
                }
            }
        }
    }


    public async Task SendmailToEmployee(DateTime date, string employee, string ERPID, string EmpMail, string Lstatus)
    {
        string apiKey = "59d4d40a9429113";
        string apiSecret = "381ce9223819c86";
        string apiURL = "https://vikramtea.erpnext.com/api/resource/ESS";

        try
        {
            string reportsTo = null;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "token " + apiKey + ":" + apiSecret);
                string employeeApiUrl = "https://vikramtea.erpnext.com/api/resource/Employee?fields=[\"*\"]&filters=[[\"name\",\"=\",\"" + ERPID + "\"]]";

                HttpResponseMessage fetchResponse = await client.GetAsync(employeeApiUrl);
                if (fetchResponse.IsSuccessStatusCode)
                {
                    string fetchResponseBody = await fetchResponse.Content.ReadAsStringAsync();
                    dynamic responseJson = Newtonsoft.Json.JsonConvert.DeserializeObject(fetchResponseBody);
                    if (responseJson.data.Count > 0)
                    {
                        reportsTo = responseJson.data[0].custom_report_to_email;
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

                var postData = new Dictionary<string, string>
        {
            {"date", date.ToString("yyyy-MM-ddTHH:mm:ssZ")},
            {"employee_ess", employee},
            {"employee_id", ERPID},
            {"employee_email", EmpMail},
            {"status", Lstatus},
            {"rm_email1",reportsTo},
            {"send_me_a_copy", "0"}
        };

                var content = new FormUrlEncodedContent(postData);
                HttpResponseMessage response = client.PostAsync(apiURL, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Data Inserted Successfully");
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

    private void UpdateLeaveStatusInDB(string connectionString, int leaveId, string status,string leaveT,string empname,double totalLeave)
    {
        string storedProc = "UpdateLeaveStatusEmployee";
        string storedProc1 = "updateLeaveBalanceOfCancel";

        string LeaveTy = leaveT;

        //if leave type not equal to LWP

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

                    cmd.ExecuteNonQuery();
                }
                if(LeaveTy != "LWP")
                { 
                using (SqlCommand cmd = new SqlCommand(storedProc1, conn))
                {
                    cmd.CommandType=CommandType.StoredProcedure;
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

    private string GetEmpCodeByEmail(string connectionString, string emailId)
    {
        string empCode = string.Empty;

        string query = "SELECT EmpCode FROM EmployeeMaster WHERE EmpEmailId = @EmailId";

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
                            empCode = reader["EmpCode"].ToString();
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

    protected void GridView1_RowEditing(object sender,System.Web.UI.WebControls.GridViewEditEventArgs e) 
    {
        GridViewLeaves.EditIndex = e.NewEditIndex;
        GetLeavesData();
    }

     private void UpdateLeaveData(string Leave,string lT)
    {
        try { 
        int toInt = Convert.ToInt32(Leave);
        string leaveType = lT;

        using(SqlConnection sql = new SqlConnection(connectionString)) 
        {

            sql.Open();
            string query = "UPDATE LeaveBalance SET " + leaveType + "=" + toInt ;

            SqlCommand cmd = new SqlCommand(query, sql);
            cmd.ExecuteNonQuery();
                string msg = "Leave updated successful";
                message.InnerText = msg;
               
            }
        }

        catch(Exception ex)
        {
          
            string mss = "You can't apply for this leave";
            message.InnerText = mss;
            return;
        }

    }

    protected void GridView1_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e) 
    {
        HiddenField id = GridViewLeaves.Rows[e.RowIndex].FindControl("hfLeaveId") as HiddenField;
        string ids = id.Value;
        TextBox textBox = GridViewLeaves.Rows[e.RowIndex].FindControl("FromDate") as TextBox;
        TextBox toDate = GridViewLeaves.Rows[e.RowIndex].FindControl("toDate") as TextBox;
        DropDownList dayTyp = GridViewLeaves.Rows[e.RowIndex].FindControl("dayType") as DropDownList;
        DropDownList HalfDaySession = GridViewLeaves.Rows[e.RowIndex].FindControl("hds") as DropDownList;
        DropDownList leaveType = GridViewLeaves.Rows[e.RowIndex].FindControl("LT") as DropDownList;
        TextBox LeaveCount = GridViewLeaves.Rows[e.RowIndex].FindControl("leaveDays") as TextBox;
        string quantity = LeaveCount.Text;
        string data = LU.Value;
      

        if(!string.IsNullOrEmpty(data)) {

            UpdateLeaveData(data, leaveType.Text);
            data = "";

            using (SqlConnection sql = new SqlConnection(connectionString)) 
        { 
           sql.Open();
            SqlCommand cmd = new SqlCommand("UPDATE leaves SET FromDate = @FromDate, toDate = @ToDate,dayType = @dayType,halfDaySession = @halfDaySession,LeaveType = @LeaveType,TotalLeaveDay = @TotalLeaveDay WHERE Id = @Id", sql);
            cmd.Parameters.AddWithValue("@FromDate", textBox.Text);
            cmd.Parameters.AddWithValue("@Id", ids);
            cmd.Parameters.AddWithValue("@ToDate", toDate.Text);
            cmd.Parameters.AddWithValue("@dayType", dayTyp.Text);
            cmd.Parameters.AddWithValue("@halfDaySession", HalfDaySession.Text);
            cmd.Parameters.AddWithValue("@LeaveType", leaveType.Text);
            cmd.Parameters.AddWithValue("@TotalLeaveDay", LeaveCount.Text);

            cmd.ExecuteNonQuery();
        }
            GridViewLeaves.EditIndex = -1;
            string success = "Changes have been applied successfully";
            message.InnerText = success;
            textBox.Text = "";
            ids = "";
            toDate.Text = "";
            dayTyp.Text = "";
            HalfDaySession.Text = "";
            leaveType.Text = "";
            LeaveCount.Text = "";
            GetLeavesData();
        }
        else
        {
            GridViewLeaves.EditIndex = -1;
            string success = "No changes have been applied. Please reselect the leave date range or check the leave balance.";
            message.InnerText = success;
            GetLeavesData();
        }
    }

    protected void GridView1_RowCancelingEdit(object sender,System.Web.UI.WebControls.GridViewCancelEditEventArgs e) 
    {

        GridViewLeaves.EditIndex = -1;
        GetLeavesData();
    }
}
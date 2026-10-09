using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class HR_ShortlistedCandidates : System.Web.UI.Page
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
            bindjobapp();
        }
    }
    protected void bindjobapp()
    {
        string str = txtsearchcand.Text.Trim();
        DataTable dt = bindjobapplogic(str);
        
        gdShortedCnds.DataSource = dt;
        gdShortedCnds.DataBind();
    }

    public DataTable bindjobapplogic(string str)
    {
        DataTable dt;
        User user = new User();
        int empId = user.getEmployeeId(Session["EmailId"].ToString());
        if (Session["UserType"].ToString().ToLower().Equals("employeehr"))
        {
            if (str != "")
            {
                dt = objSQLDataFile.ExecuteDataTableSQL(@"select distinct ja.id Id,jr.IsLoginCreated, ja.FName+' '+ja.LName FullName,ajv.Position,ajv.HQ,ajv.Department,ja.FileName
from [dbo].[JobResult] jr left join
[dbo].[JobApplications] ja
on (jr.CandidateId = ja.Id)
left join [dbo].[AvailableVacancies] ajv
on (ja.VacancyId = ajv.Id)
where jr.IntFeedback = 'Shortlisted' and (FName Like '%" + str + "%' or LName Like '%" + str + "%') and ajv.Id not in(select VacancyId from HiddenVacancies where EmpId = " + empId + ")");
            }
            else
            {
                dt = objSQLDataFile.ExecuteDataTableSQL(@"select distinct ja.id Id,jr.IsLoginCreated, ja.FName+' '+ja.LName FullName,ajv.Position,ajv.HQ,ajv.Department,ja.FileName
from [dbo].[JobResult] jr left join
[dbo].[JobApplications] ja
on (jr.CandidateId = ja.Id)
left join [dbo].[AvailableVacancies] ajv
on (ja.VacancyId = ajv.Id)
where jr.IntFeedback = 'Shortlisted' and ajv.Id not in(select VacancyId from HiddenVacancies where EmpId = " + empId + ") ");
            }
        }
        else
        {
            if (str != "")
            {
                dt = objSQLDataFile.ExecuteDataTableSQL(@"select distinct ja.id Id, ja.FName+' '+ja.LName FullName,ajv.Position,ajv.HQ,ajv.Department,ja.FileName
from [dbo].[JobResult] jr left join
[dbo].[JobApplications] ja
on (jr.CandidateId = ja.Id)
left join [dbo].[AvailableVacancies] ajv
on (ja.VacancyId = ajv.Id)
where jr.IntFeedback = 'Shortlisted' AND 
ja.Interviewer = (select EmpName from [dbo].[EmployeeMaster] where EmpEmailId='" + Session["EmailId"].ToString() + "') and FName Like '%" + str + "%' or LName Like '%" + str + "%'");
            }
            else
            {
                dt = objSQLDataFile.ExecuteDataTableSQL(@"select distinct ja.id Id, ja.FName+' '+ja.LName FullName,ajv.Position,ajv.HQ,ajv.Department,ja.FileName
from [dbo].[JobResult] jr left join
[dbo].[JobApplications] ja
on (jr.CandidateId = ja.Id)
left join [dbo].[AvailableVacancies] ajv
on (ja.VacancyId = ajv.Id)
where jr.IntFeedback = 'Shortlisted' AND 
ja.Interviewer = (select EmpName from [dbo].[EmployeeMaster] where EmpEmailId='" + Session["EmailId"].ToString() + "') ");
            }
        }

        return dt;
    }
    protected void gdShortedCands_rowcommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "viewdocs")
        {
            Response.Redirect("CandidateDocs.aspx?id=" + e.CommandArgument);
        }
        else if (e.CommandName == "createlogin")
        {
            sendEmailCandidate(e.CommandArgument.ToString());
            
        }
        else if (e.CommandName == "CVView")
        {
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("Content-Disposition", "filename=" + e.CommandArgument);
            Response.TransmitFile(Server.MapPath("~/CVUploads/") + e.CommandArgument);
            Response.End();
        }
    }
    protected void Search(object sender, EventArgs e)
    {
        this.bindjobapp();
    }
    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdShortedCnds.PageIndex = e.NewPageIndex;
        this.bindjobapp();
    }
    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int loginCreated = 0;
            try
            {
                loginCreated = int.Parse((((DataRowView)e.Row.DataItem)["IsLoginCreated"]).ToString());
            }
            catch
            {
            }
            
            if (loginCreated > 0)
            {
                Button button = e.Row.Cells[6].FindControl("createlogin") as Button;
                button.Enabled=false;
                button.Text = "Created";
            }
            if (Session["UserType"].ToString().ToLower().Equals("employee"))
            {
                gdShortedCnds.Columns[6].Visible = false;
                gdShortedCnds.Columns[7].Visible = false;
            }

            e.Row.Cells[0].Text = Regex.Replace(e.Row.Cells[0].Text, txtsearchcand.Text.Trim(), delegate (Match match)
            {
                return string.Format("<span style = 'background-color:#D9EDF7'>{0}</span>", match.Value);
            }, RegexOptions.IgnoreCase);
        }
    }

    public string CreatePassword(int length)
    {
        const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        StringBuilder res = new StringBuilder();
        Random rnd = new Random();
        while (0 < length--)
        {
            res.Append(valid[rnd.Next(valid.Length)]);
        }
        return res.ToString();
    }

    public void sendEmailCandidate(string id)
    {
        MailMessage message = null;
        string candpass = CreatePassword(6);
        var fromEmailAddress = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
        var fromEmailDisplayName = ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
        var fromEmailPassword = ConfigurationManager.AppSettings["FromEmailPassword"].ToString();
        var smtpHost = ConfigurationManager.AppSettings["SMTPHost"].ToString();
        var smtpPort = ConfigurationManager.AppSettings["SMTPPort"].ToString();
        string to = "";

        DataTable dt2 = objSQLDataFile.ExecuteDataTableSQL(@"select EmailId from [dbo].[JobApplications] where id=" + id);

        if (dt2.Rows.Count > 0)
        {
            DataRow row = dt2.Rows[0];
            to = row["EmailId"].ToString();
            message = new MailMessage(fromEmailAddress, to);
            message.Subject = "Login Credentials Vikram Tea Processor Pvt. Ltd.";
            message.Body = @"<p>Dear Candidate,</p></br><p>Please Login Using Following Credentials.</p><br/>
            <p>Link: http://ess.vikramtea.com/ <br/> Email: " + to + "<br/> Password:" + candpass+"</p>";
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(fromEmailAddress, fromEmailPassword);
            client.Host = smtpHost;
            client.EnableSsl = false;
            client.Port = !string.IsNullOrEmpty(smtpPort) ? Convert.ToInt32(smtpPort) : 0;
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            try
            {
                client.Send(message);
                User user = new User();
                user.AddUser("Candidate", to, candpass, "Active", Session["EmailId"].ToString(),0);
                DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"update JobResult set IsLoginCreated=1 where CandidateId=" + id);
                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>if(window.confirm('Mail Sent!')){window.location.href='ShortlistedCandidates.aspx'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
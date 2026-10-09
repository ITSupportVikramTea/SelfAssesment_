using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class HR_Schedule : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    static SQLDataFileSIS objSQLDataFile2 = new SQLDataFileSIS();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["EmailId"] == null || Session["UserType"].ToString().ToLower() != "employeehr")
            {
                Response.Redirect("../Default.aspx");
            }
            if (Request.QueryString.AllKeys.Contains("id"))
            {
                DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from JobApplications where id=" + Request.QueryString["id"].ToString());
                if (dt.Rows.Count > 0)
                {
                    descriptionTextBox.Text = dt.Rows[0]["Summary"].ToString();
                    txtintrname.Text = dt.Rows[0]["Interviewer"].ToString();
                    txthr.Text = dt.Rows[0]["HR"].ToString();
                    dlintvwstatus.Text = dt.Rows[0]["InterviewStatus"].ToString();
                    //btnSubmit.Text = "Update";
                }
            }

            bindIntSuggestion();
            bindHrSuggestion();

        }

    }
    public void bindIntSuggestion()
    {
        txtintrname.AllowCustomText = true;
        txtintrname.MarkFirstMatch = true;
        txtintrname.AutoCompleteSeparator = ",";

        txtintrname.Filter = (RadComboBoxFilter)Convert.ToInt32("1");

        DataTable dtgetname = objSQLDataFile2.ExecuteDataTableSQL(@"select EmpName from EmployeeMaster");
        txtintrname.DataSource = dtgetname;
        txtintrname.DataMember = "EmpName";
        txtintrname.DataValueField = "EmpName";
        txtintrname.DataTextField = "EmpName";
        txtintrname.DataBind();
    }

    public void bindHrSuggestion()
    {
        txthr.AllowCustomText = true;
        txthr.MarkFirstMatch = true;
        txthr.AutoCompleteSeparator = ",";

        txthr.Filter = (RadComboBoxFilter)Convert.ToInt32("1");

        DataTable dtgetname = objSQLDataFile2.ExecuteDataTableSQL(@"select EmpName from EmployeeMaster");
        txthr.DataSource = dtgetname;
        txthr.DataMember = "EmpName";
        txthr.DataValueField = "EmpName";
        txthr.DataTextField = "EmpName";
        txthr.DataBind();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int jid = int.Parse(Request.QueryString["id"].ToString());
        User user = new User();
        int res = user.AddIntvwSchedule(jid, dlintvwstatus.Text, descriptionTextBox.Text, txtintrname.Text, txthr.Text);
        if (res > 0)
        {
            ClientScriptManager CSM = Page.ClientScript;
            string strconfirm = "<script>if(window.confirm('Interview Scheduled')){window.location.href='Dashboard.aspx'}</script>";
            CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
        }
    }
    protected void btnSubmitMail_Click(object sender, EventArgs e)
    {
        int jid = int.Parse(Request.QueryString["id"].ToString());
        User user = new User();
        int res = user.AddIntvwSchedule(jid, dlintvwstatus.Text, descriptionTextBox.Text, txtintrname.Text, txthr.Text);
        if (res > 0)
        {
            sendEmail(txtintrname.Text, txthr.Text, descriptionTextBox.Text);
            ClientScriptManager CSM = Page.ClientScript;
            string strconfirm = "<script>if(window.confirm('Interview Scheduled')){window.location.href='Dashboard.aspx'}</script>";
            CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
        }
    }
    public void sendEmail(string intrvwname, string hrname, string msgBody)
    {
        

        //MailMessage message = null;
        MailMessage message = new MailMessage();

        var fromEmailAddress = ConfigurationManager.AppSettings["AWS_FROM_EMAIL"].ToString();
        var fromEmailDisplayName = ConfigurationManager.AppSettings["AWS_SMTP_USERNAME"].ToString();
        var fromEmailPassword = ConfigurationManager.AppSettings["AWS_SMTP_PASSWORD"].ToString();
        var smtpHost = ConfigurationManager.AppSettings["AWS_SMTP_HOST"].ToString();
        var smtpPort = ConfigurationManager.AppSettings["AWS_SMTP_PORT"].ToString();

        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"select EmpEmailId from  [dbo].[EmployeeMaster] 
                                                where EmpName = '" + intrvwname + "' or EmpName= '" + hrname + "'");

        DataTable dt2 = objSQLDataFile.ExecuteDataTableSQL(@"select EmailId from [dbo].[JobApplications] where id=" + Request.QueryString["id"].ToString());

        if (dt2.Rows.Count > 0)
        {
            DataRow row = dt2.Rows[0];
            string to = row["EmailId"].ToString();
            message = new MailMessage(fromEmailAddress, to);
        }

        foreach (DataRow row in dt.Rows)
        {
            message.CC.Add(new MailAddress(row["EmpEmailId"].ToString()));
        }

        message.Subject = "Interview is Scheduled";
        message.Body = msgBody;
        message.BodyEncoding = Encoding.UTF8;
        message.IsBodyHtml = true;

        SmtpClient client = new SmtpClient();
        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential(fromEmailAddress, fromEmailPassword);
        client.EnableSsl = true;
        client.Host = smtpHost;
        client.EnableSsl = true;
        client.Port = !string.IsNullOrEmpty(smtpPort) ? Convert.ToInt32(smtpPort) : 0;
        //client.DeliveryMethod = SmtpDeliveryMethod.Network;

      
        try
        {
            client.Send(message);
        }

        catch (Exception ex)
        {
            throw ex;
        }

    }




    protected void btncancel_click(object sender, EventArgs e)
    {
        Response.Redirect("Dashboard.aspx");
    }

    //[WebMethod]
    //public static List<string> GetEmployeeName(string empName)
    //{
    //    List<string> emps = new List<string>();

    //    DataTable dt = objSQLDataFile2.ExecuteDataTableSQL(@"select top 5 EmpName from EmployeeMaster where EmpName like '%"+empName+"%' ");
    //    if (dt.Rows.Count > 0)
    //    {
    //        DataRow row = dt.Rows[0];

    //        emps.Add(row["EmpName"].ToString());
    //    }

    //    return (emps);
    //}

 
}
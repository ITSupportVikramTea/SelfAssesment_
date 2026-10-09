using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HR_HRAssessment : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["EmailId"] == null || Session["UserType"].ToString().ToLower() != "employeehr")
            {
                Response.Redirect("../Default.aspx");
            }
        }
    }

    protected void rbchoice_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbchoice.SelectedValue == "HRAssessment")
        {
            pnlMain.Visible = true;
        }
        else
        {
            //storedprocuder addssessment pass values and create defination in users
            pnlMain.Visible = false;
        }
    }
    protected void btnsubmit_click(object sender, EventArgs e)
    {
        User user = new User();
        if (rbchoice.SelectedValue == "HRAssessment")
        {
            string candidateId = Request.QueryString["id"].ToString();
            string appearance = dlappearance.Text;
            string jobknowledge = dljobknowledge.Text;
            string relexp = dlrelexp.Text;
            string matuarity = dlmatuarity.Text;
            string comskills = dlcomskills.Text;
            string usertype = "";
            string hrfeedback = dlhrfeedback.Text;
            string positivep = txtpospoints.Text;
            string negp = txtnegpoints.Text;

            DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"select UserType from [dbo].[UserMaster] where EmailId ='" + Session["EmailId"].ToString() + "'");
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                usertype = row["UserType"].ToString();
            }
            int res = user.AddAssessment(int.Parse(candidateId), usertype, appearance, jobknowledge, relexp, matuarity, comskills, hrfeedback, "", "", positivep, negp);
            if (res > 0)
            {
                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>if(window.confirm('Assesssment Added')){window.location.href='Dashboard.aspx'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
            }
        }
        else
        {
            string usertype = "";

            DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"select UserType from [dbo].[UserMaster] where EmailId ='" + Session["EmailId"].ToString() + "'");
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                usertype = row["UserType"].ToString();
            }
            string candidateId = Request.QueryString["id"].ToString();
            int res = user.AddAssessment(int.Parse(candidateId), usertype, "", "", "", "", "", "CV Rejected", "", "", "", "");
            try
            {
                objSQLDataFile.ExecuteDataTableSQL(@"update JobApplications set FinalResult = 'CV Rejected' where Id=" + candidateId);
                objSQLDataFile.ExecuteDataTableSQL(@"update JobResult set FinalResult = 'CV Rejected' where CandidateId=" + candidateId);
            }
            catch
            {

            }
            ClientScriptManager CSM = Page.ClientScript;
            string strconfirm = "<script>if(window.confirm('CV Rejected')){window.location.href='Dashboard.aspx'}</script>";
            CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);

        }
    }
    protected void btncancel_click(object sender, EventArgs e)
    {
        Response.Redirect("Dashboard.aspx");
    }
}
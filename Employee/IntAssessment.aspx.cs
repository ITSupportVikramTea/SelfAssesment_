using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_IntAssessment : System.Web.UI.Page
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
            if (Request.QueryString.AllKeys.Contains("id"))
            {

                DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"select Appearance,JobKnowledge,RelavantExp,Matuarity,CommSkills,IntFeedback,
                PositivePoints,NegativePoints from [dbo].[JobResult] where CandidateId=" + Request.QueryString["id"].ToString()+ "and IntFeedback = 'On Hold'");
                if (dt.Rows.Count > 0)
                {
                    dlappearance.Text = dt.Rows[0]["Appearance"].ToString();
                    dljobknowledge.Text = dt.Rows[0]["JobKnowledge"].ToString();
                    dlrelexp.Text = dt.Rows[0]["RelavantExp"].ToString();
                    dlmatuarity.Text = dt.Rows[0]["Matuarity"].ToString();
                    dlcomskills.Text = dt.Rows[0]["CommSkills"].ToString();
                    txtpospoints.Text = dt.Rows[0]["PositivePoints"].ToString();
                    txtnegpoints.Text = dt.Rows[0]["NegativePoints"].ToString();
                    dlresult.Text = dt.Rows[0]["IntFeedback"].ToString();

                    btnsubmit.Text = "Update";
                }
                else
                    btnsubmit.Text = "Submit";
            }
        }
    }
    protected void btnsubmit_click(object sender, EventArgs e)
    {
        User user = new User();


        string candidateId = Request.QueryString["id"].ToString();
        string appearance = dlappearance.Text;
        string jobknowledge = dljobknowledge.Text;
        string relexp = dlrelexp.Text;
        string matuarity = dlmatuarity.Text;
        string comskills = dlcomskills.Text;
        string usertype = "";
        string positivep = txtpospoints.Text;
        string negp = txtnegpoints.Text;
        string intfeedback = dlresult.Text;
        int res = 0;
        string hrfeedback = "";
        DataRow row2;

        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"select UserType from [dbo].[UserMaster] where EmailId ='" + Session["EmailId"].ToString() + "'");
        DataTable dt2 = objSQLDataFile.ExecuteDataTableSQL(@"select HRFeedback from [dbo].[JobResult] where CandidateId="+ candidateId);
        if(dt2.Rows.Count > 0)
        {
            row2 = dt2.Rows[0];
            hrfeedback = row2["HRFeedback"].ToString();
        }
        if (dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            usertype = row["UserType"].ToString();
        }
        if (btnsubmit.Text == "Submit")
        {
            res = user.AddAssessment(int.Parse(candidateId), usertype, appearance, jobknowledge, relexp, matuarity, comskills, hrfeedback, intfeedback, "", positivep, negp);
        }
        else
        {
            res = user.UpdateAddAssessment(int.Parse(candidateId), usertype, appearance, jobknowledge, relexp, matuarity, comskills, hrfeedback, intfeedback, "", positivep, negp);
        }
       
        
        if (res > 0)
        {
            AddAssessment assmnt = new AddAssessment();
            assmnt.AssessmentAdd(dt2,hrfeedback,intfeedback,int.Parse(candidateId));

            ClientScriptManager CSM = Page.ClientScript;
            string strconfirm = "<script>if(window.confirm('Assesssment Added')){window.location.href='JobApplication.aspx'}</script>";
            CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
        }

    }
    protected void btncancel_click(object sender, EventArgs e)
    {
        Response.Redirect("JobApplication.aspx");
    }
}
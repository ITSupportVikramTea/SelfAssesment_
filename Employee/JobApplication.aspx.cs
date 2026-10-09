using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;

public partial class Employee_JobApplication : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    static SQLDataFileSIS objSQLDataFile2 = new SQLDataFileSIS();
    static string who = "";
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
        DataTable dt2 = objSQLDataFile.ExecuteDataTableSQL(@"select EmpDepartment from [dbo].[EmployeeMaster] where EmpEmailID='" + Session["EmailId"].ToString() + "'");
        if(dt2.Rows.Count > 0)
        {
            DataRow row = dt2.Rows[0];
            string department = row["EmpDepartment"].ToString();
            DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"select ja.id Id,jr.RatingType,InterviewStatus,FileName,jr.HRFeedback,jrint.IntFeedback,jr.FinalResult, FName+' '+LName FullName,MobNo,EmailId,Interviewer, Department, Position, 
NoOfVC, HQ,ja.CreatedDate,RefferedPerson,jr.Appearance 
from [dbo].[AvailableVacancies] ajv 
join [dbo].[JobApplications] ja on (ajv.Id = ja.VacancyId )
left  join [dbo].[JobResult] jr on (ja.Id = jr.CandidateId and jr.RatingType = 'EmployeeHR')
left  join [dbo].[JobResult] jrint on (ja.Id = jrint.CandidateId and jrint.RatingType = 'Employee')
where ja.Id not in (select jrint.CandidateId 
from [dbo].[JobResult] jrint where jrint.RatingType = 'Employee' and jrint.IntFeedback in( 'Shortlisted','Rejected'))
and
ja.Id not in
(select jr.CandidateId 
from [dbo].[JobResult] jr where jr.RatingType = 'EmployeeHR' and jr.HRFeedback in( 'CV Rejected'))
AND ja.Interviewer = (select EmpName from [dbo].[EmployeeMaster] where EmpEmailId='" + Session["EmailId"].ToString() + "')");

            DataTable dt3 = objSQLDataFile.ExecuteDataTableSQL(@"select UserType from [dbo].[UserMaster] where EmailID='" + Session["EmailId"].ToString() + "'");
            DataRow row3 = dt3.Rows[0];
            who = row3["UserType"].ToString();
            gdJobAppList.DataSource = dt;
            gdJobAppList.DataBind();

            gdJobAppList.DataSource = dt;
            gdJobAppList.DataBind();
        }
       
    }
    protected void Search(object sender, EventArgs e)
    {
        this.bindjobapp();
    }
    protected void gdJobAppList_rowcommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "intassess")
        {
            Response.Redirect("IntAssessment.aspx?id=" + e.CommandArgument);
        }
        else if (e.CommandName == "viewcv")
        {
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("Content-Disposition", "filename=" + e.CommandArgument);
            Response.TransmitFile(Server.MapPath("~/CVUploads/") + e.CommandArgument);
            Response.End();
        }
    }
    protected void gdJobAppList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string InterviewStatus = (((DataRowView)e.Row.DataItem)["InterviewStatus"]).ToString();
            string IntFeedback = (((DataRowView)e.Row.DataItem)["IntFeedback"]).ToString();

            if (InterviewStatus == "Attended" )
            {
                Button button = e.Row.Cells[14].FindControl("btnintassess") as Button;
                button.Visible = true;
            }
            else
            {
                Button button = e.Row.Cells[14].FindControl("btnintassess") as Button;
                button.Visible = false;
            }
            if (IntFeedback == "On Hold")
            {
                Button button = e.Row.Cells[14].FindControl("btnintassess") as Button;
                Button button2 = e.Row.Cells[14].FindControl("btnseeintassment") as Button;
                button.Visible = true;
                button2.Visible = true;
            }


        }
    }
    [WebMethod]
    public static List<string> getCandidateAssessment(int? cid)
    {
        List<string> intassess = new List<string>();

        DataTable dt = objSQLDataFile2.ExecuteDataTableSQL(@"select Appearance, JobKnowledge, RelavantExp,Matuarity, CommSkills,
        PositivePoints,NegativePoints from JobResult where CandidateId =" + cid + "and RatingType = 'Employee'");
        if (dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];

            intassess.Add(row["Appearance"].ToString());
            intassess.Add(row["JobKnowledge"].ToString());
            intassess.Add(row["RelavantExp"].ToString());
            intassess.Add(row["Matuarity"].ToString());
            intassess.Add(row["CommSkills"].ToString());
            intassess.Add(row["PositivePoints"].ToString());
            intassess.Add(row["NegativePoints"].ToString());
        }
        
        return (intassess);
    }

    protected void btnshortcands_Click(object sender, EventArgs e)
    {
        Response.Redirect("ShortlistedCandidates.aspx");
    }
}
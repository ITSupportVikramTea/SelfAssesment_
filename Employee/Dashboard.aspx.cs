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
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Org.BouncyCastle.Ocsp;
using System.Activities.Expressions;

public partial class HR_Dashboard : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    static SQLDataFileSIS objSQLDataFile2 = new SQLDataFileSIS();
    static string who = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["EmailId"] == null || Session["UserType"].ToString().ToLower() != "employeehr")
            {
                Response.Redirect("../Default.aspx");
            }
            bindjobapp();
            List<string> uniqueValues = GetUniqueValuesFromGridView();
        }
    }
    //    protected void bindjobapp()
    //    {
    //        string str = txtsearchuser.Text.Trim();
    //        DataTable dt;
    //        User user = new User();
    //        int empId = user.getEmployeeId(Session["EmailId"].ToString());
    //        if (str != "")
    //        {
    //            dt = objSQLDataFile.ExecuteDataTableSQL(@"select ja.id Id,ajv.SDM,jr.RatingType,InterviewStatus,FileName,jr.HRFeedback,jrint.IntFeedback,jr.FinalResult, FName+' '+LName FullName,MobNo,EmailId,Interviewer, Department, Position, 
    //NoOfVC, HQ,ja.CreatedDate,RefferedPerson,jr.Appearance 
    //from [dbo].[AvailableVacancies] ajv 
    //join [dbo].[JobApplications] ja on (ajv.Id = ja.VacancyId )
    //left  join [dbo].[JobResult] jr on (ja.Id = jr.CandidateId and jr.RatingType = 'EmployeeHR')
    //left  join [dbo].[JobResult] jrint on (ja.Id = jrint.CandidateId and jrint.RatingType = 'Employee')
    //where ja.Id not in (select jrint.CandidateId 
    //from [dbo].[JobResult] jrint where jrint.RatingType = 'Employee' and jrint.IntFeedback in( 'Shortlisted','Rejected'))
    //and
    //ja.Id not in
    //(select jr.CandidateId 
    //from [dbo].[JobResult] jr where jr.RatingType = 'HR' and jr.HRFeedback in( 'CV Rejected')) 
    //and (FName LIKE '%" + str + "%' or LName LIKE '%" + str + "%' or HQ LIKE '%" + str +
    //"%' or Position LIKE '%" + str + "%' or SDM LIKE '%" + str + "%') " +
    //"and ajv.Id not in(select VacancyId from HiddenVacancies where EmpId = " + empId + ")");
    //        }
    //        else
    //        {
    //            dt = objSQLDataFile.ExecuteDataTableSQL(@"select ja.id Id,ajv.SDM,jr.RatingType,InterviewStatus,FileName,jr.HRFeedback,jrint.IntFeedback,jr.FinalResult, FName+' '+LName FullName,MobNo,EmailId,Interviewer, Department, Position, 
    //NoOfVC, HQ,ja.CreatedDate,RefferedPerson,jr.Appearance 
    //from [dbo].[AvailableVacancies] ajv 
    //join [dbo].[JobApplications] ja on (ajv.Id = ja.VacancyId )
    //left  join [dbo].[JobResult] jr on (ja.Id = jr.CandidateId and jr.RatingType = 'EmployeeHR')
    //left  join [dbo].[JobResult] jrint on (ja.Id = jrint.CandidateId and jrint.RatingType = 'Employee')
    //where ja.Id not in (select jrint.CandidateId 
    //from [dbo].[JobResult] jrint where jrint.RatingType = 'Employee' and jrint.IntFeedback in( 'Shortlisted','Rejected'))
    //and
    //ja.Id not in
    //(select jr.CandidateId 
    //from [dbo].[JobResult] jr where jr.RatingType = 'EmployeeHR' and jr.HRFeedback in( 'CV Rejected')) 
    //and ajv.Id not in(select VacancyId from HiddenVacancies where EmpId = " + empId + ")");
    //        }


    //        gdJobAppList.DataSource = dt;
    //        gdJobAppList.DataBind();

    //        DataTable dt2 = objSQLDataFile.ExecuteDataTableSQL(@"select UserType from [dbo].[UserMaster] where EmailID='" + Session["EmailId"].ToString() + "'");
    //        DataRow row = dt2.Rows[0];
    //        who = row["UserType"].ToString();

    //    }


    protected void bindjobapp()
    {
        string str = txtsearchuser.Text.Trim();
        DataTable dt;
        User user = new User();
        int empId = user.getEmployeeId(Session["EmailId"].ToString());
        if (str != "")
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"select ja.id Id,ajv.SDM,jr.RatingType,InterviewStatus,FileName,jr.HRFeedback,jrint.IntFeedback,jr.FinalResult, FName+' '+LName FullName,MobNo,EmailId,Interviewer, Department, Position, 
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
from [dbo].[JobResult] jr where jr.RatingType = 'HR' and jr.HRFeedback in( 'CV Rejected')) 
and (FName LIKE '%" + str + "%' or LName LIKE '%" + str + "%' or HQ LIKE '%" + str +
"%' or Position LIKE '%" + str + "%' or SDM LIKE '%" + str + "%') " +
"and ajv.Id not in(select VacancyId from HiddenVacancies where EmpId = " + empId + ")");
        }
        else
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"select ja.id Id,ajv.SDM,jr.RatingType,InterviewStatus,FileName,jr.HRFeedback,jrint.IntFeedback,jr.FinalResult, FName+' '+LName FullName,MobNo,EmailId,Interviewer, Department, Position, 
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
and ajv.Id not in(select VacancyId from HiddenVacancies where EmpId = " + empId + ")");
        }


        gdJobAppList.DataSource = dt;
        gdJobAppList.DataBind();

        DataTable dt2 = objSQLDataFile.ExecuteDataTableSQL(@"select UserType from [dbo].[UserMaster] where EmailID='" + Session["EmailId"].ToString() + "'");
        DataRow row = dt2.Rows[0];
        who = row["UserType"].ToString();

    }

    //protected void DownloadFile(object sender, EventArgs e)
    //{
    //    string filePath = (sender as LinkButton).CommandArgument;
    //    Response.ContentType = ContentType;
    //    Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
    //    Response.WriteFile(filePath);
    //    Response.End();
    //}

    protected void DownloadFile(object sender, EventArgs e)
    {
        string filePath = (sender as LinkButton).CommandArgument;
        Response.ContentType = "application/pdf";
        Response.AppendHeader("Content-Disposition", "inline; filename=" + Path.GetFileName(filePath));
        Response.WriteFile(filePath);
        Response.Flush(); 
        Response.Clear();
    }


    //protected void gdJobAppList_rowcommand(object sender, GridViewCommandEventArgs e)
    //{

    //    if (e.CommandName == "Schedule")
    //    {
    //        Response.Redirect("Schedule.aspx?id=" + e.CommandArgument);
    //    }
    //    else if (e.CommandName == "hrassessment")
    //    {
    //        Response.Redirect("HRAssessment.aspx?id=" + e.CommandArgument);
    //    }
    //    else if(e.CommandName == "CVView")
    //    {
    //        Response.Clear();
    //        Response.ContentType = "application/octet-stream";
    //        Response.AppendHeader("Content-Disposition", "filename=" + e.CommandArgument);
    //        Response.TransmitFile(Server.MapPath("~/CVUploads/") + e.CommandArgument);
    //        Response.End();
    //    }
    //}

    protected void gdJobAppList_rowcommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Schedule")
        {
            Response.Redirect("Schedule.aspx?id=" + e.CommandArgument);
        }
        else if (e.CommandName == "hrassessment")
        {
            Response.Redirect("HRAssessment.aspx?id=" + e.CommandArgument);
        }
        else if (e.CommandName == "CVView")
        {
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("Content-Disposition", "filename=" + e.CommandArgument);
            Response.TransmitFile(Server.MapPath("~/CVUploads/") + e.CommandArgument);
            Response.End();
        }
        else if (e.CommandName == "Select")
        {
            // Adjust the row index based on the current page index and page size
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            int pageIndex = gdJobAppList.PageIndex;
            int pageSize = gdJobAppList.PageSize;
            string id = Convert.ToString(gdJobAppList.DataKeys[rowIndex % pageSize].Value);
            Response.Redirect("ApplicantDetails.aspx?id=" + id);
        }
    }




    protected void gdJobAppList_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string interviewer = (((DataRowView)e.Row.DataItem)["Interviewer"]).ToString();
            string hrfeedback = (((DataRowView)e.Row.DataItem)["HRFeedback"]).ToString();
            string Appearance = (((DataRowView)e.Row.DataItem)["Appearance"]).ToString();
            string who1 = (((DataRowView)e.Row.DataItem)["RatingType"]).ToString();
            if (interviewer != "")
            {
                Button button = e.Row.Cells[16].FindControl("btnschedule") as Button;
                //gdJobAppList.Rows[grow].btnschedule.text = "Reschedule";
                //(gdJobAppList.Row.Cells[7].Controls[0] as Button).Text = "Reschedule";
                button.Text = "Reschedule";
                button.CssClass = "btn btn-sm btn-warning";
            }
            if (Appearance != "" && (who == who1 || who == ""))
            {
                Button button = e.Row.Cells[15].FindControl("btnhrassmnt") as Button;
                Button button2 = e.Row.Cells[15].FindControl("btnseehrassment") as Button;
                button.Visible = false;
                button2.Visible = true;
            }
            if (hrfeedback != "")
            {
                Button button = e.Row.Cells[14].FindControl("btnhrassmnt") as Button;
                button.Visible = false;
            }
            e.Row.Cells[0].Text = Regex.Replace(e.Row.Cells[0].Text, txtsearchuser.Text.Trim(), delegate (Match match)
            {
                return string.Format("<span style = 'background-color:#D9EDF7'>{0}</span>", match.Value);
            }, RegexOptions.IgnoreCase);

        }
    }
    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdJobAppList.PageIndex = e.NewPageIndex;
        this.bindjobapp();
    }
    protected void Search(object sender, EventArgs e)
    {
        this.bindjobapp();
    }

    [WebMethod]
    public static List<string> getCandidateAssessment(int? cid)
    {
        List<string> intassess = new List<string>();

        DataTable dt = objSQLDataFile2.ExecuteDataTableSQL(@"select Appearance, JobKnowledge, RelavantExp,Matuarity, CommSkills,
        PositivePoints,NegativePoints from JobResult where CandidateId =" + cid + "and RatingType = 'EmployeeHR'");
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


    protected List<string> GetUniqueValuesFromGridView()
    {
        HashSet<string> uniquePositions = new HashSet<string>();

        foreach (GridViewRow row in gdJobAppList.Rows)
        {
            // Extract the value from the "Position" column (assuming it's the third column)
            string position = row.Cells[2].Text.Trim(); // Trim any leading or trailing spaces

            // Add the position to HashSet
            uniquePositions.Add(position);
        }

        // Convert HashSet to List and return
        return uniquePositions.ToList();
    }





}
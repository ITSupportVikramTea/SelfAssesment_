using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_JobApplicationsReport : System.Web.UI.Page
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
            bindJobAppReport();
        }
    }
    public void bindJobAppReport()
    {
        string str = txtsearchposhq.Text.Trim();
        DataTable dt;
        if (str != "")
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"select Position,ZONE,SDM,HQ,sum(a.Shortlited) Shortlited,sum(a.Rejected) Rejected,
sum(a.ShortlitedByInterviewer) ShortlitedByInterviewer,
sum(a.RejectedByInterviwer) RejectedByInterviwer, sum(a.RejectedByHR) RejectedByHR, sum(a.Attended) Attended,
sum(a.NotDeclared) NotDeclared ,sum(a.Total) Total
from
(
select Position,ZONE,SDM,HQ,'' Shortlited,'' ShortlitedByInterviewer,'' RejectedByInterviwer, '' Rejected
, '' RejectedByHR ,'' Attended,'' NotDeclared,count(ja.Id) Total from [dbo].[JobApplications] ja
left join [dbo].[AvailableVacancies] avj
on ja.VacancyId = avj.Id
group by Position,ZONE,SDM,HQ
union
select Position,ZONE,SDM,HQ,count(ja.Id) Shortlited,'' ShortlitedByInterviewer,'' RejectedByInterviwer, 
'' Rejected,'' RejectedByHR,'' Attended,'' NotDeclared,'' Total  from [dbo].[JobApplications] ja
left join [dbo].[AvailableVacancies] avj
on ja.VacancyId = avj.Id where ja.FinalResult in ('Shortlisted') 
group by Position,ZONE,SDM,HQ
union
select Position,ZONE,SDM,HQ,'' Shortlited,count(ja.Id) ShortlitedByInterviewer,'' RejectedByInterviwer,
'' Rejected,'' RejectedByHR,'' Attended ,'' NotDeclared,'' Total from [dbo].[JobApplications] ja
left join [dbo].[AvailableVacancies] avj
on ja.VacancyId = avj.Id where ja.FinalResult in ('Shortlisted by Interviewer')
group by Position,ZONE,SDM,HQ
union
select Position,ZONE,SDM,HQ,'' Shortlited,'' ShortlitedByInterviewer, count(ja.Id) RejectedByInterviwer,
'' Rejected,'' RejectedByHR,'' Attended,'' NotDeclared, '' Total from [dbo].[JobApplications] ja
left join [dbo].[AvailableVacancies] avj
on ja.VacancyId = avj.Id where ja.FinalResult in ('Rejected by Interviewer')
group by Position,ZONE,SDM,HQ
union
select  Position,ZONE,SDM,HQ,'' Shortlited,'' ShortlitedByInterviewer,'' RejectedByInterviwer, 
count(ja.Id) Rejected,'' RejectedByHR,'' Attended,'' NotDeclared, '' Total from [dbo].[JobApplications] ja
left join [dbo].[AvailableVacancies] avj
on ja.VacancyId = avj.Id where ja.FinalResult in ('Rejected')
group by Position,ZONE,SDM,HQ
union
select  Position,ZONE,SDM,HQ,'' Shortlited,'' ShortlitedByInterviewer,'' RejectedByInterviwer, 
'' Rejected ,count(ja.Id) RejectedByHR, '' Attended,'' NotDeclared
,'' Total from [dbo].[JobApplications] ja
left join [dbo].[AvailableVacancies] avj
on ja.VacancyId = avj.Id where ja.FinalResult = 'CV Rejected'
group by Position,ZONE,SDM,HQ
union
select  Position,ZONE,SDM,HQ,'' Shortlited,'' ShortlitedByInterviewer,'' RejectedByInterviwer, 
'' Rejected ,'' RejectedByHR, count(ja.Id) Attended,'' NotDeclared
,'' Total from [dbo].[JobApplications] ja
left join [dbo].[AvailableVacancies] avj
on ja.VacancyId = avj.Id where ja.InterviewStatus = 'Attended'
group by Position,ZONE,SDM,HQ
union
select  Position,ZONE,SDM,HQ,'' Shortlited,'' ShortlitedByInterviewer,'' RejectedByInterviwer, 
'' Rejected ,'' RejectedByHR, '' Attended,count(ja.Id) NotDeclared
,'' Total from [dbo].[JobApplications] ja
left join [dbo].[AvailableVacancies] avj
on ja.VacancyId = avj.Id where ja.FinalResult is null
group by Position,ZONE,SDM,HQ
) a where (Position LIKE '%" + str + "%' or ZONE Like '%" + str + "%' or SDM LIKE '%" + str + "%' or HQ LIKE '%" + str + "%') group by Position, ZONE, SDM, HQ");
            JobAppReport.DataSource = dt;
            JobAppReport.DataBind();
        }
        else
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"select Position,ZONE,SDM,HQ,sum(a.Shortlited) Shortlited,sum(a.Rejected) Rejected,
sum(a.ShortlitedByInterviewer) ShortlitedByInterviewer,
sum(a.RejectedByInterviwer) RejectedByInterviwer, sum(a.RejectedByHR) RejectedByHR, sum(a.Attended) Attended,
sum(a.NotDeclared) NotDeclared ,sum(a.Total) Total
from
(
select Position,ZONE,SDM,HQ,'' Shortlited,'' ShortlitedByInterviewer,'' RejectedByInterviwer, '' Rejected
, '' RejectedByHR ,'' Attended,'' NotDeclared,count(ja.Id) Total from [dbo].[JobApplications] ja
left join [dbo].[AvailableVacancies] avj
on ja.VacancyId = avj.Id
group by Position,ZONE,SDM,HQ
union
select Position,ZONE,SDM,HQ,count(ja.Id) Shortlited,'' ShortlitedByInterviewer,'' RejectedByInterviwer, 
'' Rejected,'' RejectedByHR,'' Attended,'' NotDeclared,'' Total  from [dbo].[JobApplications] ja
left join [dbo].[AvailableVacancies] avj
on ja.VacancyId = avj.Id where ja.FinalResult in ('Shortlisted') 
group by Position,ZONE,SDM,HQ
union
select Position,ZONE,SDM,HQ,'' Shortlited,count(ja.Id) ShortlitedByInterviewer,'' RejectedByInterviwer,
'' Rejected,'' RejectedByHR,'' Attended ,'' NotDeclared,'' Total from [dbo].[JobApplications] ja
left join [dbo].[AvailableVacancies] avj
on ja.VacancyId = avj.Id where ja.FinalResult in ('Shortlisted by Interviewer')
group by Position,ZONE,SDM,HQ
union
select Position,ZONE,SDM,HQ,'' Shortlited,'' ShortlitedByInterviewer, count(ja.Id) RejectedByInterviwer,
'' Rejected,'' RejectedByHR,'' Attended,'' NotDeclared, '' Total from [dbo].[JobApplications] ja
left join [dbo].[AvailableVacancies] avj
on ja.VacancyId = avj.Id where ja.FinalResult in ('Rejected by Interviewer')
group by Position,ZONE,SDM,HQ
union
select  Position,ZONE,SDM,HQ,'' Shortlited,'' ShortlitedByInterviewer,'' RejectedByInterviwer, 
count(ja.Id) Rejected,'' RejectedByHR,'' Attended,'' NotDeclared, '' Total from [dbo].[JobApplications] ja
left join [dbo].[AvailableVacancies] avj
on ja.VacancyId = avj.Id where ja.FinalResult in ('Rejected')
group by Position,ZONE,SDM,HQ
union
select  Position,ZONE,SDM,HQ,'' Shortlited,'' ShortlitedByInterviewer,'' RejectedByInterviwer, 
'' Rejected ,count(ja.Id) RejectedByHR, '' Attended,'' NotDeclared
,'' Total from [dbo].[JobApplications] ja
left join [dbo].[AvailableVacancies] avj
on ja.VacancyId = avj.Id where ja.FinalResult = 'CV Rejected'
group by Position,ZONE,SDM,HQ
union
select  Position,ZONE,SDM,HQ,'' Shortlited,'' ShortlitedByInterviewer,'' RejectedByInterviwer, 
'' Rejected ,'' RejectedByHR, count(ja.Id) Attended,'' NotDeclared
,'' Total from [dbo].[JobApplications] ja
left join [dbo].[AvailableVacancies] avj
on ja.VacancyId = avj.Id where ja.InterviewStatus = 'Attended'
group by Position,ZONE,SDM,HQ
union
select  Position,ZONE,SDM,HQ,'' Shortlited,'' ShortlitedByInterviewer,'' RejectedByInterviwer, 
'' Rejected ,'' RejectedByHR, '' Attended,count(ja.Id) NotDeclared
,'' Total from [dbo].[JobApplications] ja
left join [dbo].[AvailableVacancies] avj
on ja.VacancyId = avj.Id where ja.FinalResult is null
group by Position,ZONE,SDM,HQ
) a
group by Position,ZONE,SDM,HQ");
            JobAppReport.DataSource = dt;
            JobAppReport.DataBind();
        }
    }
    protected void Search(object sender, EventArgs e)
    {
        this.bindJobAppReport();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the run time error "  
        //Control 'GridView1' of type 'Grid View' must be placed inside a form tag with runat=server."  
    }
    public void ExportToExcel(object sender, EventArgs e)
    {
        Response.Clear();
        Response.Buffer = true;
        string filename = "JobAppReport-" + DateTime.Now + ".xls";
        Response.AddHeader("content-disposition", "attachment;filename=" + filename);
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        using (StringWriter sw = new StringWriter())
        {
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            //To Export all pages
            JobAppReport.AllowPaging = false;
            this.bindJobAppReport();

            JobAppReport.HeaderRow.BackColor = Color.White;
            foreach (TableCell cell in JobAppReport.HeaderRow.Cells)
            {
                cell.BackColor = JobAppReport.HeaderStyle.BackColor;
            }
            foreach (GridViewRow row in JobAppReport.Rows)
            {
                row.BackColor = Color.White;
                foreach (TableCell cell in row.Cells)
                {
                    if (row.RowIndex % 2 == 0)
                    {
                        cell.BackColor = JobAppReport.AlternatingRowStyle.BackColor;
                    }
                    else
                    {
                        cell.BackColor = JobAppReport.RowStyle.BackColor;
                    }
                    cell.CssClass = "textmode";
                }
            }

            JobAppReport.RenderControl(hw);

            //style to format numbers to string
            string style = @"<style> .textmode { } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
    }
    protected void JobAppReport_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void JobAppReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Text = Regex.Replace(e.Row.Cells[0].Text, txtsearchposhq.Text.Trim(), delegate (Match match)
            {
                return string.Format("<span style = 'background-color:#D9EDF7'>{0}</span>", match.Value);
            }, RegexOptions.IgnoreCase);

        }
    }
    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        JobAppReport.PageIndex = e.NewPageIndex;
        this.bindJobAppReport();
    }
}
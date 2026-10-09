using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HR_RejectedCandidates : System.Web.UI.Page
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
            bindgdRejectedCnds();
        }
    }
    protected void bindgdRejectedCnds()
    {
        string str = txtsearchcand.Text.Trim();
        DataTable dt;
        User user = new User();
        int empId = user.getEmployeeId(Session["EmailId"].ToString());
        if (str != "")
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"select ja.id Id, ja.FName+' '+ja.LName FullName,ajv.Position,ajv.HQ,ajv.Department,ja.FileName
from [dbo].[JobResult] jr left join
[dbo].[JobApplications] ja
on (jr.CandidateId = ja.Id)
left join [dbo].[AvailableVacancies] ajv
on (ja.VacancyId = ajv.Id)
where (jr.IntFeedback = 'Rejected' or jr.HRFeedback = 'CV Rejected') and (FName Like '%" + str + "%' or LName Like '%" + str + "%') and ajv.Id not in(select VacancyId from HiddenVacancies where EmpId = " + empId + ")");
        }
        else
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"select ja.id Id, ja.FName+' '+ja.LName FullName,ajv.Position,ajv.HQ,ajv.Department,ja.FileName
from [dbo].[JobResult] jr left join
[dbo].[JobApplications] ja
on (jr.CandidateId = ja.Id)
left join [dbo].[AvailableVacancies] ajv
on (ja.VacancyId = ajv.Id)
where jr.IntFeedback = 'Rejected' or jr.HRFeedback = 'CV Rejected' and ajv.Id not in(select VacancyId from HiddenVacancies where EmpId = " + empId + ")");
        }

        gdRejectedCnds.DataSource = dt;
        gdRejectedCnds.DataBind();
    }
    protected void gdRejectedCnds_rowcommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "dwnFile")
        {
            try
            {
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("Content-Disposition", "filename=" + e.CommandArgument);
                Response.TransmitFile(Server.MapPath("~/CVUploads/") + e.CommandArgument);
                Response.End();
            }
            catch (Exception ex)
            {

            }
        }


    }
    protected void Search(object sender, EventArgs e)
    {
        this.bindgdRejectedCnds();
    }
    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdRejectedCnds.PageIndex = e.NewPageIndex;
        this.bindgdRejectedCnds();
    }
    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Text = Regex.Replace(e.Row.Cells[0].Text, txtsearchcand.Text.Trim(), delegate (Match match)
            {
                return string.Format("<span style = 'background-color:#D9EDF7'>{0}</span>", match.Value);
            }, RegexOptions.IgnoreCase);
        }
    }
}
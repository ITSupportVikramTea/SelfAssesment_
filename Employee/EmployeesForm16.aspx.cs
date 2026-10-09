using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_EmployeesForm16 : System.Web.UI.Page
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
            bindPartAPartB();
        }
    }
    protected void bindPartAPartB()
    {
        string str = txtsearchbox.Text.Trim();
        //DataTable dt = bindjobapplogic(str);
        DataTable dt = null;

        if (str != "")
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"select frm.Id, emp.EmpName,emp.EmpDepartment, emp.EmpDesignation, frm.PartA, frm.PartB, frm.Year
from EmployeeMaster emp join Form16 frm
on emp.Id = frm.EmpId where emp.EmpEmailId = '"+Session["EmailId"].ToString() + "' and (Year Like '%" + str + "%')");
        }
        else
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"select frm.Id, emp.EmpName,emp.EmpDepartment, emp.EmpDesignation, frm.PartA, frm.PartB, frm.Year
from EmployeeMaster emp join Form16 frm
on emp.Id = frm.EmpId where emp.EmpEmailId = '" + Session["EmailId"].ToString() + "'");
        }

        gdFrom16.DataSource = dt;
        gdFrom16.DataBind();
    }

//    public DataTable bindjobapplogic(string str)
//    {
//        DataTable dt;
//        User user = new User();
//        int empId = user.getEmployeeId(Session["EmailId"].ToString());
//        if (Session["UserType"].ToString().ToLower().Equals("employeehr"))
//        {
//            if (str != "")
//            {
//                dt = objSQLDataFile.ExecuteDataTableSQL(@"select distinct ja.id Id,jr.IsLoginCreated, ja.FName+' '+ja.LName FullName,ajv.Position,ajv.HQ,ajv.Department,ja.FileName
//from [dbo].[JobResult] jr left join
//[dbo].[JobApplications] ja
//on (jr.CandidateId = ja.Id)
//left join [dbo].[AvailableVacancies] ajv
//on (ja.VacancyId = ajv.Id)
//where jr.IntFeedback = 'Shortlisted' and (FName Like '%" + str + "%' or LName Like '%" + str + "%') and ajv.Id not in(select VacancyId from HiddenVacancies where EmpId = " + empId + ")");
//            }
//            else
//            {
//                dt = objSQLDataFile.ExecuteDataTableSQL(@"select distinct ja.id Id,jr.IsLoginCreated, ja.FName+' '+ja.LName FullName,ajv.Position,ajv.HQ,ajv.Department,ja.FileName
//from [dbo].[JobResult] jr left join
//[dbo].[JobApplications] ja
//on (jr.CandidateId = ja.Id)
//left join [dbo].[AvailableVacancies] ajv
//on (ja.VacancyId = ajv.Id)
//where jr.IntFeedback = 'Shortlisted' and ajv.Id not in(select VacancyId from HiddenVacancies where EmpId = " + empId + ") ");
//            }
//        }
//        else
//        {
//            if (str != "")
//            {
//                dt = objSQLDataFile.ExecuteDataTableSQL(@"select distinct ja.id Id, ja.FName+' '+ja.LName FullName,ajv.Position,ajv.HQ,ajv.Department,ja.FileName
//from [dbo].[JobResult] jr left join
//[dbo].[JobApplications] ja
//on (jr.CandidateId = ja.Id)
//left join [dbo].[AvailableVacancies] ajv
//on (ja.VacancyId = ajv.Id)
//where jr.IntFeedback = 'Shortlisted' AND 
//ja.Interviewer = (select EmpName from [dbo].[EmployeeMaster] where EmpEmailId='" + Session["EmailId"].ToString() + "') and FName Like '%" + str + "%' or LName Like '%" + str + "%'");
//            }
//            else
//            {
//                dt = objSQLDataFile.ExecuteDataTableSQL(@"select distinct ja.id Id, ja.FName+' '+ja.LName FullName,ajv.Position,ajv.HQ,ajv.Department,ja.FileName
//from [dbo].[JobResult] jr left join
//[dbo].[JobApplications] ja
//on (jr.CandidateId = ja.Id)
//left join [dbo].[AvailableVacancies] ajv
//on (ja.VacancyId = ajv.Id)
//where jr.IntFeedback = 'Shortlisted' AND 
//ja.Interviewer = (select EmpName from [dbo].[EmployeeMaster] where EmpEmailId='" + Session["EmailId"].ToString() + "') ");
//            }
//        }

//        return dt;
//    }

    protected void gdShortedCands_rowcommand(object sender, GridViewCommandEventArgs e)
    {
        
        if (e.CommandName == "ViewPartA")
        {
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AppendHeader(@"Content-Disposition", "filename=" + e.CommandArgument.ToString().Trim());
            Response.TransmitFile(Server.MapPath("../Form16Files/")+e.CommandArgument.ToString().Trim());
            Response.End();
        }
        else if (e.CommandName == "ViewPartB")
        {
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("Content-Disposition", "filename=" + e.CommandArgument.ToString().Trim());
            Response.TransmitFile(Server.MapPath("../Form16Files/") + e.CommandArgument.ToString().Trim());
            Response.End();
        }
    }
    protected void Search(object sender, EventArgs e)
    {
        this.bindPartAPartB();
    }
    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdFrom16.PageIndex = e.NewPageIndex;
        this.bindPartAPartB();
    }
    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
           

            e.Row.Cells[0].Text = Regex.Replace(e.Row.Cells[0].Text, txtsearchbox.Text.Trim(), delegate (Match match)
            {
                return string.Format("<span style = 'background-color:#D9EDF7'>{0}</span>", match.Value);
            }, RegexOptions.IgnoreCase);
        }
    }

}
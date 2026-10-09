using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_EmployeeIncrementLetter : System.Web.UI.Page
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
            bindIncLetter();
        }
    }

    protected void bindIncLetter()
    {
        string str = txtsearchbox.Text.Trim();
        //DataTable dt = bindjobapplogic(str);
        DataTable dt = null;

        if (str != "")
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"select incltr.Id, emp.EmpName,emp.EmpDepartment, emp.EmpDesignation, incltr.IncrementLetter, incltr.Year
from EmployeeMaster emp join IncrementLetter incltr
on emp.Id = incltr.EmpId where emp.EmpEmailId = '" + Session["EmailId"].ToString() + "' and (Year Like '%" + str + "%')");
        }
        else
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"select incltr.Id, emp.EmpName,emp.EmpDepartment, emp.EmpDesignation, incltr.IncrementLetter, incltr.Year
from EmployeeMaster emp join IncrementLetter incltr
on emp.Id = incltr.EmpId where emp.EmpEmailId = '" + Session["EmailId"].ToString() + "'");
        }

        gdIncLetter.DataSource = dt;
        gdIncLetter.DataBind();
    }

    protected void gdIncLetter_RowCommand(object sender, GridViewCommandEventArgs e)
     {
        if (e.CommandName == "download")
        {
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AppendHeader(@"Content-Disposition", "filename=" + e.CommandArgument.ToString().Trim());
            Response.TransmitFile(Server.MapPath("../IncrementLetter/") + e.CommandArgument.ToString().Trim());
            Response.End();
        }
    }

    protected void Search(object sender, EventArgs e)
    {
        this.bindIncLetter();
    }
    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdIncLetter.PageIndex = e.NewPageIndex;
        this.bindIncLetter();
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
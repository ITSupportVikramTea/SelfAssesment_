using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_HiddedVacanciesList : System.Web.UI.Page
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
            bindgdHiddeVacancies();
        }
    }

    public void bindgdHiddeVacancies()
    {
        string str = txtsearchuser.Text.Trim();
        DataTable dt;
        if (str != "")
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"select hv.id,EmpName,Department,Position from [dbo].[HiddenVacancies] hv 
join [dbo].[AvailableVacancies] avc
on hv.VacancyId = avc.Id
join [dbo].[EmployeeMaster] emp
on hv.EmpId = emp.id where  EmpName LIKE '%" + str + "%' order by EmpName");
            gdHiddeVacancies.DataSource = dt;
            gdHiddeVacancies.DataBind();
        }
        else
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"select hv.id,EmpName,Department,Position from [dbo].[HiddenVacancies] hv 
join [dbo].[AvailableVacancies] avc
on hv.VacancyId = avc.Id
join [dbo].[EmployeeMaster] emp
on hv.EmpId = emp.id order by EmpName");
            gdHiddeVacancies.DataSource = dt;
            gdHiddeVacancies.DataBind();
        }

    }
    protected void Search(object sender, EventArgs e)
    {
        this.bindgdHiddeVacancies();
    }
    protected void gdHiddeVacancies_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int recordid = Convert.ToInt32(e.CommandName);
            objSQLDataFile.ExecuteDataTableSQL(@"DELETE FROM HiddenVacancies WHERE Id="+ recordid);
        }
        catch { }
        Response.Redirect("HiddedVacanciesList.aspx");

    }

    protected void gdHiddeVacancies_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Text = Regex.Replace(e.Row.Cells[0].Text, txtsearchuser.Text.Trim(), delegate (Match match)
            {
                return string.Format("<span style = 'background-color:#D9EDF7'>{0}</span>", match.Value);
            }, RegexOptions.IgnoreCase);

        }
    }

    protected void gdHiddeVacancies_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdHiddeVacancies.PageIndex = e.NewPageIndex;
        this.bindgdHiddeVacancies();
    }

    protected void btnhidevacancy_Click(object sender, EventArgs e)
    {
        Response.Redirect("HideVacancies.aspx");
    }
}
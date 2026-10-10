using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_HideShowSurveyList : System.Web.UI.Page
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
            bindshowsurvey();
        }
    }


    public void bindshowsurvey()
    {
        string str = txtsearchuser.Text.Trim();
        DataTable dt;
        if (str != "")
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"select ase.id, emp.EmpName,emp.EmpDepartment, sr.SurveyTitle from [dbo].[AllowSurveyEmployee] ase
left join Survey sr
on ase.SurveyId = sr.Id
left join EmployeeMaster emp
on emp.id = ase.EmpId where  EmpName LIKE '%" + str + "%' order by EmpName");
            gdShowSurvey.DataSource = dt;
            gdShowSurvey.DataBind();
        }
        else
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"select ase.id, emp.EmpName,emp.EmpDepartment, sr.SurveyTitle from [dbo].[AllowSurveyEmployee] ase
left join Survey sr
on ase.SurveyId = sr.Id
left join EmployeeMaster emp
on emp.id = ase.EmpId order by EmpName");
            gdShowSurvey.DataSource = dt;
            gdShowSurvey.DataBind();
        }

    }
    protected void Search(object sender, EventArgs e)
    {
        this.bindshowsurvey();
    }

    protected void gdShowSurvey_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //try
        //{
        //    int recordid = Convert.ToInt32(e.CommandName);
        //    objSQLDataFile.ExecuteDataTableSQL(@"DELETE FROM AllowSurveyEmployee WHERE Id=" + recordid);
        //}
        //catch { }
        //Response.Redirect("HideShowSurveyList.aspx");
    }

    protected void gdShowSurvey_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void gdShowSurvey_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdShowSurvey.PageIndex = e.NewPageIndex;
        this.bindshowsurvey();
    }

    protected void btnassignsurvey_Click(object sender, EventArgs e)
    {
        Response.Redirect("HideShowSurvey.aspx");
    }
}
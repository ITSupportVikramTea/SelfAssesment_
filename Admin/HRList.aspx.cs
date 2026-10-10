using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_HRList : System.Web.UI.Page
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
            bindHRList();
        }
    }

    public void bindHRList()
    {
        string str = txtsearchuser.Text.Trim();
        DataTable dt;
        if (str != "")
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from EmployeeMaster where IsHR=1 and EmpName LIKE '%" + str + "%' order by EmpName");
            HRList.DataSource = dt;
            HRList.DataBind();
        }
        else
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from EmployeeMaster where IsHR=1 order by EmpName");
            HRList.DataSource = dt;
            HRList.DataBind();
        }

    }
    protected void Search(object sender, EventArgs e)
    {
        this.bindHRList();
    }
    protected void HRList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int recordid = Convert.ToInt32(e.CommandName);
            Response.Redirect("AddEmployees.aspx?id=" + recordid);
        }
        catch { }

    }
    protected void HRList_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Text = Regex.Replace(e.Row.Cells[0].Text, txtsearchuser.Text.Trim(), delegate (Match match)
            {
                return string.Format("<span style = 'background-color:#D9EDF7'>{0}</span>", match.Value);
            }, RegexOptions.IgnoreCase);

        }
    }
    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        HRList.PageIndex = e.NewPageIndex;
        this.bindHRList();
    }
}
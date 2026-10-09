using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_AssignedAssetsList : System.Web.UI.Page
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
            bindgdAssetsList();
        }
    }

    protected void bindgdAssetsList()
    {
        string str = txtsearchasset.Text.Trim();

        DataTable dt = getAssetsList(str);

        gdAssetsList.DataSource = dt;
        gdAssetsList.DataBind();
    }


    protected DataTable getAssetsList(string str)
    {
        DataTable dt = null;
        if (Session["UserType"].ToString().Equals("EMPLOYEE"))
        {
            if (str != "")
            {
                dt = objSQLDataFile.ExecuteDataTableSQL(@"select caa.Id, caa.AssesstName ,EmpName,AddedBy  from [dbo].[CompanyAssesstsAssigned] caa
JOIN [dbo].[EmployeeMaster] emp on caa.EmpId = emp.Id where (AssesstName like '%" + str + "%' or EmpName LIKE '%" + str + "%' ) and AddedBy ='" + Session["EmailId"].ToString() + "'");
            }
            else
            {
                dt = objSQLDataFile.ExecuteDataTableSQL(@"select caa.Id, caa.AssesstName ,EmpName,AddedBy  from [dbo].[CompanyAssesstsAssigned] caa
JOIN [dbo].[EmployeeMaster] emp on caa.EmpId = emp.Id where AddedBy ='" + Session["EmailId"].ToString() + "'");
            }
        }
        else if (Session["UserType"].ToString().Equals("EMPLOYEEHR"))
        {
            if (str != "")
            {
                dt = objSQLDataFile.ExecuteDataTableSQL(@"select caa.Id, caa.AssesstName ,EmpName,AddedBy  from [dbo].[CompanyAssesstsAssigned] caa
JOIN [dbo].[EmployeeMaster] emp on caa.EmpId = emp.Id where (AssesstName like '%" + str + "%' or EmpName LIKE '%" + str + "%')");
            }
            else
            {
                dt = objSQLDataFile.ExecuteDataTableSQL(@"select caa.Id, caa.AssesstName ,EmpName,AddedBy  from [dbo].[CompanyAssesstsAssigned] caa
JOIN [dbo].[EmployeeMaster] emp on caa.EmpId = emp.Id");
            }
        }

        return dt;
    }

    protected void btnAssignAssets_Click(object sender, EventArgs e)
    {
        Response.Redirect("AssingAssets.aspx");
    }

    protected void gdAssetsList_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void gdAssetsList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Text = Regex.Replace(e.Row.Cells[0].Text, txtsearchasset.Text.Trim(), delegate (Match match)
            {
                return string.Format("<span style = 'background-color:#D9EDF7'>{0}</span>", match.Value);
            }, RegexOptions.IgnoreCase);

        }
    }

    protected void Search(object sender, EventArgs e)
    {
        this.bindgdAssetsList();
    }

    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdAssetsList.PageIndex = e.NewPageIndex;
        this.bindgdAssetsList();
    }
}
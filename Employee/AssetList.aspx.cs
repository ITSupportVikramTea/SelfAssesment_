using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_AssetList : System.Web.UI.Page
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
                dt = objSQLDataFile.ExecuteDataTableSQL(@"select AssetId, AssetName, DatenTime, AddeddBy from [dbo].[CompanyAssetList] where AssetName like '%" + str + "%' and AddeddBy ='" + Session["EmailId"].ToString() + "' order by DatenTime desc");
            }
            else
            {
                dt = objSQLDataFile.ExecuteDataTableSQL(@"select AssetId, AssetName, DatenTime, AddeddBy from [dbo].[CompanyAssetList] where AddeddBy ='" + Session["EmailId"].ToString() + "' order by DatenTime desc");
            }
        }
        else if (Session["UserType"].ToString().Equals("EMPLOYEEHR"))
        {
            if (str != "")
            {
                dt = objSQLDataFile.ExecuteDataTableSQL(@"select AssetId, AssetName, DatenTime, AddeddBy from [dbo].[CompanyAssetList] where AssetName like '%" + str + "%' order by DatenTime desc");
            }
            else
            {
                dt = objSQLDataFile.ExecuteDataTableSQL(@"select AssetId, AssetName, DatenTime, AddeddBy from [dbo].[CompanyAssetList] order by DatenTime desc");
            }
        }

        return dt;
    }

    protected void addAssets_Click(object sender, EventArgs e)
    {
        Response.Redirect("AddAssets.aspx");
    }

    protected void gdAssetsList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            Response.Redirect("AddAssets.aspx?id=" + e.CommandArgument);
        }
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
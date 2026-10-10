using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Admin_QueryList : System.Web.UI.Page
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
            bindgdQueryList();
        }
    }

    protected void bindgdQueryList()
    {
        string str = txtsearchquery.Text.Trim();
        DataTable dt;

        dt = getQueryList(str);

        gdQueryList.DataSource = dt;
        gdQueryList.DataBind();

    }

    protected DataTable getQueryList(string str)
    {
        DataTable dt = null;
        
            if (str != "")
            {
                dt = objSQLDataFile.ExecuteDataTableSQL(@"select QueryId,Subject,FileName, EmpName,Status,DateTime from [dbo].[PMSQueryMaster] where Subject like '%" + str + "%' or EmpName like '%" + str + "%' order by DateTime desc");
            }
            else
            {
                dt = objSQLDataFile.ExecuteDataTableSQL(@"select QueryId,Subject,FileName, EmpName, Status,DateTime from [dbo].[PMSQueryMaster] order by DateTime desc");
            }
        

        return dt;
    }
    protected void Search(object sender, EventArgs e)
    {
        this.bindgdQueryList();
    }
    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdQueryList.PageIndex = e.NewPageIndex;
        this.bindgdQueryList();
    }
    protected void gdQueryList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                Response.Redirect("Query.aspx?id=" + e.CommandArgument);
            }
            else if (e.CommandName == "view")
            {
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("Content-Disposition", "filename=" + e.CommandArgument);
                Response.TransmitFile(Server.MapPath("~/QuerySupportFiles/") + e.CommandArgument);
                Response.End();
            }
        }
        catch
        {

        }

    }

    protected void gdQueryList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string filename = (((DataRowView)e.Row.DataItem)["FileName"]).ToString();
            

            if (filename == "")
            {
                Button button = e.Row.Cells[4].FindControl("btnview") as Button;
                button.Enabled = false;
            }

            e.Row.Cells[0].Text = Regex.Replace(e.Row.Cells[0].Text, txtsearchquery.Text.Trim(), delegate (Match match)
            {
                return string.Format("<span style = 'background-color:#D9EDF7'>{0}</span>", match.Value);
            }, RegexOptions.IgnoreCase);

        }
    }


    protected void raisequery_Click(object sender, EventArgs e)
    {
        Response.Redirect("Query.aspx");
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_HRConsultancyList : System.Web.UI.Page
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
            bindConsultanciesList();
        }
    }
    public void bindConsultanciesList()
    {
        string str = txtsearchuser.Text.Trim();
        DataTable dt;
        if (str != "")
        {

            dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from ConsultancyMaster where ConsName LIKE 
            '%" + str + "%' or EmailId LIKE'%" + str + "%'order by id desc");
        }
        else
        {

            dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from ConsultancyMaster order by id desc");
        }

        ConsultanciesList.DataSource = dt;
        ConsultanciesList.DataBind();
    }

    protected void Search(object sender, EventArgs e)
    {
        this.bindConsultanciesList();
    }
    protected void ConsultanciesList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //try
        //{
        //    int recordid = Convert.ToInt32(e.CommandName);
        //    Response.Redirect("AddEmployees.aspx?id=" + recordid);
        //}
        //catch { }

    }
    protected void ConsultanciesList_RowDataBound(object sender, GridViewRowEventArgs e)
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
        ConsultanciesList.PageIndex = e.NewPageIndex;
        this.bindConsultanciesList();
    }

    public void btnaddcons_click(object sender, EventArgs e)
    {
        Response.Redirect("HRorConsultancy.aspx");
    }
}
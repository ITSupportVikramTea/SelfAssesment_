using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_SurveyList : System.Web.UI.Page
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
            bindgetSurveyList();
        }

    }

    public void bindgetSurveyList()
    {
        string str = txtsearch.Text.Trim();
        DataTable dt;
        if (str != "")
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT  * from Survey where SurveyTitle LIKE '%" + str + "%' order by SurveyTitle");
            surveyList.DataSource = dt;
            surveyList.DataBind();
        }
        else
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT  * from Survey order by SurveyTitle");
            surveyList.DataSource = dt;
            surveyList.DataBind();
        }

    }


    protected void btnaddsurvey_Click(object sender, EventArgs e)
    {
        Response.Redirect("Survey.aspx");
    }

    protected void surveyList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "add")
            {
                int recordid = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("AddQuestions.aspx?id=" + recordid);
            }

            //if (e.CommandName == "see")
            //{
            //    int recordid = Convert.ToInt32(e.CommandArgument);
            //    Response.Redirect("QuestionsList.aspx?id=" + recordid);
            //}


        }
        catch { }

    }

    protected void surveyList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Text = Regex.Replace(e.Row.Cells[0].Text, txtsearch.Text.Trim(), delegate (Match match)
            {
                return string.Format("<span style = 'background-color:#D9EDF7'>{0}</span>", match.Value);
            }, RegexOptions.IgnoreCase);

        }
    }

    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        surveyList.PageIndex = e.NewPageIndex;
        this.bindgetSurveyList();
    }

    protected void Search(object sender, EventArgs e)
    {
        this.bindgetSurveyList();
    }
}
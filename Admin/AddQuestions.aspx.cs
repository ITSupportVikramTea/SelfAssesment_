using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_AddQuestions : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    static DataTable questinList;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["EmailId"] == null)
            {
                Response.Redirect("../Default.aspx");
            }

            questinList = new DataTable();
            questinList.Columns.Add("Question");
            questinList.Columns.Add("Type");
            questinList.Columns.Add("Options");
            previousQuestion();
        }

    }

    protected void previousQuestion()
    {
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT Id, Question,Type,Options from Survey_Questions where SuerveryId=" + Request.QueryString["id"].ToString());
        if (dt.Rows.Count > 0)
        {
            gdQuestionList.DataSource = dt;
            gdQuestionList.DataBind();
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        //objSQLDataFile.ExecuteDataTableSQL(@"DELETE FROM Survey_Questions WHERE  SuerveryId = " + Request.QueryString["id"].ToString());
        foreach (GridViewRow row in gdQuestions.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                string ques = row.Cells[0].Text;
                string type = row.Cells[1].Text;
                string vals = row.Cells[2].Text;
                //vals = vals.Replace(",", "\\,");
                objSQLDataFile.ExecuteDataTableSQL(@"INSERT INTO Survey_Questions(Question,Type,Options, SuerveryId) VALUES('" + ques + "','" + type + "','" + vals + "', " + Request.QueryString["id"].ToString()+")");
            }
        }

        ClientScriptManager CSM = Page.ClientScript;
        string strconfirm = "<script>if(window.confirm('Questions Added')){window.location.href='SurveyList.aspx'}</script>";
        CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        questinList.Rows.Add(txtquestion.Text,dpType.Text, txtValues.Text);
        gdQuestions.DataSource = questinList;
        gdQuestions.DataBind();
        txtValues.Text = "";

    }

    protected void gdQuestions_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Remove")
        {
            int row = int.Parse(e.CommandArgument.ToString());
            questinList.Rows.RemoveAt(row);
            questinList.AcceptChanges();
            gdQuestions.DataSource = questinList;
            gdQuestions.DataBind();

        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("SurveyList.aspx");
    }
}
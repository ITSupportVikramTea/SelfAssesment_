using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_SurveyReport : System.Web.UI.Page
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
            bindsurveyReport();
            binddlSurvey();
        }

    }


    protected void binddlSurvey()
    {
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  Id, SurveyTitle from Survey ");
        dlSurvey.DataSource = dt;
        dlSurvey.DataMember = "SurveyTitle";
        dlSurvey.DataTextField = "SurveyTitle";
        dlSurvey.DataValueField = "Id";
        dlSurvey.DataBind();

    }

    protected void bindsurveyReport()
    {
        string str = txtsearch.Text.Trim();
        DataTable dt;
        if (str != "")
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"select EmpName, Question, Ans from Survey_Questions sq 
left join Survey_Answer sa
on sq.Id = sa.Ques_Id
join EmployeeMaster emp
on emp.id = sa.EmpId where EmpName LIKE '%" + str + "%' order by EmpName");
            surveyReport.DataSource = dt;
            surveyReport.DataBind();
        }
        else
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"select EmpName, Question, Ans from Survey_Questions sq 
left join Survey_Answer sa
on sq.Id = sa.Ques_Id
join EmployeeMaster emp
on emp.id = sa.EmpId");
            surveyReport.DataSource = dt;
            surveyReport.DataBind();
        }
    }
    protected void surveyReport_RowDataBound(object sender, GridViewRowEventArgs e)
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
        surveyReport.PageIndex = e.NewPageIndex;
        this.bindsurveyReport();
    }

    protected void Search(object sender, EventArgs e)
    {
        this.bindsurveyReport();
    }


    public void getSurveyDetails(string id)
    {

        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"select EmpName, Question, Ans from Survey_Questions sq 
left join Survey_Answer sa
on sq.Id = sa.Ques_Id
join EmployeeMaster emp
on emp.id = sa.EmpId WHERE sq.SuerveryId =" + id);
        surveyReport.DataSource = dt;
        surveyReport.DataBind();

    }

    protected void dlSurvey_TextChanged(object sender, EventArgs e)
    {
        string id = dlSurvey.SelectedValue;
        if (id != "")
        {
            getSurveyDetails(id);
        }
        else
        {
            this.bindsurveyReport();
        }

    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the run time error "  
        //Control 'GridView1' of type 'Grid View' must be placed inside a form tag with runat=server."  
    }
    public void ExportToExcel(object sender, EventArgs e)
    {
        Response.Clear();
        Response.Buffer = true;

        string filen = dlSurvey.SelectedItem.Text;
        string filename;

        if (filen == "")
        {
            filename = "SurveyReport-" + DateTime.Now + ".xls";
        }
        else
        {
            filename = filen+"-" + DateTime.Now + ".xls";
        }

        
        Response.AddHeader("content-disposition", "attachment;filename=" + filename);
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        using (StringWriter sw = new StringWriter())
        {
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            //To Export all pages
            surveyReport.AllowPaging = false;

            string id = dlSurvey.SelectedValue;
            if (id != "")
            {
                getSurveyDetails(id);
            }
            else
            {
                this.bindsurveyReport();
            }
            

            surveyReport.HeaderRow.BackColor = Color.White;
            foreach (TableCell cell in surveyReport.HeaderRow.Cells)
            {
                cell.BackColor = surveyReport.HeaderStyle.BackColor;
            }
            foreach (GridViewRow row in surveyReport.Rows)
            {
                row.BackColor = Color.White;
                foreach (TableCell cell in row.Cells)
                {
                    if (row.RowIndex % 2 == 0)
                    {
                        cell.BackColor = surveyReport.AlternatingRowStyle.BackColor;
                    }
                    else
                    {
                        cell.BackColor = surveyReport.RowStyle.BackColor;
                    }
                    cell.CssClass = "textmode";
                }
            }

            surveyReport.RenderControl(hw);

            //style to format numbers to string
            string style = @"<style> .textmode { } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
    }
}
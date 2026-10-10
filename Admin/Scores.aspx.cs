using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Scores : System.Web.UI.Page
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
            binddlyears();
            string year = dlyear.SelectedItem.Value;
            bindEmplooyewListScore(year);
        }
    }

    public void binddlyears()
    {
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"select distinct Year from [dbo].[ScoreCard] order by Year desc");
        dlyear.DataSource = dt;
        dlyear.DataTextField = "Year";
        dlyear.DataTextField = "Year";
        dlyear.DataBind();
    }

    protected void bindEmplooyewListScore(string year)
    {
        string str = txtsearchemp.Text.Trim();
        DataTable dt;
        if (str != "")
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"select sc.Id,EmpCode,EmpName,DOJ,EmpDepartment,EmpDesignation,
KPIAVG, SkillAVG,RM,SDM,ORGScore, 
DepScore, FinalScore,PMSCategory,Location,
CONCAT(DATEDIFF(YEAR,emp.DOJ,convert(date,getdate(), 105)),' years ',
DATEDIFF(MONTH,emp.DOJ,convert(date,getdate(), 105)), ' months') AS Tenure,DOB,HODRating,Year
from [dbo].[EmployeeMaster] emp
JOIN [dbo].[ScoreCard] sc
on emp.Id = sc.EmpId
where (KPIAVG > 0 AND SkillAVG > 0 and Year='" + year + @"') and EmpName LIKE'%" + str + "%' ORDER BY CASE WHEN SDM IS NULL OR SDM = '' THEN 0 ELSE 1 END DESC,SDM ASC, CASE WHEN RM IS NULL OR RM = '' THEN 0 ELSE 1 END DESC, RM ASC");
        }
        else
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"select  sc.Id,EmpCode,EmpName,DOJ,EmpDepartment,EmpDesignation,
KPIAVG, SkillAVG,RM,SDM,ORGScore, 
DepScore, FinalScore,PMSCategory,Location,
CONCAT(DATEDIFF(YEAR,emp.DOJ,convert(date,getdate(), 105)),' years ',
DATEDIFF(MONTH,emp.DOJ,convert(date,getdate(), 105)), ' months') AS Tenure,DOB,HODRating,Year
from [dbo].[EmployeeMaster] emp
JOIN [dbo].[ScoreCard] sc
on emp.Id = sc.EmpId where (KPIAVG > 0 AND SkillAVG > 0 and Year='" + year + @"') ORDER BY CASE WHEN SDM IS NULL OR SDM = '' THEN 0 ELSE 1 END DESC,SDM ASC
, CASE WHEN RM IS NULL OR RM = '' THEN 0 ELSE 1 END DESC,RM ASC");
        }
        //ERROR TRY TO SOLVE
        EmplooyewListScore.DataSource = dt;
        EmplooyewListScore.DataBind();
    }
    protected void btnsubmit_click(object sender, EventArgs e)
    {
        try
        {
            User user = new User();
            foreach (GridViewRow row in EmplooyewListScore.Rows)
            {
                int id = int.Parse(EmplooyewListScore.DataKeys[row.RowIndex].Values[0].ToString());
                string orgscore = (row.Cells[11].FindControl("txtogrscore") as TextBox).Text;
                string depscore = (row.Cells[12].FindControl("txtdepScore") as TextBox).Text;
                float forgscore = 0;
                float fdepscore = 0;
                float.TryParse(orgscore, out forgscore);
                float.TryParse(depscore, out fdepscore);

                if (orgscore == "")
                {
                    orgscore = (row.Cells[11].FindControl("lblorgscore") as Label).Text;
                    float.TryParse(orgscore, out forgscore);
                }

                if (depscore == "")
                {
                    depscore = (row.Cells[12].FindControl("lbldepScore") as Label).Text;
                    float.TryParse(depscore, out fdepscore);
                }

                int res = user.AdminScores(id, forgscore, fdepscore);

                if (res > 0 && forgscore > 0 && fdepscore > 0)
                {
                    objSQLDataFile.ExecuteDataTableSQL(@"update ScoreCard set FinalScore = (ORGScore*15+DepScore*15+KPIAVG*50+SkillAVG*20)/100, PMSCategory = (case
	when (ORGScore*15+DepScore*15+KPIAVG*50+SkillAVG*20)/100 >= 95 then 'Excellent'
	when (ORGScore*15+DepScore*15+KPIAVG*50+SkillAVG*20)/100 >= 85 then 'Very Good'
	when (ORGScore*15+DepScore*15+KPIAVG*50+SkillAVG*20)/100 >= 71 then 'Good'
	when (ORGScore*15+DepScore*15+KPIAVG*50+SkillAVG*20)/100 >= 51 then 'Satisfactory'
	when (ORGScore*15+DepScore*15+KPIAVG*50+SkillAVG*20)/100 >= 5  then 'Below Satisfactory' end) where Id=" + id + " and Year =datepart(year,getdate())");
                }
            }
        }
        catch (Exception ex)
        {

        }

        Response.Redirect("Scores.aspx");
    }
    protected void EmplooyewListScore_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            float orgscore = 0;
            float.TryParse((e.Row.Cells[12].FindControl("lblorgscore") as Label).Text, out orgscore);

            float depscore = 0;
            float.TryParse((e.Row.Cells[13].FindControl("lbldepScore") as Label).Text, out depscore);

            if (orgscore > 0)
            {
                TextBox tb1 = e.Row.Cells[12].FindControl("txtogrscore") as TextBox;
                Label lbl = e.Row.Cells[12].FindControl("lblorgscore") as Label;
                lbl.Visible = true;
                tb1.Visible = false;
            }

            if (depscore > 0)
            {
                TextBox tb1 = e.Row.Cells[13].FindControl("txtdepScore") as TextBox;
                Label lbl = e.Row.Cells[13].FindControl("lbldepScore") as Label;
                lbl.Visible = true;
                tb1.Visible = false;
            }




            e.Row.Cells[0].Text = Regex.Replace(e.Row.Cells[0].Text, txtsearchemp.Text.Trim(), delegate (Match match)
            {
                return string.Format("<span style = 'background-color:#D9EDF7'>{0}</span>", match.Value);
            }, RegexOptions.IgnoreCase);
        }
    }
    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        EmplooyewListScore.PageIndex = e.NewPageIndex;

        string year = dlyear.SelectedValue;
        bindEmplooyewListScore(year);
        //this.bindEmplooyewListScore(year);
    }
    protected void Search(object sender, EventArgs e)
    {

        string year = dlyear.SelectedValue;
        bindEmplooyewListScore(year);
        // this.bindEmplooyewListScore(year);
    }

    protected void btnexport_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable("Grid");
        dt.Columns.AddRange(new DataColumn[7] {
                    new DataColumn("Id"),
                    new DataColumn("EmpCode"),
                    new DataColumn("EmpName"),
                    new DataColumn("EmpDepartment"),
                    new DataColumn("EmpDesignation"),
                    new DataColumn("Organizational Score"),
                    new DataColumn("Departmental Score")
                    });


        dt = objSQLDataFile.ExecuteDataTableSQL(@"select  sc.Id,EmpCode,EmpName,DOJ,EmpDepartment,EmpDesignation,
KPIAVG, SkillAVG,RM,SDM,ORGScore, 
DepScore, FinalScore,PMSCategory,Location,
CONCAT(DATEDIFF(YEAR,emp.DOJ,convert(date,getdate(), 105)),' years ',
DATEDIFF(MONTH,emp.DOJ,convert(date,getdate(), 105)), ' months') AS Tenure,DOB,HODRating,Year
from [dbo].[EmployeeMaster] emp
JOIN [dbo].[ScoreCard] sc
on emp.Id = sc.EmpId where (KPIAVG > 0 AND SkillAVG > 0 and Year=datepart(year,getdate())) ORDER BY CASE WHEN SDM IS NULL OR SDM = '' THEN 0 ELSE 1 END DESC,SDM ASC
,CASE WHEN RM IS NULL OR RM = '' THEN 0 ELSE 1 END DESC,RM ASC");

        //        dt = objSQLDataFile.ExecuteDataTableSQL(@"select distinct sc.Id,EmpCode,EmpName,EmpDepartment,EmpDesignation,ORGScore, 
        //DepScore
        //from [dbo].[EmployeeMaster] emp
        //JOIN [dbo].[ScoreCard] sc
        //on emp.Id = sc.EmpId");



        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dt, "ScoreCard");
            //wb.Worksheets.Add(dt);
            using (MemoryStream stream = new MemoryStream())
            {
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=ScoreCard.xlsx");
                wb.SaveAs(stream);
                stream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }

    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the run time error "  
        //Control 'GridView1' of type 'Grid View' must be placed inside a form tag with runat=server."  
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (FileUpload1.PostedFile != null)
        {
            try
            {
                int res = 0;
                string filePath = Server.MapPath("../UploadFile/");
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss");
                filePath = filePath + fileName + ".xlsx";
                FileUpload1.PostedFile.SaveAs(filePath);
                User user = new User();

                XLWorkbook xLWorkbook = new XLWorkbook(filePath);
                int row = 2;
                while (xLWorkbook.Worksheets.Worksheet(1).Cell(row, 1).GetString() != "")
                {

                    int Id = int.Parse(xLWorkbook.Worksheets.Worksheet(1).Cell(row, 1).GetString());
                    //string EmpCode = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 2).GetString();
                    //string EmpName = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 3).GetString();
                    //string EmpDepartment = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 4).GetString();
                    //string EmpDesignation = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 5).GetString();
                    int orgscore = 0;
                    int.TryParse(xLWorkbook.Worksheets.Worksheet(1).Cell(row, 11).GetString(), out orgscore);
                    int depscore = 0;
                    int.TryParse(xLWorkbook.Worksheets.Worksheet(1).Cell(row, 12).GetString(), out depscore);

                    res = user.AdminScores(Id, orgscore, depscore);

                    if (res > 0 && orgscore > 0 && depscore > 0)
                    {
                        objSQLDataFile.ExecuteDataTableSQL(@"update ScoreCard set FinalScore = (ORGScore*15+DepScore*15+KPIAVG*50+SkillAVG*20)/100, PMSCategory = (case
	when (ORGScore*15+DepScore*15+KPIAVG*50+SkillAVG*20)/100 >= 95 then 'Excellent'
	when (ORGScore*15+DepScore*15+KPIAVG*50+SkillAVG*20)/100 >= 85 then 'Very Good'
	when (ORGScore*15+DepScore*15+KPIAVG*50+SkillAVG*20)/100 >= 71 then 'Good'
	when (ORGScore*15+DepScore*15+KPIAVG*50+SkillAVG*20)/100 >= 51 then 'Satisfactory'
	when (ORGScore*15+DepScore*15+KPIAVG*50+SkillAVG*20)/100 >= 5  then 'Below Satisfactory' end) where Id=" + Id);
                    }

                    row++;
                }
                if (res > 0)
                {
                    ClientScriptManager CSM = Page.ClientScript;
                    string strconfirm = "<script>if(window.confirm('File Uploaded')){window.location.href='Scores.aspx'}</script>";
                    CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                }
                else
                {
                    string message = "File Not Uploaded!";
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<script>alert('");
                    sb.Append(message);
                    sb.Append("'); </script>");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
                }
            }
            catch (Exception ex)
            {

            }

        }
    }


    protected void dlyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        string year = dlyear.SelectedValue;
        bindEmplooyewListScore(year);
    }
}
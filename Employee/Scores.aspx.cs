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

public partial class Employee_Scores : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    static int ISHOD = 0;
    static int empId = 0;
    static string userType = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["EmailId"] == null)
            {
                Response.Redirect("../Default.aspx");
            }
            binddlyears();
           
            bindEmplooyewListScore();
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

    protected void bindEmplooyewListScore()
    {
        DataTable res = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT Id,IsHOD from EmployeeMaster WHERE EmpEmailId = '" + Session["EmailId"].ToString() + "'");
        ISHOD = 0;
        empId = 0;
        if (res.Rows.Count > 0)
        {
            DataRow row2 = res.Rows[0];
            ISHOD = int.Parse(row2["IsHOD"].ToString());
            empId = int.Parse(row2["Id"].ToString());
        }
        userType = Session["UserType"].ToString();

        string str = txtsearchemp.Text.Trim();

        string year = dlyear.SelectedItem.Value;
        DataTable dt = getLogic(userType, str, ISHOD, empId, year);

        EmplooyewListScore.DataSource = dt;
        EmplooyewListScore.DataBind();
    }
    protected DataTable getLogic(string userType, string str, int ishod, int empId, string year)
    {
        DataTable dt = null;
        //temp changes made after contacting with nilesh sir
        // delete below condition and add following line only.(today's date 28062022 time 6:16pm)
        //below one line is test condition
         if (Session["EmailId"].Equals("vishalvk82@gmail.com"))
      
         // if (Session["EmailId"].Equals("nileshchudiwal1981@yahoo.com"))
        {
            if (str != "")
            {
                dt = objSQLDataFile.ExecuteDataTableSQL(@"select sc.Id,EmpCode,EmpName,DOJ,EmpDepartment,EmpDesignation,
KPIAVG, SkillAVG,RM,SDM,ORGScore, 
DepScore, FinalScore,PMSCategory,Location,
CONCAT(DATEDIFF(YEAR,emp.DOJ,convert(date,getdate(), 105)),' years ',
DATEDIFF(MONTH,emp.DOJ,convert(date,getdate(), 105)), ' months') AS Tenure,DOB,HODRating, Year
from [dbo].[EmployeeMaster] emp
JOIN [dbo].[ScoreCard] sc
on emp.Id = sc.EmpId
where (KPIAVG > 0 AND SkillAVG > 0 and Year='" + year + "') and EmpName LIKE'%" + str + "%' ORDER BY CASE WHEN SDM IS NULL OR SDM = '' THEN 0 ELSE 1 END DESC,SDM ASC, CASE WHEN RM IS NULL OR RM = '' THEN 0 ELSE 1 END DESC, RM ASC");
            }
            else
            {
                dt = objSQLDataFile.ExecuteDataTableSQL(@"select sc.Id,EmpCode,EmpName,DOJ,EmpDepartment,EmpDesignation,
KPIAVG, SkillAVG,RM,SDM,ORGScore, 
DepScore, FinalScore,PMSCategory,Location,
CONCAT(DATEDIFF(YEAR,emp.DOJ,convert(date,getdate(), 105)),' years ',
DATEDIFF(MONTH,emp.DOJ,convert(date,getdate(), 105)), ' months') AS Tenure,DOB,HODRating,Year
from [dbo].[EmployeeMaster] emp
JOIN [dbo].[ScoreCard] sc
on emp.Id = sc.EmpId  where KPIAVG > 0 AND SkillAVG > 0 and Year='" + year + "' ORDER BY CASE WHEN SDM IS NULL OR SDM = '' THEN 0 ELSE 1 END DESC,SDM ASC ,CASE WHEN RM IS NULL OR RM = '' THEN 0 ELSE 1 END DESC,RM ASC");
            }

        }
        else
        {
            if(ishod > 0)
            {
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
where (HOD = (SELECT EmpName from EmployeeMaster where Id = " + empId + ")" +
" or RM = (SELECT EmpName from EmployeeMaster where Id = " + empId + ")) and (KPIAVG > 0 AND SkillAVG > 0 and Year='" + year + @"') " +
"and EmpName LIKE'%" + str + "%' ORDER BY CASE WHEN SDM IS NULL OR SDM = '' THEN 0 ELSE 1 END DESC,SDM ASC, CASE WHEN RM IS NULL OR RM = '' THEN 0 ELSE 1 END DESC, RM ASC");
                }
                else
                {
                    dt = objSQLDataFile.ExecuteDataTableSQL(@"select sc.Id,EmpCode,EmpName,DOJ,EmpDepartment,EmpDesignation,
KPIAVG, SkillAVG,RM,SDM,ORGScore, 
DepScore, FinalScore,PMSCategory,Location,
CONCAT(DATEDIFF(YEAR,emp.DOJ,convert(date,getdate(), 105)),' years ',
DATEDIFF(MONTH,emp.DOJ,convert(date,getdate(), 105)), ' months') AS Tenure,DOB,HODRating,Year
from [dbo].[EmployeeMaster] emp
JOIN [dbo].[ScoreCard] sc
on emp.Id = sc.EmpId where (HOD = (SELECT EmpName from EmployeeMaster where Id = " + empId + ") " +
"or RM = (SELECT EmpName from EmployeeMaster where Id = " + empId + ")) and (KPIAVG > 0 AND SkillAVG > 0 and Year='" + year + @"') ORDER BY CASE WHEN SDM IS NULL OR SDM = '' THEN 0 ELSE 1 END DESC,SDM ASC, CASE WHEN RM IS NULL OR RM = '' THEN 0 ELSE 1 END DESC, RM ASC");
                }


            }
            else
            {
                EmplooyewListScore.Columns[16].Visible = false;
                EmplooyewListScore.Columns[10].Visible = false;
                EmplooyewListScore.Columns[11].Visible = false;
                EmplooyewListScore.Columns[14].Visible = false;
                EmplooyewListScore.Columns[15].Visible = false;

                if (str != "")
                {
                    //Added condition for SDM and ASM so that they will be able to see score card
                    dt = objSQLDataFile.ExecuteDataTableSQL(@"select distinct sc.Id,EmpCode,EmpName,DOJ,EmpDepartment,EmpDesignation,
KPIAVG, SkillAVG,RM,SDM,ORGScore, 
DepScore, FinalScore,PMSCategory,Location,
CONCAT(DATEDIFF(YEAR,emp.DOJ,convert(date,getdate(), 105)),' years ',
DATEDIFF(MONTH,emp.DOJ,convert(date,getdate(), 105)), ' months') AS Tenure,DOB,HODRating,Year
from [dbo].[EmployeeMaster] emp
JOIN [dbo].[ScoreCard] sc
on emp.Id = sc.EmpId
where (RM = (SELECT EmpName from EmployeeMaster where Id = " + empId + ") or SDM = (SELECT EmpName from EmployeeMaster where Id = " + empId + ") or ASM = (SELECT EmpName from EmployeeMaster where Id = " + empId + ")) and (KPIAVG > 0 AND SkillAVG > 0 and Year='" + year + @"') and EmpName LIKE'%" + str + "%'");
                }
                else
                {
                    dt = objSQLDataFile.ExecuteDataTableSQL(@"select distinct sc.Id,EmpCode,EmpName,DOJ,EmpDepartment,EmpDesignation,
KPIAVG, SkillAVG,RM,SDM,ORGScore, 
DepScore, FinalScore,PMSCategory,Location,
CONCAT(DATEDIFF(YEAR,emp.DOJ,convert(date,getdate(), 105)),' years ',
DATEDIFF(MONTH,emp.DOJ,convert(date,getdate(), 105)), ' months') AS Tenure,DOB,HODRating,Year
from [dbo].[EmployeeMaster] emp
JOIN [dbo].[ScoreCard] sc
on emp.Id = sc.EmpId where (RM = (SELECT EmpName from EmployeeMaster where Id = " + empId + ") or SDM = (SELECT EmpName from EmployeeMaster where Id = " + empId + ") or ASM = (SELECT EmpName from EmployeeMaster where Id = " + empId + ")) and (KPIAVG > 0 AND SkillAVG > 0 and Year='" + year + @"')");
                }

            }
        }
        return dt;

    }
    protected void btnsubmit_click(object sender, EventArgs e)
    {
        try
        {
            User user = new User();
            foreach (GridViewRow row in EmplooyewListScore.Rows)
            {
                int id = int.Parse(EmplooyewListScore.DataKeys[row.RowIndex].Values[0].ToString());
                string hodrating = (row.Cells[16].FindControl("txthodrating") as TextBox).Text;


                if (hodrating == "")
                {
                    hodrating = (row.Cells[16].FindControl("lblhodrating") as Label).Text;
                }

                objSQLDataFile.ExecuteDataTableSQL(@"update ScoreCard set HODRating = '"+ hodrating + "' where Id="+id);
                //user.AdminScores(id, forgscore, fdepscore);
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
            string hodrating = (e.Row.Cells[16].FindControl("lblhodrating") as Label).Text;

            if (hodrating != "")
            {
                TextBox tb1 = e.Row.Cells[16].FindControl("txthodrating") as TextBox;
                Label lbl = e.Row.Cells[16].FindControl("lblhodrating") as Label;
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

        this.bindEmplooyewListScore();
    }
    protected void Search(object sender, EventArgs e)
    {
        this.bindEmplooyewListScore();
    }
    protected void btnexport_Click(object sender, EventArgs e)
    {
        DataTable res = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT Id,IsHOD from EmployeeMaster WHERE EmpEmailId = '" + Session["EmailId"].ToString() + "'");
        ISHOD = 0;
        empId = 0;
        if (res.Rows.Count > 0)
        {
            DataRow row2 = res.Rows[0];
            ISHOD = int.Parse(row2["IsHOD"].ToString());
            empId = int.Parse(row2["Id"].ToString());
        }
        userType = Session["UserType"].ToString();
        string year = dlyear.SelectedValue;
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

        dt = getLogic(userType, "", ISHOD, empId, year);

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
        //if (FileUpload1.PostedFile != null)
        //{
        //    try
        //    {
        //        int res = 0;
        //        string filePath = Server.MapPath("../UploadFile/");
        //        string fileName = DateTime.Now.ToString("yyyyMMddHHmmss");
        //        filePath = filePath + fileName + ".xlsx";
        //        FileUpload1.PostedFile.SaveAs(filePath);
        //        User user = new User();

        //        XLWorkbook xLWorkbook = new XLWorkbook(filePath);
        //        int row = 2;
        //        while (xLWorkbook.Worksheets.Worksheet(1).Cell(row, 1).GetString() != "")
        //        {

        //            int Id = int.Parse(xLWorkbook.Worksheets.Worksheet(1).Cell(row, 1).GetString());
        //            //string EmpName = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 2).GetString();
        //            //string Competency = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 3).GetString();
        //            //string ManagementSkills = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 4).GetString();
        //            //string Description = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 5).GetString();
        //            //string RatingSelf = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 6).GetString();
        //            int RMRating = 0;
        //            int.TryParse(xLWorkbook.Worksheets.Worksheet(1).Cell(row, 7).GetString(), out RMRating);
        //            int HODRating = 0;
        //            int.TryParse(xLWorkbook.Worksheets.Worksheet(1).Cell(row, 8).GetString(), out HODRating);

        //            int consentRating = 0;
        //            int.TryParse(xLWorkbook.Worksheets.Worksheet(1).Cell(row, 9).GetString(), out consentRating);

        //            int idealRating = 0;
        //            int.TryParse(xLWorkbook.Worksheets.Worksheet(1).Cell(row, 10).GetString(), out idealRating);

        //            string idlrtng = Convert.ToString(idealRating);
        //            string consetntR = Convert.ToString(consentRating);

        //            string txtformula = "(consentrating/idealrating)*100";
        //            string overallscore = txtformula.Replace("idealrating", idlrtng).Replace("consentrating", consetntR);
        //            DataTable dt = new DataTable();
        //            float oas = 0;
        //            float.TryParse(dt.Compute(overallscore, "").ToString(), out oas);
        //            if (oas > 100)
        //            {
        //                oas = 100;
        //            }

        //            res = user.ImportIdealRating(Id, RMRating, HODRating, consentRating, idealRating);

        //            if (consentRating > 0 && idealRating > 0)
        //            {
        //                objSQLDataFile.ExecuteDataTableSQL(@"UPDATE SkillsUserwise SET OverallScore = " + oas + " WHERE Id = " + Id);
        //            }

        //            row++;
        //        }
        //        if (res > 0)
        //        {
        //            ClientScriptManager CSM = Page.ClientScript;
        //            string strconfirm = "<script>if(window.confirm('File Uploaded')){window.location.href='AddRatings.aspx'}</script>";
        //            CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
        //        }
        //        else
        //        {
        //            string message = "File Not Uploaded!";
        //            System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //            sb.Append("<script>alert('");
        //            sb.Append(message);
        //            sb.Append("'); </script>");
        //            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //}
    }



    protected void dlyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        string year = dlyear.SelectedValue;
        bindEmplooyewListScore();

    }
}
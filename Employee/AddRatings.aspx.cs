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

public partial class HR_AddRatings : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    static string empName;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["EmailId"] == null)
            {
                Response.Redirect("../Default.aspx");
            }
            bindgetSkills();
        }

    }
    public void bindgetSkills()
    {
        DataTable res = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT Id,EmpName from EmployeeMaster WHERE EmpEmailId = '" + Session["EmailId"].ToString() + "'");
        string str = txtsearchemp.Text.Trim();
        DataTable dt = null;



        if (Session["UserType"].ToString().Equals("EMPLOYEE"))
        {
            DataRow row2 = res.Rows[0];
            //int empId = int.Parse(row2["Id"].ToString());
            empName = row2["EmpName"].ToString();


            if (str != "")
            {
                //here added condition for current year (ratings should be binded to add current year idealrating) need to test once again
                dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT suw.Id, EmpName,Competency, 
            ManagementSkills, Description, RatingSelf, RMRating, HODRating,ConsentRating,IdealRating, OverallScore, HOD, RM ,Year from SkillsUserwise suw
            left join [dbo].[EmployeeMaster] emp on suw.EmpId = emp.Id where emp.HOD = '" + empName + "' and ConsentRating > 0 and suw.year = YEAR(GETDATE()) and EmpName LIKE'%" + str + "%'");
            }
            else
            {
                dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT suw.Id, EmpName,Competency, 
            ManagementSkills, Description, RatingSelf, RMRating, HODRating,ConsentRating,IdealRating, OverallScore, HOD, RM, Year from SkillsUserwise suw
            left join [dbo].[EmployeeMaster] emp on suw.EmpId = emp.Id where emp.HOD = '" + empName + "' and ConsentRating > 0 and suw.year = YEAR(GETDATE())");
            }

        }

        //temp changes made after contacting with nilesh sir
        // delete below condition and add folling line only.(today's date 28062022 time 6:16pm)
       //else if (Session["UserType"].ToString().Equals("EMPLOYEEHR"))
       else if (Session["EmailId"].Equals("vishalvk82@gmail.com"))
       // else if (Session["EmailId"].Equals("nileshchudiwal1981@yahoo.com"))
        {

            if (str != "")
            {
                dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT suw.Id, EmpName,Competency, 
            ManagementSkills, Description, RatingSelf, RMRating, HODRating,ConsentRating,IdealRating, OverallScore, HOD, RM, year  from SkillsUserwise suw
            left join [dbo].[EmployeeMaster] emp on suw.EmpId = emp.Id where ConsentRating > 0 and suw.year = YEAR(GETDATE()) and EmpName LIKE'%" + str + "%'");
            }
            else
            {
                dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT suw.Id, EmpName,Competency, 
            ManagementSkills, Description, RatingSelf, RMRating, HODRating,ConsentRating,IdealRating, OverallScore, HOD, RM ,year from SkillsUserwise suw
            left join [dbo].[EmployeeMaster] emp on suw.EmpId = emp.Id where ConsentRating > 0 and suw.year = YEAR(GETDATE())");
            }

        }

        SkillsList.DataSource = dt;
        SkillsList.DataBind();
    }

    protected void btnsubmit_click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in SkillsList.Rows)
        {
            string txtformula = "(consentrating/idealrating)*100";
            string SkillId = (row.Cells[11].FindControl("SkillId") as Label).Text;
            string idealrating = (row.Cells[10].FindControl("txtidealrating") as TextBox).Text;
            string consendtrating = row.Cells[7].Text;
            string idrating = row.Cells[8].Text;

            if (idealrating == "")
            {
                idealrating = idrating;
            }

            if((idealrating != "" && idealrating != "&nbsp;") && idealrating != "")
            {
                string st = txtformula.Replace("idealrating", idealrating).Replace("consentrating", consendtrating);

                DataTable dt = new DataTable();
                float oas = 0;
                float.TryParse(dt.Compute(st, "").ToString(), out oas);

                if (oas > 100)
                {
                    oas = 100;
                }

                if (idealrating != "")
                {
                    // objSQLDataFile.ExecuteDataTableSQL(@"UPDATE SkillsUserwise SET IdealRating = " + idealrating + "WHERE Id = " + SkillId);
                    objSQLDataFile.ExecuteDataTableSQL(@"UPDATE SkillsUserwise SET IdealRating = " + idealrating + "WHERE Id = " + SkillId + " and year=datepart(year,GETDATE())");
                }
                if (consendtrating != "" && idrating != "")
                {
                    //objSQLDataFile.ExecuteDataTableSQL(@"UPDATE SkillsUserwise SET OverallScore = " + oas + " WHERE Id = " + SkillId);

                    objSQLDataFile.ExecuteDataTableSQL(@"UPDATE SkillsUserwise SET OverallScore = " + oas + " WHERE Id = " + SkillId + " and year=datepart(year,GETDATE())");

                    DataTable dt2 = objSQLDataFile.ExecuteDataTableSQL(@"SELECT EmpId from [dbo].[SkillsUserwise] WHERE Id = " + SkillId);
                    int empId = int.Parse(dt2.Rows[0]["EmpId"].ToString());
                    User user = new User();
                    user.SCSkillAVG(empId);
                }
            }
            
        }
        Response.Redirect("AddRatings.aspx");
    }
    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int idealrating = 0;
            int.TryParse(e.Row.Cells[8].Text.ToString(), out idealrating);

            if (idealrating > 0)
            {
                e.Row.Cells[10].Style.Add("visibility", "hidden");
            }

            e.Row.Cells[0].Text = Regex.Replace(e.Row.Cells[0].Text, txtsearchemp.Text.Trim(), delegate (Match match)
            {
                return string.Format("<span style = 'background-color:#D9EDF7'>{0}</span>", match.Value);
            }, RegexOptions.IgnoreCase);
        }
    }
    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SkillsList.PageIndex = e.NewPageIndex;
        this.bindgetSkills();
    }
    protected void Search(object sender, EventArgs e)
    {
        this.bindgetSkills();
    }
    protected void btnexport_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable("Grid");
        //dt.Columns.AddRange(new DataColumn[10] {
        //            new DataColumn("Id"),
        //            new DataColumn("EmpName"),
        //            new DataColumn("Competency"),
        //            new DataColumn("ManagementSkills"),
        //            new DataColumn("Description"),
        //            new DataColumn("RatingSelf"),
        //            new DataColumn("RMRating"),
        //            new DataColumn("HODRating"),
        //            new DataColumn("ConsentRating"),
        //            new DataColumn("IdealRating")
        //            });

        dt.Columns.AddRange(new DataColumn[8] {
                    new DataColumn("Id"),
                    new DataColumn("EmpName"),
                    new DataColumn("Competency"),
                    new DataColumn("ManagementSkills"),
                    new DataColumn("Description"),
                    new DataColumn("ConsentRating"),
                    new DataColumn("IdealRating"),
                    new DataColumn("Year")
                    });


        if (Session["UserType"].ToString().Equals("EMPLOYEE"))
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT suw.Id, EmpName,Competency, 
            ManagementSkills, Description,ConsentRating,IdealRating,Year from SkillsUserwise suw
            left join [dbo].[EmployeeMaster] emp on suw.EmpId = emp.Id where emp.HOD = '" + empName + "' and ConsentRating > 0 and Year = datepart(year,GETDATE())");
        }
        else if (Session["UserType"].ToString().Equals("EMPLOYEEHR"))
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT suw.Id, EmpName,Competency, 
            ManagementSkills, Description,ConsentRating,IdealRating,Year from SkillsUserwise suw
            left join [dbo].[EmployeeMaster] emp on suw.EmpId = emp.Id where ConsentRating > 0 and Year = datepart(year,GETDATE())");
        }

        //dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT suw.Id, EmpName,Competency, 
        //    ManagementSkills, Description,ConsentRating,IdealRating  from SkillsUserwise suw
        //    left join [dbo].[EmployeeMaster] emp on suw.EmpId = emp.Id where ConsentRating > 0 and IdealRating is null");



        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dt, "IdealRating");
            //wb.Worksheets.Add(dt);
            using (MemoryStream stream = new MemoryStream())
            {
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=IdealRatings.xlsx");
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
                    //string EmpName = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 2).GetString();
                    //string Competency = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 3).GetString();
                    //string ManagementSkills = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 4).GetString();
                    //string Description = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 5).GetString();
                    //string RatingSelf = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 6).GetString();
                    //int RMRating = 0;
                    //int.TryParse(xLWorkbook.Worksheets.Worksheet(1).Cell(row, 7).GetString(), out RMRating);
                    //int HODRating = 0;
                    //int.TryParse(xLWorkbook.Worksheets.Worksheet(1).Cell(row, 8).GetString(), out HODRating);

                    int consentRating = 0;
                    int.TryParse(xLWorkbook.Worksheets.Worksheet(1).Cell(row, 6).GetString(), out consentRating);

                    int idealRating = 0;
                    int.TryParse(xLWorkbook.Worksheets.Worksheet(1).Cell(row, 7).GetString(), out idealRating);

                    string idlrtng = Convert.ToString(idealRating);
                    string consetntR = Convert.ToString(consentRating);

                    string txtformula = "(consentrating/idealrating)*100";
                    string overallscore = txtformula.Replace("idealrating", idlrtng).Replace("consentrating", consetntR);
                    DataTable dt = new DataTable();
                    float oas = 0;
                    float.TryParse(dt.Compute(overallscore, "").ToString(), out oas);
                    if (oas > 100)
                    {
                        oas = 100;
                    }

                    res = user.ImportIdealRating(Id, idealRating, consentRating);

                    if (consentRating > 0 && idealRating > 0)
                    {
                      // objSQLDataFile.ExecuteDataTableSQL(@"UPDATE SkillsUserwise SET OverallScore = " + oas + " WHERE Id = " + Id);
                       objSQLDataFile.ExecuteDataTableSQL(@"UPDATE SkillsUserwise SET OverallScore = " + oas + " WHERE Id = " + Id + " and year=datepart(year,getadate())");

                        DataTable dt2 = objSQLDataFile.ExecuteDataTableSQL(@"SELECT EmpId from [dbo].[SkillsUserwise] WHERE Id = " + Id);
                        int empId = int.Parse(dt2.Rows[0]["EmpId"].ToString());
                        user.SCSkillAVG(empId);
                    }

                    row++;
                }
                if (res > 0)
                {
                    ClientScriptManager CSM = Page.ClientScript;
                    string strconfirm = "<script>if(window.confirm('File Uploaded')){window.location.href='AddRatings.aspx'}</script>";
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
}

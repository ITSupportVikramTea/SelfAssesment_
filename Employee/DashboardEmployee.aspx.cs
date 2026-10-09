using ClosedXML.Excel;
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

public partial class Employee_Dashboard : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    public static string btnJobApp;
    protected static string FirstName { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["EmailId"] == null)
            {
                Response.Redirect("../Default.aspx");
            }

            getUsername();
            bindPendingDetailsSkills();
            bindPendingDetailsKPI();
            ishodorrm();
        }
    }

    protected void getUsername()
    {
        DataTable res = objSQLDataFile.ExecuteDataTableSQL(@"SELECT EmpName from EmployeeMaster
                            WHERE EmpEmailId = '" + Session["EmailId"].ToString() + "'");
        if (res.Rows.Count > 0)
        {
            DataRow row2 = res.Rows[0];
            FirstName = row2["EmpName"].ToString();
        }
        else
        {
            FirstName = "Unknown";
        }
    }
    public void bindPendingDetailsSkills()
    {
        DataTable res = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT Id,EmpName from EmployeeMaster WHERE EmpEmailId = '" + Session["EmailId"].ToString() + "'");
        DataTable dt = null;
        string str = txtsearchemp.Text.Trim();

        if (Session["UserType"].ToString().Equals("EMPLOYEE"))
        {
            DataRow row2 = res.Rows[0];
            int empId = int.Parse(row2["Id"].ToString());
            //string empName = row2["EmpName"].ToString();

            //added one condition of year to bind skill report with year
            if (str == "")
            {
                dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT   EmpName,  
                            sum( case when isnull( RatingSelf,0 ) =0 then 1 else 0 end)  PendindgSelfRating ,
                            sum( case when isnull( RMRating,0 ) =0 then 1 else 0 end)  PendingRMRating,
                            sum( case when isnull( HODRating,0 ) =0 then 1 else 0 end)  PendingHODRating ,
                            sum( case when isnull( ConsentRating,0 ) =0 then 1 else 0 end)  ConsentRating,
                            sum( case when isnull( RatingSelf,0 ) =0 then 1 else 0 end)  +
                            sum( case when isnull( RMRating,0 ) =0 then 1 else 0 end) + 
                            sum( case when isnull( HODRating,0 ) =0 then 1 else 0 end) +
                            sum( case when isnull( ConsentRating,0 ) =0 then 1 else 0 end)  
                            TotalPenRating, HOD, RM, Year from SkillsUserwise suw left join [dbo].[EmployeeMaster] emp 
                            on suw.EmpId = emp.Id  WHERE (EmpId=" + empId +
                            " or HOD = (select EmpName from EmployeeMaster where id=" + empId + " ) " +
                            "OR RM = (select EmpName from EmployeeMaster where id=" + empId + " )) and suw.year = YEAR(GETDATE())" +
                            "group by EmpName, HOD, RM, Year order by EmpName ");

            }
            else
            {
                dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT   EmpName,  
                            sum( case when isnull( RatingSelf,0 ) =0 then 1 else 0 end)  PendindgSelfRating ,
                            sum( case when isnull( RMRating,0 ) =0 then 1 else 0 end)  PendingRMRating,
                            sum( case when isnull( HODRating,0 ) =0 then 1 else 0 end)  PendingHODRating ,
                            sum( case when isnull( ConsentRating,0 ) =0 then 1 else 0 end)  ConsentRating,
                            sum( case when isnull( RatingSelf,0 ) =0 then 1 else 0 end)  +
                            sum( case when isnull( RMRating,0 ) =0 then 1 else 0 end) + 
                            sum( case when isnull( HODRating,0 ) =0 then 1 else 0 end) +
                            sum( case when isnull( ConsentRating,0 ) =0 then 1 else 0 end)  
                            TotalPenRating, HOD, RM from SkillsUserwise suw left join [dbo].[EmployeeMaster] emp 
                            on suw.EmpId = emp.Id  WHERE (EmpId=" + empId +
                            " or HOD = (select EmpName from EmployeeMaster where id=" + empId + " ) " +
                            "OR RM = (select EmpName from EmployeeMaster where id=" + empId + " ) ) and suw.year = YEAR(GETDATE()) " +
                            "and EmpName Like '%" + str + "%' group by EmpName, HOD, RM, Year order by EmpName ");

            }


        }

        //This is condition HR is also able to view skills report of all employees 
        else if (Session["UserType"].ToString().Equals("EMPLOYEEHR"))
        {

            if (str == "")
            {
                //DataRow row2 = res.Rows[0];
                //int empId = int.Parse(row2["Id"].ToString());
                //string empName = row2["EmpName"].ToString();
                dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT   EmpName,  
                            sum( case when isnull( RatingSelf,0 ) =0 then 1 else 0 end)  PendindgSelfRating ,
                            sum( case when isnull( RMRating,0 ) =0 then 1 else 0 end)  PendingRMRating,
                            sum( case when isnull( HODRating,0 ) =0 then 1 else 0 end)  PendingHODRating ,
                            sum( case when isnull( ConsentRating,0 ) =0 then 1 else 0 end)  ConsentRating,
                            sum( case when isnull( RatingSelf,0 ) =0 then 1 else 0 end)  +
                            sum( case when isnull( RMRating,0 ) =0 then 1 else 0 end) + 
                            sum( case when isnull( HODRating,0 ) =0 then 1 else 0 end) +
                            sum( case when isnull( ConsentRating,0 ) =0 then 1 else 0 end) 
                            TotalPenRating, HOD, RM from SkillsUserwise suw left join [dbo].[EmployeeMaster] emp 
                            on suw.EmpId = emp.Id where suw.year = YEAR(GETDATE()) group by EmpName, HOD, RM order by EmpName ");

            }
            else
            { //DataRow row2 = res.Rows[0];
              //int empId = int.Parse(row2["Id"].ToString());
              //string empName = row2["EmpName"].ToString();
                dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT   EmpName,  
                            sum( case when isnull( RatingSelf,0 ) =0 then 1 else 0 end)  PendindgSelfRating ,
                            sum( case when isnull( RMRating,0 ) =0 then 1 else 0 end)  PendingRMRating,
                            sum( case when isnull( HODRating,0 ) =0 then 1 else 0 end)  PendingHODRating ,
                            sum( case when isnull( ConsentRating,0 ) =0 then 1 else 0 end)  ConsentRating,
                            sum( case when isnull( RatingSelf,0 ) =0 then 1 else 0 end)  +
                            sum( case when isnull( RMRating,0 ) =0 then 1 else 0 end) + 
                            sum( case when isnull( HODRating,0 ) =0 then 1 else 0 end) +
                            sum( case when isnull( ConsentRating,0 ) =0 then 1 else 0 end) 
                            TotalPenRating, HOD, RM from SkillsUserwise suw left join [dbo].[EmployeeMaster] emp 
                            on suw.EmpId = emp.Id where EmpName Like '%" + str + "%' and suw.year = YEAR(GETDATE()) group by EmpName, HOD, RM order by EmpName ");

            }

        }

        gdPendingDetails.DataSource = dt;
        gdPendingDetails.DataBind();
    }


    //add year condition while binding
    public void bindPendingDetailsKPI()
    {
        DataTable res = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT Id,EmpName from EmployeeMaster 
                                WHERE EmpEmailId = '" + Session["EmailId"].ToString() + "'");

        DataTable dt = null;
        string str = txtsearchemp.Text.Trim();

        if (Session["UserType"].ToString().Equals("EMPLOYEE"))
        {
            DataRow row = res.Rows[0];
            int empId = int.Parse(row["Id"].ToString());
            //string EmpName = row["EmpName"].ToString();
            if (str == "")
            {
                dt = objSQLDataFile.ExecuteDataTableSQL(@"select distinct EmpName, 
                        sum( case when isnull( Achievement,0 ) =0 then 1 else 0 end)  TotalPenKPI 
                        from [dbo].[KRAUserwise] kuw left join[dbo].[EmployeeMaster] emp on kuw.EmpId = emp.Id 
                        WHERE (EmpId= " + empId + " or HOD = (select EmpName from EmployeeMaster where id=" + empId + ") OR " +
                          "RM = (select EmpName from EmployeeMaster where id=" + empId + ")) and kuw.Year = YEAR(GETDATE()) group by EmpName order by EmpName ");

            }
            else
            {
                dt = objSQLDataFile.ExecuteDataTableSQL(@"select distinct EmpName, 
                        sum( case when isnull( Achievement,0 ) =0 then 1 else 0 end)  TotalPenKPI 
                        from [dbo].[KRAUserwise] kuw left join[dbo].[EmployeeMaster] emp on kuw.EmpId = emp.Id 
                        WHERE (EmpId= " + empId + " or HOD = (select EmpName from EmployeeMaster where id=" + empId + ") OR " +
                             "RM = (select EmpName from EmployeeMaster where id=" + empId + ")) and EmpName Like '%" + str + "%' and kuw.Year = YEAR(GETDATE()) group by EmpName order by EmpName ");

            }



        }
        else if (Session["UserType"].ToString().Equals("EMPLOYEEHR"))
        {
            if (str == "")
            {
                dt = objSQLDataFile.ExecuteDataTableSQL(@"select distinct EmpName, 
                        sum( case when isnull( Achievement,0 ) =0 then 1 else 0 end)  TotalPenKPI 
                        from [dbo].[KRAUserwise] kuw left join[dbo].[EmployeeMaster] emp on kuw.EmpId = emp.Id where kuw.Year = YEAR(GETDATE())group by EmpName order by EmpName ");

            }
            else
            {

                dt = objSQLDataFile.ExecuteDataTableSQL(@"select distinct EmpName, 
                        sum( case when isnull( Achievement,0 ) =0 then 1 else 0 end)  TotalPenKPI 
                        from [dbo].[KRAUserwise] kuw left join[dbo].[EmployeeMaster] emp on kuw.EmpId = emp.Id 
                        where EmpName Like '%" + str + "%' and kuw.Year = YEAR(GETDATE()) group by EmpName order by EmpName ");
            }


        }


        gdKPIPending.DataSource = dt;
        gdKPIPending.DataBind();

    }

   

    public void ishodorrm()
    {
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT IsHOD,IsRM from EmployeeMaster where EmpEmailId ='" + Session["EmailID"] + "'");
        if (dt.Rows.Count > 0)
        {
            try
            {
                DataRow row = dt.Rows[0];
                int ishod = int.Parse(row["IsHOD"].ToString());
                int isrm = int.Parse(row["IsRM"].ToString());
                if (ishod > 0 || isrm > 0)
                {
                    btnJobApp = "true";
                }
                else
                {
                    btnJobApp = "false";
                }
            }
            catch
            {

            }
            
        }
    }

    protected void btnjobapp_click(object sender, EventArgs e)
    {
        Response.Redirect("JobApplication.aspx");
    }
    protected void btnChangepass_click(object sender, EventArgs e)
    {
        Response.Redirect("ChangePassword.aspx");
    }

    //SEARCHING FOR SKILLS
    protected void gdPendingDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Text = Regex.Replace(e.Row.Cells[0].Text, txtsearchemp.Text.Trim(), delegate (Match match)
            {
                return string.Format("<span style = 'background-color:#D9EDF7'>{0}</span>", match.Value);
            }, RegexOptions.IgnoreCase);

        }
    }
    protected void gdPendingDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdPendingDetails.PageIndex = e.NewPageIndex;
        this.bindPendingDetailsSkills();

    }
    protected void SearchSkillUser(object sender, EventArgs e)
    {
        this.bindPendingDetailsSkills();
    }

    //SEARCHING FOR KPI
    protected void SearchKPIUser(object sender, EventArgs e)
    {
        this.bindPendingDetailsKPI();
    }

    protected void gdKPIPending_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Text = Regex.Replace(e.Row.Cells[0].Text, txtsearchemp.Text.Trim(), delegate (Match match)
            {
                return string.Format("<span style = 'background-color:#D9EDF7'>{0}</span>", match.Value);
            }, RegexOptions.IgnoreCase);

        }
    }

    protected void gdKPIPending_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdKPIPending.PageIndex = e.NewPageIndex;
        this.bindPendingDetailsKPI();

    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the run time error "  
        //Control 'GridView1' of type 'Grid View' must be placed inside a form tag with runat=server."  
    }

    public void ExportToExcel(object sender, EventArgs e)
    {

        //Original Code

         Response.Clear();
         Response.Buffer = true;
         string filename = "PendingDetails-" + DateTime.Now + ".xls";
         Response.AddHeader("content-disposition", "attachment;filename=" + filename);
         Response.Charset = "";
         Response.ContentType = "application/vnd.ms-excel";
         using (StringWriter sw = new StringWriter())
         {
             HtmlTextWriter hw = new HtmlTextWriter(sw);

             //To Export all pages
             gdPendingDetails.AllowPaging = false;
             this.bindPendingDetailsSkills();

             gdPendingDetails.HeaderRow.BackColor = Color.White;
             foreach (TableCell cell in gdPendingDetails.HeaderRow.Cells)
             {
                 cell.BackColor = gdPendingDetails.HeaderStyle.BackColor;
             }
             foreach (GridViewRow row in gdPendingDetails.Rows)
             {
                 row.BackColor = Color.White;
                 foreach (TableCell cell in row.Cells)
                 {
                     if (row.RowIndex % 2 == 0)
                     {
                         cell.BackColor = gdPendingDetails.AlternatingRowStyle.BackColor;
                     }
                     else
                     {
                         cell.BackColor = gdPendingDetails.RowStyle.BackColor;
                     }
                     cell.CssClass = "textmode";
                 }
             }

             gdPendingDetails.RenderControl(hw);

             //style to format numbers to string
             string style = @"<style> .textmode { } </style>";
             Response.Write(style);
             Response.Output.Write(sw.ToString());
             Response.Flush();
             Response.End();
         }
     }

        //test code 

       /* DataTable dt1 = new DataTable("Grid1");
        dt1.Columns.AddRange(new DataColumn[4] {
    new DataColumn("Id"),
    new DataColumn("ERPID"),
    new DataColumn("EmpName"),
    new DataColumn ("TotalPenSkills")

});

        DataTable dt2 = new DataTable("Grid2");
        dt2.Columns.AddRange(new DataColumn[2] {
    new DataColumn("EmpName"),
    new DataColumn("TotalPenKPI")
});

        // Execute the first query and populate dt1
        dt1 = objSQLDataFile.ExecuteDataTableSQL(@"SELECT EmpName,
    sum(case when isnull(RatingSelf, 0) = 0 then 1 else 0 end) PendindgSelfRating,
    sum(case when isnull(RMRating, 0) = 0 then 1 else 0 end) PendingRMRating,
    sum(case when isnull(HODRating, 0) = 0 then 1 else 0 end) PendingHODRating,
    sum(case when isnull(ConsentRating, 0) = 0 then 1 else 0 end) ConsentRating,
    sum(case when isnull(RatingSelf, 0) = 0 then 1 else 0 end) +
    sum(case when isnull(RMRating, 0) = 0 then 1 else 0 end) +
    sum(case when isnull(HODRating, 0) = 0 then 1 else 0 end) +
    sum(case when isnull(ConsentRating, 0) = 0 then 1 else 0 end) TotalPenRating,
    HOD,
    RM
    FROM SkillsUserwise suw
    LEFT JOIN [dbo].[EmployeeMaster] emp ON suw.EmpId = emp.Id
    WHERE suw.year = YEAR(GETDATE())
    GROUP BY EmpName, HOD, RM
    ORDER BY EmpName");


        // Execute the second query and populate dt2
        dt2 = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT EmpName, 
    sum(case when isnull(Achievement, 0) = 0 then 1 else 0 end) TotalPenKPI
    FROM [dbo].[KRAUserwise] kuw
    LEFT JOIN [dbo].[EmployeeMaster] emp ON kuw.EmpId = emp.Id
    WHERE kuw.Year = YEAR(GETDATE())
    GROUP BY EmpName
    ORDER BY EmpName");

        using (XLWorkbook wb = new XLWorkbook())
        {
            // Add the first DataTable to the workbook
            wb.Worksheets.Add(dt1, "Pending Skill Ratings");

            // Add the second DataTable to the workbook
            wb.Worksheets.Add(dt2, "Pending KPI Ratings");

            using (MemoryStream stream = new MemoryStream())
            {
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=PendingDetails.xlsx");
                wb.SaveAs(stream);
                stream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }*/
    
    }
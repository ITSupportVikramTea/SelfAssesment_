using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webpages_Report : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["username"] == null)
            {
                Response.Redirect("../Default.aspx");
            }
            BindData();
        }
    }
    public void BindData()
    {

        AttendanceData.DataSource = null;
        AttendanceData.DataBind();
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@" SELECT        CONVERT(varchar, atlog.ADate, 105) ADate, emp.EmployeeName, emp.Department, emp.Role, CONVERT(TIME(0), atlog.InTime) InTime, emp.Location, atlog.Username, atlog.Status
FROM            [EmployeeMaster] emp JOIN
                         [dbo].[AttendanceLog] atlog ON atlog.EmployeeCode = emp.EmployeeCode
WHERE        CONVERT(varchar, atlog.ADate, 105) = CONVERT(varchar, getdate(), 105) AND atlog.Status = 'Y'
UNION ALL
SELECT        CONVERT(varchar, getdate(), 105) ADate, emp.EmployeeName, emp.Department, emp.Role, '' InTime, emp.Location, '' Username, 'N' Status
FROM            [EmployeeMaster] emp
WHERE        emp.EmployeeCode NOT IN
                             (SELECT        atlog.EmployeeCode
                               FROM            [dbo].[AttendanceLog] atlog
                               WHERE        CONVERT(varchar, atlog.ADate, 105) = CONVERT(varchar, getdate(), 105) AND atlog.Status = 'Y')");
        AttendanceData.DataSource = dt;
        AttendanceData.DataBind();
    }



    protected void btnsearch_Click(object sender, EventArgs e)
    {
        string condition1 = "", condition2 = "";
        string fromdate = txtfromdate.Text;
        string todate = txttodate.Text;
        if (Attype.Text != "All")
        {
            condition2 = " where Status = '" + Attype.Text + "'";
        }
        if (fromdate != "" && todate != "")
        {
            condition1 = " and   atlog.ADate between  convert(date, '" + fromdate + "',103) and  convert(date, '" + todate + "',103) ";
        }

        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@" Select * from
                                                            (SELECT        CONVERT(varchar, atlog.ADate, 105) ADate, emp.EmployeeName, emp.Department, emp.Role, 
                                                               CONVERT(TIME(0), atlog.InTime) InTime, emp.Location, atlog.Username, atlog.Status
                                                            FROM            [EmployeeMaster] emp JOIN
                                                                             [dbo].[AttendanceLog] atlog ON atlog.EmployeeCode = emp.EmployeeCode
                                                            WHERE         atlog.Status = 'Y'  " + condition1 + @"
                                                            UNION ALL
                                                            SELECT       '' ADate, emp.EmployeeName, emp.Department, emp.Role, '' InTime, emp.Location, '' Username, 'N' Status
                                                            FROM            [EmployeeMaster] emp
                                                            WHERE        emp.EmployeeCode NOT IN
                                                            (SELECT        atlog.EmployeeCode
                                                                        FROM            [dbo].[AttendanceLog] atlog
                                                                        WHERE       atlog.Status = 'Y'   " + condition1 + @" )
                                                            )Listdata   " + condition2);

        AttendanceData.DataSource = dt;
        AttendanceData.DataBind();
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        string condition1 = "", condition2 = "";
        string fromdate = txtfromdate.Text;
        string todate = txttodate.Text;
        if (Attype.Text != "All")
        {
            condition2 = " where Status = '" + Attype.Text + "'";
        }
        if (fromdate != "" && todate != "")
        {
            condition1 = " and   atlog.ADate between  convert(date, '" + fromdate + "',103) and  convert(date, '" + todate + "',103) ";
        }

        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@" Select * from
                                                            (SELECT        CONVERT(varchar, atlog.ADate, 105) ADate, emp.EmployeeName, emp.Department, emp.Role, 
                                                               CONVERT(TIME(0), atlog.InTime) InTime, emp.Location, atlog.Username, atlog.Status
                                                            FROM            [EmployeeMaster] emp JOIN
                                                                             [dbo].[AttendanceLog] atlog ON atlog.EmployeeCode = emp.EmployeeCode
                                                            WHERE         atlog.Status = 'Y'  " + condition1 + @"
                                                            UNION ALL
                                                            SELECT       '' ADate, emp.EmployeeName, emp.Department, emp.Role, '' InTime, emp.Location, '' Username, 'N' Status
                                                            FROM            [EmployeeMaster] emp
                                                            WHERE        emp.EmployeeCode NOT IN
                                                            (SELECT        atlog.EmployeeCode
                                                                        FROM            [dbo].[AttendanceLog] atlog
                                                                        WHERE       atlog.Status = 'Y'   " + condition1 + @" )
                                                            )Listdata   " + condition2);

        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dt, "Customers");

            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=SqlExport.csv");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }
}
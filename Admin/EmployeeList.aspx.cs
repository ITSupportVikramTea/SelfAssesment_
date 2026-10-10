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
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_EmployeeList : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    PassNMail pm = new PassNMail();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["EmailId"] == null)
            {
                Response.Redirect("../Default.aspx");
            }

            bindRm();
            bindgetEmployeeList();
        }
    }
    public void bindRm()
    {
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"Select distinct * from Employeemaster where IsRM = 1 order by EmpName asc");
        dlRm.DataSource = dt;
        dlRm.DataTextField = "EmpName";
        dlRm.DataTextField = "Empname";
        dlRm.DataBind();
    }
   
    public void bindgetEmployeeList(string empname = "")
    {
        string str = txtsearchuser.Text.Trim();
        DataTable dt;

        //Added dropdown to select RM
        //updated code to filter employee list according to RM(29_04_2023) 

        if (empname != "")
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"Select * from EmployeeMaster where RM = (Select distinct EmpName from employeemaster 
                                                        where EmpName = '" + dlRm.SelectedItem.Value + "') and EmpName LIKE '%" + str + "%' order by EmpName asc");

        }
        else if (str != "")
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT  * from EmployeeMaster where EmpName LIKE '%" + str + "%' order by EmpName");
        }
        else
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from EmployeeMaster order by EmpName");
        }

        EmployeeList.DataSource = dt;
        EmployeeList.DataBind();
    }
    protected void EmployeeList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
         try
         {
             int recordid = Convert.ToInt32(e.CommandName);
             Response.Redirect("AddEmployees.aspx?id=" + recordid);
         }
         catch { }
    }
    protected void btnaddemp_click(object sender, EventArgs e)
    {
        Response.Redirect("AddEmployees.aspx");
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            if (FileUpload1.PostedFile != null)
            {
                int res = 0, once = 0;
                string filePath = Server.MapPath("../UploadFile/");
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss");
                filePath = filePath + fileName + ".xlsx";
                FileUpload1.PostedFile.SaveAs(filePath);

                XLWorkbook xLWorkbook = new XLWorkbook(filePath);
                int row = 2;
                while (xLWorkbook.Worksheets.Worksheet(1).Cell(row, 1).GetString() != "")
                {
                    try
                    {
                        string EmpCode = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 1).GetString();
                        string ERPId = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 2).GetString();
                        string EmpName = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 3).GetString();
                        string EmpEmailId = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 4).GetString();
                        string EmpDesignation = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 5).GetString();
                        string EmpDepartment = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 6).GetString();
                        string Status = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 7).GetString();
                        string HOD = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 8).GetString();
                        string RM = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 9).GetString();
                        string DOJ = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 10).GetString();
                        string LastPromoYear = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 11).GetString();
                        string DesgChangeDate = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 12).GetString();
                        string ExistingCTC = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 13).GetString();
                        string UserType = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 14).GetString();
                        int IsHOD = 0;
                        int.TryParse(xLWorkbook.Worksheets.Worksheet(1).Cell(row, 15).GetString(), out IsHOD);
                        int IsRM = 0;
                        int.TryParse(xLWorkbook.Worksheets.Worksheet(1).Cell(row, 16).GetString(), out IsRM);
                        int IsHR = 0;
                        int.TryParse(xLWorkbook.Worksheets.Worksheet(1).Cell(row, 17).GetString(), out IsHR);
                        string sdm = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 18).GetString();
                        string asm = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 19).GetString();
                        string Location = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 20).GetString();
                        string DOB = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 21).GetString();
                        int NoticePeriod = 0;
                        int.TryParse(xLWorkbook.Worksheets.Worksheet(1).Cell(row, 22).GetString(), out NoticePeriod);

                        User addusdr = new User();
                        if (IsHR > 0)
                        {
                            UserType = "EmployeeHR";
                        }
                        if (EmpCode != "" && EmpEmailId != "")
                        {
                            res = addusdr.AddEmployee(EmpName, EmpCode, EmpEmailId, EmpDesignation, EmpDepartment,
                            Status, Session["EmailId"].ToString(), NoticePeriod, HOD, RM,DOJ,
                           /* LastPromoYear, DesgChangeDate, ExistingCTC,*/ UserType, IsHOD, IsRM, IsHR, ERPId, /*sdm, asm,*/ Location,DOB);
                        }

                    }
                    catch
                    {
                        once = 1;
                    }

                    row++;
                }
                if (once > 0)
                {
                    string message = "File Uploaded, without some entries(Emaild & Employee Code Should not be Blank)!";
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<script>alert('");
                    sb.Append(message);
                    sb.Append("'); </script>");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
                }
                else if (res > 0)
                {
                    string message = "File Uploaded Successfully";
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<script>alert('");
                    sb.Append(message);
                    sb.Append("'); </script>");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
                }
                else
                {
                    string message = "File Not Uploaded,(Emaild & Employee Code Should not be Blank)!";
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<script>alert('");
                    sb.Append(message);
                    sb.Append("'); </script>");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
                }
            }
            else
            {
                string message = "Select File!";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<script>alert('");
                sb.Append(message);
                sb.Append("'); </script>");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
            }
        }
        catch
        {
            string message = "Error, File is Missing!";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script>alert('");
            sb.Append(message);
            sb.Append("'); </script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
        }

    }

    //protected void btnUpload_Click(object sender, EventArgs e)
    //{
    //    using (OleDbConnection con = new OleDbConnection(ConfigurationManager.ConnectionStrings["ExcelCon"].ConnectionString))

    //    {

    //        con.Open(); //not rigistered error

    //        OleDbCommand com = new OleDbCommand("Select * from EmployeeMaster", con);

    //        OleDbDataReader dr = com.ExecuteReader();

    //        using (SqlConnection sqlcon = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))

    //        {

    //            sqlcon.Open();

    //            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlcon))

    //            {

    //                bulkCopy.ColumnMappings.Add("EmpCode", "EmpCode");
    //                bulkCopy.ColumnMappings.Add("ERPId", "ERPId");
    //                bulkCopy.ColumnMappings.Add("EmpName", "EmpName");
    //                bulkCopy.ColumnMappings.Add("EmpEmailId", "EmpEmailId");
    //                bulkCopy.ColumnMappings.Add("EmpDesignation", "EmpDesignation");
    //                bulkCopy.ColumnMappings.Add("EmpDepartment", "EmpDepartment");
    //                bulkCopy.ColumnMappings.Add("Status", "Status");
    //                bulkCopy.ColumnMappings.Add("HOD", "HOD");
    //                bulkCopy.ColumnMappings.Add("RM", "RM");
    //                bulkCopy.ColumnMappings.Add("DOJ", "DOJ");
    //                bulkCopy.ColumnMappings.Add("LastPromoYear", "LastPromoYear");
    //                bulkCopy.ColumnMappings.Add("DesgChangeDate", "DesgChangeDate");
    //                bulkCopy.ColumnMappings.Add("ExistingCTC", "ExistingCTC");
    //                bulkCopy.ColumnMappings.Add("UserType", "UserType");
    //                bulkCopy.ColumnMappings.Add("IsHOD", "IsHOD");
    //                bulkCopy.ColumnMappings.Add("IsRM", "IsRM");
    //                bulkCopy.ColumnMappings.Add("IsHR", "IsHR");
    //                bulkCopy.ColumnMappings.Add("SDM", "SDM");
    //                bulkCopy.ColumnMappings.Add("ASM", "ASM");
    //                bulkCopy.ColumnMappings.Add("Location", "Location");
    //                bulkCopy.ColumnMappings.Add("DOB", "DOB");
    //                bulkCopy.DestinationTableName = "EmployeeMaster";
    //                bulkCopy.WriteToServer(dr);

    //            }

    //        }

    //        dr.Close();

    //        dr.Dispose();

    //    }

    //    Response.Write("Upload Successfull!");
    //}

    protected void btnSample_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable("Grid");
        dt.Columns.AddRange(new DataColumn[22] {
            new DataColumn("EmpCode"),
            new DataColumn("ERPId"),
            new DataColumn("EmpName"),
            new DataColumn("EmpEmailId"),
            new DataColumn("EmpDesignation"),     
            new DataColumn("EmpDepartment"),
            new DataColumn("Status"),
            new DataColumn("HOD"),
            new DataColumn("RM"),
            new DataColumn("DOJ"),
            new DataColumn("LastPromoYear"),
            new DataColumn("DesgChangeDate"),
            new DataColumn("ExistingCTC"),
            new DataColumn("UserType"),
            new DataColumn("IsHOD"),
            new DataColumn("IsRM"),
            new DataColumn("IsHR"),
            new DataColumn("SDM"),
            new DataColumn("ASM"),
            new DataColumn("Location"),
            new DataColumn("DOB"),
            new DataColumn("NoticePeriod")
            });

        dt.Rows.Add("mv001", "HR-EMP-001", "Manish Verma", "mv@vttpl.com", "Developer", "IT", "Active", "Manish Verma",
            "Manish Verma", "01-01-2022", "01-02-2022", "01-03-2022", "500000", "Employee", "0", "1", "0", "Deepak Rodiya", "Sachin Ugale", "Jalna", "01-01-1992", "1");

        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dt);
            using (MemoryStream stream = new MemoryStream())
            {
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=EmployeeSample.xlsx");
                wb.SaveAs(stream);
                stream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }
    protected void Search(object sender, EventArgs e)
    {
        if(dlRm.SelectedValue != "")
        {
            string dlrm = dlRm.SelectedItem.Value;
            bindgetEmployeeList(dlrm);
        }
       else
        {
            bindgetEmployeeList();
        }
        
    }

    protected void EmployeeList_RowDataBound(object sender, GridViewRowEventArgs e)
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
        EmployeeList.PageIndex = e.NewPageIndex;
        if(dlRm.SelectedItem.Value != "")
        {
            string rm = dlRm.SelectedItem.Value;
            bindgetEmployeeList(rm);
        }
        else
        {
            bindgetEmployeeList();
        }
        
    }

    //take this reference to export attendance data in attendance module
    protected void btnexport_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"select distinct EmailId from UserMaster where EmailId is not null and EmailId != ''");
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        string email = row["EmailId"].ToString();
        //        if(email != "admin@admin.com" || email == "tupeaakash2@gmail.com" || email == "sachin.ugale@vikramtea.com" || email == "itsupport@vikramtea.com")
        //        {
        //            pm.sendEmail(email);
        //        }

        //    }
        //    ClientScriptManager CSM = Page.ClientScript;
        //    string strconfirm = "<script>if(window.confirm('Mail Sent to Employees!')){window.location.href='EmployeeList.aspx'}</script>";
        //    CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
        //}
        //catch (Exception ex)
        //{
        //    ClientScriptManager CSM = Page.ClientScript;
        //    string strconfirm = "<script>if(window.confirm('Some Error, Occured!')){window.location.href='EmployeeList.aspx'}</script>";
        //    CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
        //}

        DataTable dt = new DataTable("Grid");
        dt.Columns.AddRange(new DataColumn[16] {
                   new DataColumn("Id"),
                   new DataColumn ("ERPID"),
                   new DataColumn("EmpName"),
                   new DataColumn("EmpEmailId"),
                   new DataColumn("EmpDesignation"),
                   new DataColumn("EmpDepartment"),
                   new DataColumn("EmpZone"),
                   new DataColumn("Status"),
                   new DataColumn("HOD"),
                   new DataColumn("RM"),
                   new DataColumn("SDM"),
                   new DataColumn("ASM"),
                   new DataColumn("DOJ"),
                   new DataColumn("LastPromoYear"),
                   new DataColumn("DesgChangeDate"),
                   new DataColumn("ExistingCTC")
                   });

        dt = objSQLDataFile.ExecuteDataTableSQL(@" select Id, ERPID, EmpName, EmpEmailId, EmpDesignation, EmpDepartment, EmpZone, Status, HOD, RM,SDM,ASM, DOJ, LastPromoYear, DesgChangeDate, ExistingCTC
        from [dbo].[EmployeeMaster]");

        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dt, "Employee");
            //wb.Worksheets.Add(dt);
            using (MemoryStream stream = new MemoryStream())
            {
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=EmployeeDataESS.xlsx");
                wb.SaveAs(stream);
                stream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }

    protected void dlRm_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindgetEmployeeList(dlRm.SelectedItem.Value);
    }
}
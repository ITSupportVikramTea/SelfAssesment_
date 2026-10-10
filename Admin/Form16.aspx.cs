using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Form16 : System.Web.UI.Page
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
            bindgetEmployeeList();
        }
    }
    public void bindgetEmployeeList()
    {
        string str = txtsearchuser.Text.Trim();
        DataTable dt;
        if (str != "")
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  Id, EmpName, EmpDepartment, EmpDesignation 
from EmployeeMaster where Id not in (select EmpId from Form16 where Year = (datepart(year,getdate()))) and EmpName LIKE '%" + str + "%' order by EmpName");
            EmployeeList.DataSource = dt;
            EmployeeList.DataBind();
        }
        else
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  Id, EmpName, EmpDepartment, EmpDesignation 
from EmployeeMaster where Id not in (select EmpId from Form16 where Year = (datepart(year,getdate())))");
            EmployeeList.DataSource = dt;
            EmployeeList.DataBind();
        }

    }
    protected void btnSubmit_click(object sender, EventArgs e)
    {

        User user = new User();
        int res = 0, once = 0;
        foreach (GridViewRow row in EmployeeList.Rows)
        {
            FileUpload partA = (row.Cells[4].FindControl("partA") as FileUpload);
            FileUpload partB = (row.Cells[5].FindControl("partB") as FileUpload);

            try
            {
                if (partA.FileName != "" && partB.FileName != "")
                {

                    string filePath = Server.MapPath("../Form16Files/");
                    //string filePath2 = Server.MapPath("../Form16Files/");

                    string fileName1 = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + partA.PostedFile.FileName.ToString().Replace(" ", "_").Trim();
                    string fileName2 = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + partB.PostedFile.FileName.ToString().Replace(" ", "_").Trim();
                    partA.PostedFile.SaveAs(filePath + fileName1);
                    //partA.PostedFile.SaveAs(filePath1);
                    //filePath2 = filePath2 + fileName2;
                    partB.PostedFile.SaveAs(filePath + fileName2);

                    int empId = Convert.ToInt32(EmployeeList.DataKeys[row.RowIndex].Values[0].ToString());
                    string PartA = filePath + fileName1;
                    string PartB = filePath + fileName2;

                    res = user.AddForm16(empId, fileName1, fileName2, Session["EmailId"].ToString());
                    once = 1;
                }

            }
            catch (Exception ex)
            {
                //once += 1;
                string message = "Some Error Occured!";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<script>alert('");
                sb.Append(message);
                sb.Append("'); </script>");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
            }
        }
        if (once == 1)
        {
            ClientScriptManager CSM = Page.ClientScript;
            string strconfirm = "<script>if(window.confirm('File Uploaded')){window.location.href='Form16.aspx'}</script>";
            CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
        }
        //else if(once == 1)
        //{
        //    ClientScriptManager CSM = Page.ClientScript;
        //    string strconfirm = "<script>if(window.confirm('Files uploaded, without some files!')){window.location.href='Form16.aspx'}</script>";
        //    CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
        //}
        else if (once == 0)
        {
            string message = "Please select file(s)!";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script>alert('");
            sb.Append(message);
            sb.Append("'); </script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
        }

    }
    protected void Search(object sender, EventArgs e)
    {
        this.bindgetEmployeeList();
    }
    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        EmployeeList.PageIndex = e.NewPageIndex;
        this.bindgetEmployeeList();
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

    protected void EmployeeList_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        // Create a new DataTable to hold the exported data
        DataTable dtExport = new DataTable();

        // Add columns to the DataTable
        dtExport.Columns.Add("EmployeeID");
        dtExport.Columns.Add("EmployeeName");
        dtExport.Columns.Add("PartA");
        dtExport.Columns.Add("PartB");

        // Populate the DataTable with data from the GridView
        foreach (GridViewRow row in EmployeeList.Rows)
        {
            string employeeID = EmployeeList.DataKeys[row.RowIndex].Value.ToString();
            string employeeName = row.Cells[1].Text;

            FileUpload partA = (FileUpload)row.Cells[2].FindControl("partA");
            FileUpload partB = (FileUpload)row.Cells[3].FindControl("partB");

            string partAFileName = string.Empty;
            string partBFileName = string.Empty;

            if (partA.HasFile)
            {
                partA.SaveAs(Server.MapPath("~/Form16Files/") + partA.FileName);
                partAFileName = partA.FileName;
            }

            if (partB.HasFile)
            {
                partB.SaveAs(Server.MapPath("~/Form16Files/") + partB.FileName);
                partBFileName = partB.FileName;
            }

            dtExport.Rows.Add(employeeID, employeeName, partAFileName, partBFileName);
        }

        // Create a new Excel workbook and worksheet
        var workbook = new ClosedXML.Excel.XLWorkbook();
        var worksheet = workbook.Worksheets.Add("ExportedData");

        // Set the column names in the first row of the worksheet
        for (int i = 0; i < dtExport.Columns.Count; i++)
        {
            worksheet.Cell(1, i + 1).Value = dtExport.Columns[i].ColumnName;
        }

        // Set the cell values for each row in the worksheet
        for (int row = 0; row < dtExport.Rows.Count; row++)
        {
            for (int col = 0; col < dtExport.Columns.Count; col++)
            {
                worksheet.Cell(row + 2, col + 1).Value = dtExport.Rows[row][col].ToString();
            }
        }

        // Prepare the response for download
        HttpResponse response = HttpContext.Current.Response;
        response.Clear();
        response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        response.AddHeader("content-disposition", "attachment;filename=\"ExportedData.xlsx\"");

        // Save the workbook to the response stream
        using (MemoryStream memoryStream = new MemoryStream())
        {
            workbook.SaveAs(memoryStream);
            memoryStream.WriteTo(response.OutputStream);
            memoryStream.Close();
        }

        response.End();
    }

}
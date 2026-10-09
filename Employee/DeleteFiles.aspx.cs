using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.IO;
using System.Text;
using Ionic.Zip;
using System.Configuration;
using System.Data.OleDb;
using System.Net.Mail;
using System.Net;

public partial class Employee_Payregister : System.Web.UI.Page
{
   
  //  PassNMail mailSend = new PassNMail();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // if (Session["EmailId"] == null || Session["EmailId"].ToString() != "vishalvk82@gmail.com")
            if (Session["EmailId"] == null || Session["EmailId"].ToString() != "fake5@gmail.com")
            {
                Response.Redirect("../Default.aspx");
            }
        }
    }
    protected void fultr_click(object sender, EventArgs e)
    {
        try
        {
            if (fileuploadSO.HasFile)
            {
                string FolderPath = Server.MapPath("~/CustomerDocsToDelete");
                string FileName = Path.GetFileName(fileuploadSO.PostedFile.FileName);
                string Extension = Path.GetExtension(fileuploadSO.PostedFile.FileName);
                string FilePath = Path.Combine(FolderPath, FileName);

                
                fileuploadSO.SaveAs(FilePath);

                DataTable dtimport = Import_To_Grid(FilePath, Extension, "YES"); 
                foreach (DataRow row in dtimport.Rows)
                {
                    string originalFileName = row[0].ToString();
                    string cleanedFileName = originalFileName.Replace("~/Uploads/CustomerDocs/", "");
                }
                // Get a list of all files in the CustomerDocsToDelete folder
                string[] filesInFolder = Directory.GetFiles(FolderPath);

                foreach (string filePath in filesInFolder)
                {
                    string fileName = Path.GetFileName(filePath);
                    
                    // Check if the file name exists in the imported DataTable
                    bool fileExistsInExcel = dtimport.AsEnumerable()
                        .Any(row => string.Equals(row[0].ToString(), fileName));

                    if (fileExistsInExcel)
                    {
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                            Response.Write("Deleted file: " + fileName + "<br/>");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("An error occurred: " + ex.Message + "<br/>");
        }
    }
    private DataTable Import_To_Grid(string FilePath, string Extension, string isHDR)
    {
        DataTable dt = new DataTable();
        try
        {
            string conStr = "";
            switch (Extension)
            {
                case ".xls": // Excel 97-03
                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;

                case ".xlsx": // Excel 07
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    break;
            }
            conStr = string.Format(conStr, FilePath, isHDR);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            cmdExcel.Connection = connExcel;

            // Get the name of First Sheet
            connExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            connExcel.Close();

            // Read Data from First Sheet
            connExcel.Open();
            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);

            connExcel.Close();

        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dt;
    }

    protected void btnEmpList_click(object sender, EventArgs e)
    {
        Response.Redirect("PayRegisterEmployeeList.aspx");
    }
   
}

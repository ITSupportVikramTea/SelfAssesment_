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
    //aakash code
    PassNMail mailSend = new PassNMail();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // if (Session["EmailId"] == null || Session["EmailId"].ToString() != "vishalvk82@gmail.com")
            if (Session["EmailId"] == null || Session["EmailId"].ToString() != "janakjalna@gmail.com")
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
                string FileName = Path.GetFileName(fileuploadSO.PostedFile.FileName);

                string Extension = Path.GetExtension(fileuploadSO.PostedFile.FileName);

                //below two line is original code
                //string FolderPath = ConfigurationManager.AppSettings["FolderPath"];
                // string FilePath = Server.MapPath(FolderPath + FileName);

                //added below two new lines to save excel in paypregisterExcefiles folder
                string FolderPath = "/PaypregisterExceFiles/";
                string FilePath = Path.Combine(Server.MapPath(FolderPath), FileName);

                fileuploadSO.SaveAs(FilePath);

                DataTable dtimport = Import_To_Grid(FilePath, Extension, "YES");
                GeneratedocToPdf(dtimport);

                #region Create Zip
                try
                {
                    int Month = Convert.ToInt32(DateTime.Now.ToString("MM"));
                    int Year = Convert.ToInt32(DateTime.Now.ToString("yyy"));

                    string directoryPath = Request.PhysicalApplicationPath + "\\files\\" + Month + "_" + Year + "\\";//Location for inside Test Folder  
                    if (File.Exists(directoryPath + "\\" + Month + "_" + Year + ".zip"))
                        File.Delete(directoryPath + "\\" + Month + "_" + Year + ".zip");

                    string[] Filenames = Directory.GetFiles(directoryPath);
                    using (ZipFile zip = new ZipFile())
                    {
                        zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                        zip.AddFiles(Filenames, Month + "_" + Year);//Zip file inside filename  
                        zip.Save(directoryPath + "\\" + Month + "_" + Year + ".zip");//location and name for creating zip file  
                    }
                    //hidpathfordownload.Value = +Month + "_" + Year + "/" + Month + "_" + Year + ".zip";
                    //rbtndownloadzip.Visible = true;

                }
                catch (Exception ex)
                {
                    //CustomMSG1.ShowMessage("Unable to create zip file" + ex.ToString(), Util.CustMsg.Information);
                }
                #endregion
            }
        }
        catch (Exception ex)
        {
            
            throw ex;
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

                case ".xls": //Excel 97-03
                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;

                case ".xlsx": //Excel 07
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    break;
            }
            conStr = String.Format(conStr, FilePath, isHDR);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            cmdExcel.Connection = connExcel;

            //Get the name of First Sheet
            connExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            connExcel.Close();

            //Read Data from First Sheet
            connExcel.Open();
            cmdExcel.CommandText = "SELECT * From [Sheet1$]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);

            connExcel.Close();

        }
        catch (Exception ex)
        {
            throw ex;
            //   ShowMessage(ex.Message.ToString(), Util.CustMsg.Error.ToString());
        }
        return dt;
    }

    private void GeneratedocToPdf(DataTable GetAllEmpdatils)
    {
        StringBuilder strbuild = new StringBuilder();
        try
        {
            #region Variables

            int Month = Convert.ToInt32(DateTime.Now.ToString("MM"));
            int Year = Convert.ToInt32(DateTime.Now.ToString("yyy"));

            #endregion

            //finding exact rows count in sheet which has Emp_Name 
            string condition = "NOT (Emp_Name IS NULL OR Emp_Name = '')";
            DataRow[] filteredRows = GetAllEmpdatils.Select(condition);
            int rowCount = filteredRows.Length;

            #region Generate PDF
            if (rowCount > 0)
            {
                for (int iemp = 0; iemp < rowCount; iemp++)
                {
                    string employee_name = GetAllEmpdatils.Rows[iemp]["Emp_Name"].ToString().Trim();
                    string emp_email_id = GetAllEmpdatils.Rows[iemp]["EmailID"].ToString().Trim();
                    string department = GetAllEmpdatils.Rows[iemp]["Department"].ToString();
                    string hod_name = GetAllEmpdatils.Rows[iemp]["HOD Name"].ToString();
                    string hod_email = GetAllEmpdatils.Rows[iemp]["Email HOD"].ToString();
                    string emp_abbrivation = GetAllEmpdatils.Rows[iemp]["Abbrivaton"].ToString();
                    string erpid = GetAllEmpdatils.Rows[iemp]["ERPID"].ToString();

                    #region File and folder create or delete sections
                    string directoryPath = Request.PhysicalApplicationPath + "\\files\\" + Month + "_" + Year + "\\";
                    string pdfFileName = directoryPath + GetAllEmpdatils.Rows[iemp]["Emp_Name"].ToString() + ".pdf";

                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    if (File.Exists(pdfFileName))
                        File.Delete(pdfFileName);
                    #endregion

                    #region Writer and doc open section
                    Panel pnlPrintControl = new Panel();

                    Document doc = new Document(PageSize.A4, 36f, 36f, 36f, 36f);//36f, 36f, 90f, 100f);
                    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(pdfFileName, FileMode.CreateNew));
                    writer.PageEvent = new Footer();
                    doc.Open();



                    #endregion Writer and doc open section

                    #region Fix Line
                    ////Font BookmanOldStyle = FontFactory.GetFont("Bookman Old Style", 9f, Font.NORMAL, BaseColor.BLACK);
                    //Font BookmanOldStyle = FontFactory.GetFont("Georgia", 9f, Font.NORMAL, BaseColor.BLACK);
                    //Font BookmanOldStyleSmall = FontFactory.GetFont("Bookman Old Style", 6f, Font.NORMAL, BaseColor.BLACK);
                    //Font BookmanOldStyleTables = FontFactory.GetFont("Bookman Old Style", 9f, Font.NORMAL, BaseColor.BLACK);
                    //Font BookmanOldStyle3 = FontFactory.GetFont("Bookman Old Style", 7f, Font.NORMAL, BaseColor.BLACK);
                    //Font BookmanOldStyle3b = FontFactory.GetFont("Bookman Old Style", 6f, Font.BOLD, BaseColor.BLACK);
                    //Font BookmanOldStyle1 = FontFactory.GetFont("Bookman Old Style", 6f, Font.NORMAL, BaseColor.BLACK);
                    //Font BookmanOldStylebold = FontFactory.GetFont("Bookman Old Style", 8.5f, Font.BOLD, BaseColor.BLACK);
                    //Font BookmanOldStyleboldtotal = FontFactory.GetFont("Bookman Old Style", 8.5f, Font.BOLD, BaseColor.BLACK);
                    //Font BookmanOldStyleboldItalic = FontFactory.GetFont("Bookman Old Style", 7f, Font.BOLDITALIC, BaseColor.BLACK);

                    Font BookmanOldStyle = FontFactory.GetFont("Calibari", 9f, Font.NORMAL, BaseColor.BLACK);
                    Font BookmanOldStyleSmall = FontFactory.GetFont("Calibari", 8f, Font.NORMAL, BaseColor.BLACK);
                    Font BookmanOldStyleTables = FontFactory.GetFont("Calibari", 9f, Font.NORMAL, BaseColor.BLACK);
                    Font BookmanOldStyle3 = FontFactory.GetFont("Calibari", 9f, Font.NORMAL, BaseColor.BLACK);
                    Font BookmanOldStyle3b = FontFactory.GetFont("Calibari", 9f, Font.NORMAL, BaseColor.BLACK);
                    Font BookmanOldStyle1 = FontFactory.GetFont("Calibari", 9f, Font.NORMAL, BaseColor.BLACK);
                    Font BookmanOldStylebold = FontFactory.GetFont("Calibari", 9f, Font.BOLD, BaseColor.BLACK);
                    Font BookmanOldStyleboldtotal = FontFactory.GetFont("Calibari", 9f, Font.BOLD, BaseColor.BLACK);
                    Font BookmanOldStyleboldItalic = FontFactory.GetFont("Calibari", 9f, Font.BOLDITALIC, BaseColor.BLACK);
                   
                    string fontPath = Server.MapPath("~/Content/images/Dubai-Light.ttf");

                    BaseFont bfDubaiLight = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    Font fontDubaiLight = new Font(bfDubaiLight, 14, Font.BOLD);

                    Paragraph DataSalary = new Paragraph();
                   

                    if (GetAllEmpdatils.Rows[iemp]["New Designation"].ToString().Trim() != "")
                    {
                        PdfPTable headerTable = new PdfPTable(2);
                        headerTable.WidthPercentage = 100;
                        headerTable.SetWidths(new float[] { 50f, 50f });

                     
                        Paragraph leftPara = new Paragraph(20f);
                        leftPara.SetLeading(0, 3.0f); // line spacing

                        leftPara.Add(new Chunk("To, ", BookmanOldStylebold));
                        
                        leftPara.Add(Chunk.NEWLINE);
                        leftPara.SpacingAfter = 20f;
                      
                        leftPara.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Abbrivaton"].ToString() + " ", BookmanOldStyle));
                        leftPara.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Emp_Name"].ToString(), BookmanOldStylebold));
                        leftPara.Add(Chunk.NEWLINE);

                        leftPara.Add(new Chunk("Department: ", BookmanOldStylebold));
                        leftPara.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Department"].ToString(), BookmanOldStyle));

                        PdfPCell leftCell = new PdfPCell(leftPara);
                        leftCell.Border = Rectangle.NO_BORDER;
                        leftCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        leftCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                        leftCell.PaddingLeft = 10f;

                        // ================= RIGHT SIDE (LEFT-ALIGNED) =================
                        Paragraph rightPara = new Paragraph(20f);
                        rightPara.SetLeading(0, 2.0f); // line spacing
                        rightPara.Alignment = Element.ALIGN_LEFT; // <-- Make it left-aligned

                        rightPara.Add(new Chunk("Date: ", BookmanOldStylebold));
                        rightPara.Add(new Chunk("9", BookmanOldStyle));
                        rightPara.Add(new Chunk("th", BookmanOldStyle).SetTextRise(5));
                        rightPara.Add(new Chunk(" July 2026", BookmanOldStyle));
                        rightPara.Add(new Chunk(" ", BookmanOldStyle));
                        rightPara.Add(new Chunk(" ", BookmanOldStyle));
                        rightPara.Add(Chunk.NEWLINE);

                        rightPara.Add(new Chunk("Emp Code: ", BookmanOldStylebold));
                        rightPara.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["ERPID"].ToString(), BookmanOldStyle));
                        rightPara.Add(Chunk.NEWLINE);

                        rightPara.Add(new Chunk("Designation: ", BookmanOldStylebold));
                        rightPara.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Designation"].ToString(), BookmanOldStyle));
                        rightPara.IndentationLeft = 50f;

                        PdfPCell rightCell = new PdfPCell(rightPara);
                        rightCell.Border = Rectangle.NO_BORDER;
                        rightCell.HorizontalAlignment = Element.ALIGN_LEFT; // <-- This is the fix
                        rightCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                        rightCell.PaddingLeft = 115f; 
                        // Optional: keeps it consistent with left side

                        // ================= ADD CELLS =================
                        headerTable.AddCell(leftCell);
                        headerTable.AddCell(rightCell);

                        headerTable.SpacingBefore = 5f;
                        doc.Add(new Paragraph("\n\n"));
                        doc.Add(headerTable);


                    }
                    else
                    {  //rightPara.IndentationLeft = 50f; not working
                        PdfPTable headerTable = new PdfPTable(2);
                        headerTable.WidthPercentage = 100;
                        headerTable.SetWidths(new float[] { 50f, 50f });

                        // ================= LEFT SIDE =================
                        Paragraph leftPara = new Paragraph(20f);
                        leftPara.SetLeading(0, 2.0f); // line spacing

                        leftPara.Add(new Chunk("To, ", BookmanOldStyle));
                        leftPara.Add(new Chunk(" ", BookmanOldStylebold));
                        leftPara.Add(new Chunk(" ", BookmanOldStyle));
                        leftPara.Add(Chunk.NEWLINE);

                        leftPara.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Abbrivaton"].ToString() + " ", BookmanOldStyle));
                        leftPara.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Emp_Name"].ToString(), BookmanOldStyle));
                        leftPara.Add(Chunk.NEWLINE);

                        leftPara.Add(new Chunk("Department: ", BookmanOldStyle));
                        leftPara.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Department"].ToString(), BookmanOldStyle));

                        PdfPCell leftCell = new PdfPCell(leftPara);
                        leftCell.Border = Rectangle.NO_BORDER;
                        leftCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        leftCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                        leftCell.PaddingLeft = 10f;

                        // ================= RIGHT SIDE (LEFT-ALIGNED) =================
                        Paragraph rightPara = new Paragraph(20f);
                        rightPara.SetLeading(0, 2.0f); // line spacing
                        rightPara.Alignment = Element.ALIGN_LEFT; // <-- Make it left-aligned

                        rightPara.Add(new Chunk("Date: ", BookmanOldStyle));
                        rightPara.Add(new Chunk("9", BookmanOldStyle));
                        rightPara.Add(new Chunk("th", BookmanOldStyle).SetTextRise(5));
                        rightPara.Add(new Chunk(" July 2026", BookmanOldStyle));
                        rightPara.Add(new Chunk(" ", BookmanOldStyle));
                        rightPara.Add(new Chunk(" ", BookmanOldStyle));
                        rightPara.SpacingBefore = 10f;
                        rightPara.Add(Chunk.NEWLINE);

                        rightPara.Add(new Chunk("Emp Code: ", BookmanOldStyle));
                        rightPara.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["ERPID"].ToString(), BookmanOldStyle));
                        rightPara.Add(Chunk.NEWLINE);

                        rightPara.Add(new Chunk("Designation: ", BookmanOldStyle));
                        rightPara.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Designation"].ToString(), BookmanOldStyle));
                        rightPara.IndentationLeft = 50f;

                        PdfPCell rightCell = new PdfPCell(rightPara);
                        rightCell.Border = Rectangle.NO_BORDER;
                        rightCell.HorizontalAlignment = Element.ALIGN_LEFT; // <-- This is the fix
                        rightCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                        rightCell.PaddingLeft = 115f; // Optional: keeps it consistent with left side


                        headerTable.AddCell(leftCell);
                        headerTable.AddCell(rightCell);

                        headerTable.SpacingBefore = 5f;
                        doc.Add(new Paragraph("\n\n"));
                        doc.Add(headerTable);


                    }


                    if (GetAllEmpdatils.Rows[iemp]["New Designation"].ToString().Trim() != "")
                    {
                        //doc.Add(Chunk.NEWLINE);
                        DataSalary = new Paragraph();
                        DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                        DataSalary.IndentationLeft = 130f; // Set left margin (indentation)
                        DataSalary.IndentationRight = 0f; // Set right margin (indentation)

                        DataSalary.Add(new Chunk(@"Appreciation for your Performance with Promotion ", fontDubaiLight));
                        DataSalary.SpacingBefore = 06;
                        doc.Add(DataSalary);
                    }
                    else
                    {


                        //doc.Add(Chunk.NEWLINE);
                        DataSalary = new Paragraph();
                        DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                        DataSalary.IndentationLeft = 140f; // Set left margin (indentation)
                        DataSalary.IndentationRight = 0f; // Set right margin (indentation)

                        DataSalary.Add(new Chunk(@"Appreciation for your Performance ", fontDubaiLight));
                        DataSalary.SpacingBefore = 06;
                        doc.Add(DataSalary);
                    }



                    #endregion

                    if (!GetAllEmpdatils.Rows[iemp]["Rating"].ToString().Trim().Contains("Satisfactory"))
                    {
                        #region Case1 Section

                        DataSalary = new Paragraph();
                        //DataSalary = new Paragraph(); DataSalary.Add(new Chunk(" ", BookmanOldStyle)); doc.Add(DataSalary);

                        DataSalary.SpacingBefore = 6f;
                        DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                        DataSalary.Add(new Phrase("Dear " + GetAllEmpdatils.Rows[iemp]["Abbrivaton"].ToString(), BookmanOldStylebold));
                        DataSalary.Add(new Phrase(" " + GetAllEmpdatils.Rows[iemp]["Emp_Name"].ToString() + ",", BookmanOldStylebold));
                        //DataSalary.SpacingAfter = 10;
                        doc.Add(DataSalary);

                        // DataSalary = new Paragraph(); DataSalary.Add(new Chunk(@" ", BookmanOldStyle)); doc.Add(DataSalary); 
                        //doc.Add(new Paragraph("  ")); Edited by V

                        DataSalary = new Paragraph(13f, "");
                        DataSalary.SpacingBefore = 1f;
                        DataSalary.Alignment = Element.ALIGN_JUSTIFIED;

                        DataSalary.Add(new Chunk("Congratulations!", BookmanOldStylebold));
                        //DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Rating"].ToString(), BookmanOldStylebold));
                        //DataSalary.Add(new Chunk(" performance rating!", BookmanOldStyle));
                        DataSalary.SpacingAfter = 1;
                        doc.Add(DataSalary);

                        if (GetAllEmpdatils.Rows[iemp]["New Designation"].ToString().Trim() == "")
                        { 
                        DataSalary = new Paragraph(13f, "");
                        DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                        // DataSalary.Add(new Chunk(@"We appreciate your efforts and would like to thank you for your contribution for the year 2024-25 towards the progress of the company.", BookmanOldStyle));
                        DataSalary.Add(new Chunk(@"We sincerely appreciate your dedication and valuable contribution to Vikram Tea during the FY 2025–26.", BookmanOldStyle));
                        DataSalary.SpacingAfter = 1;
                        doc.Add(DataSalary);

                        DataSalary = new Paragraph(13f, "");
                        DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                        // DataSalary.Add(new Chunk(@"We appreciate your efforts and would like to thank you for your contribution for the year 2024-25 towards the progress of the company.", BookmanOldStyle));
                        DataSalary.Add(new Chunk(@"Based on your KPIs and the annual performance review, you have achieved an overall performance rating of ", BookmanOldStyle));
                        DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Rating"].ToString(), BookmanOldStylebold));
                        //DataSalary.Add(new Chunk(@". In recognition of your performance, commitment, and continued dedication to the organization,", BookmanOldStyle));
                            DataSalary.Add(new Chunk(" And Your monthly CTC has been revised to Rs. ", BookmanOldStyle));
                            DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Total A+C+D Revised"].ToString()+"/- .", BookmanOldStylebold));
                           // DataSalary.Add(new Chunk(" w.e.f. April 2026 ", BookmanOldStyle));
                           
                           
                            DataSalary.SpacingAfter = 1;
                        doc.Add(DataSalary);
                    }
                        //making off to test new draft....
                       /*  if (GetAllEmpdatils.Rows[iemp]["New Designation"].ToString().Trim() != "")
                         {
                             DataSalary = new Paragraph(13f, "");
                             DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                             //DataSalary.Add(new Chunk(@"You have continuously performed ", BookmanOldStyle));
                             DataSalary.Add(new Chunk(@"Your KPIs and appraisal meeting with your reporting manager have helped you consistently perform ", BookmanOldStyle));
                             DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Phrase"].ToString(), BookmanOldStylebold));
                             //DataSalary.Add(new Chunk(@" during this challenging year, based on your KPIs and the appraisal meeting you had with your reporting manager, you have been rated as “", BookmanOldStyle));
                             DataSalary.Add(new Chunk(@" during this challenging year.You have been rated as “", BookmanOldStyle));
                             //DataSalary.Add(Chunk.NEWLINE);
                             //DataSalary.Add(new Chunk(@"You have been rated as “ ", BookmanOldStyle));
                             DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Rating"].ToString(), BookmanOldStylebold));
                             DataSalary.Add(new Chunk("” for your performance. ", BookmanOldStyle));

                             //DataSalary = new Paragraph(13f, "");
                             DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                             DataSalary.Add(new Chunk("Monthly increment", BookmanOldStylebold));
                             //DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Standard Inc"].ToString() + "/- ", BookmanOldStylebold));
                             // DataSalary.Add(new Chunk(", your CTC has been revised to Rs. ", BookmanOldStyle));
                             DataSalary.Add(new Chunk(", has been revised & it will be. ", BookmanOldStyle));
                             //DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["CTC AFTER INC"].ToString(), BookmanOldStylebold));
                             DataSalary.Add(new Chunk("effective from DEC 2025 ", BookmanOldStyle));
                             //DataSalary.Add(new Chunk("st", BookmanOldStyle).SetTextRise(2));
                             // DataSalary.Add(new Chunk("  April 2025 as a part of performance appraisal and commitment towards the company. The detailed pay structure is annexed along with this letter.", BookmanOldStyle));
                             DataSalary.Add(new Chunk(" as part of the evaluation process and dedication to the organization.", BookmanOldStyle));
                             DataSalary.SpacingAfter = 1;
                             doc.Add(DataSalary);

                         }*/

                         /*else { 
                             DataSalary = new Paragraph(13f, "");
                         DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                         //DataSalary.Add(new Chunk(@"You have continuously performed ", BookmanOldStyle));
                         DataSalary.Add(new Chunk(@"Your KPIs and appraisal meeting with your reporting manager have helped you consistently perform ", BookmanOldStyle));
                         DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Phrase"].ToString(), BookmanOldStylebold));
                         //DataSalary.Add(new Chunk(@" during this challenging year, based on your KPIs and the appraisal meeting you had with your reporting manager, you have been rated as “", BookmanOldStyle));
                         DataSalary.Add(new Chunk(@" during this challenging year.You have been rated as “", BookmanOldStyle));
                         //DataSalary.Add(Chunk.NEWLINE);
                         //DataSalary.Add(new Chunk(@"You have been rated as “ ", BookmanOldStyle));
                         DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Rating"].ToString(), BookmanOldStylebold));
                         DataSalary.Add(new Chunk("” for your performance. ", BookmanOldStyle));
                         // DataSalary.SpacingAfter = 10;
                         //  DataSalary.SpacingAfter = 2f;
                         //doc.Add(DataSalary);

                         //DataSalary = new Paragraph(); DataSalary.Add(new Chunk(@"Keep up the momentum for the running year!", BookmanOldStyleboldItalic)); DataSalary.SpacingAfter = 2f; doc.Add(DataSalary);

                         //DataSalary = new Paragraph(13f, "");
                         DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                         //DataSalary.Add(new Chunk("Monthly increment of Rs." + GetAllEmpdatils.Rows[iemp]["Standard Inc"].ToString() + "/- ", BookmanOldStylebold));
                         DataSalary.Add(new Chunk("Monthly increment ", BookmanOldStylebold));
                         DataSalary.Add(new Chunk("has been revised & statutory impact & payment will be ", BookmanOldStyle));
                         //DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["CTC AFTER INC"].ToString(), BookmanOldStylebold));
                         DataSalary.Add(new Chunk("effective from ", BookmanOldStyle));
                         //DataSalary.Add(new Chunk("1", BookmanOldStyle));
                         //DataSalary.Add(new Chunk("st", BookmanOldStyle).SetTextRise(3));
                         DataSalary.Add(new Chunk("01-07-2026", BookmanOldStyle));
                         //DataSalary.Add(new Chunk("st", BookmanOldStyle).SetTextRise(2));
                         //DataSalary.Add(new Chunk("April 2025 as a part of performance appraisal and commitment towards the company. The detailed revised pay structure is annexed along with this letter.", BookmanOldStyle));
                         DataSalary.Add(new Chunk("  as part of the evaluation process and dedication to the organization.", BookmanOldStyle));
                         // DataSalary.SpacingAfter = 2;
                         doc.Add(DataSalary);
                         }*/
                        // made above off to test new draft....
                        //temporvery
                        //if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Var Pay In"].ToString().Trim()) > 0 ||
                        //   Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Performance Bonus In"].ToString().Trim()) > 0 ||
                        //   Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Retention Bonus In"].ToString().Trim()) > 0)
                        //{
                        //    //DataSalary = new Paragraph(13f, "");
                        //    DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                        //    DataSalary.Add(new Chunk("Monthly increment", BookmanOldStyle));
                        //    //DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Standard Inc"].ToString() + "/- ", BookmanOldStylebold));
                        //    // DataSalary.Add(new Chunk(", your CTC has been revised to Rs. ", BookmanOldStyle));
                        //    DataSalary.Add(new Chunk(", has been revised & statutory impact & payment will be . ", BookmanOldStyle));
                        //    //DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["CTC AFTER INC"].ToString(), BookmanOldStylebold));
                        //    DataSalary.Add(new Chunk("effective from 1st July 2025 ", BookmanOldStyle));
                        //    //DataSalary.Add(new Chunk("st", BookmanOldStyle).SetTextRise(2));
                        //   // DataSalary.Add(new Chunk("  April 2025 as a part of performance appraisal and commitment towards the company. The detailed pay structure is annexed along with this letter.", BookmanOldStyle));
                        //    DataSalary.Add(new Chunk(" as part of the evaluation process and dedication to the organization.", BookmanOldStyle));
                        //    DataSalary.SpacingAfter = 1;

                        //    DataSalary = new Paragraph(13f, "");
                        //    DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                        //    DataSalary.Add(new Chunk("The Variable Pay Rs.", BookmanOldStyle));
                        //    DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Var Pay In"].ToString(), BookmanOldStylebold));
                        //    DataSalary.Add(new Chunk("/-,performance bonus of Rs.", BookmanOldStyle));
                        //    DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Performance Bonus In"].ToString(), BookmanOldStylebold));
                        //    DataSalary.Add(new Chunk("/- & retention bonus of Rs.", BookmanOldStyle));
                        //    DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Retention Bonus In"].ToString(), BookmanOldStylebold));
                        //    DataSalary.Add(new Chunk("/-PM,Total Rs.", BookmanOldStyle));
                        //    DataSalary.Add(new Chunk((Convert.ToInt32(GetAllEmpdatils.Rows[iemp]["Var Pay In"].ToString())
                        //        + Convert.ToInt32(GetAllEmpdatils.Rows[iemp]["Performance Bonus In"].ToString()) 
                        //        + Convert.ToInt32(GetAllEmpdatils.Rows[iemp]["Retention Bonus In"].ToString())) 
                        //        + "/- ,(Performance Linked)", BookmanOldStyle));
                        //    DataSalary.SpacingAfter = 1;
                        //    doc.Add(DataSalary);
                        //}
                        //else
                        //{
                        //    //DataSalary = new Paragraph(13f, "");
                        //    DataSalary.Alignment = Element.ALIGN_JUSTIFIED;er
                        //    //DataSalary.Add(new Chunk("Monthly increment of Rs." + GetAllEmpdatils.Rows[iemp]["Standard Inc"].ToString() + "/- ", BookmanOldStylebold));
                        //    DataSalary.Add(new Chunk("Monthly increment " , BookmanOldStylebold));
                        //    DataSalary.Add(new Chunk("has been revised & statutory impact & payment will be ", BookmanOldStyle));
                        //    //DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["CTC AFTER INC"].ToString(), BookmanOldStylebold));
                        //    DataSalary.Add(new Chunk("effective from ", BookmanOldStyle));
                        //    DataSalary.Add(new Chunk("1", BookmanOldStyle));
                        //    DataSalary.Add(new Chunk("st", BookmanOldStyle).SetTextRise(3));
                        //    DataSalary.Add(new Chunk(" July 2025", BookmanOldStyle));
                        //    //DataSalary.Add(new Chunk("st", BookmanOldStyle).SetTextRise(2));
                        //    //DataSalary.Add(new Chunk("April 2025 as a part of performance appraisal and commitment towards the company. The detailed revised pay structure is annexed along with this letter.", BookmanOldStyle));
                        //    DataSalary.Add(new Chunk("  as part of the evaluation process and dedication to the organization.", BookmanOldStyle));
                        //   // DataSalary.SpacingAfter = 2;
                        //    doc.Add(DataSalary);
                        //}

                        if (GetAllEmpdatils.Rows[iemp]["New Designation"].ToString().Trim() != "")
                        {
                            //making off to test new draft....

                           
                            

                            DataSalary = new Paragraph(13f, "");
                            DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                            DataSalary.Add(new Chunk(@"We sincerely appreciate your dedication and valuable contribution to Vikram Tea during the FY 2025–26.", BookmanOldStyle));
                            DataSalary.SpacingAfter = 1;
                            DataSalary.Add(Chunk.NEWLINE);
                            // DataSalary.Add(new Chunk(@"We appreciate your efforts and would like to thank you for your contribution for the year 2024-25 towards the progress of the company.", BookmanOldStyle));
                            DataSalary.Add(new Chunk(@"Based on your KPIs and the annual performance review, you have achieved an overall performance rating of ", BookmanOldStyle));
                            DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Rating"].ToString(), BookmanOldStylebold));
                            //DataSalary.Add(new Chunk(@". In recognition of your performance, commitment, and continued dedication to the organization,", BookmanOldStyle));
                            DataSalary.Add(new Chunk(" And Your monthly CTC has been revised to Rs. ", BookmanOldStyle));
                            DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Total A+C+D Revised"].ToString() + "/- .", BookmanOldStylebold));
                            // DataSalary.Add(new Chunk(" w.e.f. April 2026 ", BookmanOldStyle));

                            DataSalary.Add(new Chunk(" Based on your above performance over the years and your demonstrated capability to take on higher responsibilitie. Accordingly, you are hereby promoted to the position of  ", BookmanOldStyle));
                            DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["New Designation"].ToString(), BookmanOldStylebold));
                            DataSalary.Add(new Chunk(" effective from July 2026.", BookmanOldStyle));
                            //  DataSalary.Add(new Chunk(".(TA & DA policy is applicable from ", BookmanOldStyleSmall));
                            ////  DataSalary.Add(new Chunk("st", BookmanOldStyleSmall).SetTextRise(2));
                            //  DataSalary.Add(new Chunk(" DEC 2025).", BookmanOldStyleSmall));
                            //DataSalary.SpacingAfter = 2;
                            doc.Add(DataSalary);
                        }

                        //if (GetAllEmpdatils.Rows[iemp]["Designation Upgrade"].ToString().Trim() != "")
                        //{
                        //    DataSalary = new Paragraph(13f, "");
                        //    DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                        //    DataSalary.Add(new Chunk("Considering your job nature we are designating you as ", BookmanOldStyle));
                        //    DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Designation Upgrade"].ToString() + ". ", BookmanOldStylebold));
                        //    DataSalary.SpacingAfter = 2;
                        //    doc.Add(DataSalary);
                        //}


                        if (GetAllEmpdatils.Rows[iemp]["Salary Correction"].ToString().Trim() == "Correction")
                        {
                            DataSalary = new Paragraph(13f, "");
                            DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                            DataSalary.Add(new Chunk(@"While fixing your new CTC, we have considered your performance as well as your commitment and loyalty towards the organization, and hence with the standard increment we are also making salary correction. Your standard increment is ", BookmanOldStyle));
                            DataSalary.Add(new Chunk("" + Math.Round(Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Standard Percentage"].ToString()), 2) + "%", BookmanOldStylebold));
                            DataSalary.Add(new Chunk(" and after salary correction it goes upto ", BookmanOldStyle));
                            DataSalary.Add(new Chunk("" + Math.Round(Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Inc Percentage"].ToString()), 2) + "%", BookmanOldStylebold));
                            DataSalary.Add(new Chunk(" for this year. ", BookmanOldStyle));
                            DataSalary.SpacingAfter = 2;
                            doc.Add(DataSalary);

                        }

                        //if (GetAllEmpdatils.Rows[iemp]["Designation Upgrade"].ToString().ToString().Trim() != "")
                        //{
                        //    DataSalary = new Paragraph(13f);
                        //    DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                        //    //DataSalary.Add(new Chunk(@"Please accept our personal congratulations on this well-deserved recognition of your excellent contribution and commitment to our organization. ", BookmanOldStyle));
                        //    DataSalary.Add(new Chunk(@"We would like to congratulate you personally on this well-deserved recognition of your exceptional contributions and dedication to our organization.", BookmanOldStyle));
                        //    DataSalary.SpacingAfter = 1;
                        //    doc.Add(DataSalary);

                        //}

                        //else { 
                        //    //DataSalary = new Paragraph(13f, "");
                        //    DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                        ////DataSalary.Add(new Chunk(@"Please accept our personal congratulations on this well-deserved recognition of your excellent contribution and commitment to our organization. ", BookmanOldStyle));
                        //DataSalary.Add(new Chunk(@"We would like to congratulate you personally on this well-deserved recognition of your exceptional contributions and dedication to our organization.", BookmanOldStyle));
                        //DataSalary.SpacingAfter = 1;
                        //doc.Add(DataSalary);
                        //}

                        //start


                        PdfPTable table1 = new PdfPTable(4);
                        int[] columnwidth = { 5,35, 20, 20 };
                        table1.SetWidths(columnwidth);
                        table1.WidthPercentage = 75;
                        table1.HorizontalAlignment = Element.ALIGN_CENTER;
                        table1.SpacingBefore = 10f;

                        Font BookmanOldStyleTable = FontFactory.GetFont("Bookman Old Style", 7, 1, new BaseColor(50, 50, 111));

                        PdfPCell cellColumnName = new PdfPCell(new Phrase("CTC Components", BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) };
                        
                        cellColumnName.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                                                //Cellcolumnname.Border = 15;
                        cellColumnName.Rowspan = 2;
                        cellColumnName.Colspan = 2;

                        table1.AddCell(cellColumnName);
                        //doc.Add(table1);
                       cellColumnName = new PdfPCell(new Phrase("CTC as on 31st Mar 2026", BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) };
                       // cellColumnName = new PdfPCell(new Phrase("Current CTC", BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) };
                        cellColumnName.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                                                //Cellcolumnname.Border = 15;        
                        cellColumnName.Rowspan = 2;
                        table1.AddCell(cellColumnName);



                        //doc.Add(table1);

                        Paragraph Datetable = new Paragraph();
                        //Datetable.Add(new Chunk(@"w.e.f. 1", BookmanOldStyleTable));
                        //Datetable.Add(new Chunk(@"st", BookmanOldStyleTable).SetTextRise(2));
                        //Datetable.Add(new Chunk(@" Oct 2024", BookmanOldStyleTable));

                        cellColumnName = new PdfPCell(Datetable) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) };
                        cellColumnName.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cellColumnName.Colspan = 5;
                        table1.AddCell(cellColumnName);

                        /*cellColumnName = new PdfPCell(new Phrase("Increment ", BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) };
                        cellColumnName.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                                                //Cellcolumnname.Border = 15;                    
                        table1.AddCell(cellColumnName);
                        //doc.Add(table1);*/

                        //cellColumnName = new PdfPCell(new Phrase("New Wage Structure        01-04-2026", BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) };
                        //cellColumnName.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        //                                        //Cellcolumnname.Border = 15;                    
                        //table1.AddCell(cellColumnName);

                        ////cellColumnName = new PdfPCell(new Phrase("Increment w.e.f. 01.07.2025 ", BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) };
                        //cellColumnName = new PdfPCell(new Phrase("Increment Amount", BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) };
                        //cellColumnName.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        //                                        //Cellcolumnname.Border = 15;                    
                        //table1.AddCell(cellColumnName);
                        //doc.Add(table1);

                        cellColumnName = new PdfPCell(new Phrase("New CTC Structure           1st July 2026 ", BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) };
                        cellColumnName.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                                                //Cellcolumnname.Border = 15;                    
                        table1.AddCell(cellColumnName);


                        int countsr = 1;

                        if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["BASIC"].ToString()) > 0)
                        {
                            cellColumnName = new PdfPCell(new Phrase("" + countsr, BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            table1.AddCell(cellColumnName);

                            cellColumnName = new PdfPCell(new Phrase("Basic Salary", BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            table1.AddCell(cellColumnName);

                            cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["BASIC"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Basic 01-04-2026"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            //table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Basic Inc"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            //table1.AddCell(cellColumnName);

                            cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Basic Revised"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            table1.AddCell(cellColumnName);

                            countsr++;
                        }



                        if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["HRA"].ToString()) > 0)
                        {
                            //2
                            cellColumnName = new PdfPCell(new Phrase("" + countsr, BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            table1.AddCell(cellColumnName);

                            cellColumnName = new PdfPCell(new Phrase("House Rent Allowance", BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            table1.AddCell(cellColumnName);

                            cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["HRA"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["HRA 01-04-2026"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            //table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["HRA Inc"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            //table1.AddCell(cellColumnName);

                            cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["HRA Revised"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            table1.AddCell(cellColumnName);


                            countsr++;
                        }

                        if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Uni All"].ToString()) > 0)
                        {
                            //3
                            cellColumnName = new PdfPCell(new Phrase("" + countsr, BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            table1.AddCell(cellColumnName);

                            cellColumnName = new PdfPCell(new Phrase("Washing Allowance", BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            table1.AddCell(cellColumnName);

                            // Retrieve, round, and format the values to 2 decimal places
                            string uniAll = Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Uni All"]).ToString();
                            cellColumnName = new PdfPCell(new Phrase(uniAll, BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            table1.AddCell(cellColumnName);

                            //string uniAllN = Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Uni 01-04-2026"]).ToString();
                            //cellColumnName = new PdfPCell(new Phrase(uniAll, BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            //table1.AddCell(cellColumnName);

                            //string uniAlloInc = Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Uni Allo Inc"]).ToString();
                            //cellColumnName = new PdfPCell(new Phrase(uniAlloInc, BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            //table1.AddCell(cellColumnName);

                            string uniAllowRevised = Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Uni Allow Revised"]).ToString();
                            cellColumnName = new PdfPCell(new Phrase(uniAllowRevised, BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            table1.AddCell(cellColumnName);

                            countsr++;
                        }


                        if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Conv"].ToString()) > 0)
                        {
                            //4
                            cellColumnName = new PdfPCell(new Phrase("" + countsr, BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            table1.AddCell(cellColumnName);

                            cellColumnName = new PdfPCell(new Phrase("Conveyance Allowance", BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            table1.AddCell(cellColumnName);

                            cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Conv"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Conv 01-04-2026"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            //table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Conv Inc"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            //table1.AddCell(cellColumnName);

                            cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Conv Revi"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            table1.AddCell(cellColumnName);


                            countsr++;
                        }
                        if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["PA"].ToString()) > 0)
                        {
                            //5
                            cellColumnName = new PdfPCell(new Phrase("" + countsr, BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            table1.AddCell(cellColumnName);

                            cellColumnName = new PdfPCell(new Phrase("Personal Allowance", BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            table1.AddCell(cellColumnName);

                            cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["PA"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["PA 01-04-2026"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            //table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["PA Inc"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            //table1.AddCell(cellColumnName);

                            cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["PA Revised"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            table1.AddCell(cellColumnName);



                            countsr++;
                        }
                        if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["DS"].ToString()) > 0)
                        {
                            //6
                            cellColumnName = new PdfPCell(new Phrase("" + countsr, BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            table1.AddCell(cellColumnName);

                            cellColumnName = new PdfPCell(new Phrase("Drivers Salary", BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            table1.AddCell(cellColumnName);

                            cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["DS"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["DS 01-04-2026"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            //table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["DS Inc"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            //table1.AddCell(cellColumnName);

                            cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["DS Revised"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            table1.AddCell(cellColumnName);



                            countsr++;
                        }
                        if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Loy Bon"].ToString()) > 0)
                        {
                            //7
                            cellColumnName = new PdfPCell(new Phrase("" + countsr, BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            table1.AddCell(cellColumnName);


                            cellColumnName = new PdfPCell(new Phrase("Loyalty Bonus", BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            table1.AddCell(cellColumnName);

                            cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Loy Bon"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Loy 01-04-2026"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            //table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Loy Bon Inc"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            //table1.AddCell(cellColumnName);

                            cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Loy Bon Revised"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            table1.AddCell(cellColumnName);



                            countsr++;
                        }

                        //TOTAL (A)
                        //cellColumnName = new PdfPCell(new Phrase("TOTAL (A)", BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                        cellColumnName = new PdfPCell(new Phrase("Gross (A)", BookmanOldStylebold)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4};
                        cellColumnName.Colspan = 2;
                        table1.AddCell(cellColumnName);

                        cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total A"].ToString(), BookmanOldStylebold)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                        table1.AddCell(cellColumnName);

                        //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total A 01-04-2026"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                        //table1.AddCell(cellColumnName);

                        //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total A Inc"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                        //table1.AddCell(cellColumnName);

                        cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total A Revised"].ToString(), BookmanOldStylebold)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                        table1.AddCell(cellColumnName);



                        cellColumnName = new PdfPCell(new Phrase("Less: PF Employee 12%", BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                        cellColumnName.Colspan = 2;
                        table1.AddCell(cellColumnName);

                        cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Less: PF 13%"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                        table1.AddCell(cellColumnName);

                        //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["PF 01-04-2026"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                        //table1.AddCell(cellColumnName);

                        //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Less: PF Inc on 13%"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                        //table1.AddCell(cellColumnName);

                        cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Less: PF Revised"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                        table1.AddCell(cellColumnName);

                        cellColumnName = new PdfPCell(new Phrase("Less: PT", BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                        cellColumnName.Colspan = 2;
                        table1.AddCell(cellColumnName);

                        cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Less: PT A"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                        table1.AddCell(cellColumnName);

                        //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Less: PT 01-04-2026"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                        //table1.AddCell(cellColumnName);

                        //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Less: PT B"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                        //table1.AddCell(cellColumnName);

                        cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Less: PT A+B"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                        table1.AddCell(cellColumnName);

                        cellColumnName = new PdfPCell(new Phrase("Less:ESIC Employee 0.75%", BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                        cellColumnName.Colspan = 2;
                        table1.AddCell(cellColumnName);

                        cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Less: ESIC A"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                        table1.AddCell(cellColumnName);

                        //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Les: ESIC 01-04-2026"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                        //table1.AddCell(cellColumnName);

                        //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Less: ESIC B"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                        //table1.AddCell(cellColumnName);

                        cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Less: ESIC A+B"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                        table1.AddCell(cellColumnName);

                        cellColumnName = new PdfPCell(new Phrase("Total Employee Contribution (B)", BookmanOldStylebold)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4};
                        cellColumnName.Colspan = 2;
                        table1.AddCell(cellColumnName);


                        /*cellColumnName = new PdfPCell(new Phrase("TOTAL (B)", BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                        cellColumnName.Colspan = 2;
                        table1.AddCell(cellColumnName);*/

                        cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total B"].ToString(), BookmanOldStylebold)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                        table1.AddCell(cellColumnName);



                        //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total B 01-04-2026"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                        //table1.AddCell(cellColumnName);

                        //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total B Inc"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                        //table1.AddCell(cellColumnName);

                        cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total B Revised"].ToString(), BookmanOldStylebold)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                        table1.AddCell(cellColumnName);

                        cellColumnName = new PdfPCell(new Phrase("Net Take Home (A-B)", BookmanOldStylebold)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                        cellColumnName.Colspan = 2;
                        table1.AddCell(cellColumnName);

                        cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Net A"].ToString(), BookmanOldStylebold)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 ,BackgroundColor = new BaseColor(236, 236, 236) };
                        table1.AddCell(cellColumnName);

                        //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Net 01-04-2026"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                        //table1.AddCell(cellColumnName);

                        //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Net B"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                        //table1.AddCell(cellColumnName);

                        cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Net A+B"].ToString(), BookmanOldStylebold)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                        table1.AddCell(cellColumnName);

                        Boolean OT_FLAG = false;

                        //temp code (added condition if rev then show columns)
                        if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Var Pay Rev"].ToString()) > 0 || Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Performance Bonus Rev"].ToString()) > 0)
                        //if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Var Pay In"].ToString()) > 0 || Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Performance Bonus In"].ToString()) > 0 || Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Retention Bonus In"].ToString()) > 0)

                        {
                            //temp added
                           /* cellColumnName = new PdfPCell(new Phrase("(B) Performance Linked (Annual) (As per Sales Team Variable Pay Circular)", BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            cellColumnName.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            cellColumnName.Colspan = 5;
                            table1.AddCell(cellColumnName);*/

                            int countbsrs = 1;
                            //below one line is temp condition
                            //if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Var Pay"].ToString()) > 0)
                            //  if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Var Pay In"].ToString()) > 0)
                            if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Var Pay Rev"].ToString()) > 0)
                            {
                                cellColumnName = new PdfPCell(new Phrase("" + countbsrs, BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);
                                //5
                                //cellColumnName = new PdfPCell(new Phrase("Variable Pay", BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                //table1.AddCell(cellColumnName);

                                cellColumnName = new PdfPCell(new Phrase("Periodic Variable Pay", BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Var Pay"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Var Pay 01-04-2026"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                //table1.AddCell(cellColumnName);

                                //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Var Pay In"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                //table1.AddCell(cellColumnName);

                                cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Var Pay Rev"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                countbsrs++;
                            }
                            //if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Performance Bonus"].ToString()) > 0)
                            //      if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Performance Bonus In"].ToString()) > 0)
                            if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["performance Bonus Rev"].ToString()) > 0)
                            {
                                cellColumnName = new PdfPCell(new Phrase("" + countbsrs, BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);
                                //5
                                cellColumnName = new PdfPCell(new Phrase("Performance Linked Bonus", BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);
                                //cellColumnName = new PdfPCell(new Phrase("Performance Bonus", BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                //table1.AddCell(cellColumnName);

                                cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Performance Bonus"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["performance Bonus 01-04-2026"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                //table1.AddCell(cellColumnName);

                                //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Performance Bonus In"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                //table1.AddCell(cellColumnName);

                                cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Performance Bonus Rev"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                countbsrs++;
                            }

                            // if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Retention Bonus"].ToString()) > 0)
                            //if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Retention Bonus In"].ToString()) > 0)
                            if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Retention Bonus Rev"].ToString()) > 0)
                            {
                                cellColumnName = new PdfPCell(new Phrase("" + countbsrs, BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);
                                //5
                                cellColumnName = new PdfPCell(new Phrase("Retention Bonus", BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Retention Bonus"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Retention Bonus 01-04-2026"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                //table1.AddCell(cellColumnName);

                                //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Retention Bonus In"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                //table1.AddCell(cellColumnName);

                                cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Retention Bonus Rev"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                countbsrs++;
                            }

                            // temp added
                            cellColumnName = new PdfPCell(new Phrase("TOTAL (C)", BookmanOldStylebold)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4};
                            cellColumnName.Colspan = 2;
                            table1.AddCell(cellColumnName);


                            /*cellColumnName = new PdfPCell(new Phrase("TOTAL (B)", BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            cellColumnName.Colspan = 2;
                            table1.AddCell(cellColumnName);*/

                            cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total D"].ToString(), BookmanOldStylebold)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            table1.AddCell(cellColumnName);

                            cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total D Revised"].ToString(), BookmanOldStylebold)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            table1.AddCell(cellColumnName);

                            ////cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total B 01-04-2026"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            ////table1.AddCell(cellColumnName);

                            ////cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total B Inc"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            ////table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total B Revised"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            //table1.AddCell(cellColumnName);

                            //end temp
                        }
                        else
                        {
                            OT_FLAG = true;
                            /*cellColumnName = new PdfPCell(new Phrase("(B) Other Benefits/Contributions ", BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            cellColumnName.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            cellColumnName.Colspan = 5;
                            table1.AddCell(cellColumnName);*/

                            int countbsr = 1;

                            if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["PF"].ToString()) > 0)
                            {
                                cellColumnName = new PdfPCell(new Phrase("" + countbsr, BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                cellColumnName = new PdfPCell(new Phrase("PF Employer @ 13%  ", BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["PF"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["PF 01-04-2026"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                //table1.AddCell(cellColumnName);

                                //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["PF Inc"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                //table1.AddCell(cellColumnName);

                                cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["PF Revised"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                countbsr++;
                            }

                            if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Gratuity"].ToString()) > 0)
                            {
                                cellColumnName = new PdfPCell(new Phrase("" + countbsr, BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);
                                //2


                                cellColumnName = new PdfPCell(new Phrase("Gratuity @ 4.81% of Basic", BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Gratuity"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Gratuity 01-04-2026"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                //table1.AddCell(cellColumnName);

                                //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Gratuity Inc"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                //table1.AddCell(cellColumnName);

                                cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Gratuity Revised"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                countbsr++;
                            }

                            if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Bonus"].ToString()) > 0)
                            {
                                cellColumnName = new PdfPCell(new Phrase("" + countbsr, BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);
                                //3

                                cellColumnName = new PdfPCell(new Phrase("Bonus ", BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Bonus"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Bonus 01-04-2026"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                //table1.AddCell(cellColumnName);

                                //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Bonus Inc"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                //table1.AddCell(cellColumnName);

                                cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Bonus Revised"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                countbsr++;
                            }

                            if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["LTA"].ToString()) > 0)
                            {
                                cellColumnName = new PdfPCell(new Phrase("" + countbsr, BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);
                                //4

                                cellColumnName = new PdfPCell(new Phrase("LTA Exp. Reimbursement", BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["LTA"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["LTA 01-04-2026"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                //table1.AddCell(cellColumnName);

                                //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["LTA  Inc"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                //table1.AddCell(cellColumnName);

                                cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["LTA Revised"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                countbsr++;
                            }
                            //added by aakash
                            //and also added THREE COLUMNS  ESIC, ESIC Inc and ESIC Revised in excel

                            if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["ESIC"].ToString()) > 0)
                            {
                                cellColumnName = new PdfPCell(new Phrase("" + countbsr, BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);
                                //4

                                cellColumnName = new PdfPCell(new Phrase("ESIC Employer @ 3.25%", BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["ESIC"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Les: ESIC 01-04-2026"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                //table1.AddCell(cellColumnName);

                                //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["ESIC Inc"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                //table1.AddCell(cellColumnName);

                                cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["ESIC Revised"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                countbsr++;
                            }

                            //end here aakash code

                            cellColumnName = new PdfPCell(new Phrase("Employer Contribution (C)", BookmanOldStylebold)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            cellColumnName.Colspan = 2;
                            table1.AddCell(cellColumnName);
                            /*cellColumnName = new PdfPCell(new Phrase("TOTAL (B)", BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            cellColumnName.Colspan = 2;
                            table1.AddCell(cellColumnName);*/
                            cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total C"].ToString(), BookmanOldStylebold)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4};
                            table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total C 01-04-2026"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            //table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total C Inc"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            //table1.AddCell(cellColumnName);

                            cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total C Revised"].ToString(), BookmanOldStylebold)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4};
                            table1.AddCell(cellColumnName);


                            cellColumnName = new PdfPCell(new Phrase("TOTAL (A+C) (Per Month)", BookmanOldStyleboldtotal)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 , BackgroundColor = new BaseColor(236, 236, 236) };
                            cellColumnName.Colspan = 2;
                            table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total ABC"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            //table1.AddCell(cellColumnName);
                            cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total A+C+D"].ToString(), BookmanOldStyleboldtotal)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            table1.AddCell(cellColumnName);
                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total ABC 01-04-2026"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            //table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total ABC Inc"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            //table1.AddCell(cellColumnName);

                            cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total A+C+D Revised"].ToString(), BookmanOldStyleboldtotal)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            table1.AddCell(cellColumnName);

                            cellColumnName = new PdfPCell(new Phrase("TOTAL CTC Per Annum", BookmanOldStyleboldtotal)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            cellColumnName.Colspan = 2;
                            table1.AddCell(cellColumnName);

                            cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total ABC"].ToString(), BookmanOldStyleboldtotal)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total ABC CTC 01-04-2026"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            //table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total ABC CTC Inc"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            //table1.AddCell(cellColumnName);

                            cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total ABC Revised"].ToString(), BookmanOldStyleboldtotal)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            table1.AddCell(cellColumnName);

                            /*cellColumnName = new PdfPCell(new Phrase("(D) Net Take Home", BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            cellColumnName.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            cellColumnName.Colspan = 5;
                            table1.AddCell(cellColumnName);*/



                            cellColumnName.FixedHeight = 10f;


                            doc.Add(table1);
                        }




                        if (!OT_FLAG)
                        {
                            /*cellColumnName = new PdfPCell(new Phrase("(C) Other Benefits/Contributions ", BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            cellColumnName.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            cellColumnName.Colspan = 5;
                            table1.AddCell(cellColumnName);*/

                            int countbsr = 1;

                            if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["PF"].ToString()) > 0)
                            {
                                cellColumnName = new PdfPCell(new Phrase("" + countbsr, BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                cellColumnName = new PdfPCell(new Phrase("PF Employer @ 13%  ", BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["PF"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Less: PF 13% 01-04-2026"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                //table1.AddCell(cellColumnName);

                                //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["PF Inc"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                //table1.AddCell(cellColumnName);

                                cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["PF Revised"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                countbsr++;

                            }
                            if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Gratuity"].ToString()) > 0)
                            {
                                cellColumnName = new PdfPCell(new Phrase("" + countbsr, BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);
                                //2

                                cellColumnName = new PdfPCell(new Phrase("Gratuity @ 4.81% of Basic", BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Gratuity"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Gratuity 01-04-2026"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                //table1.AddCell(cellColumnName);

                                //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Gratuity Inc"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                //table1.AddCell(cellColumnName);

                                cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Gratuity Revised"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                countbsr++;
                            }

                            if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Bonus"].ToString()) > 0)
                            {
                                cellColumnName = new PdfPCell(new Phrase("" + countbsr, BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);
                                //3

                                cellColumnName = new PdfPCell(new Phrase("Bonus", BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Bonus"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Bonus 01-04-2026"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                //table1.AddCell(cellColumnName);

                                //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Bonus Inc"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                //table1.AddCell(cellColumnName);

                                cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Bonus Revised"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                countbsr++;
                            }

                            if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["LTA"].ToString()) > 0)
                            {
                                cellColumnName = new PdfPCell(new Phrase("" + countbsr, BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);
                                //4

                                cellColumnName = new PdfPCell(new Phrase("LTA Exp. Reimbursement", BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["LTA"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["LTA 01-04-2026"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                //table1.AddCell(cellColumnName);

                                //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["LTA  Inc"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                //table1.AddCell(cellColumnName);

                                cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["LTA Revised"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                countbsr++;
                            }

                            //added by aakash
                            //and also added THREE COLUMNS  ESIC, ESIC Inc and ESIC Revised in excel
                            if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["ESIC"].ToString()) > 0)
                            {
                                cellColumnName = new PdfPCell(new Phrase("" + countbsr, BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);
                                //4

                                cellColumnName = new PdfPCell(new Phrase("ESIC Employer @ 3.25%", BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["ESIC"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["ESIC 01-04-2026"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                //table1.AddCell(cellColumnName);

                                //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["ESIC Inc"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                //table1.AddCell(cellColumnName);

                                cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["ESIC Revised"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                table1.AddCell(cellColumnName);

                                countbsr++;
                            }

                            //end here aakash code

                            //cellColumnName = new PdfPCell(new Phrase("TOTAL (C)", BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            //cellColumnName.Colspan = 2;
                            //table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total C"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            //table1.AddCell(cellColumnName);

                            if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Var Pay Rev"].ToString()) > 0 || Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Performance Bonus Rev"].ToString()) > 0)
                            {
                                cellColumnName = new PdfPCell(new Phrase("Total Employer Contribution (D)", BookmanOldStylebold)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                cellColumnName.Colspan = 2;
                                table1.AddCell(cellColumnName);
                            }

                            else
                            {
                                cellColumnName = new PdfPCell(new Phrase("Total Employer Contribution (C)", BookmanOldStylebold)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                                cellColumnName.Colspan = 2;
                                table1.AddCell(cellColumnName);
                            }
                                /*cellColumnName = new PdfPCell(new Phrase("TOTAL (B)", BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                                cellColumnName.Colspan = 2;
                                table1.AddCell(cellColumnName);*/
                                cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total C"].ToString(), BookmanOldStylebold)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total C 01-04-2026"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            //table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total C Inc"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            //table1.AddCell(cellColumnName);

                            cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total C Revised"].ToString(), BookmanOldStylebold)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4};
                            table1.AddCell(cellColumnName);

                            if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Var Pay Rev"].ToString()) > 0 || Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Performance Bonus Rev"].ToString()) > 0)
                            {
                                cellColumnName = new PdfPCell(new Phrase("CTC Per Month (A+C+D)", BookmanOldStyleboldtotal)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                                cellColumnName.Colspan = 2;
                                table1.AddCell(cellColumnName);
                            }

                            else
                            {
                                cellColumnName = new PdfPCell(new Phrase("CTC Per Month (A+C)", BookmanOldStyleboldtotal)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                                cellColumnName.Colspan = 2;
                                table1.AddCell(cellColumnName);
                            }

                           

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total ABC"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            //table1.AddCell(cellColumnName);
                            cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total A+C+D"].ToString(), BookmanOldStyleboldtotal)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            table1.AddCell(cellColumnName);
                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total ABC 01-04-2026"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            //table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total ABC Inc"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            //table1.AddCell(cellColumnName);

                            cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total A+C+D Revised"].ToString(), BookmanOldStyleboldtotal)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            table1.AddCell(cellColumnName);

                            cellColumnName = new PdfPCell(new Phrase("CTC Per Annum", BookmanOldStyleboldtotal)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            cellColumnName.Colspan = 2;
                            table1.AddCell(cellColumnName);

                            cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total ABC"].ToString(), BookmanOldStyleboldtotal)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total ABC CTC 01-04-2026"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            //table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total ABC CTC Inc"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            //table1.AddCell(cellColumnName);

                            cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total ABC Revised"].ToString(), BookmanOldStyleboldtotal)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total C 01-04-2026"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            //table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total C Inc"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            //table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total C Revised"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            //table1.AddCell(cellColumnName);


                            //cellColumnName = new PdfPCell(new Phrase("TOTAL (A+B+C) (Monthly)", BookmanOldStyleboldtotal)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            //cellColumnName.Colspan = 2;
                            //table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total ABC"].ToString(), BookmanOldStyleboldtotal)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            //table1.AddCell(cellColumnName);

                            ////cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total ABC 01-04-2026"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            ////table1.AddCell(cellColumnName);

                            ////cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total ABC Inc"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            ////table1.AddCell(cellColumnName);

                            ////cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total ABC Revised"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            ////table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase("TOTAL CTC (A+B+C) (Annual)", BookmanOldStyleboldtotal)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            //cellColumnName.Colspan = 2;
                            //table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total ABC"].ToString(), BookmanOldStyleboldtotal)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            //table1.AddCell(cellColumnName);

                            ////cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total ABC CTC 01-04-2026"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            ////table1.AddCell(cellColumnName);

                            ////cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total ABC CTC Inc"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            ////table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total ABC Revised"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            //table1.AddCell(cellColumnName);

                        
                            //cellColumnName = new PdfPCell(new Phrase("Less: PF 12%", BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            //cellColumnName.Colspan = 2;
                            //table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Less: PF 13%"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            //table1.AddCell(cellColumnName);

                            ////cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Less: PF 13% 01-04-2026"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            ////table1.AddCell(cellColumnName);

                            ////cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Less: PF Inc on 13%"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            ////table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Less: PF Revised"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            //table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase("Less: PT", BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            //cellColumnName.Colspan = 2;
                            //table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Less: PT A"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            //table1.AddCell(cellColumnName);

                            ////cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Less: PT 01-04-2026"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            ////table1.AddCell(cellColumnName);

                            ////cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Less: PT B"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            ////table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Less: PT A+B"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            //table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase("Less: ESIC", BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            //cellColumnName.Colspan = 2;
                            //table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Less: ESIC A"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            //table1.AddCell(cellColumnName);

                            ////cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Les: ESIC 01-04-2026"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            ////table1.AddCell(cellColumnName);

                            ////cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Less: ESIC B"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            ////table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Less: ESIC A+B"].ToString(), BookmanOldStyleTables)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                            //table1.AddCell(cellColumnName);




                            //cellColumnName = new PdfPCell(new Phrase("TOTAL (B)", BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            //cellColumnName.Colspan = 2;
                            //table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total B"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            //table1.AddCell(cellColumnName);



                            ////cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total B 01-04-2026"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            ////table1.AddCell(cellColumnName);

                            ////cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total B Inc"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            ////table1.AddCell(cellColumnName);

                            //cellColumnName = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["Total B Revised"].ToString(), BookmanOldStyleTable)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                            //table1.AddCell(cellColumnName);

                         

                            doc.Add(table1);

                        }

                      


                        /* DataSalary = new Paragraph();
                         DataSalary = new Paragraph(); DataSalary.Add(new Chunk(" ", BookmanOldStyle)); doc.Add(DataSalary);

                         DataSalary = new Paragraph();
                         DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                         DataSalary.Alignment = 2;
                         DataSalary.Add(new Chunk(@"Net Take Home", BookmanOldStyle));
                         doc.Add(DataSalary);

                         DataSalary = new Paragraph();
                         DataSalary = new Paragraph(); DataSalary.Add(new Chunk(" ", BookmanOldStyle)); doc.Add(DataSalary);

                         PdfPTable table2 = new PdfPTable(3);
                         int[] columnwidth1 = { 30, 30,30};
                         table2.SetWidths(columnwidth1); // Corrected variable name
                         table2.WidthPercentage = 100;
                         table2.HorizontalAlignment = Element.ALIGN_LEFT;
                         Font BookmanOldStyleTable1 = FontFactory.GetFont("Bookman Old Style", 10, Font.BOLD, new BaseColor(50, 50, 111));

                         PdfPCell cellColumnName1 = new PdfPCell(new Phrase("Components", BookmanOldStyleTable1)) // Consistent variable name
                         {
                             HorizontalAlignment = Element.ALIGN_CENTER,
                             Padding = 4,
                             BackgroundColor = new BaseColor(236, 236, 236),
                             Rowspan = 1
                         };
                         table2.AddCell(cellColumnName1);

                          cellColumnName1 = new PdfPCell(new Phrase("Per Month (CTC)", BookmanOldStyleTable1)) // Corrected variable name
                         {
                             HorizontalAlignment = Element.ALIGN_CENTER,
                             Padding = 4,
                             BackgroundColor = new BaseColor(236, 236, 236),
                             Rowspan = 1,
                             FixedHeight = 10
                         };
                         table2.AddCell(cellColumnName1);

                         cellColumnName1 = new PdfPCell(new Phrase("Per Annum (CTC)", BookmanOldStyleTable1)) // Consistent variable name
                         {
                             HorizontalAlignment = Element.ALIGN_CENTER,
                             Padding = 4,
                             BackgroundColor = new BaseColor(236, 236, 236),
                             Rowspan = 1
                         };
                         table2.AddCell(cellColumnName1);


                         cellColumnName1 = new PdfPCell(Datetable) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4, BackgroundColor = new BaseColor(236, 236, 236) };
                         cellColumnName1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                         cellColumnName1.Colspan = 5;
                         table1.AddCell(cellColumnName);

                         int countsrk = 1;


                         if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["TakeHome"].ToString()) > 0) // Corrected typo in column name
                         {

                             cellColumnName1 = new PdfPCell(new Phrase("Monthly Gross", BookmanOldStyle)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                             table2.AddCell(cellColumnName1);



                             cellColumnName1 = new PdfPCell(new Phrase("" + countsrk, BookmanOldStyleTable1)) // Consistent font variable name
                             {
                                 HorizontalAlignment = Element.ALIGN_CENTER,
                                 Padding = 4
                             };
                             table2.AddCell(cellColumnName1);
                             //4


                             // Changed to table2

                             cellColumnName1 = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["TakeHome Monthly Gross"].ToString(), BookmanOldStyleTable1)) // Corrected column name
                             {
                                 HorizontalAlignment = Element.ALIGN_CENTER,
                                 Padding = 4
                             };
                             table2.AddCell(cellColumnName1); // Changed to table2


                         }

                         if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["TakeHome"].ToString()) > 0) // Corrected typo in column name
                         {

                             cellColumnName1 = new PdfPCell(new Phrase("Less : Employee PF", BookmanOldStyle)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4 };
                             table2.AddCell(cellColumnName1);

                             cellColumnName1 = new PdfPCell(new Phrase("" + countsrk, BookmanOldStyleTable1)) // Consistent font variable name
                             {
                                 HorizontalAlignment = Element.ALIGN_CENTER,
                                 Padding = 4
                             };
                             table2.AddCell(cellColumnName1);
                             //4


                             // Changed to table2

                             cellColumnName1 = new PdfPCell(new Phrase(GetAllEmpdatils.Rows[iemp]["TakeHome Less : Employee PF"].ToString(), BookmanOldStyleTable1)) // Corrected column name
                             {
                                 HorizontalAlignment = Element.ALIGN_CENTER,
                                 Padding = 4
                             };
                             table2.AddCell(cellColumnName1); // Changed to table2

                         }

                         doc.Add(table2);*/


                        #endregion

                        #region Prorata Section
                        if (GetAllEmpdatils.Rows[iemp]["Days"].ToString().Trim() == "")
                        {
                            //DataSalary.Add(new Chunk("As you have joined us after 1st April 2025, prorate amount arrives ", BookmanOldStyle));
                            //DataSalary.Add(new Chunk(" Rs.", BookmanOldStyle));
                            //DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Total ABC"].ToString() + "/-", BookmanOldStylebold));
                            //DataSalary.Add(new Chunk(" PM increment in your Current CTC.Till 31.03.2026 work duration of your service is ", BookmanOldStyle));
                            //DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Days"].ToString(), BookmanOldStylebold));
                            ////DataSalary.Add(new Chunk(" days. Hence prorate calculation is as below.", BookmanOldStyle));
                            //DataSalary.Add(new Chunk(" days.", BookmanOldStyle));
                            //DataSalary.Add(new Chunk("Calculation as ", BookmanOldStyle));
                            //DataSalary.Add(new Chunk("Rs.", BookmanOldStyle));
                            //DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Whole Year Incr Amount"].ToString() + "/-", BookmanOldStylebold));
                            //DataSalary.Add(new Chunk(" increment for the year and Rs.", BookmanOldStyle));
                            //DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Total ABC"].ToString() + "/-", BookmanOldStylebold));
                            //DataSalary.Add(new Chunk(" Calculated as per prorate basis. i..e  Rs.", BookmanOldStyle));
                            //DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Whole Year Incr Amount"].ToString() + "", BookmanOldStylebold));
                            //DataSalary.Add(new Chunk(" ÷ 365 × ", BookmanOldStyle));
                            //DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Days"].ToString(), BookmanOldStylebold));
                            //DataSalary.Add(new Chunk(" = Rs.", BookmanOldStyle));
                            //DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Total ABC"].ToString() + "/-", BookmanOldStylebold));
                            //DataSalary.Add(new Chunk(" PM.", BookmanOldStyle));

                            
                            //DataSalary = new Paragraph(12f, "");
                            //DataSalary.SpacingBefore = 1f;
                            //DataSalary.Alignment = Element.ALIGN_JUSTIFIED;

                            // Monthly Increment
                           /* DataSalary.Add(new Chunk("● Monthly Salary CTC Increment: ", BookmanOldStylebold));
                            DataSalary.Add(new Chunk(
                                GetAllEmpdatils.Rows[iemp]["Whole Year Incr Amount"] + "/-",
                                BookmanOldStyle));
                            DataSalary.Add(Chunk.NEWLINE);
                            DataSalary.SpacingBefore = 1f;*/
                            // Effective Date
                           /* DataSalary.Add(new Chunk("● Effective Date: ", BookmanOldStylebold));
                            DataSalary.Add(new Chunk(
                                "01-07-2026",
                                BookmanOldStyle));*/
                            //DataSalary.Add(Chunk.NEWLINE);

                            //// Notice Period
                            //DataSalary.Add(new Chunk("● Notice Period: ", BookmanOldStylebold));
                            //DataSalary.Add(new Chunk(
                            //    GetAllEmpdatils.Rows[iemp]["notice period"].ToString() +
                            //    "  months (As per the terms applicable to your current/revised designation)",
                            //    BookmanOldStyle));
                            //DataSalary.Add(Chunk.NEWLINE);
                           
                            //doc.Add(DataSalary);
                            /*doc.Add(new Paragraph("  "));*/

                            //DataSalary = new Paragraph(12f,"");
                            //DataSalary.Add(new Chunk("Calculation as ", BookmanOldStyle));
                            //DataSalary.Add(new Chunk("Rs.", BookmanOldStyle));
                            //DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Whole Year Incr Amount"].ToString() + "/-", BookmanOldStylebold));
                            //DataSalary.Add(new Chunk(" increment for the year and Rs.", BookmanOldStyle));
                            //DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Total ABC Inc"].ToString() + "/-", BookmanOldStylebold));
                            //DataSalary.Add(new Chunk(" Calculated as per prorate basis. i..e  Rs.", BookmanOldStyle));
                            //DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Whole Year Incr Amount"].ToString() + "", BookmanOldStylebold));
                            //DataSalary.Add(new Chunk(" ÷ 365 × ", BookmanOldStyle));
                            //DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Days"].ToString(), BookmanOldStylebold));
                            //DataSalary.Add(new Chunk(" = Rs.", BookmanOldStyle));
                            //DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Total ABC Inc"].ToString() + "/-", BookmanOldStylebold));
                            //DataSalary.Add(new Chunk(" PM.", BookmanOldStyle));
                            //DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                            //doc.Add(DataSalary);
                        }



                        #region Footer Section
                        DataSalary = new Paragraph(12f);
                        DataSalary.SpacingBefore = 6;
                        DataSalary.Leading = 13f;
                        BaseColor baseColor = new BaseColor(255, 0, 0);
                        Font redFont = new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, baseColor);
                       
                        DataSalary.Add(new Chunk("*", redFont));
                        DataSalary.SpacingBefore = 1f;
                       
                        //DataSalary.Add(new Chunk("This letter is being sent to you as a confirmed salaried employee of Vikram Tea Processor Private Limited and according to your current designation your notice period will be ", BookmanOldStyle1));
                        //DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["notice period"].ToString() + " months.", BookmanOldStyle1));
                        DataSalary.Add(new Chunk("Kindly treat this salary revision as strictly confidential and do not share it with colleagues, distributors, or any external parties.", BookmanOldStyle));
                        
                        DataSalary.SpacingBefore = 2f;
                        DataSalary.Add(Chunk.NEWLINE);
                        //DataSalary.Add(new Chunk("We wish you continued success and look forward to your ongoing association with Vikram Tea Processor Private Limited", BookmanOldStyle));
                        //DataSalary.Add(Chunk.NEWLINE);
                  
                        DataSalary.Add(new Chunk("Note:-", BookmanOldStylebold));
                        DataSalary.Add(new Chunk(" Management has included an additional increment amount, wherever applicable, along with the annual increment as part of this appraisal cycle. The revised salary structure has been aligned with the requirements of the New Wage Code and applicable statutory compliances. Accordingly, statutory components under the revised structure shall be effective from 1st July 2026.", BookmanOldStyleSmall));
                        DataSalary.SpacingBefore = 2f;
                       


                        //DataSalary.IndentationLeft = 32f;
                      


                        DataSalary.Add(Chunk.NEWLINE);
                       
                        // Notice Period
                        DataSalary.Add(new Chunk("**", redFont));
                        DataSalary.SpacingBefore = 1f;
                        DataSalary.Add(new Chunk("Notice Period: ", BookmanOldStylebold));
                        DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["notice period"].ToString() + " Month ", BookmanOldStylebold));
                        DataSalary.Add(new Chunk("as per the terms & conditions applicable to your current/revised designation.",
                            BookmanOldStyle));
                       

                        doc.Add(DataSalary);

                        DataSalary = new Paragraph();
                        DataSalary.SpacingBefore = 6;
                        DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                        DataSalary.Add(new Chunk(@"Wishing you long term & fruitful career with Vikram Tea !", BookmanOldStyle));
                        DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                        DataSalary.SpacingAfter = 1;
                        doc.Add(DataSalary);

                        DataSalary = new Paragraph();
                        DataSalary.Add(new Chunk(@"Best Regards,", BookmanOldStyle));
                        DataSalary.SpacingAfter = 7;
                        DataSalary.SpacingBefore = 3;
                        doc.Add(DataSalary);

                        PdfPTable tablesingnature1 = new PdfPTable(1);
                        int[] columnwidthtop1 = { 100 };
                        tablesingnature1.SetWidths(columnwidthtop1);
                        tablesingnature1.WidthPercentage = 100;
                        tablesingnature1.HorizontalAlignment = 0;

                        string imagePathsignature1 = Server.MapPath("../Content/images/") + GetAllEmpdatils.Rows[iemp]["HOD Name"].ToString() + ".png";
                        iTextSharp.text.Image imagelogosignature1 = iTextSharp.text.Image.GetInstance(imagePathsignature1);
                        imagelogosignature1.Alignment = Element.ALIGN_LEFT;
                        // set width and height
                        //   imagelogosignature.ScaleToFit(120f, 150f);
                        imagelogosignature1.ScaleToFit(75f, 95f);

                        PdfPCell imagecellColumnValue2 = new PdfPCell(imagelogosignature1);
                        imagecellColumnValue2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        imagecellColumnValue2.Border = 0;
                        tablesingnature1.AddCell(imagecellColumnValue2);

                        doc.Add(tablesingnature1);

                        DataSalary = new Paragraph(12f, "");
                        DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                        DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["HOD Name"].ToString(), BookmanOldStylebold));
                        doc.Add(DataSalary);

                        DataSalary = new Paragraph(12f, "");
                        DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                        DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["HOD Designation"].ToString(), BookmanOldStylebold));
                        doc.Add(DataSalary);


                        // 6) Addling blank paragraphe
                        //doc.Add(new Paragraph("  "));

                        #endregion

                       

                        /*doc.Add(new Paragraph("  "));*/

                       
                       
                        
                        //Note.Add(new Chunk(" Kindly treat this letter as highly confidential and do not disclose with colleagues, distributor, and other associated partner of our organization.", BookmanOldStyle1));
                        //Note.Leading = 12f;
                        //Note.Alignment = Element.ALIGN_JUSTIFIED;
                        //Note.IndentationLeft = 31f;
                        //doc.Add(Note);

                        //Paragraph Note1 = new Paragraph();
                        //Note1.Add(new Chunk("partner of our organization.and as per cretire in case of resign your notice priod will be of " + GetAllEmpdatils.Rows[iemp]["NoOFSal"].ToString() + " Months", BookmanOldStyle1));
                        //Note1.Leading = 12f;
                        //Note1.Alignment = Element.ALIGN_JUSTIFIED;
                        //Note1.IndentationLeft = 31f;
                        //doc.Add(Note1);




                        //doc.NewPage();
                        #endregion
                    }
                    else
                    {
                        #region Case2 Section
                        DataSalary = new Paragraph(); DataSalary.Add(new Chunk(@" ", BookmanOldStyle)); doc.Add(DataSalary);

                        DataSalary = new Paragraph();
                        DataSalary.SpacingBefore = 3f;
                        DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                        DataSalary.Add(new Phrase("Dear " + GetAllEmpdatils.Rows[iemp]["Abbrivaton"].ToString(), BookmanOldStyle));
                        DataSalary.Add(new Phrase(" " + GetAllEmpdatils.Rows[iemp]["Emp_Name"].ToString() + ",", BookmanOldStyle));
                        //DataSalary.SpacingAfter = 10;
                        doc.Add(DataSalary);

                        // DataSalary = new Paragraph(); DataSalary.Add(new Chunk(@" ", BookmanOldStyle)); doc.Add(DataSalary); 
                        //doc.Add(new Paragraph("  ")); Edited by V

                        DataSalary = new Paragraph();
                        DataSalary.SpacingBefore = 4f;
                        DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                        DataSalary.Add(new Chunk(@"Congratulations!", BookmanOldStyle));
                        DataSalary.SpacingBefore = 8;
                        doc.Add(DataSalary);
                        /*DataSalary = new Paragraph();
                        DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                        DataSalary.Add(new Chunk(@"Subject: Performance Feedback and salary revision letter.", BookmanOldStyle));
                        doc.Add(DataSalary);*/

                        //DataSalary = new Paragraph(); DataSalary.Add(new Chunk(@" ", BookmanOldStyle)); doc.Add(DataSalary);

                        DataSalary = new Paragraph();
                        DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                        DataSalary.Add(new Chunk(@"We have completed Performance appraisal process for the year 2023 – 24 and your performance during the year was “", BookmanOldStyle));
                        DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Phrase"].ToString(), BookmanOldStylebold));
                        DataSalary.Add(new Chunk("”! Your manager rated you as “", BookmanOldStyle));
                        DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Rating"].ToString(), BookmanOldStylebold));
                        DataSalary.Add(new Chunk("” for your performance.", BookmanOldStyle));
                        DataSalary.SpacingAfter = 6;
                        doc.Add(DataSalary);

                        //DataSalary = new Paragraph(); DataSalary.Add(new Chunk(@" ", BookmanOldStyle)); doc.Add(DataSalary)

                        DataSalary = new Paragraph();
                        DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                        DataSalary.Add(new Chunk(@"Hope you will improve your working and upgrade yourself to reach next level of rating for the running year.", BookmanOldStyle));
                        DataSalary.SpacingAfter = 6;
                        doc.Add(DataSalary);

                        //DataSalary = new Paragraph(); DataSalary.Add(new Chunk(@" ", BookmanOldStyle)); doc.Add(DataSalary);

                        DataSalary = new Paragraph();
                        DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                        DataSalary.Add(new Chunk(@"From our side for your motivation and considering your performance we have revised your CTC.Following increment effect have been given in your salary.", BookmanOldStyle));
                        DataSalary.SpacingAfter = 6;
                        doc.Add(DataSalary);

                        //DataSalary = new Paragraph(); DataSalary.Add(new Chunk(@" ", BookmanOldStyle)); doc.Add(DataSalary);

                        string empname = GetAllEmpdatils.Rows[iemp]["Emp_Name"].ToString().Trim();
                        //aakash code temp(commented esic condition)

                        //if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["ESIC"].ToString().Trim()) > 0 && Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Var Pay In"].ToString().Trim()) > 0 && Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Performance Bonus In"].ToString().Trim()) > 0 && Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Retention Bonus In"].ToString().Trim()) > 0)
                        //{
                        //    //DataSalary = new Paragraph();
                        //    //DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                        //    //DataSalary.Add(new Chunk(@"     A)  The company has added 3.25% of your Gross salary towards ESIC", BookmanOldStyle));
                        //    //doc.Add(DataSalary);

                        //    DataSalary = new Paragraph();
                        //    DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                        //    DataSalary.Add(new Chunk(@"     A)  Monthly increment of Rs." + GetAllEmpdatils.Rows[iemp]["Standard Inc"].ToString() + "/-", BookmanOldStyle));
                        //    doc.Add(DataSalary);

                        //    DataSalary = new Paragraph();
                        //    DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                        //    DataSalary.Add(new Chunk(@"     B)  The Variable Pay Rs." + GetAllEmpdatils.Rows[iemp]["Var Pay In"].ToString() + "/- and performance bonus of Rs." + GetAllEmpdatils.Rows[iemp]["Performance Bonus In"].ToString() + "/- and retention bonus of Rs." + GetAllEmpdatils.Rows[iemp]["Retention Bonus In"].ToString() + "/- Per month, Total Rs." + (Convert.ToInt32(GetAllEmpdatils.Rows[iemp]["Var Pay In"].ToString()) + Convert.ToInt32(GetAllEmpdatils.Rows[iemp]["Performance Bonus In"].ToString()) + Convert.ToInt32(GetAllEmpdatils.Rows[iemp]["Retention Bonus In"].ToString())) + "/-, (Performance Linked)", BookmanOldStyle));
                        //    doc.Add(DataSalary);
                        //}
                        //else if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["ESIC"].ToString().Trim()) > 0)
                        //{
                        //    //DataSalary = new Paragraph();
                        //    //DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                        //    //DataSalary.Add(new Chunk(@"     A)  The company has added 3.25% of your Gross salary towards ESIC", BookmanOldStyle));
                        //    //doc.Add(DataSalary);

                        //    DataSalary = new Paragraph();
                        //    DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                        //    DataSalary.Add(new Chunk(@"     B)  Monthly increment of Rs." + GetAllEmpdatils.Rows[iemp]["Standard Inc"].ToString() + "/-", BookmanOldStyle));
                        //    doc.Add(DataSalary);
                        //}

                        //var pay in error
                        //if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Var Pay In"].ToString().Trim()) > 0 || Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Performance Bonus In"].ToString().Trim()) > 0 || Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Retention Bonus In"].ToString().Trim()) > 0)
                        ////if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Var Pay In"].ToString().Trim()) > 0 && Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Performance Bonus In"].ToString().Trim()) > 0 && Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Retention Bonus In"].ToString().Trim()) > 0)
                        //{
                        //    DataSalary = new Paragraph();
                        //    DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                        //    DataSalary.Add(new Chunk(@"     A)  Monthly increment of Rs." + GetAllEmpdatils.Rows[iemp]["Standard Inc"].ToString() + "/-", BookmanOldStyle));
                        //    doc.Add(DataSalary);

                        //    DataSalary = new Paragraph();
                        //    DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                        //    DataSalary.Add(new Chunk(@"     B)  The Variable Pay Rs." + GetAllEmpdatils.Rows[iemp]["Var Pay In"].ToString() + "/- and performance bonus of Rs." + GetAllEmpdatils.Rows[iemp]["Performance Bonus In"].ToString() + "/- and retention bonus of Rs." + GetAllEmpdatils.Rows[iemp]["Retention Bonus In"].ToString() + "/- Per month, Total Rs." + (Convert.ToInt32(GetAllEmpdatils.Rows[iemp]["Var Pay In"].ToString()) + Convert.ToInt32(GetAllEmpdatils.Rows[iemp]["Performance Bonus In"].ToString()) + Convert.ToInt32(GetAllEmpdatils.Rows[iemp]["Retention Bonus In"].ToString())) + "/- , (Performance Linked).", BookmanOldStylebold));
                        //    doc.Add(DataSalary);
                        //}


                        //test code if var pay and performace bonus is there not retention bonus 
                        //else if (Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Var Pay In"].ToString().Trim()) > 0 && Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Performance Bonus In"].ToString().Trim()) > 0)
                        //{
                        //    DataSalary = new Paragraph();
                        //    DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                        //    DataSalary.Add(new Chunk(@"     B)  Monthly increment of Rs." + GetAllEmpdatils.Rows[iemp]["Standard Inc"].ToString() + "/-", BookmanOldStyle));
                        //    doc.Add(DataSalary);

                        //    DataSalary = new Paragraph();
                        //    DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                        //    DataSalary.Add(new Chunk(@"     B)  The Variable Pay Rs." + GetAllEmpdatils.Rows[iemp]["Var Pay In"].ToString() + "/- and performance bonus of Rs." + GetAllEmpdatils.Rows[iemp]["Performance Bonus In"].ToString() + "/- Per month, Total Rs." + (Convert.ToInt32(GetAllEmpdatils.Rows[iemp]["Var Pay In"].ToString()) + Convert.ToInt32(GetAllEmpdatils.Rows[iemp]["Performance Bonus In"].ToString())) + "/- , (Performance Linked)", BookmanOldStyle));
                        //    doc.Add(DataSalary);
                        //}
                        //else
                        //{
                        //    /*DataSalary = new Paragraph();
                        //    DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                        //    DataSalary.Add(new Chunk(@"  Monthly increment of Rs." + GetAllEmpdatils.Rows[iemp]["Standard Inc"].ToString() + "/-", BookmanOldStylebold));
                        //    DataSalary.Add(new Chunk(@", your increased CTC effective from 01.04.2024 is Rs.", BookmanOldStyle));
                        //    DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Total ABC Revised"].ToString(), BookmanOldStylebold));
                        //    DataSalary.Add(new Chunk("/-.Annexure is attached with this letter containing existing CTC and Revised CTC Breakup.", BookmanOldStyle));
                        //    doc.Add(DataSalary);*/

                        //    DataSalary = new Paragraph();
                        //    DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                        //    DataSalary.Add(new Chunk("Monthly increment of Rs. ", BookmanOldStyle));
                        //    DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Standard Inc"].ToString() + "/- ", BookmanOldStylebold));
                        //    DataSalary.Add(new Chunk(", your CTC has been revised to Rs. ", BookmanOldStyle));
                        //    DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["CTC AFTER INC"].ToString(), BookmanOldStylebold));
                        //    DataSalary.Add(new Chunk("/- PM w.e.f. 1", BookmanOldStyle));
                        //    DataSalary.Add(new Chunk("st", BookmanOldStyle).SetTextRise(2));
                        //    DataSalary.Add(new Chunk("  April 2024 as a part of performance appraisal and commitment towards the company. The detailed pay structure is annexed along with this letter.", BookmanOldStyle));
                        //    DataSalary.SpacingAfter = 4;
                        //    doc.Add(DataSalary);
                        //}

                        //DataSalary = new Paragraph(); DataSalary.Add(new Chunk(@" ", BookmanOldStyle)); doc.Add(DataSalary);

                        ///aakash code temp end

                        /*DataSalary = new Paragraph();
                        DataSalary.Add(new Chunk(@", your increased CTC effective from 01.04.2024 is Rs.", BookmanOldStyle));
                        DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Total ABC Revised"].ToString(), BookmanOldStylebold));
                        DataSalary.Add(new Chunk("/-.Annexure is attached with this letter containing existing CTC and Revised CTC Breakup.", BookmanOldStyle));
                        doc.Add(DataSalary);*/

                        if (GetAllEmpdatils.Rows[iemp]["Salary Correction"].ToString().Trim() == "Correction")
                        {
                            DataSalary = new Paragraph();
                            DataSalary.Add(new Chunk(@" ", BookmanOldStyle));
                            doc.Add(DataSalary);
                            DataSalary = new Paragraph();
                            DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                            DataSalary.Add(new Chunk(@"While fixing your new CTC, we have considered your performance as well as your commitment and loyalty towards the organization, and hence with the standard increment we are also making salary correction. Your standard increment is ", BookmanOldStyle));
                            DataSalary.Add(new Chunk("" + Math.Round(Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Standard Percentage"].ToString()), 2) + " %", BookmanOldStylebold));
                            DataSalary.Add(new Chunk("and after salary correction it goes upto ", BookmanOldStyle));
                            DataSalary.Add(new Chunk("" + Math.Round(Convert.ToDouble(GetAllEmpdatils.Rows[iemp]["Increment Percentage"].ToString()), 2) + " %", BookmanOldStylebold));
                            DataSalary.Add(new Chunk("for this year. ", BookmanOldStyle));
                            if (GetAllEmpdatils.Rows[iemp]["Petrol Allowance"].ToString().Trim() != "")
                            {
                                if (GetAllEmpdatils.Rows[iemp]["Petrol Allowance"].ToString().Trim().ToUpper() == "YES")
                                {
                                    DataSalary.Add(new Chunk(@"Including petrol reimbursement.", BookmanOldStylebold));
                                }
                            }
                            doc.Add(DataSalary);
                        }

                        //if (GetAllEmpdatils.Rows[iemp]["Designation Upgrade"].ToString().Trim() != "")
                        //{
                        //    DataSalary = new Paragraph(); DataSalary.Add(new Chunk(@" ", BookmanOldStyle)); doc.Add(DataSalary);
                        //    DataSalary = new Paragraph();
                        //    DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                        //    DataSalary.Add(new Chunk(@"Considering your job nature we are designating you as ", BookmanOldStyle));
                        //    DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Designation Upgrade"].ToString() + ". ", BookmanOldStylebold));

                        //    doc.Add(DataSalary);
                        //}

                        //DataSalary = new Paragraph();
                        ////DataSalary = new Paragraph(new Phrase(5f, " ", BookmanOldStyle));
                        //doc.Add(DataSalary);

                        doc.Add(DataSalary);

                        DataSalary = new Paragraph();
                        DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                        DataSalary.SpacingAfter = 7;
                        DataSalary.Add(new Chunk(@"Kindly consider this as opportunity and improve your performance, best of luck for the year 2025 – 25. We at organization and your reporting manager are always there to increase your performance upto the mark.", BookmanOldStyle));
                       
                        doc.Add(DataSalary);

                        DataSalary = new Paragraph();
                        DataSalary.Add(new Chunk(@" ", BookmanOldStyle));
                        doc.Add(DataSalary);

                        DataSalary = new Paragraph();
                        DataSalary.Add(new Chunk(@"Best Regards,", BookmanOldStyle));
                        doc.Add(DataSalary);

                        DataSalary = new Paragraph(); DataSalary.Add(new Chunk(@" ", BookmanOldStyle)); doc.Add(DataSalary);

                        PdfPTable tablesingnature = new PdfPTable(1);
                        int[] columnwidthtop = { 100 };
                        tablesingnature.SetWidths(columnwidthtop);
                        tablesingnature.WidthPercentage = 100;
                        tablesingnature.HorizontalAlignment = 0;

                        string imagePathsignature = Server.MapPath("../Content/images/") + GetAllEmpdatils.Rows[iemp]["HOD Name"].ToString() + ".png";
                        iTextSharp.text.Image imagelogosignature = iTextSharp.text.Image.GetInstance(imagePathsignature);
                        imagelogosignature.Alignment = Element.ALIGN_LEFT;
                        // set width and height
                        imagelogosignature.ScaleToFit(100f, 100f);

                        PdfPCell imagecellColumnValue1 = new PdfPCell(imagelogosignature);
                        imagecellColumnValue1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        imagecellColumnValue1.Border = 0;
                        tablesingnature.AddCell(imagecellColumnValue1);

                        doc.Add(tablesingnature);

                        DataSalary = new Paragraph();
                        DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                        DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["HOD Name"].ToString(), BookmanOldStylebold));
                        doc.Add(DataSalary);

                        DataSalary = new Paragraph();
                        DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                        DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["HOD Designation"].ToString(), BookmanOldStylebold));
                        doc.Add(DataSalary);


                        DataSalary = new Paragraph(); DataSalary.Add(new Chunk(@" ", BookmanOldStyle)); doc.Add(DataSalary);
                        doc.NewPage();
                        #endregion
                    }

                    #region Page2 Section

                    //DataSalary = new Paragraph();
                    //DataSalary = new Paragraph(); DataSalary.Add(new Chunk(" ", BookmanOldStyle)); doc.Add(DataSalary);
                    //DataSalary = new Paragraph(); DataSalary.Add(new Chunk(" ", BookmanOldStyle)); doc.Add(DataSalary);
                    ///*DataSalary = new Paragraph(); DataSalary.Add(new Chunk(" ", BookmanOldStyle)); doc.Add(DataSalary);*/
                    //DataSalary.Alignment = Element.ALIGN_RIGHT; // Align to the right
                    //DataSalary.SpacingBefore = 5f;
                    //DataSalary.Add(new Chunk(@"Date 06", BookmanOldStyle));
                    //DataSalary.Add(new Chunk(@"th", BookmanOldStyle).SetTextRise(2));
                    //DataSalary.Add(new Chunk(@" Aug 2025", BookmanOldStyle));
                    
                    //doc.Add(DataSalary);

                    //** second page

                    //DataSalary = new Paragraph();
                    //DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                    //DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Abbrivaton"].ToString() + " " + GetAllEmpdatils.Rows[iemp]["Emp_Name"].ToString(), BookmanOldStylebold));
                    //doc.Add(DataSalary);

                    //if (GetAllEmpdatils.Rows[iemp]["New Designation"].ToString().Trim() != "")
                    //{
                    //    DataSalary = new Paragraph();
                    //    DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                    //    DataSalary.Add(new Phrase("Designation: ", BookmanOldStylebold));
                    //    DataSalary.Add(new Chunk(" " + GetAllEmpdatils.Rows[iemp]["New Designation"].ToString(), BookmanOldStyle));
                    //    doc.Add(DataSalary);
                    //}
                    //else
                    //{
                    //    DataSalary = new Paragraph();
                    //    DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                    //    DataSalary.Add(new Phrase("Designation: ", BookmanOldStylebold));
                    //    DataSalary.Add(new Chunk(" " + GetAllEmpdatils.Rows[iemp]["Designation"].ToString(), BookmanOldStyle));
                    //    doc.Add(DataSalary);
                    //}

                    //DataSalary = new Paragraph();
                    //DataSalary.Alignment = Element.ALIGN_JUSTIFIED;
                    //DataSalary.Add(new Chunk(@"Department: ", BookmanOldStylebold));
                    //DataSalary.Add(new Chunk(GetAllEmpdatils.Rows[iemp]["Department"].ToString(), BookmanOldStyle));
                    //DataSalary.SpacingAfter = 10;
                    //doc.Add(DataSalary);


                    //DataSalary = new Paragraph(); DataSalary.Add(new Chunk(" ", BookmanOldStyle)); doc.Add(DataSalary);
                    //doc.Add(DataSalary);

                    
                    #endregion

                    #region Writer and doc Close section
                    doc.Close();
                    writer.Close();
                    #endregion Writer and doc Close section

                    //string emailcontent = @"<html>
                    //<body>
                    //<form>
                    //<p>Dear [empname] </br></p>
                    //<p>It gives us immense pleasure to announce our <b>Annual Appraisal</b>. After an assessment of your performance based on last year KPI’s and ratings from your reporting manager, management has decided to appraise you effective from 01.04.2023. </br></p>
                    //<p>Your Appraisal Letter and copy of revised pay structure is attached herewith for your reference. We wish you success towards the continual commitment and efforts in the growth of the organisation. Please feel free to contact the HR department for salary structure related queries. </br></p>
                    //<p>Once again, congratulations on your success and cheers to more good work in the future. </br></p>
                    //<p>We wish you the best!</br></p>
                    //<p>Regards,</br></p>
                    //<p>Vishal V Kulkarni</br> </p>
                    //<p>Human Resources and Administration</p> 
                    //</form>
                    //</body>
                    //</html>
                    //";
                    //temp commented
                    //if (GetAllEmpdatils.Rows[iemp]["EmailID"].ToString().Trim() != "")
                    //{

                    //    emailcontent = emailcontent.Replace("[empname]", GetAllEmpdatils.Rows[iemp]["Abbrivaton"].ToString() + " " + GetAllEmpdatils.Rows[iemp]["Emp_Name"].ToString());
                    //    mailSend.SendIncrementLetter(GetAllEmpdatils.Rows[iemp]["EmailID"].ToString(), emailcontent, "Annual Appraisal for the 2023-2024", pdfFileName);

                    //}
                    if (GetAllEmpdatils.Rows.Count > 0)
                    {
                        string relativeFilePath = "/files/" + Month + "_" + Year + "/" + GetAllEmpdatils.Rows[iemp]["Emp_Name"].ToString() + ".pdf";
                        string physicalFilePath = Request.MapPath(relativeFilePath);

                        User user = new User();
                        user.AddOrUpdatePayregisterEmployees(erpid, emp_abbrivation, employee_name, emp_email_id, department, hod_name, hod_email, relativeFilePath, 0);
                    }

                }
            }
            else
            {
                //CustomMSG1.ShowMessage("No Record For salary Slip", Util.CustMsg.Information);
                //strbuild.Append("No Record For salary Slip");
                //rbtndownloadzip.Visible = false;
                return;
            }
            #endregion
        }
        catch (Exception ex)
        {
            throw ex;
            // return;
        }
    }

    protected void btnEmpList_click(object sender, EventArgs e)
    {
        Response.Redirect("PayRegisterEmployeeList.aspx");
    }


    public partial class Footer : PdfPageEventHelper
    {
        private iTextSharp.text.Image waterMark;
        //public Footer(iTextSharp.text.Image img)
        public Footer()
        {
            // waterMark = img;
        }
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            #region Header Section

            // Get the image paths
            string imagePath1 = System.Web.HttpContext.Current.Server.MapPath("../Content/images/vtppl_logo_1.png");
            string imagePath2 = System.Web.HttpContext.Current.Server.MapPath("../Content/images/vtppl_logo_2.jpg");

            // Load images
            iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(imagePath1);
            iTextSharp.text.Image image2 = iTextSharp.text.Image.GetInstance(imagePath2);

            // Set image properties
            //image1.ScaleToFit(75f, 85f);
            //image2.ScaleToFit(100f, 110f);

            image1.ScaleToFit(50f, 60f);
            image2.ScaleToFit(75f, 85f);

            // Create a table with 3 columns
            PdfPTable tabletop = new PdfPTable(3);
            int[] columnwidthtop = { 33, 34, 33 };
            tabletop.SetWidths(columnwidthtop);
            tabletop.WidthPercentage = 100;
            tabletop.HorizontalAlignment = Element.ALIGN_LEFT;
            tabletop.SpacingBefore = 20f;

            PdfPCell imagecellColumnValue1 = new PdfPCell(image1);
            imagecellColumnValue1.HorizontalAlignment = Element.ALIGN_LEFT;
            imagecellColumnValue1.Border = 0;
            imagecellColumnValue1.PaddingLeft = 10f;
            imagecellColumnValue1.PaddingRight = 10f;
            imagecellColumnValue1.PaddingBottom = 20f;
            tabletop.AddCell(imagecellColumnValue1);


            PdfPCell titlecellColumnValue = new PdfPCell(new Phrase(@" ", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111))));
            titlecellColumnValue.HorizontalAlignment = Element.ALIGN_CENTER;
            titlecellColumnValue.Border = 0;
            tabletop.AddCell(titlecellColumnValue);

          
            PdfPCell imagecellColumnValue2 = new PdfPCell(image2);
            imagecellColumnValue2.HorizontalAlignment = Element.ALIGN_RIGHT;
            imagecellColumnValue2.Border = 0;
            imagecellColumnValue2.PaddingRight = 75f;
            imagecellColumnValue2.PaddingTop = 8f;
            imagecellColumnValue2.PaddingBottom = 30f; 
            tabletop.AddCell(imagecellColumnValue2);

            // Set the total width of the table and write it to the document
            tabletop.TotalWidth = 600;
            tabletop.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height - 10, writer.DirectContent);

            #endregion
        }
        //test

        public override void OnEndPage(PdfWriter writer, Document doc)
        {

            //Font BookmanOldStyle = FontFactory.GetFont("Bookman Old Style", 6, BaseColor.BLACK);

            //iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(System.Web.HttpContext.Current.Server.MapPath(@"../Content/images") + "\\Pdffooter.png");
            //image1.Alignment = Element.ALIGN_CENTER;
            //// set width and height
            //image1.ScaleToFit(600f, 500f);

            //PdfPTable footerTbl = new PdfPTable(1);

            //footerTbl.TotalWidth = 600;

            //footerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            //PdfPCell cell = new PdfPCell(image1);

            //cell.Border = 0;

            

            //footerTbl.AddCell(cell);

            //footerTbl.WriteSelectedRows(0, -1, 1, 90, writer.DirectContent);

            
        }
    }
}

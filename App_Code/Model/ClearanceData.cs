using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for ClearanceData
/// </summary>
public class ClearanceData
{
    public string empName { get; set; }
    public string designation { get; set; }
    public string department { get; set; }
    public string doj { get; set; }
    public string location { get; set; }
    public string dor { get; set; }
    public string dorelieving { get; set; }
    public string rm { get; set; }
    public string imgsource { get; set; }
    public string thankingName { get; set; }
    public string thankingdate { get; set; }
    public string remark { get; set; }
    public string erpid { get; set; }
    public string clearancePdfFileName { get; set; }

    public SQLDataFileSIS objSQLDataFile;
    public DataTable basicCleranceDetails;
    public DataTable assetCleranceDetails;
    public DataTable supereriorCleranceDetails;
    public DataTable deptNocCleranceDetails;

    public ClearanceData(int clearanceId)
    {
        objSQLDataFile = new SQLDataFileSIS();
        setBasicCleranceData(clearanceId);
        setAssetCleranceData(clearanceId);
        setSuperiorClearanceData(clearanceId);
        setDeptNocClearanceData(clearanceId);
        GenerateClearancePDF(clearanceId);
    }

    public void setBasicCleranceData(int clearanceId)
    {
        basicCleranceDetails = objSQLDataFile.ExecuteDataTableSQL(@"select resg.EmpId, emp.EmpName,emp.EmpEmailId,emp.EmpDesignation, emp.EmpDepartment, emp.DOJ, emp.ERPID, resg.DatenTime, resg.Remarks,
emp.RM, cf.DateOfResignation,
DateofRelivieng, ThankingSignIMGPATH, ThankingName, ThankingDate from EmployeeMaster emp 
join Resignations resg
on emp.Id = resg.EmpId
join [dbo].[ClearanceForm] cf
on resg.ResignationId = cf.ResignationId
where resg.ResignationId=" + clearanceId + " AND IsWithdrawn = 0");

        empName = basicCleranceDetails.Rows[0]["EmpName"].ToString();
        designation = basicCleranceDetails.Rows[0]["EmpDesignation"].ToString();
        department = basicCleranceDetails.Rows[0]["EmpDepartment"].ToString();
        doj = getDateInDateFormat(basicCleranceDetails.Rows[0]["DOJ"].ToString());
        rm = basicCleranceDetails.Rows[0]["RM"].ToString();
        dor = getDateInDateFormat(basicCleranceDetails.Rows[0]["DateOfResignation"].ToString());
        dorelieving = getDateInDateFormat(basicCleranceDetails.Rows[0]["DateofRelivieng"].ToString());
        imgsource = basicCleranceDetails.Rows[0]["ThankingSignIMGPATH"].ToString().Substring(1);
        thankingName = basicCleranceDetails.Rows[0]["ThankingName"].ToString();
        thankingdate = getDateInDateFormat(basicCleranceDetails.Rows[0]["ThankingDate"].ToString());
        remark = basicCleranceDetails.Rows[0]["Remarks"].ToString();
        erpid = basicCleranceDetails.Rows[0]["ERPID"].ToString();
    }

    public void setAssetCleranceData(int clearanceId)
    {
        assetCleranceDetails = objSQLDataFile.ExecuteDataTableSQL(@"select AssesstName, ReturnBackStatus, ReturnedToWhom from EmployeeMaster emp 
join Resignations resg
on emp.Id = resg.EmpId
join [dbo].[AssetChildClearance] ccf
on resg.ResignationId = ccf.ResignationId
join [dbo].[ClearanceForm] cf
on resg.ResignationId = cf.ResignationId
where resg.ResignationId=" + clearanceId + " AND IsWithdrawn = 0");
    }

    public DataTable getAssetCleranceData()
    {
        return assetCleranceDetails;
    }

    public void setSuperiorClearanceData(int clearanceId)
    {
        supereriorCleranceDetails = objSQLDataFile.ExecuteDataTableSQL(@"select DistributorName, IsNOCRecieved, BalanceStatus from EmployeeMaster emp 
join Resignations resg
on emp.Id = resg.EmpId
join [dbo].[DistributorChildClearance] ccf
on resg.ResignationId = ccf.ResignationId
join [dbo].[ClearanceForm] cf
on resg.ResignationId = cf.ResignationId
where (resg.ResignationId=" + clearanceId + " AND IsWithdrawn = 0) and (DistributorName is not null or DistributorName != '')");
    }

    public DataTable getSuperiorClearanceData()
    {
        return supereriorCleranceDetails;
    }

    public void setDeptNocClearanceData(int clearanceId)
    {
        deptNocCleranceDetails = objSQLDataFile.ExecuteDataTableSQL(@"select department_name, description, is_noc_recieved, amount_if_payable from EmployeeMaster emp 
join Resignations resg
on emp.Id = resg.EmpId
join [dbo].[ClearanceForm] cf
on resg.ResignationId = cf.ResignationId
join [dbo].[departmental_noc_child_clearance] dncc
on resg.ResignationId = dncc.ResignationId
where (resg.ResignationId=" + clearanceId + " AND IsWithdrawn = 0)");
    }

    public DataTable getDeptNocClearanceData()
    {
        return deptNocCleranceDetails;
    }

    public string getDateInDateFormat(string date)
    {
        if (!string.IsNullOrEmpty(date))
        {
            try
            {
                date = date.Substring(0, date.IndexOf(' '));
                DateTime dateTime = DateTime.Parse(date);
                date = dateTime.ToString("dd-MM-yyyy");
            }
            catch (Exception ex)
            {
                // Handle the exception if needed
            }
        }
        return date;
    }

    public void GenerateClearancePDF(int clearanceId)
    {

        #region File and folder create or delete sections
        string directoryPath = HttpContext.Current.Request.PhysicalApplicationPath + "\\clearance_files\\";
        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        string pdfFileName = directoryPath + empName + "-Clearance-" + timestamp + ".pdf";
        clearancePdfFileName = pdfFileName;

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        if (File.Exists(pdfFileName))
        {
            File.Delete(pdfFileName);
            //return;
        }
        System.Threading.Thread.Sleep(2000);
        #endregion
       
            Document document = new Document(PageSize.A4, 36f, 36f, 36f, 36f);//36f, 36f, 90f, 100f);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(pdfFileName, FileMode.CreateNew));
            document.Open();

            PdfContentByte contentByte = writer.DirectContent;
            contentByte.Rectangle(20f, 20f, PageSize.A4.Width - 40f, PageSize.A4.Height - 50f);
            contentByte.SetColorStroke(BaseColor.BLACK);
            contentByte.SetLineWidth(1f);
            contentByte.Stroke();

            Font BookmanOldStyle = FontFactory.GetFont("Bookman Old Style", 10f, Font.NORMAL, BaseColor.BLACK);
            Font BookmanOldStylebold = FontFactory.GetFont("Bookman Old Style", 10f, Font.BOLD, BaseColor.BLACK);
            Font BookmanOldStyleboldItalic = FontFactory.GetFont("Bookman Old Style", 10f, Font.BOLDITALIC, BaseColor.BLACK);

            Font titleFont = FontFactory.GetFont("Bookman Old Style", 16f, Font.BOLD, BaseColor.BLACK);

            Paragraph title = new Paragraph();
            title.Add(new Chunk("Clearance Form", titleFont));
            title.Alignment = Element.ALIGN_CENTER;
            document.Add(title);

            Paragraph empNameParagraph = new Paragraph();
            empNameParagraph.Add(new Phrase("Name: ", BookmanOldStyle));
            empNameParagraph.Add(new Chunk(empName, BookmanOldStylebold));
            document.Add(empNameParagraph);

            Paragraph designationP = new Paragraph();
            designationP.Add(new Phrase("Designation: ", BookmanOldStyle));
            designationP.Add(new Chunk(" " + designation, BookmanOldStylebold));
            document.Add(designationP);

            Paragraph ErpId = new Paragraph();
            ErpId.Add(new Phrase("ERPNext Id: ", BookmanOldStyle));
            ErpId.Add(new Chunk(" " + erpid, BookmanOldStylebold));
            document.Add(ErpId);

            Paragraph departmentParagraph = new Paragraph();
            departmentParagraph.Add(new Phrase("Department: ", BookmanOldStyle));
            departmentParagraph.Add(new Chunk(" " + department, BookmanOldStylebold));
            document.Add(departmentParagraph);

            Paragraph dojParagraph = new Paragraph();
            dojParagraph.Add(new Phrase("Date of Joining: ", BookmanOldStyle));
            dojParagraph.Add(new Chunk(" " + doj, BookmanOldStylebold));
            document.Add(dojParagraph);

            Paragraph locationParagraph = new Paragraph();
            locationParagraph.Add(new Phrase("Location: ", BookmanOldStyle));
            locationParagraph.Add(new Chunk(" " + location, BookmanOldStylebold));
            document.Add(locationParagraph);

            Paragraph dorParagraph = new Paragraph();
            dorParagraph.Add(new Phrase("Date of Resignation: ", BookmanOldStyle));
            dorParagraph.Add(new Chunk(" " + dor, BookmanOldStylebold));
            document.Add(dorParagraph);

            Paragraph dorelievingParagraph = new Paragraph();
            dorelievingParagraph.Add(new Phrase("Date of Relieving: ", BookmanOldStyle));
            dorelievingParagraph.Add(new Chunk(" " + dorelieving, BookmanOldStylebold));
            document.Add(dorelievingParagraph);

            Paragraph rmParagraph = new Paragraph();
            rmParagraph.Add(new Phrase("Reporting to: ", BookmanOldStyle));
            rmParagraph.Add(new Chunk(" " + rm, BookmanOldStylebold));
            document.Add(rmParagraph);

            Paragraph remarks = new Paragraph();
            remarks.Add(new Phrase("Remarks: ", BookmanOldStyle));
            remarks.Add(new Chunk(" " + remark, BookmanOldStylebold));
            document.Add(remarks);

            Paragraph spacingAfterTable = new Paragraph(new Chunk(""));
            spacingAfterTable.SpacingAfter = 20f;
            document.Add(spacingAfterTable);


            BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font headerFont = new Font(baseFont, 10, Font.NORMAL, BaseColor.WHITE);
            Font normalFont = new Font(baseFont, 10, Font.NORMAL, BaseColor.BLACK);
            BaseColor tableHeaderColor = new BaseColor(89, 89, 89);

            if (assetCleranceDetails.Rows.Count > 0)
            {
                PdfPTable assetDataTable = new PdfPTable(3);
                assetDataTable.WidthPercentage = 100;

                PdfPCell headerCell;
                headerCell = new PdfPCell(new Phrase("Asset Name", headerFont));
                headerCell.BackgroundColor = tableHeaderColor;
                assetDataTable.AddCell(headerCell);

                headerCell = new PdfPCell(new Phrase("Return Back Status", headerFont));
                headerCell.BackgroundColor = tableHeaderColor;
                assetDataTable.AddCell(headerCell);

                headerCell = new PdfPCell(new Phrase("Returned To Whom", headerFont));
                headerCell.BackgroundColor = tableHeaderColor;
                assetDataTable.AddCell(headerCell);

                bool isAlternateRow = false;
                if (assetCleranceDetails != null)
                {
                    foreach (DataRow row in assetCleranceDetails.Rows)
                    {
                        PdfPCell dataCell;
                        dataCell = new PdfPCell(new Phrase(row["AssesstName"].ToString(), normalFont));
                        dataCell.BackgroundColor = isAlternateRow ? BaseColor.LIGHT_GRAY : BaseColor.WHITE;
                        assetDataTable.AddCell(dataCell);

                        dataCell = new PdfPCell(new Phrase(row["ReturnBackStatus"].ToString(), normalFont));
                        dataCell.BackgroundColor = isAlternateRow ? BaseColor.LIGHT_GRAY : BaseColor.WHITE;
                        assetDataTable.AddCell(dataCell);

                        dataCell = new PdfPCell(new Phrase(row["ReturnedToWhom"].ToString(), normalFont));
                        dataCell.BackgroundColor = isAlternateRow ? BaseColor.LIGHT_GRAY : BaseColor.WHITE;
                        assetDataTable.AddCell(dataCell);

                        isAlternateRow = !isAlternateRow;
                    }
                    document.Add(assetDataTable);

                }

                Paragraph spacingAfterAssetTable = new Paragraph();
                spacingAfterAssetTable.Add(new Chunk("\n"));
                document.Add(spacingAfterAssetTable);
            }
            if (supereriorCleranceDetails.Rows.Count > 0)
            {
                PdfPTable supiriorClearanceDataTable = new PdfPTable(3);
                supiriorClearanceDataTable.WidthPercentage = 100;

                // Add headers with colored background
                PdfPCell headerCell;
                headerCell = new PdfPCell(new Phrase("Distributor Name", headerFont));
                headerCell.BackgroundColor = tableHeaderColor;
                supiriorClearanceDataTable.AddCell(headerCell);

                headerCell = new PdfPCell(new Phrase("Is NOC Received", headerFont));
                headerCell.BackgroundColor = tableHeaderColor;
                supiriorClearanceDataTable.AddCell(headerCell);

                headerCell = new PdfPCell(new Phrase("Balance Status", headerFont));
                headerCell.BackgroundColor = tableHeaderColor;
                supiriorClearanceDataTable.AddCell(headerCell);

                // Add data rows with alternating background color
                bool isAlternateRow = false;
                foreach (DataRow row in supereriorCleranceDetails.Rows)
                {
                    PdfPCell dataCell;
                    dataCell = new PdfPCell(new Phrase(row["DistributorName"].ToString(), normalFont));
                    dataCell.BackgroundColor = isAlternateRow ? BaseColor.LIGHT_GRAY : BaseColor.WHITE;
                    supiriorClearanceDataTable.AddCell(dataCell);

                    dataCell = new PdfPCell(new Phrase(row["IsNOCRecieved"].ToString(), normalFont));
                    dataCell.BackgroundColor = isAlternateRow ? BaseColor.LIGHT_GRAY : BaseColor.WHITE;
                    supiriorClearanceDataTable.AddCell(dataCell);

                    dataCell = new PdfPCell(new Phrase(row["BalanceStatus"].ToString(), normalFont));
                    dataCell.BackgroundColor = isAlternateRow ? BaseColor.LIGHT_GRAY : BaseColor.WHITE;
                    supiriorClearanceDataTable.AddCell(dataCell);

                    isAlternateRow = !isAlternateRow;
                }

                // Add the supiriorClearanceDataTable to the PDF
                document.Add(supiriorClearanceDataTable);

                Paragraph spacingAfterSupiriorClearanceTable = new Paragraph();
                spacingAfterSupiriorClearanceTable.Add(new Chunk("\n")); // Add an empty line
                document.Add(spacingAfterSupiriorClearanceTable);
            }

            if (deptNocCleranceDetails.Rows.Count > 0)
            {
                PdfPTable deptNocDataTable = new PdfPTable(4);
                deptNocDataTable.WidthPercentage = 100;

                PdfPCell headerCell;
                headerCell = new PdfPCell(new Phrase("Department Name", headerFont));
                headerCell.BackgroundColor = tableHeaderColor;
                deptNocDataTable.AddCell(headerCell);

                headerCell = new PdfPCell(new Phrase("Description", headerFont));
                headerCell.BackgroundColor = tableHeaderColor;
                deptNocDataTable.AddCell(headerCell);

                headerCell = new PdfPCell(new Phrase("Is NOC Received", headerFont));
                headerCell.BackgroundColor = tableHeaderColor;
                deptNocDataTable.AddCell(headerCell);

                headerCell = new PdfPCell(new Phrase("Amount", headerFont));
                headerCell.BackgroundColor = tableHeaderColor;
                deptNocDataTable.AddCell(headerCell);

                bool isAlternateRow = false;
                foreach (DataRow row in deptNocCleranceDetails.Rows)
                {
                    PdfPCell dataCell;
                    dataCell = new PdfPCell(new Phrase(row["department_name"].ToString(), normalFont));
                    dataCell.BackgroundColor = isAlternateRow ? BaseColor.LIGHT_GRAY : BaseColor.WHITE;
                    deptNocDataTable.AddCell(dataCell);

                    dataCell = new PdfPCell(new Phrase(row["description"].ToString(), normalFont));
                    dataCell.BackgroundColor = isAlternateRow ? BaseColor.LIGHT_GRAY : BaseColor.WHITE;
                    deptNocDataTable.AddCell(dataCell);

                    dataCell = new PdfPCell(new Phrase(row["is_noc_recieved"].ToString(), normalFont));
                    dataCell.BackgroundColor = isAlternateRow ? BaseColor.LIGHT_GRAY : BaseColor.WHITE;
                    deptNocDataTable.AddCell(dataCell);

                    dataCell = new PdfPCell(new Phrase(row["amount_if_payable"].ToString(), normalFont));
                    dataCell.BackgroundColor = isAlternateRow ? BaseColor.LIGHT_GRAY : BaseColor.WHITE;
                    deptNocDataTable.AddCell(dataCell);

                    isAlternateRow = !isAlternateRow;

                }
                document.Add(deptNocDataTable);

                Paragraph spacingAfterDeptNocTable = new Paragraph();
                spacingAfterDeptNocTable.Add(new Chunk("\n"));
                document.Add(spacingAfterDeptNocTable);
            }

            Font boldFont = FontFactory.GetFont("Bookman Old Style", 10f, Font.BOLD, BaseColor.BLACK);

            Paragraph thankingYouParagraph = new Paragraph();
            thankingYouParagraph.Add(new Phrase("I herewith confirm that there is nothing pending with Mr./Ms. ", BookmanOldStyle));
            thankingYouParagraph.Add(new Chunk(empName, boldFont));
            thankingYouParagraph.Add(new Phrase(" other than mentioned above.", BookmanOldStyle));

            document.Add(thankingYouParagraph);

            Paragraph spacingTable = new Paragraph();
            spacingTable.Add(new Chunk("\n"));
            document.Add(spacingTable);

            //Paragraph thankingSignParagraph = new Paragraph();
            //thankingSignParagraph.Add(new Phrase("Thanking Signature: ", BookmanOldStyle));
            //document.Add(thankingSignParagraph);

            Paragraph spacingImage = new Paragraph();
            spacingImage.Add(new Chunk("\n"));
            document.Add(spacingImage);

            PdfPTable tablesingnature = new PdfPTable(1);
            int[] columnwidthtop = { 100 };
            tablesingnature.SetWidths(columnwidthtop);
            tablesingnature.WidthPercentage = 100;
            tablesingnature.HorizontalAlignment = 0;

            imgsource = "~" + imgsource;

            string physicalImagePath = HttpContext.Current.Server.MapPath(imgsource);
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(physicalImagePath);
            image.Alignment = Element.ALIGN_LEFT;
            image.ScaleToFit(75f, 95f);

            PdfPCell imagecellColumnValue1 = new PdfPCell(image);
            imagecellColumnValue1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            imagecellColumnValue1.Border = 0;
            tablesingnature.AddCell(imagecellColumnValue1);

            document.Add(tablesingnature);

            //Paragraph spacingAfterImage = new Paragraph();
            //spacingAfterImage.Add(new Chunk("\n"));
            //document.Add(spacingAfterImage);

            Paragraph thankingNameParagraph = new Paragraph();
            thankingNameParagraph.Add(new Phrase("Thanking Name: ", BookmanOldStyle));
            thankingNameParagraph.Add(new Chunk(" " + thankingName, BookmanOldStylebold));
            document.Add(thankingNameParagraph);

            Paragraph thankingDateParagraph = new Paragraph();
            thankingDateParagraph.Add(new Phrase("Thanking Date: ", BookmanOldStyle));
            thankingDateParagraph.Add(new Chunk(" " + thankingdate, BookmanOldStylebold));
            document.Add(thankingDateParagraph);

            document.Close();

            //byte[] pdfBytes = memoryStream.ToArray();

            //Response.ContentType = "application/pdf";
            //Response.AppendHeader("Content-Disposition", "attachment; filename=GeneratedPDF.pdf");
            //Response.OutputStream.Write(pdfBytes, 0, pdfBytes.Length);
            //Response.Flush();
            //Response.End();
        }

}
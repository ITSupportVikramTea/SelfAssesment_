using ClosedXML.Excel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_AssingedSkillsList : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    static DataTable dtemplist;
    bool hideAssignedDescription = false;
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
            ViewState["HideAssignedDescription"] = true;
            bindAssingSkillList(year);
        }
    }

    private void ApplyDescriptionToggle()
    {
        bool hideDesc = ViewState["HideAssignedDescription"] != null &&
                        Convert.ToBoolean(ViewState["HideAssignedDescription"]);

        // column visibility
        AssingSkillList.Columns[3].Visible = !hideDesc;

        // same CSS logic as button click
        if (hideDesc)
        {
            AssingSkillList.Attributes["class"] =
                "table table-bordered";

            AssingSkillList.AllowPaging = false;
        }
        else
        {
            AssingSkillList.Attributes["class"] =
                "table table-bordered";

            AssingSkillList.AllowPaging = true;
            AssingSkillList.PageSize = 10;
        }
    }

    protected void btnToggleDescriptionAssigned_Click(
    object sender,
    EventArgs e)
    {
        bool isVisible =
            AssingSkillList.Columns[3].Visible;

        AssingSkillList.Columns[3].Visible =
            !isVisible;

        hideAssignedDescription =
            !AssingSkillList.Columns[3].Visible;

        ViewState["HideAssignedDescription"] =
            hideAssignedDescription;

        // nowrap logic
        if (AssingSkillList.Columns[3].Visible)
        {
            AssingSkillList.Attributes["class"] =
    "table table-bordered table-striped";
        }
        else
        {
            AssingSkillList.Attributes["class"] =
     "table table-bordered table-striped";
        }

        // paging logic
        if (!AssingSkillList.Columns[3].Visible)
        {
            AssingSkillList.AllowPaging = false;
        }
        else
        {
            AssingSkillList.AllowPaging = true;
            AssingSkillList.PageSize = 10;
        }

        string year = dlyear.SelectedValue;

        bindAssingSkillList(year);
    }


    public void binddlyears()
    {
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"select distinct Year from [dbo].[SkillsUserwise] order by Year desc");
        dlyear.DataSource = dt;
        dlyear.DataTextField = "Year";
        dlyear.DataTextField = "Year";
        dlyear.DataBind();
    }
    protected void bindAssingSkillList(string year)
    {

        string str = txtsearchskill.Text.Trim();
        DataTable dt;
        if (str == "")
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"select suw.Id, EmpName, Competency, ManagementSkills, Description,suw.year from [dbo].[SkillsUserwise] suw
                                                            left join [dbo].[EmployeeMaster] emp on suw.EmpId = emp.Id
															WHERE suw.Year = " + year);
        }
        else
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"select suw.Id, EmpName, Competency, ManagementSkills, Description,suw.year from [dbo].[SkillsUserwise] suw
                                                            left join [dbo].[EmployeeMaster] emp on suw.EmpId = emp.Id where (EmpName like '%" + str + "%' or Competency like '%" + str + "%') and suw.Year = " + year);
        }

        AssingSkillList.DataSource = dt;
        ApplyDescriptionToggle();
        AssingSkillList.DataBind();
        MergeAssignedSkillRows();
    }

    private void MergeAssignedSkillRows()
    {
        for (int rowIndex = AssingSkillList.Rows.Count - 2; rowIndex >= 0; rowIndex--)
        {
            GridViewRow row = AssingSkillList.Rows[rowIndex];
            GridViewRow previousRow = AssingSkillList.Rows[rowIndex + 1];

            string currentEmp =
                row.Cells[0].Text.Trim();

            string nextEmp =
                previousRow.Cells[0].Text.Trim();

            string currentCompetency =
                row.Cells[1].Text.Trim();

            string nextCompetency =
                previousRow.Cells[1].Text.Trim();

            string currentManagement =
                row.Cells[2].Text.Trim();

            string nextManagement =
                previousRow.Cells[2].Text.Trim();

            // =========================
            // SAME GROUP
            // =========================

            bool hideDesc =
      ViewState["HideAssignedDescription"] != null &&
      Convert.ToBoolean(
          ViewState["HideAssignedDescription"]);

            if (currentEmp == nextEmp &&
                currentCompetency == nextCompetency &&
                currentManagement == nextManagement)
            {
                MergeAssignedCell(row, previousRow, 0);

                MergeAssignedCell(row, previousRow, 1);

                MergeAssignedCell(row, previousRow, 2);

                MergeAssignedCell(row, previousRow, 4);

                MergeAssignedCell(row, previousRow, 5);

                // hide description -> single row feel
                if (hideDesc)
                {
                    MergeAssignedCell(row, previousRow, 3);
                }
            }
        }
    }

    private void MergeAssignedCell(
    GridViewRow row,
    GridViewRow previousRow,
    int cellIndex)
    {
        if (previousRow.Cells[cellIndex].RowSpan < 2)
        {
            row.Cells[cellIndex].RowSpan = 2;
        }
        else
        {
            row.Cells[cellIndex].RowSpan =
                previousRow.Cells[cellIndex].RowSpan + 1;
        }

        previousRow.Cells[cellIndex].Visible = false;

        // center align
        row.Cells[cellIndex].Style["vertical-align"] =
            "middle";

        row.Cells[cellIndex].Style["text-align"] =
            "center";
    }
    protected void Search(object sender, EventArgs e)
    {
        string year = dlyear.SelectedValue;
        bindAssingSkillList(year);
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (FileUpload1.PostedFile != null)
        {
            //int res = 0, once = 0;
            int res = 0, duplicates = 0, assigned = 0;
            string filePath = Server.MapPath("../UploadFile/"); ;
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss");
            filePath = filePath + fileName + ".xlsx";
            FileUpload1.PostedFile.SaveAs(filePath);


            XLWorkbook xLWorkbook = new XLWorkbook(filePath);
            int row = 2;
            while (xLWorkbook.Worksheets.Worksheet(1).Cell(row, 1).GetString() != "")
            {
                try
                {
                    string EmpId = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 1).GetString();
                    string Competency = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 2).GetString();
                    string ManagementSkills = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 3).GetString();
                    string Description = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 4).GetString();

                    User addusdr = new User();

                    string chName = "";
                    try
                    {

                        DataTable chk = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT EmpName from EmployeeMaster WHERE Id = " + EmpId + "");
                        DataRow row2 = chk.Rows[0];
                        chName = row2["EmpName"].ToString();
                    }
                    catch (Exception ex)
                    {

                    }
                    if (chName != "")
                    {
                        int result = addusdr.AddSkillsUW(EmpId, Competency, ManagementSkills, Description, Session["EmailId"].ToString());

                        if (result == -1) // Duplicate skill entry
                        {
                            duplicates++;
                        }
                        else if (result == -2) // Assigned skill
                        {
                            assigned++;
                        }
                        else if (result > 0) // Successful insert
                        {
                            res++;
                        }
                    }
                }
                catch
                {
                   // once = 1;
                }

                row++;
            }

            string message = "";
            if (duplicates > 0)
            {
                message += "There were " + duplicates + " duplicate skill entries.";
            }
            if (assigned > 0)
            {
                message += "One or more skills were already assigned.";
            }
           

            if (res > 0)
            {
                message += "File uploaded successfully!";
                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>if(window.confirm('"+ message + "')){window.location.href='AssingedSkillsList.aspx'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
            }
            else
            {
                message = "There was an error uploading the file," + message;
                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>if(window.confirm('"+ message + "')){window.location.href='AssingedSkillsList.aspx'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
            }

        }

        //test code 

       /*
        if (FileUpload1.PostedFile != null)
        {
            int res = 0, duplicates = 0, assigned = 0;
            string filePath = Server.MapPath("../UploadFile/"); ;
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss");
            filePath = filePath + fileName + ".xlsx";
            FileUpload1.PostedFile.SaveAs(filePath);

            XLWorkbook xLWorkbook = new XLWorkbook(filePath);
            int row = 2;
            while (xLWorkbook.Worksheets.Worksheet(1).Cell(row, 1).GetString() != "")
            {
                try
                {
                    string EmpId = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 1).GetString();
                    string Competency = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 2).GetString();
                    string ManagementSkills = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 3).GetString();
                    string Description = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 4).GetString();

                    User addusdr = new User();

                    string chName = "";
                    try
                    {

                        DataTable chk = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT EmpName from EmployeeMaster WHERE Id = " + EmpId + "");
                        DataRow row2 = chk.Rows[0];
                        chName = row2["EmpName"].ToString();
                    }
                    catch (Exception ex)
                    {

                    }
                    if (chName != "")
                    {
                        int result = addusdr.AddSkillsUW(EmpId, Competency, ManagementSkills, Description, Session["EmailId"].ToString());
                        if (result == -1) // Duplicate skill entry
                        {
                            duplicates++;
                        }
                        else if (result == -2) // Assigned skill
                        {
                            assigned++;
                        }
                        else if (result > 0) // Successful insert
                        {
                            res++;
                        }
                    }
                }
                catch
                {
                    // handle exception
                }

                row++;
            }

            string message = "";
            if (duplicates > 0)
            {
                message += "There were " + duplicates + " duplicate skill entries.\n";
            }
            if (assigned > 0)
            {
                message += "One or more skills were already assigned.\n";
            }

            if (res > 0)
            {
                message += "File uploaded successfully!";
                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>if(window.confirm('" + message + "')){window.location.href='AssingedSkillsList.aspx'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);

            }
            else
            {
                message = "There was an error uploading the file.\n" + message;
                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>if(window.confirm('" + message + "')){window.location.href='AssingedSkillsList.aspx'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);




                /*ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>if(window.confirm('There is some Error, File Not Uploaded')){window.location.href='AssingedSkillsList.aspx'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);*/


            
        }
    


    

    protected void btnSample_Click(object sender, EventArgs e)
    {

        DataTable dt = new DataTable("Grid");
        dt.Columns.AddRange(new DataColumn[4] {
            new DataColumn("Emp Id"),
            new DataColumn("Competency"),
            new DataColumn("ManagementSkills"),
            new DataColumn("Description")
            });
        dt.Rows.Add("1", "Better", "7", "Better in RND");
        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dt);
            using (MemoryStream stream = new MemoryStream())
            {
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=SkillsUserWise.xlsx");
                wb.SaveAs(stream);
                stream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }
    protected void btnassgnskills_click(object sender, EventArgs e)
    {
        Response.Redirect("AddSkillsUserwise.aspx");
    }
    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        AssingSkillList.PageIndex = e.NewPageIndex;
        string year = dlyear.SelectedValue;

        bindAssingSkillList(year);
    }
    protected void AssingSkillList_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void AssingSkillList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Text = Regex.Replace(e.Row.Cells[0].Text, txtsearchskill.Text.Trim(), delegate (Match match)
            {
                return string.Format("<span style = 'background-color:#D9EDF7'>{0}</span>", match.Value);
            }, RegexOptions.IgnoreCase);
        }
    }

    //added to forward previous year data to current year
    protected void btnForPrev_Click(object sender, EventArgs e)
    {
        try
        {

            string message = "";
            string created_by = Session["EmailId"].ToString();
            DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"select * from SkillsUserwise where year = YEAR(GETDATE())");

            if (dt.Rows.Count > 0) //it means current year data is already available
            {
                message = "Data already forwarded!";
            }
            else
            {

                //test query to forward data of those employees who have currently active status

                objSQLDataFile.ExecuteDataTableSQL(@"INSERT INTO SkillsUserwise( EmpId, Competency, ManagementSkills, Description)
                SELECT DISTINCT EmpId, Competency, ManagementSkills, Description
FROM SkillsUserwise suw
JOIN EmployeeMaster em ON suw.EmpId = em.Id 
WHERE suw.Year = YEAR(GETDATE()) - 1
AND em.Status = 'Active'
AND em.Id IS NOT NULL");

              //old query

              /*  objSQLDataFile.ExecuteDataTableSQL(@"INSERT INTO SkillsUserwise( EmpId, Competency, ManagementSkills, Description)
            SELECT distinct EmpId, Competency, ManagementSkills, Description
            FROM SkillsUserwise suw
            WHERE suw.Year = YEAR(GETDATE()) - 1");*/

                message = "Previous data forwarded!";

            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script>alert('");
            sb.Append(message);
            sb.Append("'); </script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void dlyears_SelectedIndexChanged(object sender, EventArgs e)
    {
        string year = dlyear.SelectedValue;
        bindAssingSkillList(year);
    }

  
       protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        string year = dlyear.SelectedValue;
        string er = "";
        if (string.IsNullOrEmpty(year))
        {
            er = "Please select a year.";
          
            return;
        }

        DataTable dt = null;

        try
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"
            SELECT
                emp.EmpName,
                suw.Competency,
                suw.ManagementSkills,
                suw.Year,
                suw.Description,
                MAX(suw.RatingSelf) OVER (
                    PARTITION BY suw.EmpId, suw.Competency,
                                 suw.ManagementSkills, suw.Year
                ) AS RatingSelf,
                MAX(suw.RMRating) OVER (
                    PARTITION BY suw.EmpId, suw.Competency,
                                 suw.ManagementSkills, suw.Year
                ) AS RMRating,
                COUNT(suw.Description) OVER (
                    PARTITION BY suw.EmpId, suw.Competency,
                                 suw.ManagementSkills, suw.Year
                ) AS DescCount,
                ROW_NUMBER() OVER (
                    PARTITION BY suw.EmpId, suw.Competency,
                                 suw.ManagementSkills, suw.Year
                    ORDER BY suw.Description
                ) AS RowNum
            FROM SkillsUserwise suw
            LEFT JOIN EmployeeMaster emp
                ON suw.EmpId = emp.Id
            WHERE suw.Year = " + Convert.ToInt32(year) + @"
            ORDER BY
                emp.EmpName,
                suw.Competency,
                suw.ManagementSkills,
                RowNum");
        }
        catch (Exception ex)
        {
            er = "SQL ERROR: " + ex.Message;
           
            return;
        }

        if (dt == null || dt.Rows.Count == 0)
        {
            er = "No data found for selected year.";
            
            return;
        }

        ExcelPackage.License.SetNonCommercialOrganization(
            "My Noncommercial organization");

        using (ExcelPackage pkg = new ExcelPackage())
        {
            ExcelWorksheet ws = pkg.Workbook.Worksheets.Add("Skills Report");

            ws.Cells.Style.Font.Name = "Calibri";
            ws.Cells.Style.Font.Size = 11;

            // ── Headers ───────────────────────────────────────────────────
            string[] headers = new string[]
            {
            "Employee Name",
            "Competency",
            "Management Skills",
            "Description",
            "Self Rating",
            "RM Rating",
            "Year"
            };

            for (int h = 0; h < headers.Length; h++)
                ws.Cells[1, h + 1].Value = headers[h];

            using (ExcelRange headerRange = ws.Cells[1, 1, 1, 7])
            {
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Font.Color.SetColor(
                    System.Drawing.Color.White);
                headerRange.Style.Fill.PatternType =
                    OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerRange.Style.Fill.BackgroundColor.SetColor(
                    System.Drawing.Color.FromArgb(30, 41, 59));
                headerRange.Style.HorizontalAlignment =
                    OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                headerRange.Style.VerticalAlignment =
                    OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            }

            // ── Data rows ─────────────────────────────────────────────────
            int excelRow = 2;
            bool isAltGroup = false;
            string lastKey = "";

            foreach (DataRow row in dt.Rows)
            {
                int rowNum = Convert.ToInt32(row["RowNum"]);
                int descCount = Convert.ToInt32(row["DescCount"]);

                string empName = row["EmpName"] == DBNull.Value
                    ? "" : row["EmpName"].ToString();
                string competency = row["Competency"] == DBNull.Value
                    ? "" : row["Competency"].ToString();
                string mgmtSkills = row["ManagementSkills"] == DBNull.Value
                    ? "" : row["ManagementSkills"].ToString();
                string desc = row["Description"] == DBNull.Value
                    ? "" : "• " + row["Description"].ToString().Trim();
                string selfRating = row["RatingSelf"] == DBNull.Value
                    ? "" : row["RatingSelf"].ToString();
                string rmRating = row["RMRating"] == DBNull.Value
                    ? "" : row["RMRating"].ToString();
                string yearVal = row["Year"] == DBNull.Value
                    ? "" : row["Year"].ToString();

                // ── Alternate background per group ────────────────────────
                string groupKey = empName + "|" + competency + "|" + mgmtSkills;
                if (groupKey != lastKey)
                {
                    isAltGroup = !isAltGroup;
                    lastKey = groupKey;
                }

                System.Drawing.Color bgColor = isAltGroup
                    ? System.Drawing.Color.FromArgb(241, 245, 249)
                    : System.Drawing.Color.White;

                // ── First row of group: write + merge ─────────────────────
                if (rowNum == 1)
                {
                    int endRow = excelRow + descCount - 1;

                    ws.Cells[excelRow, 1].Value = empName;
                    ws.Cells[excelRow, 2].Value = competency;
                    ws.Cells[excelRow, 3].Value = mgmtSkills;
                    ws.Cells[excelRow, 5].Value = selfRating;
                    ws.Cells[excelRow, 6].Value = rmRating;
                    ws.Cells[excelRow, 7].Value = yearVal;

                    // Merge vertically if multiple descriptions
                    if (descCount > 1)
                    {
                        ws.Cells[excelRow, 1, endRow, 1].Merge = true;
                        ws.Cells[excelRow, 2, endRow, 2].Merge = true;
                        ws.Cells[excelRow, 3, endRow, 3].Merge = true;
                        ws.Cells[excelRow, 5, endRow, 5].Merge = true;
                        ws.Cells[excelRow, 6, endRow, 6].Merge = true;
                        ws.Cells[excelRow, 7, endRow, 7].Merge = true;
                    }

                    // Center + middle align cols 1-3
                    using (ExcelRange r = ws.Cells[excelRow, 1, endRow, 3])
                    {
                        r.Style.HorizontalAlignment =
                            OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        r.Style.VerticalAlignment =
                            OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        r.Style.Font.Bold = true;
                    }

                    // Center + middle align cols 5-7
                    using (ExcelRange r = ws.Cells[excelRow, 5, endRow, 7])
                    {
                        r.Style.HorizontalAlignment =
                            OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        r.Style.VerticalAlignment =
                            OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    }

                    // Background color across full group span
                    using (ExcelRange r = ws.Cells[excelRow, 1, endRow, 7])
                    {
                        r.Style.Fill.PatternType =
                            OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(bgColor);
                    }
                }

                // ── Description: written every row ────────────────────────
                ws.Cells[excelRow, 4].Value = desc;

                using (ExcelRange r = ws.Cells[excelRow, 4])
                {
                    r.Style.VerticalAlignment =
                        OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                    r.Style.WrapText = true;
                }

                excelRow++;
            }

            // ── Column widths ─────────────────────────────────────────────
            ws.Column(1).Width = 25;
            ws.Column(2).Width = 25;
            ws.Column(3).Width = 35;
            ws.Column(4).Width = 60;
            ws.Column(5).Width = 15;
            ws.Column(6).Width = 15;
            ws.Column(7).Width = 12;

            // ── Borders on full range in one call ─────────────────────────
            int lastRow = excelRow - 1;

            using (ExcelRange dataRange = ws.Cells[1, 1, lastRow, 7])
            {
                dataRange.Style.Border.Top.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                dataRange.Style.Border.Bottom.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                dataRange.Style.Border.Left.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                dataRange.Style.Border.Right.Style =
                    OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            }

            ws.View.FreezePanes(2, 1);

            // ── Download ──────────────────────────────────────────────────
            Response.Clear();
            Response.Buffer = false;
            Response.Charset = "";
            Response.ContentType =
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader(
                "content-disposition",
                "attachment;filename=AssignedSkillsReport.xlsx");

            Response.BinaryWrite(pkg.GetAsByteArray());
            Response.Flush();
            Response.End();
        }
    }
}



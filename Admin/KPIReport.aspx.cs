using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_KPIReport : System.Web.UI.Page
{
    string conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGrid();
        }

    }

    protected void FilterChanged(object sender, EventArgs e)
    {
        BindGrid();
    }

    private void BindGrid()
    {
        DataTable dt = GetFilteredData();
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }

    private DataTable GetFilteredData()
    {
        using (SqlConnection con = new SqlConnection(conStr))
        {
            string query = @"SELECT 
                                Id, EmpId, Competency, ManagementSkills,
                                Description, RatingSelf, RMRating,
                                HODRating, ConsentRating, IdealRating,
                                OverallScore, CreatedBy, [Year]
                             FROM SkillsUserwise
                             WHERE 1=1";


            if (!string.IsNullOrEmpty(ddlYear.SelectedValue))
            {
                query += " AND [Year] = @Year";
            }


            if (!string.IsNullOrEmpty(ddlStatus.SelectedValue))
            {
                if (ddlStatus.SelectedValue == "EMP")
                {
                    query += " AND (RatingSelf IS NULL OR RatingSelf = '')";
                }
                else if (ddlStatus.SelectedValue == "RM")
                {
                    query += " AND (RatingSelf IS NOT NULL AND RatingSelf <> '') AND (RMRating IS NULL OR RMRating = '')";
                }
            }

            SqlCommand cmd = new SqlCommand(query, con);

            if (!string.IsNullOrEmpty(ddlYear.SelectedValue))
            {
                cmd.Parameters.AddWithValue("@Year", ddlYear.SelectedValue);
            }

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            return dt;
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        DataTable dt = GetFilteredData();

        using (XLWorkbook wb = new XLWorkbook())
        {
            var ws = wb.Worksheets.Add(dt, "Skills Report");


            ws.Row(1).Style.Font.Bold = true;
            ws.Row(1).Style.Fill.BackgroundColor = XLColor.LightGray;

            ws.Columns().AdjustToContents();


            var table = ws.RangeUsed().CreateTable();
            table.Theme = XLTableTheme.TableStyleMedium9;

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=SkillsReport.xlsx");

            using (MemoryStream ms = new MemoryStream())
            {
                wb.SaveAs(ms);
                ms.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }

}
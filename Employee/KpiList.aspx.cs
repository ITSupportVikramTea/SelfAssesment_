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


public partial class Admin_KpiList : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    static DataTable dtemplist;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["EmailId"] == null || Session["UserType"].ToString().ToLower() != "employeehr")
            {
                Response.Redirect("../Default.aspx");
            }
            binddlyears();
            bindKPIList();
        }

    }
    public void binddlyears()
    {
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"select distinct Year from [dbo].[KRAUserwise] order by Year desc");
        dlyear.DataSource = dt;
        dlyear.DataTextField = "Year";
        dlyear.DataTextField = "Year";
        dlyear.DataBind();
    }

    protected void bindKPIList(string year = "YEAR(GETDATE())")
    {

        string str = txtsearchkpi.Text.Trim();
        DataTable dt;
        if (str == "")
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"select kuw.Id,EmpName, KPI,Target, Weightage,Year from [dbo].[KRAUserwise] kuw
LEFT JOIN [dbo].[EmployeeMaster] emp on kuw.EmpId = emp.Id WHERE kuw.Year = " + year);
        }
        else
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"select kuw.Id,EmpName, KPI,Target, Weightage,Year from [dbo].[KRAUserwise] kuw
LEFT JOIN [dbo].[EmployeeMaster] emp on kuw.EmpId = emp.Id where KPI like'%" + str + "%' or  EmpName like'%" + str + "%' and kuw.Year = " + year );
        }

        KPIList.DataSource = dt;
        KPIList.DataBind();
    }
    protected void Search(object sender, EventArgs e)
    {
        string year = dlyear.SelectedValue;
        this.bindKPIList(year);
        //this.bindKPIList();
    }
    protected void btnaddkra_click(object sender, EventArgs e)
    {
        Response.Redirect("AddKRA.aspx");
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (FileUpload1.PostedFile != null)
        {
            int res = 0, once = 0;
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
                    string KPI = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 2).GetString();
                    string Target = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 3).GetString();
                    string Weightage = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 4).GetString();
                    string UOM = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 5).GetString();

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
                        res = addusdr.AddKRAUW(EmpId, KPI, int.Parse(Target), float.Parse(Weightage), UOM, Session["EmailId"].ToString());
                }
                catch
                {
                    once = 1;
                }
                row++;

            }
            if (once > 0)
            {
                string message = "File Uploaded, without some entries!";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<script>alert('");
                sb.Append(message);
                sb.Append("'); </script>");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
            }
            else if (res > 0)
            {
                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>if(window.confirm('File Uploaded Successfully')){window.location.href='KPIList.aspx'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
            }
            else
            {

                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>if(window.confirm('There is some Error, File Not Uploaded')){window.location.href='KPIList.aspx'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
            }
        }

    }


    protected void btnSample_Click(object sender, EventArgs e)
    {

        DataTable dt = new DataTable("Grid");
        dt.Columns.AddRange(new DataColumn[5] {
            new DataColumn("EmpId"),
            new DataColumn("KPI"),
            new DataColumn("Target"),
            new DataColumn("Weightage"),
            new DataColumn("UOM")
            });

        dt.Rows.Add("1", "Project/kizen", "200", "58.65", "PKZ");
        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dt);
            using (MemoryStream stream = new MemoryStream())
            {
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=KraUserWiseSample.xlsx");
                wb.SaveAs(stream);
                stream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();

            }
        }
    }

    protected void btnassgnkpi_click(object sender, EventArgs e)
    {
        Response.Redirect("AddKRAUserwise.aspx");
    }

    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        KPIList.PageIndex = e.NewPageIndex;
        string year = dlyear.SelectedValue;
        this.bindKPIList(year);
    }

    protected void dlyears_SelectedIndexChanged(object sender, EventArgs e)
    {
        string year = dlyear.SelectedValue;
        bindKPIList(year);
    }
    protected void KPIList_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void KPIList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Text = Regex.Replace(e.Row.Cells[0].Text, txtsearchkpi.Text.Trim(), delegate (Match match)
            {
                return string.Format("<span style = 'background-color:#D9EDF7'>{0}</span>", match.Value);
            }, RegexOptions.IgnoreCase);
        }
    }
}
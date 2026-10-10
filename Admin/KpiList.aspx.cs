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
            if (Session["EmailId"] == null)
            {
                Response.Redirect("../Default.aspx");
            }
           // binddlyears();

            binddlyears();
            string year = dlyear.SelectedItem.Value;
            bindKPIList(year);
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

    protected void bindKPIList(string year)
    {

        string str = txtsearchkpi.Text.Trim();
        DataTable dt;
        if (str == "")
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"select kuw.Id,EmpName, KPI,Target, Weightage, kuw.Year from [dbo].[KRAUserwise] kuw
LEFT JOIN [dbo].[EmployeeMaster] emp on kuw.EmpId = emp.Id WHERE kuw.Year = " + year);
        }
        else
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"select kuw.Id,EmpName, KPI,Target, Weightage, kuw.Year from [dbo].[KRAUserwise] kuw
LEFT JOIN [dbo].[EmployeeMaster] emp on kuw.EmpId = emp.Id where KPI like'%" + str + "%' or  EmpName like'%" + str + "%' and kuw.Year = " + year);
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
                    string KPI = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 2).GetString();
                    string Target = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 3).GetString();
                    string Weightage = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 4).GetString();
                    string UOM = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 5).GetString();

                    int Target2 = 0;
                    int Weightage2 = 0;
                    int.TryParse(Target, out Target2);
                    int.TryParse(Weightage, out Weightage2);

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
                        int result = addusdr.AddKRAUW(EmpId, KPI, Target2, Weightage2, UOM, Session["EmailId"].ToString());

                        if (result == -1) // Duplicate KPI entry
                        {
                            duplicates++;
                        }
                        else if (result == -2) // Assigned KPI
                        {
                            assigned++;
                        }
                        else if (result > 0) // Successful insert
                        {
                            res++;
                        }
                    
                    }
                       
                }
                catch (Exception ex)
                {
                   // once = 1;
                }
                row++;

            }
            /* if (once > 0)
             {
                 string message = "File Uploaded, without some entries!";
                 System.Text.StringBuilder sb = new System.Text.StringBuilder();
                 sb.Append("<script>alert('");
                 sb.Append(message);
                 sb.Append("'); </script>");
                 ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
             }*/

            string message = "";
            if (duplicates > 0)
            {
                message += "There were " + duplicates + " duplicate KPI entries.";
            }
            if (assigned > 0)
            {
                message += "One or more KPI were already assigned.";
            }

            if (res > 0)
            {
                message += "File uploaded successfully!";
                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>if(window.confirm('" + message + "')){window.location.href='KPIList.aspx'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
            }
            else
            {
                message = "There was an error uploading the file," + message;
                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>if(window.confirm('" + message + "')){window.location.href='KPIList.aspx'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
            }
           /* else
            {

                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>if(window.confirm('File Uploaded , but Some Employee with these KPI, Target, Weightage and UOM already exists so duplicate not added ')){window.location.href='KPIList.aspx'}</script>";
             //   string strconfirm = "<script>if(window.confirm('There is some Error, File Not Uploaded')){window.location.href='KPIList.aspx'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
            }*/
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

        dt.Rows.Add("1", "Project/kizen", "200", "58.65", "%");
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
       // this.bindKPIList();
    }

    protected void KPIList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Remove")
        {
            string id = e.CommandArgument.ToString();
            int empid = 0;
            User user = new User();
            DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"select EmpId from [dbo].[KRAUserwise] where id="+id);
            if (dt.Rows.Count > 0)
            {
                empid = int.Parse(dt.Rows[0]["EmpId"].ToString());
                //update first updatekpiavg sp
                //objSQLDataFile.ExecuteDataTableSQL(@"delete from [dbo].[KRAUserwise] where id=" + id);
                //user.UpdateKPIAVG(empid);

                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>if(window.confirm('KPI not deleted')){window.location.href='KpiList.aspx'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);

            }

        }
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

    protected void btnForPrev_Click(object sender, EventArgs e)
    {

        try
        {
            string message = "";

            DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"select * from KRAUserWise where year = YEAR(GETDATE())");
            string created_by = Session["EmailId"].ToString(); //for future referenbce adding createdby column 

            if (dt.Rows.Count > 0) //it means current year data is already available
            {
                message = "Data already forwarded!";
            }
            else
            { 
              //new updated test query to forward data of those employees who have status as active and also if uom is Kg or Nos then can add new values for target and weightage for new year

                   objSQLDataFile.ExecuteDataTableSQL(@"INSERT INTO KRAUserwise( EmpId, KPI, Target, Weightage, UOM, CreatedBy)
                    SELECT DISTINCT EmpId, KPI,
    CASE WHEN kuw.UOM IN ('Kg', 'Nos.') THEN 0 ELSE kuw.Target END AS Target,
    CASE WHEN kuw.UOM IN ('Kg', 'Nos.') THEN 0 ELSE kuw.Weightage END AS Weightage,
    kuw.UOM, '" + created_by + @"'
FROM KRAUserwise kuw
JOIN EmployeeMaster emp ON kuw.EmpId = emp.Id
WHERE kuw.Year = YEAR(GETDATE()) - 1
AND emp.Status = 'Active'
AND emp.Id IS NOT NULL");

                //old query

              /*  objSQLDataFile.ExecuteDataTableSQL(@"INSERT INTO KRAUserwise( EmpId, KPI, UOM, CreatedBy)
            SELECT distinct EmpId, KPI, UOM, CreatedBy
            FROM KRAUserwise kuw
            WHERE kuw.Year = YEAR(GETDATE()) - 1");*/
                

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
        bindKPIList(year);
    }

}
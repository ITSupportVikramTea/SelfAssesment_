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

public partial class Admin_KRAList : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["EmailId"] == null || Session["UserType"].ToString().ToLower() != "employeehr")
            {
                Response.Redirect("../Default.aspx");
            }

            bindgetKRAList();
            //btnupdate.Visible = false;
        }

    }
    public void bindgetKRAList()
    {

        string str = txtsearchkpi.Text.Trim();
        DataTable dt;
        if (str == "")
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from KRAMaster ");
        }
        else
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from KRAMaster where KPI like'%" + str + "%' ");
        }


        KRAList.DataSource = dt;
        KRAList.DataBind();
    }

    protected void Search(object sender, EventArgs e)
    {
        this.bindgetKRAList();
    }

    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        KRAList.PageIndex = e.NewPageIndex;
        this.bindgetKRAList();
    }
    protected void KRA_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "Page" )
        {
            int recordid = Convert.ToInt32(e.CommandName);
            Response.Redirect("AddKRA.aspx?id=" + recordid);

        }
            
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
                    string KPI = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 1).GetString();
                    string Target = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 2).GetString();
                    string Weightage = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 3).GetString();
                    string UOM = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 4).GetString();

                    User addusdr = new User();
                    if(KPI != "")
                    {
                        if (Target != "" && Weightage != "" && UOM != "")
                        {
                            res = addusdr.AddKRA(KPI, Target, float.Parse(Weightage), UOM, Session["EmailId"].ToString());
                        }
                        else
                        {
                            DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"INSERT INTO KRAMaster(KPI, CreatedBy) values('" + KPI + "', '" + Session["EmailId"].ToString() + "')");
                        }
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
                string strconfirm = "<script>if(window.confirm('File Uploaded Successfully')){window.location.href='KRAList.aspx'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
            }
            else
            {
                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>if(window.confirm('There is some Error, File Not Uploaded')){window.location.href='KRAList.aspx'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
            }
        }


    }

    protected void btnSample_Click(object sender, EventArgs e)
    {

        DataTable dt = new DataTable("Grid");
        dt.Columns.AddRange(new DataColumn[4] {
            new DataColumn("KPI"),
            new DataColumn("Target"),
            new DataColumn("Weightage"),
            new DataColumn("UOM")
            });
        dt.Rows.Add("Maintenance of Plant and Machinery(OEE)", "6", "10", "OEE");
        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dt);
            using (MemoryStream stream = new MemoryStream())
            {

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=KRASample.xlsx");
                wb.SaveAs(stream);
                stream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();

            }
        }
    }

    protected void KRAList_RowDataBound(object sender, GridViewRowEventArgs e)
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
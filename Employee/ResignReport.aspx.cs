using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_ResignReport : System.Web.UI.Page
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
            bindgdResginReport();
        }
    }

    protected void bindgdResginReport()
    {
        string str = txtsearch.Text.Trim();
        DataTable dt;
        if (str != "")
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"select emp.EmpName, emp.EmpDepartment, Reason, DatenTime, resg.Status, resg.IsWithdrawn from [dbo].[Resignations] resg
left join [dbo].[EmployeeMaster] emp on resg.EmpId = emp.Id where EmpName like '%" + str+ "%' or EmpDepartment like '%" + str + "%'order by resg.DatenTime");
            gdResginReport.DataSource = dt;
            gdResginReport.DataBind();
        }
        else
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"select emp.EmpName, emp.EmpDepartment, Reason, DatenTime, resg.Status, resg.IsWithdrawn from [dbo].[Resignations] resg
left join [dbo].[EmployeeMaster] emp on resg.EmpId = emp.Id order by resg.DatenTime desc");
            gdResginReport.DataSource = dt;
            gdResginReport.DataBind();
        }
    }
    protected void Search(object sender, EventArgs e)
    {
        this.bindgdResginReport();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the run time error "  
        //Control 'GridView1' of type 'Grid View' must be placed inside a form tag with runat=server."  
    }

    public void ExportToExcel(object sender, EventArgs e)
    {
        Response.Clear();
        Response.Buffer = true;
        string filename = "ResignationsReport-" + DateTime.Now + ".xls";
        Response.AddHeader("content-disposition", "attachment;filename=" + filename);
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        using (StringWriter sw = new StringWriter())
        {
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            //To Export all pages
            gdResginReport.AllowPaging = false;
            this.bindgdResginReport();

            gdResginReport.HeaderRow.BackColor = Color.White;
            foreach (TableCell cell in gdResginReport.HeaderRow.Cells)
            {
                cell.BackColor = gdResginReport.HeaderStyle.BackColor;
            }
            foreach (GridViewRow row in gdResginReport.Rows)
            {
                row.BackColor = Color.White;
                foreach (TableCell cell in row.Cells)
                {
                    if (row.RowIndex % 2 == 0)
                    {
                        cell.BackColor = gdResginReport.AlternatingRowStyle.BackColor;
                    }
                    else
                    {
                        cell.BackColor = gdResginReport.RowStyle.BackColor;
                    }
                    cell.CssClass = "textmode";
                }
            }

            gdResginReport.RenderControl(hw);

            //style to format numbers to string
            string style = @"<style> .textmode { } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
    }
    protected void gdResginReport_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void gdResginReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            string iswithdran = (((DataRowView)e.Row.DataItem)["IsWithdrawn"]).ToString();

            if(iswithdran == "True")
            {
                e.Row.Cells[5].Text = "Yes";
            }
            else
            {
                e.Row.Cells[5].Text = "No";
            }

            e.Row.Cells[0].Text = Regex.Replace(e.Row.Cells[0].Text, txtsearch.Text.Trim(), delegate (Match match)
            {
                return string.Format("<span style = 'background-color:#D9EDF7'>{0}</span>", match.Value);
            }, RegexOptions.IgnoreCase);

        }
    }

    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdResginReport.PageIndex = e.NewPageIndex;
        this.bindgdResginReport();
    }
}
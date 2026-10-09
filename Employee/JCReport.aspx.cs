using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_JCReport : System.Web.UI.Page
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
            bindgdJCReport();
            binddlyears();
        }
    }
    protected void binddlyears()
    {
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT Year from ScoreCard");
        dlyears.DataSource = dt;
        dlyears.DataTextField = "Year";
        dlyears.DataValueField = "Year";
        dlyears.DataBind();
    }

    protected void bindgdJCReport(string condition_year = "")
    {
        string str = txtsearch.Text.Trim();
        //string condition_year = "";
        User user = new User();
        int empId = user.getEmployeeId(Session["EmailId"].ToString());

        if(condition_year != "")
        {
            condition_year = " and sc.Year = "+ dlyears.SelectedValue;
        }

        DataTable dt;
        if (str != "")
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"select sc.EmpId, emp.EmpName, emp.EmpDepartment, emp.EmpDesignation, emp.RM, emp.HOD,
(case when sc.JCType = 1 then sc.KPIAVG end) as JC1,
(case when sc.JCType = 2 then sc.KPIAVG end) as JC2,
(case when sc.JCType = 3 then sc.KPIAVG end) as JC3,
(case when sc.JCType = 4 then sc.KPIAVG end) as JC4,
sc.Year
from Employeemaster emp
join ScoreCard sc
on sc.EmpId = emp.Id
WHERE (EmpId= "+ empId + " or emp.HOD = (select EmpName from EmployeeMaster where id=" + empId + ") OR " +
"emp.RM = (select EmpName from EmployeeMaster where id=" + empId + ") or emp.SDM = (select EmpName from EmployeeMaster where id=" + empId + ")" +
"or emp.ASM = (select EmpName from EmployeeMaster where id=" + empId + ")) and emp.EmpName like '%"+ str +"%'"+ condition_year+" order by Year Desc");
            gdJCReport.DataSource = dt;
            gdJCReport.DataBind();
        }
        else
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"select sc.EmpId, emp.EmpName, emp.EmpDepartment, emp.EmpDesignation, emp.RM, emp.HOD,
(case when sc.JCType = 1 then sc.KPIAVG end) as JC1,
(case when sc.JCType = 2 then sc.KPIAVG end) as JC2,
(case when sc.JCType = 3 then sc.KPIAVG end) as JC3,
(case when sc.JCType = 4 then sc.KPIAVG end) as JC4,
sc.Year
from Employeemaster emp
join ScoreCard sc
on sc.EmpId = emp.Id
WHERE(EmpId = "+ empId + " or emp.HOD = (select EmpName from EmployeeMaster where id = " + empId + ") OR " +
"emp.RM = (select EmpName from EmployeeMaster where id=" + empId + ") or emp.SDM = (select EmpName from EmployeeMaster where id=" + empId + ")" +
"or emp.ASM = (select EmpName from EmployeeMaster where id=" + empId + ")) " + condition_year + " order by Year Desc");
            gdJCReport.DataSource = dt;
            gdJCReport.DataBind();
        }
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
        string filename = "JCReport-" + DateTime.Now + ".xls";
        Response.AddHeader("content-disposition", "attachment;filename=" + filename);
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        using (StringWriter sw = new StringWriter())
        {
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            //To Export all pages
            gdJCReport.AllowPaging = false;
            this.bindgdJCReport(dlyears.SelectedValue);

            gdJCReport.HeaderRow.BackColor = Color.White;
            foreach (TableCell cell in gdJCReport.HeaderRow.Cells)
            {
                cell.BackColor = gdJCReport.HeaderStyle.BackColor;
            }
            foreach (GridViewRow row in gdJCReport.Rows)
            {
                row.BackColor = Color.White;
                foreach (TableCell cell in row.Cells)
                {
                    if (row.RowIndex % 2 == 0)
                    {
                        cell.BackColor = gdJCReport.AlternatingRowStyle.BackColor;
                    }
                    else
                    {
                        cell.BackColor = gdJCReport.RowStyle.BackColor;
                    }
                    cell.CssClass = "textmode";
                }
            }

            gdJCReport.RenderControl(hw);

            //style to format numbers to string
            string style = @"<style> .textmode { } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
    }


    protected void gdJCReport_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void gdJCReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Text = Regex.Replace(e.Row.Cells[0].Text, txtsearch.Text.Trim(), delegate (Match match)
            {
                return string.Format("<span style = 'background-color:#D9EDF7'>{0}</span>", match.Value);
            }, RegexOptions.IgnoreCase);

        }
    }


    protected void Search(object sender, EventArgs e)
    {
        this.bindgdJCReport();
    }

    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdJCReport.PageIndex = e.NewPageIndex;
        this.bindgdJCReport();
    }

    protected void dlyears_TextChanged(object sender, EventArgs e)
    {
        this.bindgdJCReport(dlyears.SelectedValue);
    }
}
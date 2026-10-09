using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_BlankKPI : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    static int empId;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["EmailId"] == null)
            {
                Response.Redirect("../Default.aspx");
            }
            binddlgdBlankKPI();
        }
        // bidndluom();


    }
    protected void binddlgdBlankKPI()
    {

        User user = new User();
        empId = user.getEmployeeId(Session["EmailId"].ToString());

        string str = txtsearchuser.Text.Trim();
        DataTable dt;
        if (str != "")
        {
            //added year condition so that only current years data should be fetched 

            dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT  kuw.Id,EmpName,KPI,Target, Weightage, kuw.UOM, Achievement,ul.Formula from [dbo].[KRAUserwise] kuw 
left join [dbo].[EmployeeMaster] emp
 on kuw.EmpId = emp.Id left join [dbo].[UOMList] ul
ON kuw.UOM = ul.UOM where EmpId in (select Id from EmployeeMaster where HOD = (SELECT EmpName from EmployeeMaster where Id = " + empId + ") or RM = (SELECT EmpName from EmployeeMaster where Id = " + empId + ") or Id=" + empId + ") " +
"and (kuw.Target = 0.00 or kuw.Target is null " +
"or kuw.Weightage = 0.00 or kuw.Weightage is null or kuw.UOM = '' or kuw.UOM is null ) and (EmpName like '%" + str + "%' or KPI like '%" + str + "%'  )  and kuw.year = YEAR(GETDATE())");

        }
        else
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT  kuw.Id,EmpName,KPI,Target, Weightage, kuw.UOM, Achievement,ul.Formula from [dbo].[KRAUserwise] kuw 
left join [dbo].[EmployeeMaster] emp
 on kuw.EmpId = emp.Id left join [dbo].[UOMList] ul
ON kuw.UOM = ul.UOM
where EmpId in (select Id from EmployeeMaster where HOD =
(SELECT EmpName from EmployeeMaster where Id = " + empId + ") or RM = (SELECT EmpName from EmployeeMaster where Id = " + empId + ") or Id=" + empId + ") and (kuw.Target = 0.00 or kuw.Target is null " +
"or kuw.Weightage = 0.00 or kuw.Weightage is null or kuw.UOM = '' or kuw.UOM is null) and kuw.year = YEAR(GETDATE())");

        }

        gdBlankKPI.DataSource = dt;
        gdBlankKPI.DataBind();

    }
    protected void bidndluom() 
    {
        //not working try another solution
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT UOM from UOMList ");
        DropDownList dluom = new DropDownList();
        //dluom = (DropDownList)gdBlankKPI.Rows.Cells[3].FindControl("DropDownList1");
        //dluom.DataSource = dt;
        //dluom.DataMember = "UOM";
        //dluom.DataTextField = "UOM";
        //dluom.DataValueField = "UOM";
        //dluom.DataBind();

    }
    protected void Search(object sender, EventArgs e)
    {
        this.binddlgdBlankKPI();
    }

    protected void gdBlankKPI_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //not working this code check it
            float target = 0;

            //target = int.Parse((e.Row.Cells[3].FindControl("lbltarget") as Label).ToString());
            float.TryParse((e.Row.Cells[3].FindControl("lbltarget") as Label).Text, out target);

            float weightage = 0;

            //weightage = float.Parse((e.Row.Cells[4].FindControl("Weightage") as Label).Text);
            float.TryParse((e.Row.Cells[4].FindControl("lblwieghtage") as Label).Text, out weightage);


            string uom = (e.Row.Cells[5].FindControl("lbluom") as Label).Text;
            //string uom = (((DataRowView)e.Row.DataItem)["lbluom"]).ToString();

            float achmnt = 0;

            float.TryParse((e.Row.Cells[6].FindControl("lblachievment") as Label).Text, out achmnt);

            if (uom != "" && uom != "&nbsp;")
            {
                TextBox tb1 = e.Row.Cells[6].FindControl("txtuom") as TextBox;
                Label lbl = (e.Row.Cells[6].FindControl("lbluom") as Label);
                lbl.Visible = true;
                tb1.Visible = false;
            }

            if (target > 0)
            {
                TextBox tb1 = e.Row.Cells[3].FindControl("txttarget") as TextBox;
                Label lbl = (e.Row.Cells[3].FindControl("lbltarget") as Label);
                lbl.Visible = true;
                tb1.Visible = false;
            }

            if (weightage > 0)
            {
                TextBox tb1 = e.Row.Cells[4].FindControl("txtweightage") as TextBox;
                Label lbl = (e.Row.Cells[4].FindControl("lblwieghtage") as Label);
                lbl.Visible = true;
                tb1.Visible = false;
            }
            if (achmnt >= 1)
            {
                TextBox tb1 = e.Row.Cells[6].FindControl("txtachievement") as TextBox;
                Label lbl = (e.Row.Cells[6].FindControl("lblachievment") as Label);
                lbl.Visible = true;
                tb1.Visible = false;
            }


            e.Row.Cells[0].Text = Regex.Replace(e.Row.Cells[0].Text, txtsearchuser.Text.Trim(), delegate (Match match)
            {
                return string.Format("<span style = 'background-color:#D9EDF7'>{0}</span>", match.Value);
            }, RegexOptions.IgnoreCase);

        }
    }
    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdBlankKPI.PageIndex = e.NewPageIndex;
        this.binddlgdBlankKPI();
    }

    protected void btnexport_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable("Grid");
        dt.Columns.AddRange(new DataColumn[8] {
                    new DataColumn("Id"),
                    new DataColumn("ERPID"),
                    new DataColumn("EmpName"),
                    new DataColumn("KPI"),
                    new DataColumn("Target"),
                    new DataColumn("Weightage"),
                    new DataColumn("UOM"),
                    new DataColumn("Achievement")
                    });

        dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT kuw.Id,ERPID,EmpName,KPI,Target, Weightage, UOM, Achievement from [dbo].[KRAUserwise] kuw join [dbo].[EmployeeMaster] emp
on kuw.EmpId = emp.Id
where EmpId in ((select Id from EmployeeMaster where HOD = (SELECT EmpName from EmployeeMaster where Id = " + empId + ") or RM = (SELECT EmpName from EmployeeMaster where Id = " + empId + ") or Id=" + empId + ")) " +
"and (kuw.Target = 0.00 or kuw.Target is null or kuw.Weightage = 0.00 or kuw.Weightage is null or kuw.UOM = '' or kuw.UOM is null) and kuw.year = YEAR(GETDATE())");



        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dt, "KPIData");
            //wb.Worksheets.Add(dt);
            using (MemoryStream stream = new MemoryStream())
            {
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=KPIData.xlsx");
                wb.SaveAs(stream);
                stream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }

    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the run time error "  
        //Control 'GridView1' of type 'Grid View' must be placed inside a form tag with runat=server."  
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (FileUpload1.PostedFile != null)
        {
            int res = 0;
            string filePath = Server.MapPath("../UploadFile/");
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss");
            filePath = filePath + fileName + ".xlsx";
            FileUpload1.PostedFile.SaveAs(filePath);

            XLWorkbook xLWorkbook = new XLWorkbook(filePath);
            int row = 2;
            while (xLWorkbook.Worksheets.Worksheet(1).Cell(row, 1).GetString() != "")
            {
                try
                {
                    float fTarget = 0;

                    int Id = int.Parse(xLWorkbook.Worksheets.Worksheet(1).Cell(row, 1).GetString());
                    //string EmpName = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 2).GetString();
                    //string KPI = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 3).GetString();
                    string Target = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 5).GetString();

                    float Weightage = 0;
                    float.TryParse(xLWorkbook.Worksheets.Worksheet(1).Cell(row, 6).GetString(), out Weightage);


                    string UOM = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 7).GetString();

                    float achievment = 0;
                    float.TryParse(xLWorkbook.Worksheets.Worksheet(1).Cell(row, 8).GetString(), out achievment);

                    if (Weightage > 100)
                    {
                        Weightage = 0;
                    }


                    float.TryParse(Target, out fTarget);

                    if (fTarget > 0 && achievment > 0 && UOM != "")
                    {
                        string txtFormula = "(Achievement/Target)*100";
                        string st = txtFormula.Replace("Achievement", achievment.ToString()).Replace("Target", fTarget.ToString());
                        DataTable dt = new DataTable();
                        float fscore = 0;
                        float.TryParse(dt.Compute(st, "").ToString(), out fscore);
                        objSQLDataFile.ExecuteDataTableSQL(@"UPDATE KRAUserwise SET Target = " + fTarget + ",Weightage = " + Weightage + ", Achievement = " + achievment + ", UOM ='" + UOM + "' ,Score=" + fscore + " WHERE Id = " + Id + " and year = YEAR(GETDATE())");
                        res = 1;

                        DataTable dt2 = objSQLDataFile.ExecuteDataTableSQL(@"SELECT EmpId,JCType,Year from KRAUserwise WHERE Id = " + Id);
                        int empId = int.Parse(dt2.Rows[0]["EmpId"].ToString());
                        int JCType = int.Parse(dt2.Rows[0]["JCType"].ToString());
                        string Year = dt2.Rows[0]["Year"].ToString();
                        User user = new User();
                        user.SCKPIAvg(empId, JCType, Year);


                    }
                    else
                    {
                        User addusdr = new User();
                        res = addusdr.AddBlankKPI(Id, Target, Weightage, UOM, achievment);
                    }
                }
                catch (Exception ex)
                {

                }

                row++;
            }

            //here need to change error msg because when file uploaded without entries even if filled data is submitted it shows file not uploaded error
            if (res > 0)
            {
                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>if(window.confirm('File Uploaded')){window.location.href='BlankKPI.aspx'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
            }
            else
            {
                string message = "File Not Uploaded!";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<script>alert('");
                sb.Append(message);
                sb.Append("'); </script>");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
            }
        }
    }

    protected void btnsubmit_click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gdBlankKPI.Rows)
        {
            try
            {
                float fTarget = 0;
                float fweightage = 0;
                float fachivement = 0;
                string st = "";
                int re = 0;
                int Id = Convert.ToInt32(gdBlankKPI.DataKeys[row.RowIndex].Values[0].ToString());
                string Target = (row.Cells[3].FindControl("txttarget") as TextBox).Text;
                float.TryParse(Target, out fTarget);

                if (Target == "")
                {
                    Target = (row.Cells[3].FindControl("lbltarget") as Label).Text;
                    fTarget = float.Parse(Target);
                }
                string weightage = (row.Cells[4].FindControl("txtweightage") as TextBox).Text;
                float.TryParse(weightage, out fweightage);

                if (weightage == "")
                {
                    weightage = (row.Cells[4].FindControl("lblwieghtage") as Label).Text;
                    fweightage = float.Parse(weightage);
                }

                string uom = (row.Cells[5].FindControl("txtuom") as TextBox).Text;
                if (uom == "")
                {
                    uom = (row.Cells[4].FindControl("lbluom") as Label).Text;
                }
                string Achievement = (row.Cells[6].FindControl("txtachievement") as TextBox).Text;
                float.TryParse(Achievement, out fachivement);

                if (Achievement == "")
                {
                    Achievement = (row.Cells[4].FindControl("lblachievment") as Label).Text;
                    fachivement = float.Parse(Achievement);
                }

                if (fTarget > 0 && fachivement > 0 && uom != "")
                {
                    string txtFormula = (row.Cells[7].FindControl("txtFormula") as TextBox).Text;

                    if (txtFormula == "")
                    {
                        txtFormula = "(Achievement/Target)*100";
                    }

                    st = txtFormula.Replace("Achievement", fachivement.ToString()).Replace("Target", fTarget.ToString());
                    DataTable dt = new DataTable();
                    float fscore = 0;
                    float.TryParse(dt.Compute(st, "").ToString(), out fscore);
                    objSQLDataFile.ExecuteDataTableSQL(@"UPDATE KRAUserwise SET Target = " + fTarget + ",Weightage = " + fweightage + ", Achievement = " + fachivement + ", UOM ='" + uom + "' , Score=" + fscore + " WHERE Id = " + Id + " AND year=datepart(year,GETDATE())");
                    re = 1;


                    DataTable dt2 = objSQLDataFile.ExecuteDataTableSQL(@"SELECT EmpId,JCType,Year from KRAUserwise WHERE Id = " + Id);
                    int empId = int.Parse(dt2.Rows[0]["EmpId"].ToString());
                    int JCType = int.Parse(dt2.Rows[0]["JCType"].ToString());
                    string Year = dt2.Rows[0]["Year"].ToString();
                    User user = new User();
                    user.SCKPIAvg(empId, JCType, Year);
                }

                else
                {
                    User addusdr = new User();
                    re = addusdr.AddBlankKPI(Id, Target, fweightage, uom, fachivement);
                }

                if (re > 0)
                {
                    ClientScriptManager CSM = Page.ClientScript;
                    string strconfirm = " <script>if(window.confirm('Data Added Successfully')){window.location.href='BlankKPI.aspx'}</script>";
                    CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                }

            }
            catch (Exception ex)
            {
                Response.Redirect("BlankKPI.aspx");
            }

        }
        //Response.Redirect("KRA.aspx");
    }
}
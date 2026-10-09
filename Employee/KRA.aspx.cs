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

public partial class Employee_KRA : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    string empName;
    protected static string kpiavg { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["EmailId"] == null)
            {
                Response.Redirect("../Default.aspx");
            }

            hideEmployeeKPIBtn();
            binddlyears();
            string year = dlyear.SelectedItem.Value;
            bindgetKra(1, year);
            getKpiAvg(1);
        
            //btnupdate.Visible = false;
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

  


    public void bindgetKra(int jc, string year)
    {
        DataTable res = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT Id,EmpName from EmployeeMaster WHERE EmpEmailId = '" + Session["EmailId"].ToString() + "'");
       

        DataRow row2 = res.Rows[0];
        int empId = int.Parse(row2["Id"].ToString());
        empName = row2["EmpName"].ToString();
      
        string str = txtsearchuser.Text.Trim();
        DataTable dt = null;


        if (Session["UserType"].ToString().Equals("EMPLOYEE"))
        {
            if (str != "")
            {
                dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT ku.Id, emp.EmpName, ku.KPI,ku.Target,ku.Weightage, ku.UOM, 
ku.Score as Scores, ku.Achievement,ul.Formula,emp.SDM,emp.ASM,HOD, RM, Year
from KRAUserwise ku left join [dbo].[UOMList] ul
ON ku.UOM = ul.UOM
join  [dbo].[EmployeeMaster] emp 
on ku.EmpId = emp.Id
WHERE ku.EmpId IN ((select Id from EmployeeMaster where HOD = (SELECT EmpName from EmployeeMaster where Id = " + empId + ") or RM = (SELECT EmpName from EmployeeMaster where Id = " + empId + ") " +
    "or SDM = (select EmpName from EmployeeMaster where id=" + empId + ") or ASM = (select EmpName from EmployeeMaster where id=" + empId + ")  or Id = " + empId + "))" +
    " and (ku.Target != 0 and ku.Weightage != 0 and ku.UOM != '' ) and (EmpName like '%" + str + "%' or KPI like '%" + str + "%'  ) and (JCType = " + jc + " and Year =" + year + ") order by Weightage desc");

            }
            else
            {
                dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT ku.Id, emp.EmpName, ku.KPI,ku.Target,ku.Weightage, ku.UOM, 
ku.Score as Scores, ku.Achievement,ul.Formula,emp.SDM,emp.ASM,HOD, RM, Year
from KRAUserwise ku left join [dbo].[UOMList] ul
ON ku.UOM = ul.UOM
join  [dbo].[EmployeeMaster] emp 
on ku.EmpId = emp.Id
WHERE ku.EmpId IN ((select Id from EmployeeMaster where HOD = (SELECT EmpName from EmployeeMaster where Id = " + empId + ") or RM = (SELECT EmpName from EmployeeMaster where Id = " + empId + ") " +
    "or SDM = (select EmpName from EmployeeMaster where id=" + empId + ") or ASM = (select EmpName from EmployeeMaster where id=" + empId + ")  or Id = " + empId + "))" +
    " and (ku.Target != 0 and ku.Weightage != 0 and ku.UOM != '' ) and (JCType = " + jc + " and Year =" + year + ") order by Weightage desc ");

            }

        }
        else if (Session["UserType"].ToString().Equals("EMPLOYEEHR"))
        {
            if (str != "")
            {
                dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT ku.Id, emp.EmpName, ku.KPI,ku.Target,ku.Weightage, ku.UOM, 
ku.Score as Scores, ku.Achievement,ul.Formula,emp.SDM,emp.ASM,HOD, RM
from KRAUserwise ku left join [dbo].[UOMList] ul
ON ku.UOM = ul.UOM
join  [dbo].[EmployeeMaster] emp 
on ku.EmpId = emp.Id
WHERE (ku.Target != 0 and ku.Weightage != 0 and ku.UOM != '' ) and (EmpName like '%" + str + "%' or KPI like '%" + str + "%'  ) and (JCType = " + jc + " and Year =" + year + ")  order by Weightage desc");

            }
            else
            {
                dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT ku.Id, emp.EmpName, ku.KPI,ku.Target,ku.Weightage, ku.UOM, 
ku.Score as Scores, ku.Achievement,ul.Formula,emp.SDM,emp.ASM,HOD, RM
from KRAUserwise ku left join [dbo].[UOMList] ul
ON ku.UOM = ul.UOM
join  [dbo].[EmployeeMaster] emp 
on ku.EmpId = emp.Id
WHERE (ku.Target != 0 and ku.Weightage != 0 and ku.UOM != '' ) and (JCType = " + jc + " and Year =" + year + ")  order by Weightage desc ");

            }

        }

        KRAList.DataSource = dt;
        KRAList.DataBind();
    }

    //protected void KRAList_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    int recordid = Convert.ToInt32(e.CommandName);
    //    //Response.Redirect("AddEmployees.aspx?id=" + recordid);

    //}
    protected void btnsubmit_click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in KRAList.Rows)
        {
            string Id = (row.Cells[11].FindControl("KpiId") as Label).Text;
            string Achievement = (row.Cells[6].FindControl("txtachievement") as TextBox).Text;
            string Target = (row.Cells[13].FindControl("txtTarget") as TextBox).Text;
            string txtFormula = (row.Cells[11].FindControl("txtFormula") as TextBox).Text;

            if (txtFormula == "")
            {
                txtFormula = "(Achievement/Target)*100";
            }


            string st = txtFormula.Replace("Achievement", Achievement).Replace("Target", Target);
            int re = 0;
            if (Achievement != "")
            {
                float achievement2 = float.Parse(Achievement);
                if (achievement2 > 0)
                {

                    // objSQLDataFile.ExecuteDataTableSQL(@"UPDATE KRAUserwise SET Achievement = " + achievement2 + ",Score=" + st + " WHERE Id = " + Id );
                    objSQLDataFile.ExecuteDataTableSQL(@"UPDATE KRAUserwise SET Achievement = " + achievement2 + ",Score=" + st + " WHERE Id = " + Id + " AND year=datepart(year,GETDATE())");
                    re = 1;
                }
                if (re > 0)
                {
                    DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT EmpId,JCType,Year from KRAUserwise WHERE Id = " + Id);
                    int empId = int.Parse(dt.Rows[0]["EmpId"].ToString());
                    int JCType = int.Parse(dt.Rows[0]["JCType"].ToString());
                    string Year = dt.Rows[0]["Year"].ToString();
                    User user = new User();
                    user.SCKPIAvg(empId, JCType, Year);

                    ClientScriptManager CSM = Page.ClientScript;
                    string strconfirm = " <script>if(window.confirm('Achievement Added Successfully')){window.location.href='KRA.aspx'}</script>";
                    CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                }
            }

        }
        Response.Redirect("KRA.aspx");
    }

    protected void KRAList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            float achmnt;
            float.TryParse(e.Row.Cells[5].Text, out achmnt);
            //float achmnt = float.Parse(e.Row.Cells[5].Text);
            string Employee = (((DataRowView)e.Row.DataItem)["EmpName"]).ToString();
            string SDM = (((DataRowView)e.Row.DataItem)["SDM"]).ToString();
            string ASM = (((DataRowView)e.Row.DataItem)["ASM"]).ToString();
            string hod = (((DataRowView)e.Row.DataItem)["HOD"]).ToString();
            string rm = (((DataRowView)e.Row.DataItem)["RM"]).ToString();

            DataTable res = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT Id,EmpName from EmployeeMaster WHERE EmpEmailId = '" + Session["EmailId"].ToString() + "'");


            DataRow row2 = res.Rows[0];
            string emps = row2["EmpName"].ToString();

            if (Employee == emps)
            {
                TextBox textRM = (TextBox)e.Row.FindControl("txtachievement");
                textRM.Enabled = false;
            }

            if (achmnt >= 1)
            {
                e.Row.Cells[12].Style.Add("visibility", "hidden");
            }


            if (SDM == empName || ASM == empName)
            {
                if (hod == empName || rm == empName)
                {
                    if (achmnt >= 1)
                    {
                        e.Row.Cells[12].Style.Add("visibility", "hidden");
                    }
                    else
                    {
                        e.Row.Cells[12].Style.Add("visibility", "block");
                    }

                }
                else
                {
                    e.Row.Cells[12].Style.Add("visibility", "hidden");
                }

            }

            e.Row.Cells[0].Text = Regex.Replace(e.Row.Cells[0].Text, txtsearchuser.Text.Trim(), delegate (Match match)
            {
                return string.Format("<span style = 'background-color:#D9EDF7'>{0}</span>", match.Value);
            }, RegexOptions.IgnoreCase);
        }
    }

    protected void btnBlankKPI_Click(object sender, EventArgs e)
    {
        Response.Redirect("BlankKPI.aspx");
    }

    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        KRAList.PageIndex = e.NewPageIndex;
        string year = dlyear.SelectedItem.Value;
        this.bindgetKra(1, year);
        //this.bindgetKra();
    }


    protected void Search(object sender, EventArgs e)
    {
        string year = dlyear.SelectedItem.Value;
        this.bindgetKra(1, year);
        //this.bindgetKra();
    }

    protected void dljcs_TextChanged(object sender, EventArgs e)
    {
        try
        {
            bindgetKra(int.Parse(dljcs.SelectedValue), dlyear.SelectedValue);
            getKpiAvg(int.Parse(dljcs.SelectedValue));
        }
        catch
        {
            string year = dlyear.SelectedItem.Value;
            this.bindgetKra(1, year);
            // bindgetKra();
        }

    }
    protected void getKpiAvg(int jc)
    {
        DataTable res = objSQLDataFile.ExecuteDataTableSQL(@"select KPIAVG from ScoreCard where JCType=" + jc + " and Year= " + dlyear.SelectedValue + " and EmpId = (select Id from EmployeeMaster where EmpEmailId='" + Session["EmailId"].ToString() + "')");
        if (res.Rows.Count > 0)
        {
            DataRow row2 = res.Rows[0];
            kpiavg = row2["KPIAVG"].ToString();
        }
        else
        {
            kpiavg = "Not Available";
        }
    }


    //to check if logged in user is hod or hr then can only see the employees kpi button and add target weightage 
  /*  protected void hideEmployeeKPIBtn()
    {

        DataTable res = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT IsHOD,IsRM,ISHR from EmployeeMaster WHERE EmpEmailId = '" + Session["EmailId"].ToString() + "'");
        DataRow row = res.Rows[0];
        //int ishod = int.TryParse(row["IsHOD"].ToString());
        // int isrm = int.Parse(row["IsRM"].ToString());
        // int ishr = int.Parse(row["ISHR"].ToString());
        int ishod = 0;
        int ishr = 0;
        int.TryParse(row["IsHOD"].ToString(), out ishod);
        int.TryParse(row["ISHR"].ToString(), out ishr);

        if (ishod > 0 || ishr > 0)
        {
            btnBlankKPI.Visible = true;
            btnexport.Visible = true;
        }
        else
        {
            btnBlankKPI.Visible = false;
            btnexport.Visible = false;
        }

    }*/
  //updated above method - added button for export data
    protected void hideEmployeeKPIBtn()
    {
        DataTable res = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT IsHOD, IsRM, ISHR FROM EmployeeMaster WHERE EmpEmailId = '" + Session["EmailId"].ToString() + "'");

        if (res.Rows.Count > 0)
        {
            DataRow row = res.Rows[0];
            int isHOD = 0;
            int isHR = 0;

            int.TryParse(row["IsHOD"].ToString(), out isHOD);
            int.TryParse(row["ISHR"].ToString(), out isHR);

            if (isHOD > 0 || isHR > 0)
            {
                btnBlankKPI.Visible = true;
            }
            else
            {
                btnBlankKPI.Visible = false;
            }

            if (isHR > 0)
            {
                btnexport.Visible = true;
            }
            else
            {
                btnexport.Visible = false;
            }
        }
        else
        {
            btnBlankKPI.Visible = false;
            btnexport.Visible = false;
        }
    }

    protected void dlyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindgetKra(int.Parse(dljcs.SelectedValue), dlyear.SelectedValue);
            getKpiAvg(int.Parse(dljcs.SelectedValue));
        }
        catch
        {
            string year = dlyear.SelectedItem.Value;
            this.bindgetKra(1, year);
            //bindgetKra();
        }
    }
    //added this to download the data of kra
    protected void btnexport_Click(object sender, EventArgs e)
    {
        int jc = int.Parse(dljcs.SelectedValue);

        DataTable dt = new DataTable("Grid");
        dt.Columns.AddRange(new DataColumn[14] {
        new DataColumn("Id"),
        new DataColumn("ERPId"),
        new DataColumn("EmpName"),
        new DataColumn("KPI"),
        new DataColumn("Target"),
        new DataColumn("Weightage"),
        new DataColumn("UOM"),
        new DataColumn("Scores"),
        new DataColumn("Achievement"),
        new DataColumn("Year"),
        new DataColumn("SDM"),
        new DataColumn("ASM"),
        new DataColumn("HOD"),
        new DataColumn("RM")
    }); 

        dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT ku.Id, emp.ERPId, emp.EmpName, ku.KPI, ku.Target, ku.Weightage, ku.UOM, ku.Score AS Scores, ku.Achievement, ku.year, emp.SDM, emp.ASM, HOD, RM
                     FROM KRAUserwise ku
                     JOIN [dbo].[EmployeeMaster] emp ON ku.EmpId = emp.Id
                     WHERE (ku.Target != 0 AND ku.Weightage != 0 AND ku.UOM != '') AND (JCType = " + jc + " AND Year = YEAR(GETDATE())) ORDER BY Weightage DESC");


        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dt, "KRAData");

            using (MemoryStream stream = new MemoryStream())
            {
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=KRAData.xlsx");
                wb.SaveAs(stream);
                stream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }


}
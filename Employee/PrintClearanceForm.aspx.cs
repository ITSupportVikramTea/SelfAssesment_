using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_PrintClearanceForm : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    protected static string empName { get; set; }
    protected static string designation { get; set; }
    protected static string department { get; set; }
    protected static string doj { get; set; }
    protected static string location { get; set; }
    protected static string dor { get; set; }
    protected static string dorelieving { get; set; }
    protected static string rm { get; set; }
    protected static string imgsource { get; set; }
    protected static string thankingName { get; set; }
    protected static string thankingdate { get; set; }
    protected static string remark { get; set; }
    protected static string erpid { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["EmailId"] == null)
            {
                Response.Redirect("../Default.aspx");
            }
            if (Request.QueryString.AllKeys.Contains("id"))
            {
               bindData();
            }
        }
    }
    protected void bindData()
    {
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"select resg.EmpId, emp.EmpName,emp.EmpEmailId,emp.EmpDesignation, emp.EmpDepartment, emp.DOJ, emp.ERPID, resg.DatenTime, resg.Remarks,
emp.RM, cf.DateOfResignation,
DateofRelivieng, ThankingSignIMGPATH, ThankingName, ThankingDate from EmployeeMaster emp 
join Resignations resg
on emp.Id = resg.EmpId
join [dbo].[ClearanceForm] cf
on resg.ResignationId = cf.ResignationId
where resg.ResignationId=" + Request.QueryString["id"].ToString() + " AND IsWithdrawn = 0");

        DataTable dt1 = objSQLDataFile.ExecuteDataTableSQL(@"select AssesstName, ReturnBackStatus, ReturnedToWhom from EmployeeMaster emp 
join Resignations resg
on emp.Id = resg.EmpId
join [dbo].[AssetChildClearance] ccf
on resg.ResignationId = ccf.ResignationId
join [dbo].[ClearanceForm] cf
on resg.ResignationId = cf.ResignationId
where resg.ResignationId=" + Request.QueryString["id"].ToString() + " AND IsWithdrawn = 0");

        DataTable dt2 = objSQLDataFile.ExecuteDataTableSQL(@"select DistributorName, IsNOCRecieved, BalanceStatus, Description from EmployeeMaster emp 
join Resignations resg
on emp.Id = resg.EmpId
join [dbo].[DistributorChildClearance] ccf
on resg.ResignationId = ccf.ResignationId
join [dbo].[ClearanceForm] cf
on resg.ResignationId = cf.ResignationId
where (resg.ResignationId=" + Request.QueryString["id"].ToString() + " AND IsWithdrawn = 0) and (DistributorName is not null or DistributorName != '')");

        DataTable dt3 = objSQLDataFile.ExecuteDataTableSQL(@"select department_name, description, is_noc_recieved, amount_if_payable from EmployeeMaster emp 
join Resignations resg
on emp.Id = resg.EmpId
join [dbo].[ClearanceForm] cf
on resg.ResignationId = cf.ResignationId
join [dbo].[departmental_noc_child_clearance] dncc
on resg.ResignationId = dncc.ResignationId
where (resg.ResignationId=" + Request.QueryString["id"].ToString() + " AND IsWithdrawn = 0)");

        if (dt.Rows.Count > 0)
        {
            empName = dt.Rows[0]["EmpName"].ToString();
            designation = dt.Rows[0]["EmpDesignation"].ToString();
            department = dt.Rows[0]["EmpDepartment"].ToString();
            doj = getDateInDateFormat(dt.Rows[0]["DOJ"].ToString());
            rm = dt.Rows[0]["RM"].ToString();
            //location = dt.Rows[0]["DatenTime"].ToString();
            dor = getDateInDateFormat(dt.Rows[0]["DateOfResignation"].ToString());
            dorelieving = getDateInDateFormat(dt.Rows[0]["DateofRelivieng"].ToString());
            imgsource = dt.Rows[0]["ThankingSignIMGPATH"].ToString().Substring(1);
            thankingName = dt.Rows[0]["ThankingName"].ToString();
            thankingdate = getDateInDateFormat(dt.Rows[0]["ThankingDate"].ToString());
            remark = dt.Rows[0]["Remarks"].ToString();
            erpid = dt.Rows[0]["ERPID"].ToString();
            //test code if data for any of gridview is not avaiable then also it should bind data of existing gridviews

            if (dt1.Rows.Count > 0)
            {
                gdAssetData.DataSource = dt1;
                gdAssetData.DataBind();
                gdAssetData.Visible = true;
            }
            else
            {
                gdAssetData.Visible = false;
            }

            if (dt2.Rows.Count > 0)
            {
                gdSupiriorClearanceData.DataSource = dt2;
                gdSupiriorClearanceData.DataBind();
                gdSupiriorClearanceData.Visible = true;
            }
            else
            {
                gdSupiriorClearanceData.Visible = false;
            }

            if (dt3.Rows.Count > 0)
            {
                gdDeptNocData.DataSource = dt3;
                gdDeptNocData.DataBind();
                gdDeptNocData.Visible = true;
            }
            else
            {
                gdDeptNocData.Visible = false;
            }
        }
    }
    protected string getDateInDateFormat(string date)
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
}
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_ResingList : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    static SQLDataFileSIS objSQLDataFile2 = new SQLDataFileSIS();
    static int empId;
    static string name;
    static string isHR = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["EmailId"] == null)
            {
                Response.Redirect("../Default.aspx");
            }
            isHR = Session["UserType"].ToString();

            bindgdResignationList();
        }
    }
    protected void bindgdResignationList()
    {
        string str = txtsearchresignation.Text.Trim();
        DataTable dt;
        User user = new User();
        empId = user.getEmployeeId(Session["EmailId"].ToString());
        name = user.getEmployeeName(Session["EmailId"].ToString());

        dt = getgdResignationList(empId, str, name);

        gdResignationList.DataSource = dt;
        gdResignationList.DataBind();
    }

    protected DataTable getgdResignationList(int empId, string str, string name)
    {
        DataTable dt = null;
        User user = new User();//
        string departmentName = user.GetDepartmentByEmail(Session["EmailId"].ToString()); 
        if (Session["UserType"].ToString().Equals("EMPLOYEE"))
        {
            if (str != "")
            {
                
                dt = objSQLDataFile.ExecuteDataTableSQL(@"select resg.EmpId,resg.ResignationId, Reason, emp.EmpName, resg.Status, cf.IsSubmitted, resg.IsWithDrawn, resg.DatenTime, resg.HOD, 
resg.RM  from [dbo].[Resignations] resg
join [dbo].[EmployeeMaster] emp
on resg.EmpId = emp.Id 
left join [dbo].[ClearanceForm] cf
on resg.ResignationId = cf.ResignationId where (resg.EmpId = " + empId + " and EmpName like '%" + str + "%' or resg.HOD = '" + name + "' or resg.RM = '" + name + "'" +
" or (select EmpEmailId from EmployeeMaster where id = " + empId + ") in (select department_head_email from department_head where department_head_email = (select EmpEmailId from EmployeeMaster where id = " + empId + "))) and IsWithdrawn = 0 order by DatenTime desc");
            }
            else
            {

                dt = objSQLDataFile.ExecuteDataTableSQL(@"select resg.EmpId,resg.ResignationId, Reason, emp.EmpName, resg.Status, cf.IsSubmitted, resg.IsWithDrawn, resg.DatenTime, resg.HOD, 
resg.RM  from [dbo].[Resignations] resg
join [dbo].[EmployeeMaster] emp
on resg.EmpId = emp.Id 
left join [dbo].[ClearanceForm] cf
on resg.ResignationId = cf.ResignationId where (resg.EmpId = " + empId + " or resg.HOD = '" + name + "' or resg.RM = '" + name + "' " +
"or (select EmpEmailId from EmployeeMaster where id = " + empId + ") in (select department_head_email from department_head where department_head_email = (select EmpEmailId from EmployeeMaster where id = " + empId + "))) and IsWithdrawn = 0 order by DatenTime desc");

            }
        }
        else if (Session["UserType"].ToString().Equals("EMPLOYEEHR"))
        {
            if (str != "")
            {
                dt = objSQLDataFile.ExecuteDataTableSQL(@"select resg.EmpId,resg.ResignationId, Reason, emp.EmpName, resg.Status, cf.IsSubmitted, resg.IsWithDrawn, resg.DatenTime, resg.HOD, 
resg.RM  from [dbo].[Resignations] resg
join [dbo].[EmployeeMaster] emp
on resg.EmpId = emp.Id 
left join [dbo].[ClearanceForm] cf
on resg.ResignationId = cf.ResignationId where EmpName like '%" + str + "%' and IsWithdrawn = 0 and resg.Status != 'Rejected' order by DatenTime desc");
            }
            else
            {
                dt = objSQLDataFile.ExecuteDataTableSQL(@"select resg.EmpId,resg.ResignationId, Reason, emp.EmpName, resg.Status, cf.IsSubmitted, resg.IsWithDrawn, resg.DatenTime, resg.HOD, 
resg.RM  from [dbo].[Resignations] resg
join [dbo].[EmployeeMaster] emp
on resg.EmpId = emp.Id 
left join [dbo].[ClearanceForm] cf
on resg.ResignationId = cf.ResignationId where IsWithdrawn = 0 and resg.Status != 'Rejected' order by DatenTime desc");
            }
        }

        return dt;
    }
    protected void Search(object sender, EventArgs e)
    {
        this.bindgdResignationList();
    }

    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdResignationList.PageIndex = e.NewPageIndex;
        this.bindgdResignationList();
    }
    protected void addresignation_Click(object sender, EventArgs e)
    {
        Response.Redirect("Resign.aspx");
    }

    protected void gdResignationList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            Response.Redirect("Resign.aspx?id=" + e.CommandArgument);
        }
        else if (e.CommandName == "withdraw")
        {
            objSQLDataFile.ExecuteDataTableSQL(@"UPDATE Resignations set IsWithdrawn = 1 where ResignationId = " + e.CommandArgument);
            Response.Redirect("ResingList.aspx");
        }
        
        else if (e.CommandName == "Reject")
        {
            objSQLDataFile.ExecuteDataTableSQL(@"UPDATE Resignations set Status = 'Rejected' where ResignationId = " + e.CommandArgument);
            Response.Redirect("ResingList.aspx");
        }

        else if (e.CommandName == "ClearanceForm")
        {
            Response.Redirect("ClearanceForm.aspx?id=" + e.CommandArgument);
        }

        else if (e.CommandName == "ViewClearanceForm")
        {
            Response.Redirect("PrintClearanceForm.aspx?id=" + e.CommandArgument);
        }

        else if(e.CommandName == "Download")
        {
            int clearanceId = Convert.ToInt32(e.CommandArgument);
            ClearanceData clearanceData = new ClearanceData(clearanceId);
            string clearancePdfFile = clearanceData.clearancePdfFileName;

            string physicalDirectoryPath = Server.MapPath("~/clearance_files/");
            string physicalFilePath = Path.Combine(physicalDirectoryPath, clearancePdfFile);

            byte[] pdfBytes = File.ReadAllBytes(physicalFilePath);

            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=GeneratedPDF.pdf");
            Response.OutputStream.Write(pdfBytes, 0, pdfBytes.Length);
            Response.Flush();
            Response.End();
        }
    }

    protected void gdResignationList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        User user = new User();
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int empId2 = int.Parse((((DataRowView)e.Row.DataItem)["EmpId"]).ToString());
            string rm = (((DataRowView)e.Row.DataItem)["RM"]).ToString();
            string hod = (((DataRowView)e.Row.DataItem)["HOD"]).ToString();
            string status = (((DataRowView)e.Row.DataItem)["Status"]).ToString();
            string issubmitted = (((DataRowView)e.Row.DataItem)["IsSubmitted"]).ToString();
            string department = user.GetDepartmentByEmail(Session["EmailId"].ToString());

            DataTable departmentTable = objSQLDataFile.ExecuteDataTableSQL("SELECT DISTINCT department_name FROM department_head");

            List<string> departmentNames = new List<string>();
            foreach (DataRow row in departmentTable.Rows)
            {
                departmentNames.Add(row["department_name"].ToString());
            }

            if (empId2 == empId)
            {
                Button button = e.Row.Cells[8].FindControl("btnWithDraw") as Button;
                button.Visible = true;
            }
            if (name == rm)
            {
                Button button = e.Row.Cells[9].FindControl("btnApproved") as Button;
                Button button2 = e.Row.Cells[9].FindControl("btnReject") as Button;
                button.Visible = true;
                button2.Visible = true;
            }
            if (status == "Approved")
            {
                Button button = e.Row.Cells[9].FindControl("btnApproved") as Button;
                Button button2 = e.Row.Cells[9].FindControl("btnReject") as Button;
                button.OnClientClick = null;
               
                if (issubmitted == "True")
                {
                    button.Visible = true;
                    button.CssClass = "btn btn-sm btn-bitbucket";
                    button.Text = "View Clearance Form";
                    button.CommandName = "ViewClearanceForm";

                    button2.Visible = true;
                    button2.CssClass = "btn btn-sm btn-success";
                    button2.Text = "Download";
                    button2.CommandName = "Download";
                }
                else if (name == rm || name == hod || isHR.Equals("EMPLOYEEHR") || departmentNames.Contains(department))
                {
                    button.Visible = true;
                    button.CssClass = "btn btn-sm btn-info";
                    button.Text = "Clearance Form";
                    button.CommandName = "ClearanceForm";
                    button2.Visible = false;
                   
                }
            }
            else if (status == "Rejected")
            {
                Button button = e.Row.Cells[8].FindControl("btnWithDraw") as Button;
                Button button2 = e.Row.Cells[8].FindControl("btnselect") as Button;

                //need to check if these buttons to show or hide
                Button button3 = e.Row.Cells[9].FindControl("btnApproved") as Button;
                Button button4 = e.Row.Cells[9].FindControl("btnReject") as Button;

                button.Visible = false;
                button2.Visible = false;

                button3.Visible = false;
                button4.Visible = false;
            }
            e.Row.Cells[0].Text = Regex.Replace(e.Row.Cells[0].Text, txtsearchresignation.Text.Trim(), delegate (Match match)
            {
                return string.Format("<span style = 'background-color:#D9EDF7'>{0}</span>", match.Value);
            }, RegexOptions.IgnoreCase);

        }

    }
  [WebMethod]
  public static void AddRemarkAndApprove(int resingId, string remark)
  {
        #region Send Seperation mail to employee when approved by RM

        User user = new User();

        string employeeEmailAndDate = user.GetEmployeeEmailAndResignationDateByResignationId(resingId);

        string[] parts = employeeEmailAndDate.Split('|');
        string empname = parts[0];
        string employeeEmail = parts[1];
        string resignationDateString = parts[2];
        int noticePeriod = Convert.ToInt32(parts[3]);

        PassNMail pm = new PassNMail();
        pm.SendSeperationMailToEmp(employeeEmail, resignationDateString, noticePeriod);
        //test code to send mail to deptheads when approved
        List<string> departmentHeads = new List<string>();

        DataTable departmentHeadEmails = objSQLDataFile2.ExecuteDataTableSQL("SELECT department_head_email FROM department_head");

        foreach (DataRow row in departmentHeadEmails.Rows)
        {
            string email = row["department_head_email"].ToString();
            departmentHeads.Add(email);
        }
        #endregion

        objSQLDataFile2.ExecuteDataTableSQL(@"UPDATE Resignations set Status = 'Approved', Remarks = '"+remark+"' where ResignationId = " + resingId);
        pm.SendEmailToDepartmentHeads(empname, departmentHeads, resignationDateString);
    }
}
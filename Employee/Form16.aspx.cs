using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Form16 : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["EmailId"] == null || !(Session["EmailId"].ToString().Equals("hanifkhallikhan786@gmail.com")))
            {
                Response.Redirect("../Default.aspx");
            }
            bindgetEmployeeList();
        }
    }
    public void bindgetEmployeeList()
    {
        string str = txtsearchuser.Text.Trim();
        DataTable dt;
        if (str != "")
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  Id, EmpName, EmpDepartment, EmpDesignation 
from EmployeeMaster where Id not in (select EmpId from Form16 where Year = (datepart(year,getdate()))) and EmpName LIKE '%" + str + "%' order by EmpName");
            EmployeeList.DataSource = dt;
            EmployeeList.DataBind();
        }
        else
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  Id, EmpName, EmpDepartment, EmpDesignation 
from EmployeeMaster where Id not in (select EmpId from Form16 where Year = (datepart(year,getdate())))");
            EmployeeList.DataSource = dt;
            EmployeeList.DataBind();
        }

    }
    protected void btnSubmit_click(object sender, EventArgs e)
    {

        User user = new User();
        int res = 0, once = 0;
        foreach (GridViewRow row in EmployeeList.Rows)
        {
            FileUpload partA = (row.Cells[4].FindControl("partA") as FileUpload);
            FileUpload partB = (row.Cells[5].FindControl("partB") as FileUpload);

            try
            {

                if (partA.FileName != "" && partB.FileName != "")
                {
                    once = 1;

                    string filePath = Server.MapPath("../Form16Files/");
                    //string filePath2 = Server.MapPath("../Form16Files/");

                    string fileName1 = DateTime.Now.ToString("yyyyMMddHHmmss")+"_"+partA.PostedFile.FileName.ToString().Replace(" ","_").Trim();
                    string fileName2 = DateTime.Now.ToString("yyyyMMddHHmmss")+"_"+partB.PostedFile.FileName.ToString().Replace(" ", "_").Trim();
                    partA.PostedFile.SaveAs(filePath + fileName1);
                    //partA.PostedFile.SaveAs(filePath1);
                    //filePath2 = filePath2 + fileName2;
                    partB.PostedFile.SaveAs(filePath + fileName2);

                    int empId = Convert.ToInt32(EmployeeList.DataKeys[row.RowIndex].Values[0].ToString());
                    string PartA = filePath + fileName1;
                    string PartB = filePath + fileName2;

                    res = user.AddForm16(empId, fileName1, fileName2, Session["EmailId"].ToString());                   
                }
               
            }
            catch (Exception ex)
            {
                once += 1;
                string message = "Some Error Occured!";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<script>alert('");
                sb.Append(message);
                sb.Append("'); </script>");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
            }
        }
        if(once > 0)
        {
            ClientScriptManager CSM = Page.ClientScript;
            string strconfirm = "<script>if(window.confirm('File Uploaded')){window.location.href='Form16.aspx'}</script>";
            CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
        }
        else if(once > 1)
        {
            ClientScriptManager CSM = Page.ClientScript;
            string strconfirm = "<script>if(window.confirm('Files uploaded, without some files!')){window.location.href='Form16.aspx'}</script>";
            CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);

        }
        else if(once == 0)
        {
            string message = "Please select file(s)!";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script>alert('");
            sb.Append(message);
            sb.Append("'); </script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
        }

    }
    protected void Search(object sender, EventArgs e)
    {
        this.bindgetEmployeeList();
    }
    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        EmployeeList.PageIndex = e.NewPageIndex;
        this.bindgetEmployeeList();
    }
    protected void EmployeeList_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Text = Regex.Replace(e.Row.Cells[0].Text, txtsearchuser.Text.Trim(), delegate (Match match)
            {
                return string.Format("<span style = 'background-color:#D9EDF7'>{0}</span>", match.Value);
            }, RegexOptions.IgnoreCase);

        }
    }

    protected void EmployeeList_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
}
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_ESICLetter : System.Web.UI.Page
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
from EmployeeMaster where Id not in (select EmpId from ESICLetter where Year = (datepart(year,getdate()))) and EmpName LIKE '%" + str + "%' order by EmpName");
            EmployeeList.DataSource = dt;
            EmployeeList.DataBind();
        }
        else
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  Id, EmpName, EmpDepartment, EmpDesignation 
from EmployeeMaster where Id not in (select EmpId from ESICLetter where Year = (datepart(year,getdate())))");
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
            FileUpload IncLetter = (row.Cells[4].FindControl("IncLetter") as FileUpload);

            try
            {
                if (IncLetter.FileName != "")
                {

                    string filePath = Server.MapPath("../ESICLetter/");
                    //string filePath2 = Server.MapPath("../Form16Files/");

                    string fileName1 = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + IncLetter.PostedFile.FileName.ToString().Replace(" ", "_").Trim();
                    IncLetter.PostedFile.SaveAs(filePath + fileName1);

                    int empId = Convert.ToInt32(EmployeeList.DataKeys[row.RowIndex].Values[0].ToString());
                    //string incLetter = filePath + fileName1;

                    res = user.AddESICLetter(empId, fileName1, Session["EmailId"].ToString());
                    once = 1;
                }

            }
            catch (Exception ex)
            {
                //once += 1;
                string message = "Some Error Occured!";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<script>alert('");
                sb.Append(message);
                sb.Append("'); </script>");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
            }
        }
        if (once == 1)
        {
            ClientScriptManager CSM = Page.ClientScript;
            string strconfirm = "<script>if(window.confirm('File Uploaded')){window.location.href='ESICLetter.aspx'}</script>";
            CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
        }
        //else if(once == 1)
        //{
        //    ClientScriptManager CSM = Page.ClientScript;
        //    string strconfirm = "<script>if(window.confirm('Files uploaded, without some files!')){window.location.href='Form16.aspx'}</script>";
        //    CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
        //}
        else if (once == 0)
        {
            string message = "Please select file!";
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
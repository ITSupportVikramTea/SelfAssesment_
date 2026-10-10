using System;
using System.Collections;
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

public partial class Admin_HideVacancies : System.Web.UI.Page
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
            bindvacanylist();
            //updateVacnEmp();
        }
    }

    public void bindgetEmployeeList()
    {
        string str = txtsearch.Text.Trim();
        DataTable dt;
        if (str != "")
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT  * from EmployeeMaster where  EmpName LIKE '%" + str + "%' order by EmpName");
            EmployeeList.DataSource = dt;
            EmployeeList.DataBind();
        }
        else
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from EmployeeMaster order by EmpName");
            EmployeeList.DataSource = dt;
            EmployeeList.DataBind();
        }

    }
    protected void bindvacanylist()
    {
        string str = txtsearch.Text.Trim();
        DataTable dt;
        if (str == "")
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from AvailableVacancies ORDER BY Position");
        }
        else
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from AvailableVacancies where 
            Department LIKE '%" + str + "%' or Position LIKE '%" + str + "%' or HQ LIKE '%" + str + "%' ORDER BY Position");
        }

        gdvacancies.DataSource = dt;
        gdvacancies.DataBind();
    }
    //public void updateVacnEmp()
    //{
    //    if (Request.QueryString.AllKeys.Contains("id"))
    //    {

    //        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"select * from [dbo].[HiddenVacancies] where id=" + Request.QueryString["id"].ToString());

    //        if (dt.Rows.Count > 0)
    //        {
    //            int EmpId = int.Parse(dt.Rows[0]["EmpId"].ToString());
    //            int VacancyId = int.Parse(dt.Rows[0]["VacancyId"].ToString());

    //            DataTable dt2 = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from EmployeeMaster where Id=" + EmpId);
    //            DataTable dt3 = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from AvailableVacancies where Id=" + VacancyId);

    //            EmployeeList.DataSource = dt2;
    //            EmployeeList.DataBind();
    //            gdvacancies.DataSource = dt3;
    //            gdvacancies.DataBind();

    //            foreach (GridViewRow row in EmployeeList.Rows)
    //            {
    //                CheckBox chkRowEmp = (row.Cells[5].FindControl("emp_chkRow") as CheckBox);
    //                if (row.RowType == DataControlRowType.DataRow)
    //                {
    //                    chkRowEmp.Checked = true;
    //                }


    //            }
    //            foreach (GridViewRow row in gdvacancies.Rows)
    //            {
    //                CheckBox chkRowVac = (row.Cells[6].FindControl("vac_chkRow") as CheckBox);
    //                if (row.RowType == DataControlRowType.DataRow)
    //                {
    //                    chkRowVac.Checked = true;
    //                }
    //            }


    //            btnSubmit.Text = "Update";
    //        }
    //        else
    //            btnSubmit.Text = "Submit";
    //    }
    //}
    protected void btnusersearch_click(object sender, EventArgs e)
    {
        this.bindgetEmployeeList();
    }
    protected void btnvacancysearrch_click(object sender, EventArgs e)
    {
        this.bindvacanylist();
    }

    protected void btnSubmit_click(object sender, EventArgs e)
    {
        if (EmployeeList.Rows.Count > 0)
        {
            User user = new User();
            HideEmployeeVacancy ev = new HideEmployeeVacancy();
            ArrayList empIds = new ArrayList();
            ArrayList vacIds = new ArrayList();
            foreach (GridViewRow row in EmployeeList.Rows)
            {
                CheckBox chkRow = (row.Cells[5].FindControl("emp_chkRow") as CheckBox);
                if (row.RowType == DataControlRowType.DataRow)
                {
                    /*Taking all employees which are checked and adding to empIds array*/

                    if (chkRow.Checked)
                    {
                        int empId = Convert.ToInt32(EmployeeList.DataKeys[row.RowIndex].Value);
                        empIds.Add(empId);
                    }

                }
            }
            foreach (GridViewRow row in gdvacancies.Rows)
            {
                CheckBox chkRow = (row.Cells[6].FindControl("vac_chkRow") as CheckBox);
                if (row.RowType == DataControlRowType.DataRow)
                {
                    /*Taking all vacancies which are checked and adding to vacId array*/
                    if (chkRow.Checked)
                    {
                        int vacId = Convert.ToInt32(gdvacancies.DataKeys[row.RowIndex].Value);
                        vacIds.Add(vacId);
                    }

                }
            }

            int res = ev.addHiddenVacanciesToDB(empIds, vacIds, Session["EmailId"].ToString());
            if (res > 0)
            {
                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>alert('Vacanies Hided!');</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "alert", strconfirm, false);
            }
            else
            {
                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>alert('Already Hidden!');</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "alert", strconfirm, false);
            }

        }
    }

    protected void EmployeeList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //try
        //{
        //    int recordid = Convert.ToInt32(e.CommandName);
        //    Response.Redirect("AddEmployees.aspx?id=" + recordid);
        //}
        //catch { }

    }
    protected void EmployeeList_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Text = Regex.Replace(e.Row.Cells[0].Text, txtsearch.Text.Trim(), delegate (Match match)
            {
                return string.Format("<span style = 'background-color:#D9EDF7'>{0}</span>", match.Value);
            }, RegexOptions.IgnoreCase);

        }
    }
    protected void EmployeeList_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        EmployeeList.PageIndex = e.NewPageIndex;
        this.bindgetEmployeeList();
    }


    protected void gdvacancies_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //try
        //{
        //    int recordid = Convert.ToInt32(e.CommandName);
        //    Response.Redirect("AddVacancies.aspx?id=" + recordid);
        //}
        //catch { }

    }
    protected void OnPageIndexChanging_gdvacancies(object sender, GridViewPageEventArgs e)
    {
        gdvacancies.PageIndex = e.NewPageIndex;
        this.bindvacanylist();
    }
    protected void gdvacancies_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Text = Regex.Replace(e.Row.Cells[0].Text, txtsearch.Text.Trim(), delegate (Match match)
            {
                return string.Format("<span style = 'background-color:#D9EDF7'>{0}</span>", match.Value);
            }, RegexOptions.IgnoreCase);
        }
    }
}
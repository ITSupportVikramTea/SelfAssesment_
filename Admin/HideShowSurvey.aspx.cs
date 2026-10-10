using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_HideShowSurvey : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    static DataTable dtemplist;
    protected void Page_Load(object sender, EventArgs e)
    {
        SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();

        if (!IsPostBack)
        {
            if (Session["EmailId"] == null)
            {
                Response.Redirect("../Default.aspx");
            }
            //bindgetEmployeeList();
            bindgetgrdSurveyList();
            binddldepartments();
            binddldesignations();
            dtemplist = new DataTable();
            dtemplist.Columns.Add("SurveyTitle");
            dtemplist.Columns.Add("EmpName");
            dtemplist.Columns.Add("EmpId");
            dtemplist.Columns.Add("Id");
        }

    }

    //public void bindgetEmployeeList()
    //{
    //    string str = txtautosearch.Text.Trim();
    //    DataTable dt;
    //    if (str != "")
    //    {
    //        dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT  * from EmployeeMaster where  EmpName LIKE '%" + str + "%' order by EmpName");
    //        EmployeeList.DataSource = dt;
    //        EmployeeList.DataBind();
    //    }
    //    else
    //    {
    //        dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from EmployeeMaster order by EmpName");
    //        EmployeeList.DataSource = dt;
    //        EmployeeList.DataBind();
    //    }

    //}


    public void bindgetgrdSurveyList()
    {
        string str = txtautosearch.Text.Trim();
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT  * from Survey order by SurveyTitle");
        dlsurvey.DataSource = dt;
        dlsurvey.DataTextField = "SurveyTitle";
        dlsurvey.DataValueField = "id";
        dlsurvey.DataBind();

    }

    public void binddldepartments()
    {
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT * FROM Departments");
        dldepartments.DataSource = dt;
        dldepartments.DataTextField = "Departments";
        dldepartments.DataTextField = "Departments";
        dldepartments.DataBind();
    }
    public void binddldesignations()
    {
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"select distinct EmpDesignation from [dbo].[EmployeeMaster] where EmpDesignation != ''");
        dlDesg.DataSource = dt;
        dlDesg.DataTextField = "EmpDesignation";
        dlDesg.DataTextField = "EmpDesignation";
        dlDesg.DataBind();
    }

    public void searchemp(string empname)
    {
        if (empname.Trim() != "")
        {
            DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from EmployeeMaster where EmpName like '%" + empname + "%'");
            EmployeeList.DataSource = dt;
            EmployeeList.DataBind();
        }
        else
        {
            DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from EmployeeMaster  ");
            EmployeeList.DataSource = dt;
            EmployeeList.DataBind();
        }
    }

    protected void txtautosearch_TextChanged(object sender, EventArgs e)
    {
        searchemp(txtautosearch.Text);
    }


    protected void dlsurvey_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (dlsurvey.SelectedValue != "0")
        {
            DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT  * from Survey  where id=" + dlsurvey.SelectedValue);
            grdSurvey.DataSource = dt;
            grdSurvey.DataBind();
        }
        else
        {
            string message = "Select Survey";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script>alert('");
            sb.Append(message);
            sb.Append("'); </script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
        }
    }


    protected void btnAddEmp_Click(object sender, EventArgs e)
    {
        if (dlsurvey.SelectedValue != "0" && EmployeeList.Rows.Count > 0)
        {
            foreach (GridViewRow row in EmployeeList.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkRow") as CheckBox);
                    if (chkRow.Checked)
                    {

                        string empname = row.Cells[1].Text;
                        string lblEmployeeCode = (row.Cells[2].FindControl("lblEmpId") as Label).Text;


                        dtemplist.Rows.Add(dlsurvey.SelectedItem.Text, empname, lblEmployeeCode, dlsurvey.SelectedValue);
                    }
                }
            }
            grdAssignedSurvey.DataSource = dtemplist;
            grdAssignedSurvey.DataBind();
        }
        else
        {
            ClientScriptManager CSM = Page.ClientScript;
            string strconfirm = "<script>alert('Please Select Survey or Check all the fields!');</script>";
            CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
        }

    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        if (dlsurvey.SelectedValue != "0")
        {
            User user = new User();
            if (grdAssignedSurvey.Rows.Count > 0)
            {
                foreach (GridViewRow row in grdAssignedSurvey.Rows)
                {
                    string empId = (row.Cells[2].FindControl("EmpId") as Label).Text;
                    string SurveyId = (row.Cells[3].FindControl("SurveyId") as Label).Text;

                    user.AddSurveyUW(int.Parse(SurveyId), int.Parse(empId), Session["EmailId"].ToString());

                }

                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>if(window.confirm('Survey Assinged Successfully')){window.location.href='HideShowSurvey.aspx'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
            }
            else if (dlroles.Text != "" && dldepartments.Text != "")
            {
                int checkroles = int.Parse(dlroles.SelectedValue);
                //User user = new User();
                foreach (GridViewRow row in grdSurvey.Rows)
                {
                    string SurveyId = row.Cells[0].Text;

                    DataTable emps = null;
                    switch (checkroles)
                    {
                        case 1:
                            emps = objSQLDataFile.ExecuteDataTableSQL(@"select Id from[dbo].[EmployeeMaster] where IsHOD = 1 and EmpDepartment = '" + dldepartments.Text + "'");
                            break;
                        case 2:
                            emps = objSQLDataFile.ExecuteDataTableSQL(@"select Id from[dbo].[EmployeeMaster] where IsRM = 1 and EmpDepartment = '" + dldepartments.Text + "'");
                            break;
                        case 3:
                            emps = objSQLDataFile.ExecuteDataTableSQL(@"select Id from[dbo].[EmployeeMaster] where IsHOD = 0 and IsRM = 0 and EmpDepartment = '" + dldepartments.Text + "'");
                            break;
                    }
                    if (emps.Rows.Count > 0)
                    {
                        for (int i = 0; i < emps.Rows.Count; i++)
                        {
                            DataRow emprow = emps.Rows[i];
                            string EmpId = emprow["Id"].ToString();
                            user.AddSurveyUW(int.Parse(SurveyId), int.Parse(EmpId), Session["EmailId"].ToString());
                        }
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = " <script>if(window.confirm('Survey Assigned Successfully')){window.location.href='HideShowSurvey.aspx'}</script>";
                        CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                    }
                    else
                    {
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = " <script>if(window.confirm('There is some Error Please try again')){window.location.href='HideShowSurvey.aspx'}</script>";
                        CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                    }
                }
            }

            else if (dlDesg.Text != "")
            {

                //User user = new User();
                foreach (GridViewRow row in grdSurvey.Rows)
                {
                    string SurveyId = row.Cells[0].Text;

                    DataTable emps = null;

                    emps = objSQLDataFile.ExecuteDataTableSQL(@"select Id from[dbo].[EmployeeMaster] where EmpDesignation = '" + dlDesg.Text + "' and cast(DOJ as date) < cast('2021-12-31' as date) and DOJ != ''");


                    if (emps.Rows.Count > 0)
                    {
                        for (int i = 0; i < emps.Rows.Count; i++)
                        {
                            DataRow emprow = emps.Rows[i];
                            string EmpId = emprow["Id"].ToString();
                            user.AddSurveyUW(int.Parse(SurveyId), int.Parse(EmpId), Session["EmailId"].ToString());
                        }
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = " <script>if(window.confirm('Survey Assigned Successfully')){window.location.href='HideShowSurvey.aspx'}</script>";
                        CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                    }
                    else
                    {
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = " <script> alert('Employees not found'); </script>";
                        CSM.RegisterClientScriptBlock(this.GetType(), "Alert", strconfirm, false);
                    }

                }
            }
        }


        else
        {
            ClientScriptManager CSM = Page.ClientScript;
            string strconfirm = "<script>alert('Please Select Survey or Check all the fields!');</script>";
            CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
        }
    }



    protected void btnaddsurvey_Click(object sender, EventArgs e)
    {
        Response.Redirect("Survey.aspx");
    }

    protected void btnback_Click(object sender, EventArgs e)
    {
        Response.Redirect("HideShowSurveyList.aspx");
    }

    protected void dlroles_TextChanged(object sender, EventArgs e)
    {
        dlDesg.Visible = false;
        txtautosearch.Visible = false;
        btnAddEmp.Visible = false;
    }

    protected void dlDesg_TextChanged(object sender, EventArgs e)
    {
        dlroles.Visible = false;
        dldepartments.Visible = false;
        txtautosearch.Visible = false;
        btnAddEmp.Visible = false;

    }

}
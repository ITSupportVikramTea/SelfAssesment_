using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webpages_AddKRAUserwise : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    static DataTable dtemplist;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["EmailId"] == null || Session["UserType"].ToString().ToLower() != "employeehr")
            {
                Response.Redirect("../Default.aspx");
            }
            binddrpKPI();
            binddldepartments();
            binddldesignations();
            dtemplist = new DataTable();
            dtemplist.Columns.Add("KPI");
            dtemplist.Columns.Add("EmpName");
            dtemplist.Columns.Add("EmpId");
            dtemplist.Columns.Add("Id");

        }

    }
    public void binddrpKPI()
    {

        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from KRAMaster  ");
        dlKpi.DataSource = dt;
        dlKpi.DataTextField = "KPI";
        dlKpi.DataValueField = "id";
        dlKpi.DataBind();
        //dlKpi.Items.Add(new ListItem("Sleect KPI", "0"));


    }
    public void binddldepartments()
    {
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"select distinct EmpDepartment from [dbo].[EmployeeMaster] where EmpDepartment != ''");
        dldepartments.DataSource = dt;
        dldepartments.DataTextField = "EmpDepartment";
        dldepartments.DataTextField = "EmpDepartment";
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
    protected void dlKpi_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (dlKpi.SelectedValue != "0")
        {
            DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from KRAMaster  where id=" + dlKpi.SelectedValue);
            grdkpi.DataSource = dt;
            grdkpi.DataBind();
        }
        else
        {
            string message = "Select KPI";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script>alert('");
            sb.Append(message);
            sb.Append("'); </script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
        }
    }
    protected void btnAddEmp_Click(object sender, EventArgs e)
    {
        if (dlKpi.SelectedValue != "0")
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


                        dtemplist.Rows.Add(dlKpi.SelectedItem.Text, empname, lblEmployeeCode, dlKpi.SelectedValue);
                    }
                }
            }
            grdAssignKPI.DataSource = dtemplist;
            grdAssignKPI.DataBind();
        }
        else
        {
            ClientScriptManager CSM = Page.ClientScript;

            string strconfirm = "<script>alert('Please Select KPI');</script>";
            CSM.RegisterClientScriptBlock(this.GetType(), "Alert", strconfirm, false);

        }

    }
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        if(dlKpi.SelectedValue != "0")
        {
            if (grdAssignKPI.Rows.Count > 0)
            {
                User user = new User();
                foreach (GridViewRow row in grdAssignKPI.Rows)
                {
                    string empId = (row.Cells[3].FindControl("EmployeeId") as Label).Text;
                    string kpiId = (row.Cells[3].FindControl("KpiId") as Label).Text;
                    DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT KPI,Target,Weightage, UOM from KRAMaster  where id=" + kpiId);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow row2 = dt.Rows[0];

                        string KPI = row2["KPI"].ToString();

                        float Weightage = 0;
                        try
                        {
                            Weightage = float.Parse(row2["Weightage"].ToString());

                        }
                        catch
                        {

                        }

                        float Target = 0;
                        try
                        {
                            Target = float.Parse(row2["Target"].ToString());

                        }
                        catch
                        {

                        }



                        string UOM = row2["UOM"].ToString();

                        user.AddKRAUW(empId, KPI, Target, Weightage, UOM, Session["EmailId"].ToString());
                    }

                }
                ClientScriptManager CSM = Page.ClientScript;

                string strconfirm = "<script>if(window.confirm('KPI Assinged Successfully to Selected Employees')){window.location.href='KpiList.aspx'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
            }
            else if (dlroles.Text != "" && dldepartments.Text != "")
            {
                int checkroles = int.Parse(dlroles.SelectedValue);
                User user = new User();
                foreach (GridViewRow row in grdkpi.Rows)
                {
                    string KPI = row.Cells[0].Text;
                    float Weightage = 0;
                    try
                    {
                        Weightage = float.Parse(row.Cells[2].Text);

                    }
                    catch
                    {

                    }
                    float Target = 0;
                    try
                    {
                        Target = float.Parse(row.Cells[1].Text);

                    }
                    catch
                    {

                    }
                    string UOM = row.Cells[3].Text; ;
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
                            user.AddKRAUW(EmpId, KPI, Target, Weightage, UOM, Session["EmailId"].ToString());
                        }
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = " <script>if(window.confirm('KPI Assigned Successfully')){window.location.href='KpiList.aspx'}</script>";
                        CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                    }
                    else
                    {
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = " <script>if(window.confirm('Error occured')){window.location.href='AddKRAUserwise.aspx'}</script>";
                        CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                    }

                }
            }

            else if (dlDesg.Text != "")
            {

                User user = new User();
                foreach (GridViewRow row in grdkpi.Rows)
                {
                    string KPI = row.Cells[0].Text;
                    float Weightage =  0;
                    try
                    {
                        Weightage = float.Parse(row.Cells[2].Text);

                    }
                    catch
                    {

                    }

                    float Target = 0;
                    try
                    {
                        Target = float.Parse(row.Cells[1].Text);

                    }
                    catch
                    {

                    }

                    string UOM = row.Cells[3].Text; ;
                    DataTable emps = null;

                    emps = objSQLDataFile.ExecuteDataTableSQL(@"select Id from[dbo].[EmployeeMaster] where EmpDesignation = '" + dlDesg.Text + "'");


                    if (emps.Rows.Count > 0)
                    {
                        for (int i = 0; i < emps.Rows.Count; i++)
                        {
                            DataRow emprow = emps.Rows[i];
                            string EmpId = emprow["Id"].ToString();
                            user.AddKRAUW(EmpId, KPI, Target, Weightage, UOM, Session["EmailId"].ToString());
                        }
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = " <script>if(window.confirm('KPI Assigned Successfully')){window.location.href='KpiList.aspx'}</script>";
                        CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                    }
                    else
                    {
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = " <script>if(window.confirm('Employees not found')){window.location.href='AddKRAUserwise.aspx'}</script>";
                        CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                    }

                }
            }
        }
       
        else
        {
            ClientScriptManager CSM = Page.ClientScript;

            string strconfirm = "<script>alert('Select KPI');</script>";
            CSM.RegisterClientScriptBlock(this.GetType(), "Alert", strconfirm, false);

        }
    }
    protected void btnaddkra_click(object sender, EventArgs e)
    {
        Response.Redirect("AddKRA.aspx");
    }
    protected void btnkpilist_click(object sender, EventArgs e)
    {
        Response.Redirect("KpiList.aspx");
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
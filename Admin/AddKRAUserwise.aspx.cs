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
            if (Session["EmailId"] == null)
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
        if (dlKpi.SelectedValue != "0")
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
                    //old query
                    //emps = objSQLDataFile.ExecuteDataTableSQL(@"select Id from [dbo].[EmployeeMaster] where EmpDesignation = '" + dlDesg.Text + "' and cast(DOJ as date) < cast('2021-12-31' as date) and DOJ != ''");

                    //condition to assign kpis to emp designation wise whose doj between 01-01-2022 and 31-12-2022 and status = active
                    emps = objSQLDataFile.ExecuteDataTableSQL(@"select Id from [dbo].[EmployeeMaster] where EmpDesignation = '" + dlDesg.Text + "' and CAST(DOJ AS DATE) BETWEEN CAST('2022-01-01' AS DATE) AND CAST('2022-12-31' AS DATE) and DOJ != '' and Status = 'Active'");

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


        //test code 
        else if (dlKpi.SelectedValue == "0")
        {

            User user = new User();
            if (dlDesg.Text != "" && dlDesg.SelectedValue != "")
            {
                string designation = dlDesg.Text;

                // Get the KPIs assigned to the selected designation
                DataTable kpis = objSQLDataFile.ExecuteDataTableSQL(@"SELECT distinct KPI, Target, Weightage, UOM FROM KRAUserwise kuw
                                                        JOIN EmployeeMaster emp ON kuw.EmpId = emp.Id
                                                        WHERE emp.EmpDesignation = '" + designation + "' AND kuw.Year = YEAR(GETDATE()) - 1");

                if (kpis.Rows.Count > 0)
                {
                    foreach (GridViewRow row in grdkpi.Rows)
                    {
                        string KPI = row.Cells[0].Text;
                        //float Target = 0;
                        //float Weightage = 0;
                        // string UOM = "";

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
                        // Find the corresponding KPI details from the DataTable
                        /* DataRow[] matchingRows = kpis.Select("KPI = '" + KPI + "'");
                         if (matchingRows.Length > 0)
                         {
                             DataRow kpiRow = matchingRows[0];
                             Target = float.Parse(kpiRow["Target"].ToString());
                             Weightage = float.Parse(kpiRow["Weightage"].ToString());
                             UOM = kpiRow["UOM"].ToString();
                         }*/

                        DataTable emps = null;

                        // Get the employees with the selected designation
                        emps = objSQLDataFile.ExecuteDataTableSQL(@"SELECT Id FROM [dbo].[EmployeeMaster] WHERE EmpDesignation = '" + designation + "' AND CAST(DOJ AS DATE) BETWEEN CAST('2022-01-01' AS DATE) AND CAST('2022-12-31' AS DATE) AND DOJ != '' AND Status = 'Active'");

                        if (emps.Rows.Count > 0)
                        {
                            foreach (DataRow emprow in emps.Rows)
                            {
                                string EmpId = emprow["Id"].ToString();

                                // Assign the KPI to each employee
                                user.AddKRAUW(EmpId, KPI, Target, Weightage, UOM, Session["EmailId"].ToString());
                            }

                            // Show success message
                            ClientScriptManager CSM = Page.ClientScript;
                            string strconfirm = " <script>if(window.confirm('KPI Assigned Successfully')){window.location.href='KpiList.aspx'}</script>";
                            CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                        }
                        else
                        {
                            // Show error message if no employees with the selected designation are found
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


    protected void dlDesg_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (dlDesg.SelectedValue != "")
        {
            string designation = dlDesg.SelectedValue;

           /* DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT distinct KPI, Target, Weightage, UOM FROM KRAUserwise kuw
                                                        JOIN EmployeeMaster emp ON kuw.EmpId = emp.Id
                                                        WHERE emp.EmpDesignation = '" + designation + "' AND kuw.Year = YEAR(GETDATE()) - 1");*/

            //test code to fetch target , weightage , uom as blank 

            DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT KPI, '' AS Target, '' AS Weightage, '' AS UOM
FROM KRAUserwise kuw
JOIN EmployeeMaster emp ON kuw.EmpId = emp.Id
WHERE emp.EmpDesignation = '" + designation + "' AND kuw.Year = YEAR(GETDATE()) - 1");

            grdkpi.DataSource = dt;
            grdkpi.DataBind();

        }
    }
}
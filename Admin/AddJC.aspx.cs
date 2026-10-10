using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_AddJC : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    static DataTable kpiList;
    static string checkDL = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["EmailId"] == null)
            {
                Response.Redirect("../Default.aspx");
            }
            binddldepartments();
            binddlemployees();
            binddlKPI();
           

        }

    }

    public void binddlKPI()
    {
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"select distinct * from [dbo].[KRAMaster]");
        dlKPI.DataSource = dt;
        dlKPI.DataTextField = "KPI";
        dlKPI.DataValueField = "Id";
        dlKPI.DataBind();
    }


    public void binddldepartments()
    {
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"select distinct EmpDepartment from [dbo].[EmployeeMaster] where EmpDepartment != ''");
        dldepartments.DataSource = dt;
        dldepartments.DataTextField = "EmpDepartment";
        dldepartments.DataTextField = "EmpDepartment";
        dldepartments.DataBind();
    }

    public void binddlemployees()
    {
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"select distinct Id,EmpName from [dbo].[EmployeeMaster]");
        dlemployees.DataSource = dt;
        dlemployees.DataTextField = "EmpName";
        dlemployees.DataValueField = "Id";
        dlemployees.DataBind();
    }

    public void bindgrdkpi(string empid)
    {
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from KRAUserwise where EmpId="+empid);

        foreach (DataRow row in dt.Rows)
        {
            kpiList.Rows.Add(row["Id"], row["KPI"]);
        }

        grdkpi.DataSource = kpiList;
        grdkpi.DataBind();
        //dlKpi.Items.Add(new ListItem("Sleect KPI", "0"));


    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {

    }

    protected void btnRemove_Click(object sender, EventArgs e)
    {

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            User user = new User();
            string CreatedBy = Session["EmailId"].ToString();
            string JCType = dljcs.SelectedValue;
            if (checkDL == "employee")
            {
                
                int EmpId = int.Parse(dlemployees.SelectedValue.ToString());
                

                foreach (GridViewRow row in grdkpi.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        string KPI = row.Cells[0].Text;
                        user.AddJCKPI(EmpId, KPI, CreatedBy, JCType);

                    }
                }
            }
            else if(checkDL == "department")
            {
                int jc = 1;

                if (jc1.Checked)
                    jc = 1;
                else if (jc2.Checked)
                    jc = 2;
                else if (jc3.Checked)
                    jc = 3;
                else if (jc4.Checked)
                    jc = 4;

                string Department = dldepartments.SelectedValue;
                user.AddJCKPIDepartmentWise(Department, CreatedBy, int.Parse(JCType),jc);

            }
            

            ClientScriptManager CSM = Page.ClientScript;
            string strconfirm = "<script>if(window.confirm('JC Added')){window.location.href='AddJC.aspx'}</script>";
            CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);

        }
        catch(Exception ex)
        {

            ClientScriptManager CSM = Page.ClientScript;
            string strconfirm = "<script>if(window.confirm('Some Error, Occured')){window.location.href='AddJC.aspx'}</script>";
            CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
        }
    }

    protected void dlemployees_TextChanged(object sender, EventArgs e)
    {
        checkDL = "employee";
        kpiList = new DataTable();
        kpiList.Columns.Add("Id");
        kpiList.Columns.Add("KPI");
        bindgrdkpi(dlemployees.SelectedValue);
        dvdep.Visible = false;
    }

    protected void dldepartments_TextChanged(object sender, EventArgs e)
    {
        dvEmp.Visible = false;
        checkDL = "department";
    }

    protected void grdkpi_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Remove")
        {
            int row = int.Parse(e.CommandArgument.ToString());
            kpiList.Rows.RemoveAt(row);
            kpiList.AcceptChanges();
            grdkpi.DataSource = kpiList;
            grdkpi.DataBind();

        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        kpiList.Rows.Add(dlKPI.SelectedItem.Value, dlKPI.SelectedItem.Text);
        grdkpi.DataSource = kpiList;
        grdkpi.DataBind();
    }
}
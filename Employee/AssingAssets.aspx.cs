using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_AssingAssets : System.Web.UI.Page
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
            binddrpdlAssets();
            dtemplist = new DataTable();
            dtemplist.Columns.Add("AssetName");
            dtemplist.Columns.Add("EmpName");
            dtemplist.Columns.Add("EmpId");
            dtemplist.Columns.Add("AssetId");
        }
    }

    public void binddrpdlAssets()
    {
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"select * from [dbo].[CompanyAssetList]");
        dlAssets.DataSource = dt;
        dlAssets.DataTextField = "AssetName";
        dlAssets.DataValueField = "AssetId";
        dlAssets.DataBind();
        dlAssets.Items.Add(new ListItem("Select Assets", "0"));
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
    protected void dlAssets_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (dlAssets.SelectedValue != "0")
        {
            DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from CompanyAssetList  where AssetId=" + dlAssets.SelectedValue);
            grdAssets.DataSource = dt;
            grdAssets.DataBind();
        }
        else
        {
            string message = "Select Assets";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script>alert('");
            sb.Append(message);
            sb.Append("'); </script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
        }
    }

    protected void btnAddEmp_Click(object sender, EventArgs e)
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


                    dtemplist.Rows.Add(dlAssets.SelectedItem.Text, empname, lblEmployeeCode, dlAssets.SelectedValue);
                }
            }
        }
        grdAssingAssets.DataSource = dtemplist;
        grdAssingAssets.DataBind();
    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        if (grdAssingAssets.Rows.Count > 0)
        {
            User user = new User();
            foreach (GridViewRow row in grdAssingAssets.Rows)
            {
                string empId = (row.Cells[3].FindControl("EmployeeId") as Label).Text;
                string assetId = (row.Cells[3].FindControl("AssetId") as Label).Text;
                DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT AssetName from CompanyAssetList where AssetId=" + assetId);
                if (dt.Rows.Count > 0)
                {
                    DataRow row2 = dt.Rows[0];

                    string asset = row2["AssetName"].ToString();
                    //string Target = row2["Target"].ToString();

                    objSQLDataFile.ExecuteDataTableSQL(@"INSERT INTO CompanyAssesstsAssigned(EmpId,AssesstName, AddedBy) VALUES(" + empId + ", '" + asset + "', '"+ Session["EmailId"].ToString() + "' )");
                }
            }
            ClientScriptManager CSM = Page.ClientScript;

            string strconfirm = "<script>if(window.confirm('Assets Assinged Successfully to Selected Employees')){window.location.href='AssignedAssetsList.aspx'}</script>";
            CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
        }
        else
        {
            ClientScriptManager CSM = Page.ClientScript;

            string strconfirm = "<script>alert('Check Fields and Try Again!')</script>";
            CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
        }
    }
    protected void assgnAssets_Click(object sender, EventArgs e)
    {
        Response.Redirect("AssignedAssetsList.aspx");
    }
}
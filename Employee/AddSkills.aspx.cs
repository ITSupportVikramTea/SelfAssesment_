using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webpages_AddSkills : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["EmailId"] == null)
            {
                Response.Redirect("../Default.aspx");
            }
            //binddeparment();
            //binddesignations();
            //bindhodrm();
            //bindgetEmployeeList();
            btnupdate.Visible = false;
        }
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        //User addusdr = new User();
        //if (txtfullname.Text != "")
        //{
        //    int checkStatus = addusdr.AddEmployee(txtfullname.Text, txtemailid.Text, dldesignation.Text,
        //       dlDepartment.Text, dlstatus.Text, Session["EmailId"].ToString(),
        //       DateTime.Now, Session["EmailId"].ToString(), DateTime.Now, dlhod.Text, dlrm.Text);
        //    if (checkStatus > 0)
        //    {
        //        string message = "Employee Added Successfully";
        //        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //        sb.Append("<script>alert('");
        //        sb.Append(message);
        //        sb.Append("'); </script>");
        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
        //        //txtempcode.Text = "";
        //        txtfullname.Text = "";
        //    }
        //}


    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        //User addusdr = new User();
        //if (txtfullname.Text != "" && empid.Value != "")
        //{
        //    //int checkStatus = addusdr.UpdateEmployee(Convert.ToInt32(empid.Value), txtempcode.Text, txtfullname.Text, dlDepartment.Text, dlrole.Text, dllocation.Text, Session["username"].ToString());
        //    if (checkStatus > 0)
        //    {
        //        string message = "Employee Updated Successfully";
        //        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //        sb.Append("<script>alert('");
        //        sb.Append(message);
        //        sb.Append("'); </script>");
        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
        //        txtempcode.Text = "";
        //        txtfullname.Text = "";
        //        empid.Value = "";
        //        btnsave.Visible = true;
        //        btnupdate.Visible = false;
        //        bindgetEmployeeList();
        //    }
        //}

    }

    protected void EmployeeList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //dlDepartment.ClearSelection();
        ////dllocation.ClearSelection();
        //dldesignation.ClearSelection();

        ////Determine the RowIndex of the Row whose Button was clicked.
        //int rowIndex = Convert.ToInt32(e.CommandArgument);

        ////Reference the GridView Row.
        //GridViewRow row = EmployeeList.Rows[rowIndex];
        //empid.Value = e.CommandName;

        ////txtempcode.Text = row.Cells[0].Text;
        //txtfullname.Text = row.Cells[1].Text;
        ////dlDepartment.SelectedItem.Value = row.Cells[2].Text.ToUpper();
        //dlDepartment.Items.FindByText(row.Cells[2].Text.ToUpper()).Selected = true;
        //dldesignation.Items.FindByText(row.Cells[3].Text.ToUpper()).Selected = true;
        ////dllocation.Items.FindByText(row.Cells[4].Text.ToUpper()).Selected = true;

        btnsave.Visible = false;
        btnupdate.Visible = true;

    }
}
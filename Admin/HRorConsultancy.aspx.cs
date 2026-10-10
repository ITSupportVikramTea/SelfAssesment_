using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_HRorContaltancy : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["EmailId"] == null)
            {
                Response.Redirect("../Default.aspx");
            }
        }
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        User addusdr = new User();
        if (txtname.Text != "" && txtemailid.Text != "")
        {
            int checkStatus = 0;

            checkStatus = addusdr.AddConsultancy(txtname.Text, txtemailid.Text, txtlocation.Text, Session["EmailId"].ToString(), dltype.Text);


            if (checkStatus > 0)
            {
                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>if(window.confirm('Consultancy Added Successfully')){window.location.href='EmployeeList.aspx'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
            }
            else
            {
                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>alert('There is some Error');</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                txtemailid.Text = "";
            }
        }

    }
    protected void btncancel_click(object sender, EventArgs e)
    {
        Response.Redirect("HRConsultancyList.aspx");
    }
}
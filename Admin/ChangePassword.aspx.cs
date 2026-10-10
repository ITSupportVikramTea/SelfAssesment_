using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_ChangePassword : System.Web.UI.Page
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
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        if (txtpass1.Text != "" && txtpass1.Text == txtpass2.Text)
        {
            DataTable dt = objSQLDataFile.ExecuteDataTableSQL("select * from  UserMaster where EmailId='" + Session["EmailId"].ToString() + "' and Password = '" + txtoldpass.Text + "'");
            if (dt.Rows.Count > 0)
            {
                objSQLDataFile.ExecuteDataTableSQL("UPDATE UserMaster set Password='" + txtpass1.Text + "' WHERE EmailId='" + Session["EmailId"].ToString() + "'");

                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>if(window.confirm('Password Changed,Please Login Again!')){window.location.href='ChangePassword.aspx'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);

            }
            else
            {
                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>if(window.confirm('Old Password does not match!')){window.location.href='ChangePassword.aspx'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);

            }
        }
        else
        {
            ClientScriptManager CSM = Page.ClientScript;
            string strconfirm = "<script>if(window.confirm('Please try again!')){window.location.href='ChangePassword.aspx'}</script>";
            CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);

        }

    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_HRAddConsultancy : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["EmailId"] == null || Session["UserType"].ToString().ToLower() != "employeehr")
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
            DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT * FROM ConsultancyMaster  where ConsName = '"+txtname.Text+"' or EmailId = '"+txtemailid.Text+"'");
            int checkStatus = 0;

            if(dt.Rows.Count > 0)
            {
                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>alert('Already Addedd');</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
            }
            else
            {
                checkStatus = addusdr.AddConsultancy(txtname.Text, txtemailid.Text, txtlocation.Text, Session["EmailId"].ToString(), dltype.Text, "Active"); //Add Status dropdown in UI


                if (checkStatus > 0)
                {
                    ClientScriptManager CSM = Page.ClientScript;
                    string strconfirm = "<script>if(window.confirm('Consultancy Added Successfully')){window.location.href='HRConsultancyList.aspx'}</script>";
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

    }
    protected void btncancel_click(object sender, EventArgs e)
    {
        Response.Redirect("DashboardEmployee.aspx");
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ForgotPassword : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        { }
    }

    //pending apply after disscussion
    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        string username = string.Empty;
        string password = string.Empty;
        string email = txtemail.Text;
        //string message = "Dear Employee, You will get an email shortly.";

        if (email != "")
        {
            DataTable res = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT EmpEmailId from EmployeeMaster WHERE EmpEmailId = '"+email+"'");
            if(res.Rows.Count > 0)
            {
                PassNMail mail = new PassNMail();
                string email2 = res.Rows[0]["EmpEmailId"].ToString();
                mail.sendEmail(email2);
            }
            ClientScriptManager CSM = Page.ClientScript;
            string strconfirm = "<script>if(window.confirm('If email exists, You will get an email shortly.')){window.location.href='Default.aspx'}</script>";
            CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
        }

    }
}
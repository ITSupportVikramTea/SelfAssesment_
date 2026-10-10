using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Survey : System.Web.UI.Page
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
        try
        {
            string surevey_title = txtsureveytitle.Text;
            string exprires = txtexpiores.Text;
            string description = txtdesc.Text;

            objSQLDataFile.ExecuteDataTableSQL(@"INSERT INTO Survey(SurveyTitle, SurveyDescription, ExpiresOn, CreatedBy) VALUES('" + surevey_title + "','" + description + "', CONVERT(date,'" + exprires + "',105), '" + Session["EmailId"].ToString() + "')");


            ClientScriptManager CSM = Page.ClientScript;
            string strconfirm = "<script>if(window.confirm('Survey Added')){window.location.href='SurveyList.aspx'}</script>";
            CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);

        }
        catch
        {

        }

    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("SurveyList.aspx");
    }
}
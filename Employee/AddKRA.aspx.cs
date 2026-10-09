using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_AddKRA : System.Web.UI.Page
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
            bidndluom();
            if (Request.QueryString.AllKeys.Contains("id"))
            {
                DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from KRAMaster where id=" + Request.QueryString["id"].ToString());
                if (dt.Rows.Count > 0)
                {
                    txtkpi.Text = dt.Rows[0]["KPI"].ToString();
                    txttarget.Text = dt.Rows[0]["Target"].ToString();
                    dluom.Text = dt.Rows[0]["UOM"].ToString();
                    txtweightage.Text = dt.Rows[0]["Weightage"].ToString();
                    btnsave.Text = "Update";
                }
                else
                    btnsave.Text = "Save";
            }
        }
    }
    protected void bidndluom()
    {
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from UOMList ");
        dluom.DataSource = dt;
        dluom.DataMember = "UOM";
        dluom.DataTextField = "UOM";
        dluom.DataValueField = "UOM";
        dluom.DataBind();
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        User obj = new User();
        try
        {
            if (txtkpi.Text != "")
            {
                int checkStatus = 0;
                if (btnsave.Text == "Save")
                {
                    try
                    {
                        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"INSERT INTO KRAMaster(KPI, CreatedBy) values('" + txtkpi.Text + "', '" + Session["EmailId"].ToString() + "')");


                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = "<script>if(window.confirm('KPI Added Successfully')){window.location.href='KRAList.aspx'}</script>";
                        CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                    }
                    catch
                    {
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = "<script>alert('There is some Error');</script>";
                        CSM.RegisterClientScriptBlock(this.GetType(), "Alert", strconfirm, false);
                    }

                }
                else if (btnsave.Text == "Update")
                {
                    checkStatus = obj.UpdateKRA(txtkpi.Text, txttarget.Text, float.Parse(txtweightage.Text),
                       dluom.Text, Session["EmailId"].ToString(), Request.QueryString["id"].ToString());
                    if (checkStatus > 0)
                    {
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = "<script>if(window.confirm('KPI Updated Successfully')){window.location.href='KRAList.aspx'}</script>";
                        CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                    }
                }
                else
                {
                    ClientScriptManager CSM = Page.ClientScript;
                    string strconfirm = "<script>alert('There is some Error');</script>";
                    CSM.RegisterClientScriptBlock(this.GetType(), "Alert", strconfirm, false);
                }
            }
            else
            {
                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>if(window.confirm('Please check fields')){window.location.href='AddKRA.aspx'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
            }
        }
        catch
        {

        }

    }
    protected void btncancel_click(object sender, EventArgs e)
    {
        Response.Redirect("KRAList.aspx");
    }
}

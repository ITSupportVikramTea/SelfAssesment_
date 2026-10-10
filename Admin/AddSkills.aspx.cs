using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webpages_AddSkills : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
        if (!IsPostBack)
        {
            if (Session["EmailId"] == null)
            {
                Response.Redirect("../Default.aspx");
            }
            if (Request.QueryString.AllKeys.Contains("id"))
            {
                DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from Skills where id=" + Request.QueryString["id"].ToString());
                if (dt.Rows.Count > 0)
                {
                    txtcompetency.Text = dt.Rows[0]["Competency"].ToString();
                    txtmgmtskills.Text = dt.Rows[0]["ManagementSkills"].ToString();
                    txtdesc.Text = dt.Rows[0]["Description"].ToString();
                    //txtemailid.Text = dt.Rows[0]["EmailId"].ToString();
                    //txtpromodate.Text = dt.Rows[0]["LastPromoYear"].ToString();
                    //dlhod.Text = dt.Rows[0]["HOD"].ToString();
                    //dlrm.Text = dt.Rows[0]["RM"].ToString();
                    //dldesignation.Text = dt.Rows[0]["EmpDesignation"].ToString();
                    //dlDepartment.Text = dt.Rows[0]["EmpDepartment"].ToString();
                    //dlstatus.Text = dt.Rows[0]["Status"].ToString();
                    //desgchangedate.Text = dt.Rows[0]["DesgChangeDate"].ToString();
                    //existingctc.Text = dt.Rows[0]["ExistingCTC"].ToString();
                    btnsave.Text = "Update";
                }
                else
                    btnsave.Text = "Save";
            }
        }
    }
    //create sp for update skills
    protected void btnsave_click(object sender, EventArgs e)
    {
        User addusdr = new User();
        if (txtcompetency.Text != "")
        {
            int checkStatus = 0;
            if (btnsave.Text == "Save")
            {
                //checkStatus = addusdr.AddSkills(txtcompetency.Text, txtmgmtskills.Text, txtdesc.Text, txtemailid.Text,Session["EmailId"].ToString());
                checkStatus = addusdr.AddSkills(txtcompetency.Text, txtmgmtskills.Text, txtdesc.Text, Session["EmailId"].ToString());
                if (checkStatus > 0)
                {
                    ClientScriptManager CSM = Page.ClientScript;
                    string strconfirm = "<script>if(window.confirm('Skill Added Successfully')){window.location.href='SkillsList.aspx'}</script>";
                    CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                }
            }
            else if (btnsave.Text == "Update")
            {
               /* checkStatus = addusdr.UpdateSkills(int.Parse(Request.QueryString["id"].ToString()), txtcompetency.Text, txtmgmtskills.Text, 
                    txtdesc.Text, txtemailid.Text);*/ //removed empemailid col from all updateskills functions and sp(in db also)

                checkStatus = addusdr.UpdateSkills(int.Parse(Request.QueryString["id"].ToString()), txtcompetency.Text, txtmgmtskills.Text,
                   txtdesc.Text);

                if (checkStatus > 0)
                {
                    ClientScriptManager CSM = Page.ClientScript;
                    string strconfirm = "<script>if(window.confirm('Skill Updated Successfully')){window.location.href='SkillsList.aspx'}</script>";
                    CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                }
            }

            else
            {

            }
        }
    }
    protected void btncancel_click(object sender, EventArgs e)
    {
        Response.Redirect("SkillsList.aspx");
    }

}
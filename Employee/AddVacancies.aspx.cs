using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HR_AddVacancies : System.Web.UI.Page
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
            //binddeparment();
            if (Request.QueryString.AllKeys.Contains("id"))
            {

                DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from AvailableVacancies where id=" + Request.QueryString["id"].ToString());
                if (dt.Rows.Count > 0)
                {
                    dlDepartment.Text = dt.Rows[0]["Department"].ToString();
                    txtposition.Text = dt.Rows[0]["Position"].ToString();
                    txtnoofvacancies.Text = dt.Rows[0]["NoOfVC"].ToString();
                    txtzone.Text = dt.Rows[0]["ZONE"].ToString();
                    txtsdm.Text = dt.Rows[0]["SDM"].ToString();
                    dlhq.Text = dt.Rows[0]["HQ"].ToString();
                    
                    btnsave.Text = "Update";
                }
                else
                    btnsave.Text = "Save";
            }
        }
    }
    //public void binddeparment()
    //{

    //    DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from Departments ");
    //    dlDepartment.DataSource = dt;
    //    dlDepartment.DataMember = "Departments";
    //    dlDepartment.DataTextField = "Departments";
    //    dlDepartment.DataValueField = "Departments";
    //    dlDepartment.DataBind();
    //}
    protected void btnsave_Click(object sender, EventArgs e)
    {
        User addusdr = new User();
        if (dlDepartment.Text != "")
        {
            try
            {
                int checkStatus = 0;
                if (btnsave.Text == "Save")
                {
                    checkStatus = addusdr.AddVacancies(dlDepartment.Text, txtposition.Text, int.Parse(txtnoofvacancies.Text),
                    dlhq.Text, Session["EmailId"].ToString(), txtzone.Text, txtsdm.Text);

                    if (checkStatus > 0)
                    {
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = "<script>if(window.confirm(Vacancy Added Successfully!')){window.location.href='VacanciesList.aspx'}</script>";
                        CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                    }
                    else if (checkStatus < 0)
                    {
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = "<script>alert('Duplicate Vacancy!');</script>";
                        CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                        dlDepartment.Text = "";
                    }
                    else
                    {
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = "<script>if(window.confirm('Server Error, Please try again!')){window.location.href='AddVacancies.aspx'}</script>";
                        CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                        dlDepartment.Text = "";
                    }
                }
                else if (btnsave.Text == "Update")
                {
                    checkStatus = addusdr.UpdateVacancies(int.Parse(Request.QueryString["id"].ToString()), dlDepartment.Text, txtposition.Text,
                        int.Parse(txtnoofvacancies.Text), dlhq.Text, txtzone.Text, txtsdm.Text);
                    if (checkStatus > 0)
                    {
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = "<script>if(window.confirm('Vacancy Updated Successfully')){window.location.href='VacanciesList.aspx'}</script>";
                        CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                    }

                    else
                    {
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = "<script>if(window.confirm('There is some Error!')){window.location.href='VacanciesList.aspx?id='" + Request.QueryString["id"] + "'}</script>";
                        CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                    }
                }
            }
            catch
            {
                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>alert('Please Check all the fields');</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
            }
            
        }
        else
        {
            ClientScriptManager CSM = Page.ClientScript;
            string strconfirm = "<script>if(window.confirm('Please check fields')){window.location.href='AddVacancies.aspx'}</script>";
            CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
            dlDepartment.Text = "";
        }

    }
    protected void btncancel_click(object sender, EventArgs e)
    {
        Response.Redirect("VacanciesList.aspx");
    }
}
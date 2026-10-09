using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    SqlOperation ObjSQl = new SqlOperation();

    protected void Page_Load(object sender, EventArgs e)
    {
        //temp code till maintancnce on live server
        //remove only below line to make it work properly after making changes to live 
       // Response.Redirect("Maintenance.aspx");
        if (!IsPostBack)
        { 
        }

    }

    protected void customCheck1_CheckedChanged(object sender, EventArgs e)
    {

    }

    protected void btnlogin_Click(object sender, EventArgs e)
    {
        int? userId = ObjSQl.Fn_CHeckUser(txtuserid.Text, txtPassword.Text);
        string message = string.Empty;
        switch (userId.Value)
        {
            case -1:
                message = "Username and/or password is incorrect.";
                break;
            case -2:
                message = "Account has not been activated.";
                break;
            default:

                Session["EmailId"] = txtuserid.Text;
                //Session["Location"] = "";

                User userobj = new User();
                DataTable getuser = userobj.CheckUserAuth(Session["EmailId"].ToString(), txtPassword.Text);
                if (getuser.Rows.Count > 0)
                {
                    //Session["Location"] = getuser.Rows[0]["Location"].ToString().ToUpper();
                    Session["UserType"] = getuser.Rows[0]["UserType"].ToString().ToUpper();
                    //string userType = getuser.Columns.Contains("Admin");
                    if (Session["UserType"].ToString().ToLower().Equals("employee") || Session["UserType"].ToString().ToLower().Equals("employeehr"))
                    {
                        Response.Redirect("Employee/DashboardEmployee.aspx");
                    }
                    else if(Session["UserType"].ToString().ToLower().Equals("admin"))
                    {
                        Response.Redirect("Admin/Dashboard.aspx");
                    }
                    //else if (Session["UserType"].ToString().ToLower().Equals("hr"))
                    //{
                    //    Response.Redirect("HR/Dashboard.aspx");
                    //}
                    else if(Session["UserType"].ToString().ToLower().Equals("consultancy"))
                    {
                        Response.Redirect("Consultancy/Dashboard.aspx");
                    }
                    else if (Session["UserType"].ToString().ToLower().Equals("candidate"))
                    {
                        Response.Redirect("Candidate/Dashboard.aspx");
                    }

                }
                return;
        }
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("<script>alert('");
        sb.Append(message);
        sb.Append("'); </script>");
        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
    }
}
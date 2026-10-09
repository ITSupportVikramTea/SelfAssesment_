using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_ViewPolicies : System.Web.UI.Page
{
    string conStr =
           ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) 
        { 
            BindPolicies();
        }
    }

    private void BindPolicies()
    {
        using (SqlConnection con =
            new SqlConnection(conStr))
        {
            SqlDataAdapter da =
                new SqlDataAdapter(
                @"SELECT *
                      FROM PolicyMaster
                      WHERE IsActive=1
                      ORDER BY UploadDate DESC",
                con);

            DataTable dt = new DataTable();

            da.Fill(dt);

            rptPolicies.DataSource = dt;
            rptPolicies.DataBind();
        }
    }
}

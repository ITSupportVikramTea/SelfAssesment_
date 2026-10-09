using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_UniApplications : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
            if (!IsPostBack)
            {
                GridView1.DataSource = SqlDataSource1;
                GridView1.DataBind();
            }
        

    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            int index = Convert.ToInt32(e.CommandArgument); // Get the index of the clicked row
            GridViewRow row = GridView1.Rows[index];

            // Access the data in the clicked row
            string vacancyId = row.Cells[6].Text; // Assuming VacancyId is in the seventh column

            // Redirect to Details.aspx page with VacancyId as query parameter
            Response.Redirect("TotalApplicant.aspx?vacancyId=" + vacancyId);
        }
    }




}




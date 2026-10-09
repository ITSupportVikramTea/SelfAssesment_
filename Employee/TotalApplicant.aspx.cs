using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_ApplicantDetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Retrieve the VacancyId from the query parameter
            string vacancyId = Request.QueryString["vacancyId"];

            if (!string.IsNullOrEmpty(vacancyId))
            {
                // Construct the SQL query to fetch data based on VacancyId
                string query = @"SELECT JobApplications.FName, JobApplications.LName, JobApplications.MobNo, UniAppList.Position,UniAppList.VaccancyId
                            FROM JobApplications
                            RIGHT JOIN UniAppList ON JobApplications.VacancyId = UniAppList.VaccancyId
                            WHERE UniAppList.VaccancyId = @VacancyId";

                // Set up SqlConnection and SqlCommand
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@VacancyId", vacancyId);

                    // Execute the query and bind the data to the GridView
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    GridViewDetails.DataSource = reader;
                    GridViewDetails.DataBind();
                }
            }
            else
            {
                // Handle case when VacancyId is null or empty
                // For example:
                // Response.Redirect("PreviousPage.aspx");
            }
        }
    }


    protected void GridViewDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            int index = Convert.ToInt32(e.CommandArgument); // Get the index of the clicked row
            GridViewRow row = GridViewDetails.Rows[index];

            // Access the data in the clicked row
            string vacancyId = row.Cells[3].Text; // Assuming VacancyId is in the seventh column

            // Redirect to Details.aspx page with VacancyId as query parameter
            Response.Redirect("ApplicantDetails.aspx?vacancyId=" + vacancyId);
        }
    }


}
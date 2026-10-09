using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_ApplicantDetails : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    static SQLDataFileSIS objSQLDataFile2 = new SQLDataFileSIS();
    protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["EmailId"] == null || Session["UserType"].ToString().ToLower() != "employeehr")
                {
                    Response.Redirect("../Default.aspx");
                }
            string id = Request.QueryString["id"];
            if (!string.IsNullOrEmpty(id))
            {
                
                DataTable dt = GetJobApplicationDetailsById(id);
                LabelFullName.Text = dt.Rows[0]["FName"].ToString();
                LabelDepartment.Text = dt.Rows[0]["LName"].ToString();
                Created.Text = dt.Rows[0]["CreatedDate"].ToString();
                Mobile.Text = dt.Rows[0]["MobNo"].ToString();
                Email.Text = dt.Rows[0]["EmailId"].ToString();
                Status.Text = dt.Rows[0]["InterviewStatus"].ToString();
                Interviewer.Text = dt.Rows[0]["Interviewer"].ToString();
                Refferd.Text = dt.Rows[0]["RefferedBy"].ToString();
            }
        }
    }
    private DataTable GetJobApplicationDetailsById(string id)
    {
        DataTable dt = new DataTable();
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM jobapplications WHERE Id = @Id", con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
        }
        return dt;
    }


}
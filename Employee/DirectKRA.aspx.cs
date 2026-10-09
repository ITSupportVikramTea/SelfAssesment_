using System;
using System.Configuration;
using System.Data.SqlClient;

public partial class Employee_DirectKRA : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            LoadEmployeeInfo();
            if (Session["EmailId"] == null)
            {
                Response.Redirect("../Default.aspx");
            }
        }
    }

    private void LoadEmployeeInfo()
    {
        
        if (Session["EmailId"].ToString() == null)
            return;

        string email = Session["EmailId"].ToString();
        string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand(
                "SELECT EmpName, EmpDepartment, EmpDesignation FROM EmployeeMaster WHERE EmpEmailId=@Email", con);

            cmd.Parameters.AddWithValue("@Email", email);
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                txtEmployeeName.Text = dr["EmpName"].ToString();
                txtDepartment.Text = dr["EmpDepartment"].ToString();
                txtDesignation.Text = dr["EmpDesignation"].ToString();
            }
        }

        txtEmployeeName.ReadOnly = true;
        txtDepartment.ReadOnly = true;
        txtDesignation.ReadOnly = true;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string emp = txtEmployeeName.Text;
        string date = txtDate.Text;
        string dept = txtDepartment.Text;
        string desig = txtDesignation.Text;

        string[] daily = Request.Form.GetValues("daily[]");
        string[] dailyKra = Request.Form.GetValues("dailyKra[]");
       

        string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        using (SqlConnection con = new SqlConnection(cs))
        {
            con.Open();

            for (int i = 0; i < daily.Length; i++)
            {
                SqlCommand cmd = new SqlCommand(@"
            INSERT INTO DirectKRA
            (EmployeeName, EntryDate, Department, Designation, DailyType, DailyKRA)
            VALUES
            (@Emp,@Date,@Dept,@Desig,@Daily,@DailyKRA)", con);

                cmd.Parameters.AddWithValue("@Emp", emp);
                cmd.Parameters.AddWithValue("@Date", date);
                cmd.Parameters.AddWithValue("@Dept", dept);
                cmd.Parameters.AddWithValue("@Desig", desig);
                cmd.Parameters.AddWithValue("@Daily", daily[i]);
                cmd.Parameters.AddWithValue("@DailyKRA", dailyKra[i]);
              

                cmd.ExecuteNonQuery();
            }
        }

        ClientScript.RegisterStartupScript(this.GetType(), "msg", "alert('KRA saved successfully');", true);
    }
}
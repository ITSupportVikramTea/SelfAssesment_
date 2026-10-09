using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class Employee_KRAReport : System.Web.UI.Page
{
    string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            LoadGrid();
    }

    void LoadGrid()
    {
        if (Session["Emailid"] == null)
            return;

        string email = Session["Emailid"].ToString();

        using (SqlConnection con = new SqlConnection(cs))
        {
            string query = @"
        SELECT K.*
        FROM DirectKRA K
        INNER JOIN EmployeeMaster E ON E.EmpName = K.EmployeeName
        WHERE E.EmpEmailId = @Email
        ORDER BY K.EntryDate DESC";

            SqlDataAdapter da = new SqlDataAdapter(query, con);
            da.SelectCommand.Parameters.AddWithValue("@Email", email);

            DataTable dt = new DataTable();
            da.Fill(dt);

            gvKRA.DataSource = dt;
            gvKRA.DataBind();
        }
    }


    protected void gvKRA_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvKRA.EditIndex = e.NewEditIndex;
        LoadGrid();

        
        DropDownList ddl = (DropDownList)gvKRA.Rows[e.NewEditIndex].FindControl("ddlType");
        string type = gvKRA.Rows[e.NewEditIndex].Cells[3].Text;
        ddl.SelectedValue = type;
    }

    protected void gvKRA_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvKRA.EditIndex = -1;
        LoadGrid();
    }

    protected void gvKRA_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int id = Convert.ToInt32(gvKRA.DataKeys[e.RowIndex].Value);

        GridViewRow row = gvKRA.Rows[e.RowIndex];

        TextBox txtKra = (TextBox)row.FindControl("txtKra");
        DropDownList ddlType = (DropDownList)row.FindControl("ddlType");

        string kra = txtKra.Text;
        string type = ddlType.SelectedValue;

        using (SqlConnection con = new SqlConnection(
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand(
                "UPDATE DirectKRA SET DailyKra=@Kra, DailyType=@Type WHERE Id=@Id", con);

            cmd.Parameters.AddWithValue("@Kra", kra);
            cmd.Parameters.AddWithValue("@Type", type);
            cmd.Parameters.AddWithValue("@Id", id);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        gvKRA.EditIndex = -1;
        LoadGrid();  
    }

}
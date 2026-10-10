using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_KRAReport : System.Web.UI.Page
{
    string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            if (Session["EmailId"] == null)
            {
                Response.Redirect("../Default.aspx");
            }
            LoadGrid();
        }
    }

    void LoadGrid(string search = "")
    {
        using (SqlConnection con = new SqlConnection(cs))
        {
            string query = @"
SELECT 
    dk.Id,
    em.ERPID,
    dk.EmployeeName,
    dk.Department,
    dk.DailyKra,
    dk.DailyType
FROM DirectKRA dk
JOIN EmployeeMaster em ON em.empname = dk.EmployeeName
WHERE (@Search = '' 
    OR dk.EmployeeName LIKE '%' + @Search + '%'
    OR dk.DailyKra LIKE '%' + @Search + '%'
    OR dk.DailyType LIKE '%' + @Search + '%')
ORDER BY dk.EntryDate DESC";



            SqlDataAdapter da = new SqlDataAdapter(query, con);
            da.SelectCommand.Parameters.AddWithValue("@Search", search);

            DataTable dt = new DataTable();
            da.Fill(dt);

            gvKRA.DataSource = dt;
            gvKRA.DataBind();

          
            ViewState["KRAData"] = dt;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        LoadGrid(txtSearch.Text.Trim());
    }
    protected void gvKRA_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvKRA.EditIndex = e.NewEditIndex;
        LoadGrid();


        DropDownList ddl = (DropDownList)gvKRA.Rows[e.NewEditIndex].FindControl("ddlType");
        string type = gvKRA.Rows[e.NewEditIndex].Cells[3].Text;
        ddl.SelectedValue = type;
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        LoadGrid();
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

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("content-disposition",
            "attachment;filename=KRA_Report.xls");

        System.IO.StringWriter sw = new System.IO.StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);

        gvKRA.EditIndex = -1;

        if (ViewState["KRAData"] != null)
        {
            gvKRA.DataSource = ViewState["KRAData"];
            gvKRA.DataBind();
        }

        gvKRA.RenderControl(htw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
    }
}
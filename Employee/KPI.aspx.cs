using Org.BouncyCastle.Ocsp;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.UI;
public partial class Employee_KPI : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CreateTable();
            BindGrid();
            BindKPIReport();
        }
    }

    private void CreateTable()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("KPI");
        dt.Columns.Add("Weightage");
        dt.Columns.Add("UOM");
        dt.Columns.Add("Year");

        dt.Rows.Add("", "", "", "2026");

        ViewState["KPIDetails"] = dt;
    }

    private void BindGrid()
    {
        gvKPI.DataSource = ViewState["KPIDetails"];
        gvKPI.DataBind();
    }

    protected void btnAddRow_Click(object sender, EventArgs e)
    {
        DataTable dt = ViewState["KPIDetails"] as DataTable;

        if (dt == null)
        {
            CreateTable();
            dt = ViewState["KPIDetails"] as DataTable;
        }

       
        for (int i = 0; i < gvKPI.Rows.Count; i++)
        {
            TextBox txtKPI =
                (TextBox)gvKPI.Rows[i].FindControl("txtKPI");

            TextBox txtWeightage =
                (TextBox)gvKPI.Rows[i].FindControl("txtWeightage");

            DropDownList txtUOM =
                (DropDownList)gvKPI.Rows[i].FindControl("txtUOM");

            dt.Rows[i]["KPI"] = txtKPI.Text.Trim();
            dt.Rows[i]["Weightage"] = txtWeightage.Text.Trim();
            dt.Rows[i]["UOM"] = txtUOM.Text.Trim();
            dt.Rows[i]["Year"] = "2026";
        }

       
        if (dt.Rows.Count < 10)
        {
            DataRow dr = dt.NewRow();

            dr["KPI"] = "";
            dr["Weightage"] = "";
            dr["UOM"] = "";
            dr["Year"] = "2026";

            dt.Rows.Add(dr);
        }
        else
        {
            lblMessage.Text = "Maximum 10 rows allowed";
        }

        ViewState["KPIDetails"] = dt;

        gvKPI.DataSource = dt;
        gvKPI.DataBind();

      
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            TextBox txtKPI =
                (TextBox)gvKPI.Rows[i].FindControl("txtKPI");

            TextBox txtWeightage =
                (TextBox)gvKPI.Rows[i].FindControl("txtWeightage");

            DropDownList txtUOM =
                (DropDownList)gvKPI.Rows[i].FindControl("txtUOM");

            TextBox txtYear =
                (TextBox)gvKPI.Rows[i].FindControl("txtYear");

            txtKPI.Text = dt.Rows[i]["KPI"].ToString();
            txtWeightage.Text = dt.Rows[i]["Weightage"].ToString();
            txtUOM.Text = dt.Rows[i]["UOM"].ToString();
            txtYear.Text = "2026";
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";

        // ── Step 1: Collect new rows from entry grid ─────────────────────
        DataTable newRows = new DataTable();
        newRows.Columns.Add("KPI");
        newRows.Columns.Add("Weightage");
        newRows.Columns.Add("UOM");

        for (int i = 0; i < gvKPI.Rows.Count; i++)
        {
            TextBox txtKPI = (TextBox)gvKPI.Rows[i].FindControl("txtKPI");
            TextBox txtWeightage = (TextBox)gvKPI.Rows[i].FindControl("txtWeightage");
            DropDownList txtUOM = (DropDownList)gvKPI.Rows[i].FindControl("txtUOM");

            if (txtKPI == null || txtWeightage == null || txtUOM == null)
                continue;

            if (txtKPI.Text.Trim() == "")
                continue;

            DataRow dr = newRows.NewRow();
            dr["KPI"] = txtKPI.Text.Trim();
            dr["Weightage"] = txtWeightage.Text.Trim();
            dr["UOM"] = txtUOM.SelectedValue.Trim();
            newRows.Rows.Add(dr);
        }

        // ── Step 2: Sum new entries ───────────────────────────────────────
        float newTotal = 0;
        foreach (DataRow row in newRows.Rows)
        {
            float w = 0;
            float.TryParse(row["Weightage"].ToString(), out w);
            newTotal += w;
        }

        if (newRows.Rows.Count == 0)
        {
            lblMessage.Text = "Please enter at least one KPI.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }

        // ── Step 3: Get EmpId ─────────────────────────────────────────────
        DataTable res = objSQLDataFile.ExecuteDataTableSQL(
            @"SELECT DISTINCT Id 
          FROM EmployeeMaster 
          WHERE EmpEmailId = '"
            + Session["EmailId"].ToString() + "'");

        int empId = 0;
        if (res.Rows.Count > 0)
            empId = Convert.ToInt32(res.Rows[0]["Id"]);

        if (empId == 0)
        {
            lblMessage.Text = "Employee not found. Please log in again.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }

        // ── Step 4: Get existing weightage from DB ────────────────────────
        float existingTotal = 0;

        DataTable dtTotal = objSQLDataFile.ExecuteDataTableSQL(
            @"SELECT ISNULL(SUM(CAST(Weightage AS FLOAT)), 0) AS TotalWeightage
          FROM KraUserWise
          WHERE Year = 2026 AND EmpId = " + empId);

        if (dtTotal.Rows.Count > 0)
            existingTotal = Convert.ToSingle(dtTotal.Rows[0]["TotalWeightage"]);

        // ── Step 5: Check existing + new = exactly 100 ───────────────────
        float combinedTotal = existingTotal + newTotal;

        if (combinedTotal > 100)
        {
            lblMessage.Text =
                "Cannot save. Existing weightage is " + existingTotal
                + ". Adding " + newTotal
                + " would make total " + combinedTotal
                + ", which exceeds 100.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }

        if (combinedTotal < 100)
        {
            lblMessage.Text =
                "Cannot save. Existing weightage is " + existingTotal
                + ". Adding " + newTotal
                + " makes total " + combinedTotal
                + ". Total must equal exactly 100. "
                + "You still need " + (100 - combinedTotal) + " more.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }

      
        string connStr = ConfigurationManager
            .ConnectionStrings["DefaultConnection"]
            .ConnectionString;

        using (SqlConnection con = new SqlConnection(connStr))
        {
            con.Open();

            foreach (DataRow row in newRows.Rows)
            {
                string query =
                    @"INSERT INTO KraUserWise
                    (EmpId, Target, KPI, Weightage, UOM, Year)
                  VALUES
                    (@EmpId, @Target, @KPI, @Weightage, @UOM, @Year)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@EmpId", empId);
                    cmd.Parameters.AddWithValue("@Target", 100);
                    cmd.Parameters.AddWithValue("@KPI", row["KPI"].ToString());
                    cmd.Parameters.AddWithValue("@Weightage", row["Weightage"].ToString());
                    cmd.Parameters.AddWithValue("@UOM", row["UOM"].ToString());
                    cmd.Parameters.AddWithValue("@Year", "2026");

                    cmd.ExecuteNonQuery();
                }
            }
        }

        lblMessage.Text = "KPI Data Saved Successfully.";
        lblMessage.ForeColor = System.Drawing.Color.Green;

        BindKPIReport();
        CreateTable();
        BindGrid();
    }

    protected void gvKPIReport_RowEditing(
    object sender,
    GridViewEditEventArgs e)
    {
        gvKPIReport.EditIndex = e.NewEditIndex;

        BindKPIReport();
    }

    protected void gvKPIReport_RowCancelingEdit(
    object sender,
    GridViewCancelEditEventArgs e)
    {
        gvKPIReport.EditIndex = -1;

        BindKPIReport();
    }
    // ── Safe helper: finds first TextBox inside a GridView cell ──────────
    private TextBox FindTextBoxInCell(TableCell cell)
    {
        foreach (Control ctrl in cell.Controls)
        {
            if (ctrl is TextBox)
                return (TextBox)ctrl;
        }
        return null;
    }
    protected void gvKPIReport_RowUpdating(
    object sender,
    GridViewUpdateEventArgs e)
    {
        int id = Convert.ToInt32(
            gvKPIReport.DataKeys[e.RowIndex].Value);

        GridViewRow editRow = gvKPIReport.Rows[e.RowIndex];

        // ── Safely find TextBoxes regardless of Controls index ───────────
        TextBox txtKPI = FindTextBoxInCell(editRow.Cells[0]);
        TextBox txtWeightage = FindTextBoxInCell(editRow.Cells[1]);
        TextBox txtUOM = FindTextBoxInCell(editRow.Cells[2]);

        if (txtKPI == null || txtWeightage == null || txtUOM == null)
        {
            lblMessage.Text = "Could not read edit controls.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }

        string kpi = txtKPI.Text.Trim();
        string weightageText = txtWeightage.Text.Trim();
        string uom = txtUOM.Text.Trim();

        // ── Validate numeric ─────────────────────────────────────────────
        int newWeightage = 0;
        if (!int.TryParse(weightageText, out newWeightage))
        {
            lblMessage.Text =
                "Weightage must be numeric.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }

        // ── Calculate total across ALL rows ──────────────────────────────
        float totalWeightage = 0;

        for (int i = 0; i < gvKPIReport.Rows.Count; i++)
        {
            float rowWeightage = 0;

            if (i == e.RowIndex)
            {
                rowWeightage = newWeightage;
            }
            else
            {
                string cellText = gvKPIReport.Rows[i].Cells[1].Text
                    .Replace("&nbsp;", "")
                    .Trim();

                float.TryParse(cellText, out rowWeightage);
            }

            totalWeightage += rowWeightage;
        }

        // ── Validate total ───────────────────────────────────────────────
        if (totalWeightage > 100)
        {
            lblMessage.Text =
                "Total Weightage should be exactly 100.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }

        // ── Persist to database ──────────────────────────────────────────
        string connStr = ConfigurationManager
            .ConnectionStrings["DefaultConnection"]
            .ConnectionString;

        using (SqlConnection con = new SqlConnection(connStr))
        {
            const string query =
                @"UPDATE KraUserWise
              SET KPI       = @KPI,
                  Weightage = @Weightage,
                  UOM       = @UOM
              WHERE Id = @Id";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@KPI", kpi);
                cmd.Parameters.AddWithValue("@Weightage", newWeightage);
                cmd.Parameters.AddWithValue("@UOM", uom);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        gvKPIReport.EditIndex = -1;
        BindKPIReport();

        lblMessage.Text = "KPI updated successfully.";
        lblMessage.ForeColor = System.Drawing.Color.Green;
    }
    protected void gvKPIReport_RowDeleting(
    object sender,
    GridViewDeleteEventArgs e)
    {
        int id = Convert.ToInt32(
            gvKPIReport.DataKeys[e.RowIndex].Value);

        string connStr =
            ConfigurationManager
            .ConnectionStrings["DefaultConnection"]
            .ConnectionString;

        using (SqlConnection con =
            new SqlConnection(connStr))
        {
            string query =
                @"DELETE FROM KraUserWise
              WHERE Id=@Id";

            SqlCommand cmd =
                new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@Id", id);

            con.Open();

            cmd.ExecuteNonQuery();

            con.Close();
        }

        BindKPIReport();
    }

    private void BindKPIReport()
    {
        DataTable res = objSQLDataFile.ExecuteDataTableSQL(
            @"SELECT DISTINCT Id
          FROM EmployeeMaster
          WHERE EmpEmailId = '" +
              Session["EmailId"].ToString() + "'");

        int empId = 0;

        if (res.Rows.Count > 0)
        {
            empId = Convert.ToInt32(
                res.Rows[0]["Id"]);
        }

        DataTable dt =
            objSQLDataFile.ExecuteDataTableSQL(
            @"SELECT
            Id,
            KPI,
            Weightage,
            UOM,
            Year
          FROM KraUserWise
          WHERE EmpId = " + empId + @"
          AND Year = 2026
          ORDER BY Id DESC");

        gvKPIReport.DataSource = dt;

        gvKPIReport.DataBind();

     
        int totalWeightage = 0;

        foreach (DataRow row in dt.Rows)
        {
            int weightage = 0;

            int.TryParse(
                row["Weightage"].ToString(),
                out weightage);

            totalWeightage += weightage;
        }

  
        lblTotalWeightage.Text =
            "Total Weightage : " + totalWeightage;

  
        if (totalWeightage > 100)
        {
            lblTotalWeightage.ForeColor =
                System.Drawing.Color.Red;

            lblMessage.Text =
                "Total Weightage exceeds 100.";
        }
        else if (totalWeightage == 100)
        {
            lblTotalWeightage.ForeColor =
                System.Drawing.Color.Green;
        }
        else
        {
            lblTotalWeightage.ForeColor =
                System.Drawing.Color.Orange;
        }
    }
}
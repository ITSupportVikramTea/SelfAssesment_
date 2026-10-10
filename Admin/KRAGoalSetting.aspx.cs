using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_KRAGoalSetting : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["EmailId"] == null)
            {
                Response.Redirect("../Default.aspx");
            }
            LoadEmployeeData();
        }
    }

    private void LoadEmployeeData()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        string query = "SELECT EmpName, ERPID, EmpDepartment, RM FROM EmployeeMaster";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    TableRow tableRow = new TableRow();
                    tableRow.Cells.Add(new TableCell { Text = row["EmpName"].ToString() });
                    tableRow.Cells.Add(new TableCell { Text = row["ERPID"].ToString() });
                    tableRow.Cells.Add(new TableCell { Text = row["EmpDepartment"].ToString() });
                    tableRow.Cells.Add(new TableCell { Text = row["RM"].ToString() });
                    tableBody.Controls.Add(tableRow);
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                Response.Write("An error occurred while loading data.");
            }
        }
    }

    [WebMethod]
    public static string GetEmployeeDetails(string erpId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        string query = @"SELECT k.ERPID, k.TargetKRA, k.UOM, k.UOP, k.QTarget, k.Weightage ,k.achievement,k.score,k.id
                     FROM KRAGoalSettings k 
                     JOIN EmployeeMaster e ON e.ERPID = k.ERPID 
                     WHERE e.ERPID = @ERPID";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ERPID", erpId);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                var employeeDataList = new List<object>(); 

                while (reader.Read())
                {
                    var employeeData = new
                    {
                        ERPID = reader["ERPID"].ToString(),
                        TargetKRA = reader["TargetKRA"].ToString(),
                        UOM = reader["UOM"].ToString(),
                        UOP = reader["UOP"].ToString(),
                        QTarget = reader["QTarget"].ToString(),
                        Weightage = reader["Weightage"].ToString(),
                        achievement = reader["achievement"].ToString(),
                        score = reader["score"].ToString(),
                        id = reader["id"].ToString()
                    };

                    employeeDataList.Add(employeeData);
                }

                if (employeeDataList.Count == 0)
                {
                    return "No data found for the specified ERPID.";
                }

                return Newtonsoft.Json.JsonConvert.SerializeObject(employeeDataList);
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        return null; // Return null if no data found
    }

    private static void LogError(Exception ex)
    {
       
    }

    [System.Web.Services.WebMethod]
    public static object SaveData(string targetKRA, string uom, string uop, string qTarget, string weightage, string achievement, string score, string id)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE KRAGoalSettings SET UOM = @uom, UOP = @uop, QTarget = @qTarget, Weightage = @weightage, TargetKRA = @targetKRA, Achievement = @achievement, Score = @score WHERE id = @id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@targetKRA", targetKRA);
                cmd.Parameters.AddWithValue("@uom", uom);
                cmd.Parameters.AddWithValue("@uop", uop);
                cmd.Parameters.AddWithValue("@qTarget", qTarget);
                cmd.Parameters.AddWithValue("@weightage", weightage);
                cmd.Parameters.AddWithValue("@achievement", achievement);
                cmd.Parameters.AddWithValue("@score", score);
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return new { success = true };
                }
                else
                {
                    return new { success = false, message = "No rows updated. Check if the ID is correct." };
                }
            }
        }
        catch (SqlException sqlEx)
        {
            //System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/SqlErrorLog.txt"), "SQL Error: " + sqlEx.Message + "\n");
            return new { success = false, message = "A SQL error occurred: " + sqlEx.Message };
        }
        catch (Exception ex)
        {
            //System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/ErrorLog.txt"), "General Error: " + ex.ToString());
            return new { success = false, message = "An error occurred while saving data: " + ex.Message };
        }
    }

    [WebMethod]
    public static string ImportXlsxData(string ERPID, string TargetKRA, string UOM, string UOP, string QTarget, string Weightage, string Achievement, string Score)
    {
        string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            conn.Open();
            SqlTransaction transaction = conn.BeginTransaction();

            try
            {
               
                UOM = HttpUtility.UrlDecode(UOM);

                string query = "INSERT INTO KRAGoalSettings (ERPID, TargetKRA, UOM, UOP, QTarget, Weightage, Achievement, Score) " +
                               "VALUES (@ERPID, @TargetKRA, @UOM, @UOP, @QTarget, @Weightage, @Achievement, @Score)";

                using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@ERPID", ERPID);
                    cmd.Parameters.AddWithValue("@TargetKRA", TargetKRA);
                    cmd.Parameters.AddWithValue("@UOM", UOM); 
                    cmd.Parameters.AddWithValue("@UOP", UOP);
                    cmd.Parameters.AddWithValue("@QTarget", QTarget);
                    cmd.Parameters.AddWithValue("@Weightage", Weightage);
                    cmd.Parameters.AddWithValue("@Achievement", Achievement);
                    cmd.Parameters.AddWithValue("@Score", Score);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        transaction.Commit();
                        return "Data imported successfully!";
                    }
                    else
                    {
                        return "No data was inserted.";
                    }
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                //File.WriteAllText(System.Web.HttpContext.Current.Server.MapPath("~/ErrorLog.txt"), ex.ToString());
                return "Error: " + ex.Message;
            }
        }
    }

}


using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_UploadPolicy : System.Web.UI.Page
{
    string conStr =
           ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
         
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!fuPolicy.HasFile)
            return;

        string folder =
            Server.MapPath("~PolicyFiles/");

        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        string fileName =
            Guid.NewGuid() +
            Path.GetExtension(fuPolicy.FileName);

        string fullPath =
            folder + fileName;

        fuPolicy.SaveAs(fullPath);

        string dbPath =
            "PolicyFiles/" + fileName;

        using (SqlConnection con =
            new SqlConnection(conStr))
        {
            SqlCommand cmd = new SqlCommand(
            @"INSERT INTO PolicyMaster
                (
                    PolicyTitle,
                    Description,
                    FileName,
                    FilePath
                )
                VALUES
                (
                    @PolicyTitle,
                    @Description,
                    @FileName,
                    @FilePath
                )", con);

            cmd.Parameters.AddWithValue("@PolicyTitle", txtTitle.Text);
            cmd.Parameters.AddWithValue("@Description", txtDescription.Text);
            cmd.Parameters.AddWithValue("@FileName", fuPolicy.FileName);
            cmd.Parameters.AddWithValue("@FilePath", dbPath);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "Policy Uploaded Successfully");
       
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_UploadSign : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    static SQLDataFileSIS objSQLDataFile2 = new SQLDataFileSIS();
    string loggedEmpEmail = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        User user = new User();
        if (!IsPostBack)
        {
            if (Session["EmailId"] == null)
            {
                Response.Redirect("../Default.aspx");
            }
            else
            {
                int empId = user.getEmployeeId(Session["EmailId"].ToString());
                hidEmpId.Value = empId.ToString();
                ShowImageExists(empId);
            }

        }
        loggedEmpEmail = Session["EmailId"].ToString();
    }
    public void ShowImageExists(int empid)
    {
        DataTable dt = null;

        try
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"select * from EmployeeSignature where EmpId = " + empid);
            if (dt.Rows[0]["SignImg"].ToString() != "" && dt.Rows[0]["SignImg"].ToString() != null)
            {
                sign_img_msg.InnerText = "Available";
                viewSign.Visible = true;
                sign_img_msg.Attributes.Add("class", "text-success");
            }
            else
            {
                sign_img_msg.InnerText = "Not Available";
                sign_img_msg.Attributes.Add("class", "text-danger");
            }

        }
        catch (Exception ex)
        {

        }
    }
    protected void UploadSignature_Click(object sender, EventArgs e)
    {
        User user = new User();
        int empId = user.getEmployeeId(Session["EmailId"].ToString());
        string empName = user.getEmployeeName(Session["EmailId"].ToString());

        DataTable IsexistsSignature = objSQLDataFile.ExecuteDataTableSQL(@"select * from EmployeeSignature where EmpId = " + empId);
        if (IsexistsSignature.Rows.Count > 0)
        {
            // Update the existing signature
            try
            {
                string uploadPath = "~/EmployeeSignature/";
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd");

                string sign_img = SignImage.PostedFile.FileName != "" ? uploadPath + empName + "_" + timestamp + "_" + SignImage.PostedFile.FileName : "";
                if (sign_img != "")
                {
                    SignImage.PostedFile.SaveAs(Server.MapPath(sign_img));
                }
                user.AddEmployeeSignature(empId, empName, sign_img);
                // Update the existing record with the new signature
                //objSQLDataFile.ExecuteDataTableSQL(@"UPDATE EmployeeSignature SET SignImg = "+sign_img+" WHERE EmpId = "+empId);

                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>if(window.confirm('File Updated!'))</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
            }
            catch (Exception ex)
            {
                Response.Write("Error updating signature: " + ex.Message);
            }
        }
        else
        {
            if (SignImage.HasFile)
            {
                try
                {

                    string uploadPath = "~/EmployeeSignature/";
                    string timestamp = DateTime.Now.ToString("yyyy-MM-dd");

                    string sign_img = SignImage.PostedFile.FileName != "" ? uploadPath + empName + "_" + timestamp + "_" + SignImage.PostedFile.FileName : "";
                    if (sign_img != "")
                    {
                        SignImage.PostedFile.SaveAs(Server.MapPath(sign_img));
                    }
                    user.AddEmployeeSignature(empId, empName, sign_img);
                    ShowImageExists(empId);
                    ClientScriptManager CSM = Page.ClientScript;
                    string strconfirm = "<script>if(window.confirm('File Uploaded!'))</script>";
                    CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                }
                catch (Exception ex)
                {
                    Response.Write("Error uploading signature: " + ex.Message);
                }
            }
            else
            {
                Response.Write("Please select a signature image to upload.");
            }
        }
       
    }
    [WebMethod]
    public static string getImgPreviewLink(int id, string img_name)
    {
        string src = "";

        try
        {
            DataTable dt = objSQLDataFile2.ExecuteDataTableSQL(@"select * from EmployeeSignature where Empid = " + id);

            if (!string.IsNullOrEmpty(img_name))
            {
                src = dt.Rows[0]["SignImg"].ToString();
            }

            src = src.Replace("~/", "/");
        }
        catch (Exception ex)
        {

        }

        return src;
    }
}
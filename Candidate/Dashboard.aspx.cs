using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Candidate_Dashboard : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["EmailId"] == null)
            {
                Response.Redirect("../Default.aspx");

            }
            isDocUploaded();
        }
    }
    protected void isDocUploaded()
    {
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL("SELECT Id from JobApplications WHERE EmailId ='" + Session["EmailId"].ToString() + "'");

        if (dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            string cid = row["Id"].ToString();
            DataTable dt2 = objSQLDataFile.ExecuteDataTableSQL("select * from CandidateDocs where CandidateId=" + cid);
            if(dt2.Rows.Count > 0)
            {
                pnlMain.Visible = false;
                lblShowMsg.Visible = true;
            }
        }
    }
    protected void btnUpload_click(object sender, EventArgs e)
    {
        string tenthmark = string.Empty;
        string twevlemarks = string.Empty;
        string graduationcert = string.Empty;
        string aadhar = string.Empty;
        string expcert = string.Empty;
        string slslip = string.Empty;
        string drivinglicense = string.Empty;


        if (fuaadhar.PostedFile.FileName != "")
        {
            aadhar = DateTime.Now.ToString("yyyyMMddHHmmss") + fuaadhar.PostedFile.FileName;
            string FilePath1 = @"~/CandidatesDocs/" + aadhar.Trim().ToString();
            fuaadhar.PostedFile.SaveAs(Server.MapPath(FilePath1));

            if (futenthmark.PostedFile.FileName != "")
            {
                tenthmark = DateTime.Now.ToString("yyyyMMddHHmmss") + futenthmark.PostedFile.FileName;
                string FilePath2 = @"~/CandidatesDocs/" + tenthmark.Trim().ToString();
                futenthmark.PostedFile.SaveAs(Server.MapPath(FilePath2));
            }
            if (futwevlemarks.PostedFile.FileName != "")
            {
                twevlemarks = DateTime.Now.ToString("yyyyMMddHHmmss") + futwevlemarks.PostedFile.FileName;
                string FilePath3 = @"~/CandidatesDocs/" + twevlemarks.Trim().ToString();
                futwevlemarks.PostedFile.SaveAs(Server.MapPath(FilePath3));
            }
            if (fugraduationcert.PostedFile.FileName != "")
            {
                graduationcert = DateTime.Now.ToString("yyyyMMddHHmmss") + fugraduationcert.PostedFile.FileName;
                string FilePath4 = @"~/CandidatesDocs/" + graduationcert.Trim().ToString();
                fugraduationcert.PostedFile.SaveAs(Server.MapPath(FilePath4));
            }

            if(fuexpcert.PostedFile.FileName != "")
            {
                expcert = DateTime.Now.ToString("yyyyMMddHHmmss") + fuexpcert.PostedFile.FileName;
                string FilePath5 = @"~/CandidatesDocs/" + expcert.Trim().ToString();
                fuexpcert.PostedFile.SaveAs(Server.MapPath(FilePath5));
            }
            
            if(fuslslip.PostedFile.FileName != "")
            {
                slslip = DateTime.Now.ToString("yyyyMMddHHmmss") + fuslslip.PostedFile.FileName;
                string FilePath6 = @"~/CandidatesDocs/" + slslip.Trim().ToString();
                fuslslip.PostedFile.SaveAs(Server.MapPath(FilePath6));
            }
            if (fudrivinglicense.PostedFile.FileName != "")
            {
                drivinglicense = DateTime.Now.ToString("yyyyMMddHHmmss") + fudrivinglicense.PostedFile.FileName;
                string FilePath7 = @"~/CandidatesDocs/" + drivinglicense.Trim().ToString();
                fuslslip.PostedFile.SaveAs(Server.MapPath(FilePath7));
            }


            DataTable dt = objSQLDataFile.ExecuteDataTableSQL("SELECT Id from JobApplications WHERE EmailId ='" + Session["EmailId"].ToString()+ "'");

            if(dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                string cid = row["Id"].ToString();
                User user = new User();
                int status = user.AddCandidateDocs(int.Parse(cid), tenthmark, twevlemarks, graduationcert, expcert, slslip,drivinglicense,aadhar);
                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>if(window.confirm('Documents Uploaded')){window.location.href='Dashboard.aspx'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
            }
            else
            {
                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>if(window.confirm('Email Miss Match')){window.location.href='Dashboard.aspx'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
            }

        }
        else
        {
            ClientScriptManager CSM = Page.ClientScript;
            string strconfirm = "<script>if(window.confirm('Aadhar is Compulsory')){window.location.href='Dashboard.aspx'}</script>";
            CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
        }
    }
}
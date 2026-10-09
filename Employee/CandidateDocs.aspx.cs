using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HR_CandidateDocs : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    protected static string CandName { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["EmailId"] == null)
            {
                Response.Redirect("../Default.aspx");
            }
            bindgdDocuments();
            getCandidateName();
        }
    }

    protected void bindgdDocuments()
    {
        string candidateId = Request.QueryString["id"].ToString();
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"select AadharCard,TenthCert,TwevleCert,GraduationCert,DrivingLicence,ExpCert,SalarySlip from CandidateDocs Where CandidateId=" + candidateId);
        gdDocuments.DataSource = dt;
        gdDocuments.DataBind();
    }
    public void getCandidateName()
    {
        string candidateId = Request.QueryString["id"].ToString();
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"Select FName+' '+LName FullName from [dbo].[JobApplications] where id=" + candidateId);
        DataRow row = dt.Rows[0];
        CandName = row["FullName"].ToString();
    }
    protected void gdDocuments_rowcommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "tenthCert" && e.CommandArgument.ToString() != "")
        {
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("Content-Disposition", "filename=" + e.CommandArgument);
            Response.TransmitFile(Server.MapPath("~/CandidatesDocs/") + e.CommandArgument);
            Response.End();
        }
        else if (e.CommandName == "TwevleCert" && e.CommandArgument.ToString() != "")
        {
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("Content-Disposition", "filename=" + e.CommandArgument);
            Response.TransmitFile(Server.MapPath("~/CandidatesDocs/") + e.CommandArgument);
            Response.End();
        }
        else if (e.CommandName == "GraduationCert" && e.CommandArgument.ToString() != "")
        {
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("Content-Disposition", "filename=" + e.CommandArgument);
            Response.TransmitFile(Server.MapPath("~/CandidatesDocs/") + e.CommandArgument);
            Response.End();
        }
        else if (e.CommandName == "AadharCard" && e.CommandArgument.ToString() != "")
        {
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("Content-Disposition", "filename=" + e.CommandArgument);
            Response.TransmitFile(Server.MapPath("~/CandidatesDocs/") + e.CommandArgument);
            Response.End();
        }
        else if (e.CommandName == "ExpCert" && e.CommandArgument.ToString() != "")
        {
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("Content-Disposition", "filename=" + e.CommandArgument);
            Response.TransmitFile(Server.MapPath("~/CandidatesDocs/") + e.CommandArgument);
            Response.End();
        }
        else if (e.CommandName == "SalarySlip" && e.CommandArgument.ToString() != "")
        {
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("Content-Disposition", "filename=" + e.CommandArgument);
            Response.TransmitFile(Server.MapPath("~/CandidatesDocs/") + e.CommandArgument);
            Response.End();
        }
        else if (e.CommandName == "DrivingLicence" && e.CommandArgument.ToString() != "")
        {
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("Content-Disposition", "filename=" + e.CommandArgument);
            Response.TransmitFile(Server.MapPath("~/CandidatesDocs/") + e.CommandArgument);
            Response.End();
        }
        else
        {
            ClientScriptManager CSM = Page.ClientScript;
            string strconfirm = " <script>if(window.confirm('This Document is not Uploaded!')){}</script>";
            CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
        }

    }
}
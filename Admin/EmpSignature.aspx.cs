using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_EmpSignature : System.Web.UI.Page
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
            bindgetEmployeeList();
        }
    }
    public void bindgetEmployeeList()
    {
        string str = txtsearchuser.Text.Trim();
        DataTable dt;

        if (str != "")
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT  * from EmployeeSignature where EmpName LIKE '%" + str + "%'");
        }
        else
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from EmployeeSignature");
        }

        EmployeeList.DataSource = dt;
        EmployeeList.DataBind();
    }
    protected void Search(object sender, EventArgs e)
    {
        bindgetEmployeeList();
    }

    protected void EmployeeList_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Text = Regex.Replace(e.Row.Cells[0].Text, txtsearchuser.Text.Trim(), delegate (Match match)
            {
                return string.Format("<span style = 'background-color:#D9EDF7'>{0}</span>", match.Value);
            }, RegexOptions.IgnoreCase);

        }
    }
    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        EmployeeList.PageIndex = e.NewPageIndex;

        this.bindgetEmployeeList();
    }
    protected void lnkDownload_Click(object sender, EventArgs e)
    {
        LinkButton lnkDownload = (LinkButton)sender;
        string filePath = lnkDownload.CommandArgument;
        filePath = Server.MapPath(filePath);
        DownloadFile(filePath);
    }

    private void DownloadFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
            Response.TransmitFile(filePath);
            Response.End();
        }
        else
        {

            Response.Write("File not found.");
        }
    }
    protected void lnkView_Click(object sender, EventArgs e)
    {
        LinkButton lnkView = (LinkButton)sender;
        string imagePath = lnkView.CommandArgument;
        imgPreviewModal.Attributes["src"] = ResolveUrl(imagePath);

        // Show the modal or perform any other action to display the image 
        ScriptManager.RegisterStartupScript(this, GetType(), "ShowImage", "$('#modalImagePreview').modal('show');", true);
    }


}
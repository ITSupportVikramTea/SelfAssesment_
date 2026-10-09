using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_VacanciesList : System.Web.UI.Page
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
            bindvacanylist();
            //txtvcnid.Enabled = true;
            //txtvcnid.Attributes.Add("visible", "false");
        }

    }
    protected void bindvacanylist()
    {
        string str = txtsearchcand.Text.Trim();
        DataTable dt;
        User user = new User();
        int empId = user.getEmployeeId(Session["EmailId"].ToString());
        if (str == "")
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from AvailableVacancies where 
Id not In(select VacancyId from HiddenVacancies where EmpId = " + empId + ") and NoOfVC > 0 ORDER BY Position");
        }
        else
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from AvailableVacancies 
            where Id not In(select VacancyId from HiddenVacancies where EmpId = " + empId + ") and " +
            "(Department LIKE '%" + str + "%' or Position LIKE '%" + str + "%' or HQ LIKE '%" + str + "%') and NoOfVC > 0  ORDER BY Position");
        }

        VacanciesList.DataSource = dt;
        VacanciesList.DataBind();
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        string FileName = string.Empty;
        string FilePath = string.Empty;
        string extension = string.Empty;
        string FileSize = string.Empty;
        string reffering = "";

        if (txtfname.Text != "" && txtmobno.Text != "" && FileUpload1.PostedFile.FileName != "")
        {
            string fullname = txtfname.Text + txtlname.Text;
            DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from JobApplications WHERE 
            Fname+LName='" + fullname + "' AND MobNo = '" + txtmobno.Text + "'");

            DataTable dt2 = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  EmpName from EmployeeMaster WHERE 
            EmpEmailId='" + Session["EmailId"].ToString() + "'");
            if (dt2.Rows.Count > 0)
            {
                DataRow row = dt2.Rows[0];
                reffering = row["EmpName"].ToString();
            }
       
            if (dt.Rows.Count > 0)
            {
                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>if(window.confirm('Already Applied')){window.location.href='VacanciesList.aspx'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
            }
            else
            {
                extension = Path.GetExtension(FileUpload1.FileName);
                FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + FileUpload1.PostedFile.FileName;
                FileSize = FileName.Length.ToString() + " Bytes";
                FilePath = @"~/CVUploads/" + FileName.Trim().ToString();
                FileUpload1.PostedFile.SaveAs(Server.MapPath(FilePath));

                int rowin = int.Parse(txtvcnid.Value);
                int vid = int.Parse(VacanciesList.DataKeys[rowin]["Id"].ToString());
                //int vid = int.Parse(VacanciesList.Rows[rowin].Cells[4].Text);
                User user = new User();
                //string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                //FileUpload1.PostedFile.SaveAs(Server.MapPath("~/CVUploads/") + fileName);
                int status = user.AddJobApplication(txtfname.Text, txtmname.Text, txtlname.Text, txtmobno.Text, txtemail.Text, vid,
                    FileName, FileSize, FilePath, extension, Session["EmailId"].ToString(), reffering);
                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>if(window.confirm('Application Submitted')){window.location.href='VacanciesList.aspx'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
            }

        }


    }

    protected void Search(object sender, EventArgs e)
    {
        this.bindvacanylist();
    }
    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        VacanciesList.PageIndex = e.NewPageIndex;
        this.bindvacanylist();
    }
    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Text = Regex.Replace(e.Row.Cells[0].Text, txtsearchcand.Text.Trim(), delegate (Match match)
            {
                return string.Format("<span style = 'background-color:#D9EDF7'>{0}</span>", match.Value);
            }, RegexOptions.IgnoreCase);
        }
    }
}
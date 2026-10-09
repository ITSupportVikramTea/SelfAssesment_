using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_UniqueApplicant : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["EmailId"] == null || Session["UserType"].ToString().ToLower() != "employeehr")
            {
                Response.Redirect("../Default.aspx");

            }
            bindvacanylist();
        }
    }
    protected void Search(object sender, EventArgs e)
    {
        this.bindvacanylist();
    }
    protected void bindvacanylist()
    {
        string str = txtsearchvacancy.Text.Trim();
        DataTable dt;
        User user = new User();
        int empId = user.getEmployeeId(Session["EmailId"].ToString());
        if (str == "")
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT  * from AvailableVacancies where 
Id not In(select VacancyId from HiddenVacancies where EmpId = " + empId + ") order by CASE WHEN ZONE is null then 1 else 0 end, ZONE");
        }
        else
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT  * from AvailableVacancies 
            where Id not In(select VacancyId from HiddenVacancies where EmpId = " + empId + ") and " +
            "(Department LIKE '%" + str + "%' or Position LIKE '%" + str + "%' or HQ LIKE '%" + str + "%'or ZONE LIKE '%" + str + "%'or SDM LIKE '%" + str + "%') order by  CASE WHEN ZONE is null then 1 else 0 end, ZONE");
        }

        VacanciesList.DataSource = dt;
        VacanciesList.DataBind();
    }
    protected void btnaddvacancy_click(object sender, EventArgs e)
    {
        Response.Redirect("AddVacancies.aspx");
    }
    protected void VacanciesList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //if(e.CommandName == "Delete")
        //{
        //    int id = 0;
        //    int.TryParse(e.CommandArgument.ToString(), out id);
        //    if(id != 0)
        //    {
        //        objSQLDataFile.ExecuteDataTableSQL(@"DELETE FROM AvailableVacancies WHERE Id=" + id);
        //    }
        //    ClientScriptManager CSM = Page.ClientScript;
        //    string strconfirm = "<script>if(window.confirm('Vacancy Deleted')){window.location.href='VacanciesList.aspx'}</script>";
        //    CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);

        //}

        try
        {
            int recordid = Convert.ToInt32(e.CommandName);
            Response.Redirect("AddVacancies.aspx?id=" + recordid);
        }
        catch { }



    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (FileUpload1.PostedFile != null)
        {
            int res = 0;


            try
            {
                string filePath = Server.MapPath("../UploadFile/");
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss");
                filePath = filePath + fileName + ".xlsx";
                FileUpload1.PostedFile.SaveAs(filePath);

                XLWorkbook xLWorkbook = new XLWorkbook(filePath);
                int row = 2;
                while (xLWorkbook.Worksheets.Worksheet(1).Cell(row, 1).GetString() != "")
                {
                    string Department = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 1).GetString();
                    string Position = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 2).GetString();
                    string Vacancies = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 3).GetString();
                    string ZONE = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 4).GetString();
                    string SDM = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 5).GetString();
                    string HQ = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 6).GetString();

                    User addusdr = new User();

                    res = addusdr.AddVacancies(Department, Position, int.Parse(Vacancies), HQ,
                        Session["EmailId"].ToString(), ZONE, SDM);

                    row++;
                }
                if (res > 0)
                {
                    string message = "File Uploaded Successfully";
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<script>alert('");
                    sb.Append(message);
                    sb.Append("'); </script>");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
                }
                else if (res < 0)
                {
                    string message = "Some Duplicate Vacancies is/are not uploaded!";
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<script>alert('");
                    sb.Append(message);
                    sb.Append("'); </script>");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
                }
            }
            catch
            {
                string message = "There is some Error, File Not Uploaded";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<script>alert('");
                sb.Append(message);
                sb.Append("'); </script>");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
            }
        }
    }
    protected void btnSample_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable("Grid");
        dt.Columns.AddRange(new DataColumn[6] {
            new DataColumn("Department"),
            new DataColumn("Position"),
            new DataColumn("Vacancies"),
            new DataColumn("ZONE"),
            new DataColumn("SDM"),
            new DataColumn("HQ")
            });

        dt.Rows.Add("IT", "Software Developer", "2", "", "", "JALNA");

        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dt);
            using (MemoryStream stream = new MemoryStream())
            {
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=VacancySample.xlsx");
                wb.SaveAs(stream);
                stream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
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
            e.Row.Cells[0].Text = Regex.Replace(e.Row.Cells[0].Text, txtsearchvacancy.Text.Trim(), delegate (Match match)
            {
                return string.Format("<span style = 'background-color:#D9EDF7'>{0}</span>", match.Value);
            }, RegexOptions.IgnoreCase);
        }
    }
    protected void btnuploadcv_Click(object sender, EventArgs e)
    {
        string FileName = string.Empty;
        string FilePath = string.Empty;
        string extension = string.Empty;
        string FileSize = string.Empty;
        string reffering = "";

        if (txtfname.Text != "" && txtmobno.Text != "" && FileUpload2.PostedFile.FileName != "")
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
                extension = Path.GetExtension(FileUpload2.FileName);
                FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + FileUpload2.PostedFile.FileName;
                FileSize = FileName.Length.ToString() + " Bytes";
                FilePath = @"~/CVUploads/" + FileName.Trim().ToString();
                FileUpload2.PostedFile.SaveAs(Server.MapPath(FilePath));

                int rowin = int.Parse(txtvcnid.Value);
                int vid = int.Parse(VacanciesList.DataKeys[rowin]["Id"].ToString());
                //int vid = int.Parse(VacanciesList.Rows[rowin].Cells[4].Text);
                User user = new User();
                //string fileName = Path.GetFileName(FileUpload2.PostedFile.FileName);
                //FileUpload2.PostedFile.SaveAs(Server.MapPath("~/CVUploads/") + fileName);
                int status = user.AddJobApplication(txtfname.Text, txtmname.Text, txtlname.Text, txtmobno.Text, txtemail.Text, vid,
                    FileName, FileSize, FilePath, extension, Session["EmailId"].ToString(), reffering);
                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>if(window.confirm('Application Submitted')){window.location.href='VacanciesList.aspx'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
            }

        }
        else
        {
            string message = "Please Check all the fields!";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script>alert('");
            sb.Append(message);
            sb.Append("'); </script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
        }
    }
    protected void VacanciesList_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
}
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

public partial class Admin_SkillsList : System.Web.UI.Page
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

            bindgetSkillList();

        }
    }

    public void bindgetSkillList()
    {
        DataTable dt;
        string str = txtsearchskills.Text.Trim();

        if (str == "")
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from Skills");
        }
        else
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from Skills where Competency like'%" + str + "%' or ManagementSkills like'%" + str + "%' ");
        }

        SkillsList.DataSource = dt;
        SkillsList.DataBind();
    }

    protected void btnaddskills_click(object sender, EventArgs e)
    {
        Response.Redirect("AddSkills.aspx");
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (FileUpload1.PostedFile != null)
        {
            int res = 0, once =0;
            string filePath = Server.MapPath("../UploadFile/"); ;
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss");
            filePath = filePath + fileName + ".xlsx";
            FileUpload1.PostedFile.SaveAs(filePath);



            XLWorkbook xLWorkbook = new XLWorkbook(filePath);
            int row = 2;
            while (xLWorkbook.Worksheets.Worksheet(1).Cell(row, 1).GetString() != "")
            {
                try
                {
                    string Competency = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 1).GetString();
                    string ManagementSkills = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 2).GetString();
                    string Description = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 3).GetString();
                    string EmpEmailId = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 4).GetString();
                    //string RatingSelf = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 5).GetString();
                    //string RMRating = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 6).GetString();
                    //string HODRating = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 7).GetString();
                    string OverallScore = xLWorkbook.Worksheets.Worksheet(1).Cell(row, 8).GetString();


                    User addusdr = new User();
                    //res = addusdr.AddSkills(Competency, ManagementSkills, Description, EmpEmailId, int.Parse(RatingSelf),
                    //int.Parse(RMRating),int.Parse(HODRating),int.Parse(OverallScore), Session["EmailId"].ToString());
                    res = addusdr.AddSkills(Competency, ManagementSkills, Description, Session["EmailId"].ToString());


                }
                catch
                {
                    once = 1;
                }


                row++;
            }
            if (once > 0)
            {
                string message = "File Uploaded, without some entries!";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<script>alert('");
                sb.Append(message);
                sb.Append("'); </script>");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
            }
            else if (res > 0)
            {
                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>if(window.confirm('File Uploaded Successfully')){window.location.href='SkillsList.aspx'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
            }
            else
            {
                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>if(window.confirm('There is some Error, File Not Uploaded')){window.location.href='SkillsList.aspx'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
            }


        }
    }

    protected void SkillsList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            int recordid = Convert.ToInt32(e.CommandName);
            Response.Redirect("AddSkills.aspx?id=" + recordid);
        }
          

    }

    protected void btnSample_Click(object sender, EventArgs e)
    {

        DataTable dt = new DataTable("Grid");
        //dt.Columns.AddRange(new DataColumn[8] {
        //    new DataColumn("Competency"),
        //    new DataColumn("ManagementSkills"),
        //    new DataColumn("Description"),
        //    new DataColumn("EmpEmailId"),
        //    new DataColumn("RatingSelf"),
        //    new DataColumn("RMRating"),
        //    new DataColumn("HODRating"),
        //    new DataColumn("OverallScore")
        //    });
        dt.Columns.AddRange(new DataColumn[3] {
            new DataColumn("Competency"),
            new DataColumn("Management Skills"),
            new DataColumn("Description")
           // new DataColumn("EmpEmailId")
            });
        dt.Rows.Add("Good", "9", "Good at Management");
        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dt);
            using (MemoryStream stream = new MemoryStream())
            {
                //wb.SaveAs(stream);
                //return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Export.xlsx");
                //wb.Worksheets.Add(dt, "Customers");

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=SkillSample.xlsx");
                wb.SaveAs(stream);
                stream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();

            }
        }
    }

    protected void SkillsList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Text = Regex.Replace(e.Row.Cells[0].Text, txtsearchskills.Text.Trim(), delegate (Match match)
            {
                return string.Format("<span style = 'background-color:#D9EDF7'>{0}</span>", match.Value);
            }, RegexOptions.IgnoreCase);
        }
    }

    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SkillsList.PageIndex = e.NewPageIndex;
        this.bindgetSkillList();
    }

    protected void Search(object sender, EventArgs e)
    {
        this.bindgetSkillList();
    }
}
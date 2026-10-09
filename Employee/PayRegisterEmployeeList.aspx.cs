using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_PayRegisterEmployeeList : System.Web.UI.Page
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
            bindPayRegisterEmployeesList();
        }
    }

    protected void bindPayRegisterEmployeesList(string empname = "")
    {
       /* DataTable dt;
        dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from PayRegisterEmployees where year = (datepart(year,getdate())) AND is_mail_sent = 0");
      
        PayRegisterEmployeeList.DataSource = dt;
        PayRegisterEmployeeList.DataBind();*/

        string str = txtsearchuser.Text.Trim();
        DataTable dt;

        if (empname != "")
        {
            string isMailSentValue = dlIsMailSent.SelectedItem.Value;
            string query = "SELECT * FROM PayRegisterEmployees WHERE employee_name LIKE '%" + str + "%' ";

            if (isMailSentValue == "1") //  For Mail Sent
            {
                query += "AND is_mail_sent = 1";
            }
            else if (isMailSentValue == "2") // For Mail Not Sent
            {
                query += "AND is_mail_sent = 0";
            }
            query += " AND year = YEAR(GETDATE())";
            query += " ORDER BY employee_name ASC";

            dt = objSQLDataFile.ExecuteDataTableSQL(query);
        }


        else if (str != "")
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT  * from PayRegisterEmployees where employee_name LIKE '%" + str + "%' AND year = (datepart(year,getdate())) order by employee_name");
        }

        else
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from PayRegisterEmployees where year = (datepart(year,getdate())) order by employee_name");
        }

        PayRegisterEmployeeList.AllowPaging = false;
        PayRegisterEmployeeList.DataSource = dt;
        PayRegisterEmployeeList.DataBind();
    }


    //protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    PayRegisterEmployeeList.PageIndex = e.NewPageIndex;
    //    this.bindPayRegisterEmployeesList();
    //}
    protected void Search(object sender, EventArgs e)
    {

        if(dlIsMailSent.SelectedValue != "")
        {
            string dlmail = dlIsMailSent.SelectedItem.Value;
            bindPayRegisterEmployeesList(dlmail);
        }
        else
        {
            bindPayRegisterEmployeesList();
        }
    }

    protected void btnSendEmails_Click(object sender, EventArgs e)
    {
        PassNMail mailSend = new PassNMail();
        string emailContent = @"<html>

        <body>
        <form>
        <p>Dear [empname],</p>
         <h5>Revised Letter</h5>
        <p>It gives us immense pleasure to announce our <b>Annual Appraisal for the Year FY 2024-25. </b>. Based on your performance evaluation against defied KPIs and feedback received from your reporting manager, Management has decided to appraise to your compensation with effect of DEC 2025 & all statutory payables will also be applicable from this effective date.</p>
        <p>Please find attached your Appraisal Letter and a along with the revised pay structure for your reference.</p> 
        <p>We wish you success in your continued commitment and efforts for the growth of the organization.</p>  
        <P>Please feel free to contact the HR department for any salary structure-related queries. </p>
        <p>kindly go through it and provide your acceptance within next 2 days.</p>
        <p>Reply is mandatory for the same, else will consider it as your acceptance.</p>
        <p>Once again, congratulations on your success and here's to more great work in the future.</p>
        <p>We wish you all the best!</p>
    
        <h5>Regards,<br>Vishal V Kulkarni <br>Human Resources and Administration</h5>
        </form>
        </body>
        </html>";
        //check for increment letter row while accessing it
        foreach (GridViewRow row in PayRegisterEmployeeList.Rows)
        {
            CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
            if (chkSelect.Checked)
            {
                int id = Convert.ToInt32(PayRegisterEmployeeList.DataKeys[row.RowIndex].Value);
                string emp_abbr = row.Cells[2].Text;
                string employeeName = row.Cells[3].Text;
                string emailId = row.Cells[4].Text;
              //string incrementLetterPath = row.Cells[6].Text;
                HyperLink hlIncrementLetter = (HyperLink)row.FindControl("hlIncrementLetter");
                string incrementLetterPath = hlIncrementLetter.NavigateUrl;
                string physicalFilePath = Server.MapPath(incrementLetterPath);

                // string replacedEmailContent = emailContent.Replace("[empname]", employeeName);
                string replacedEmailContent = emailContent.Replace("[empname]", emp_abbr + " " + employeeName);

                mailSend.SendIncrementLetter(emailId, replacedEmailContent, "Annual Appraisal for the 2024-2025", physicalFilePath);
                objSQLDataFile.ExecuteDataTableSQL("Update PayRegisterEmployees SET is_mail_sent = 1 WHERE Id = '" + id + "'");
            }
        }
        bindPayRegisterEmployeesList();
    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelectAll = (CheckBox)sender;

        foreach (GridViewRow row in PayRegisterEmployeeList.Rows)
        {
            CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
            chkSelect.Checked = chkSelectAll.Checked;
        }
    }

    protected void dlIsMailSent_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindPayRegisterEmployeesList(dlIsMailSent.SelectedItem.Value);
    }
}
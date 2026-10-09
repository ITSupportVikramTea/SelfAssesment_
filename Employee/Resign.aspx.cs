using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_Resign : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    DateTimeConverter dateTimeConverter = new DateTimeConverter();
    static int empId;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["EmailId"] == null)
            {
                Response.Redirect("../Default.aspx");
            }

            if (Request.QueryString.AllKeys.Contains("id"))
            {

                DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT Reason, Details  from Resignations where ResignationId=" + Request.QueryString["id"].ToString());
                if (dt.Rows.Count > 0)
                {
                    ddlReason.Text = dt.Rows[0]["Reason"].ToString();
                    descriptionTextBox.Text = dt.Rows[0]["Details"].ToString();

                    btnSubmit.Visible = false;
                    btnCancel.Visible = false;
                    descriptionTextBox.ReadOnly = true;
                    ddlReason.Enabled = false;
                }
                //else
                //    btnsave.Text = "Save";
            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        User user = new User();
        int res = 0;
        DataTable dt = new DataTable();
        string loggedUserEmail = Session["EmailId"].ToString();

        dt = user.GetEmployeeDetailsByEmail(Session["EmailId"].ToString());
        string reason = ddlReason.SelectedValue;
        string details = descriptionTextBox.Text;

        string empname = "";
        string location = "";
        string rmname = "";
        DateTime serverTime = DateTime.Now;
        if (dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            int EmpId = int.Parse(row["Id"].ToString());
            string ErpId = row["ERPID"].ToString();
            string hodname = row["HOD"].ToString();
            rmname = row["RM"].ToString();
            empname = row["EmpName"].ToString();
            location = row["Location"].ToString();
            DateTime istTime = DateTimeConverter.ConvertToIST(serverTime);
           
            res = user.AddResignations(EmpId, ErpId, reason, details, rmname, hodname, istTime);
        }
        if (res > 0)
        {
            PassNMail passNMail = new PassNMail();
            #region send email to RM, HOD, HR that Resignation is added by employee

            empId = user.getEmployeeId(Session["EmailId"].ToString());

            List<string> tos = new List<string>();
            DataTable rmHodEmailTable = objSQLDataFile.ExecuteDataTableSQL(@"SELECT EmpEmailId FROM EmployeeMaster WHERE EmpName IN 
            ((SELECT RM FROM EmployeeMaster WHERE id = " + empId + "), (SELECT HOD FROM EmployeeMaster WHERE id = " + empId + "))");

            foreach (DataRow row in rmHodEmailTable.Rows)
            {
                tos.Add(row["EmpEmailId"].ToString());
            }

            //working on this (14-10-2023)-Comment pr remove below region to not send mail to deptheads
            // Add other email recipients (no need to add deptheads email when sending mail on resignation added - Changes made as per Chetan Sirs requirement)

            // #region Adding mail of department_Heads to list
            // List<string> otherEmailList = new List<string>();
            // otherEmailList.Add("itsupport@vikramtea.com");
            //// otherEmailList.Add("hrmail");
            // tos.AddRange(otherEmailList);
            // #endregion

            passNMail.SendMailToRM(tos, empname, location, rmname);

            #endregion

            ClientScriptManager CSM = Page.ClientScript;
            string strconfirm = "<script>if(window.confirm('Resignation Added')){window.location.href='ResingList.aspx'}</script>";
            CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
        }
        else
        {
            ClientScriptManager CSM = Page.ClientScript;
            string strconfirm = "<script>if(window.confirm('Existing Resignation is already approved.')){window.location.href='ResingList.aspx'}</script>";
            CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);

        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ResingList.aspx");
    }
}
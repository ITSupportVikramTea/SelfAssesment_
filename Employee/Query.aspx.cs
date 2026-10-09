using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections;

public partial class Employee_Query : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    string statusForSubmitClose = "Open";
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
                fillDetails();
            }

        }
    }
    protected void fillDetails()
    {
        btnSubmit.Text = "Send Reply";
        pnlhistory.Visible = true;
        dvattachment.Visible = false;
        StringBuilder Enqury = new StringBuilder();

        if (Session["UserType"].ToString().Equals("EMPLOYEEHR"))
        {
            btnSubmitClose.Visible = true;
        }

        
        

        DataTable dt2 = objSQLDataFile.ExecuteDataTableSQL(@"select distinct pqm.QueryId,FileName, pqmr.RepliedByName, pqmr.ReplyQuery,pqmr.DateTime from [dbo].[PMSQueryReply] pqmr
right join [dbo].[PMSQueryMaster] pqm on
pqm.QueryId = pqmr.QueryId
where pqm.QueryId=" + Request.QueryString["id"].ToString() + " and ReplyQuery != '' order by DateTime desc");

        if (dt2.Rows.Count > 0)
        {

            foreach (DataRow row in dt2.Rows)
            {
                //descriptionTextBox2.Text = row["ReplyQuery"].ToString();
                //datentime.InnerText = row["DateTime"].ToString();
                //list.Add(row["ReplyQuery"].ToString());
                //list.Add(row["RepliedByName"].ToString());
                //list.Add(row["DateTime"].ToString());
                //data.InnerText = builder.ToString();
                Enqury.Append(@"<div class='row'><hr />
	                                <div class='col-md-6'>
	                                    <h4>
		                                    <b><lable class='form-group pull-left text-primary'>" + row["DateTime"].ToString() + @"</lable></b>
                                        </h4>
                                     </div>
                                    <div class='col-md-6'>
		                                <h4>
			                                <b><lable class='form-group pull-right text-primary'>" + row["RepliedByName"].ToString() + @" </lable></b>
		                                </h4>
	                                </div>
                                </div>
                            <div class='row'>
	                            <div class='col-md-12' style='word-wrap: break-word'>
		                            <b><p>" + row["ReplyQuery"].ToString() + @"</p></b>
	                            </div>
                            </div>");
            }
            pnlhistory.Controls.Add(new Literal { Text = Enqury.ToString() });

        }


        //employee query in the last
        Enqury = null;
        Enqury = new StringBuilder();
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"select FileName, EmpName, Query, pqm.DateTime,Subject from [dbo].[PMSQueryMaster] pqm
where pqm.QueryId=" + Request.QueryString["id"].ToString());
        if (dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            txtsubject.Text = row["Subject"].ToString();
            txtsubject.Attributes.Add("disabled", "true");

            Enqury.Append(@"<div class='row'><hr />
	                                <div class='col-md-6'>
	                                    <h4>
		                                    <b><lable class='form-group pull-left text-primary'>" + row["DateTime"].ToString() + @"</lable></b>
                                        </h4>
                                     </div>
                                    <div class='col-md-6'>
		                                <h4>
			                                <b><lable class='form-group pull-right text-primary'>" + row["EmpName"].ToString() + @" </lable></b>
		                                </h4>
	                                </div>
                                </div>
                            <div class='row'>
	                            <div class='col-md-12' style='word-wrap: break-word'>
		                            <b><p>" + row["Query"].ToString() + @"</p></b>
	                            </div>
                            </div>");
            pnlhistory.Controls.Add(new Literal { Text = Enqury.ToString() });

        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if ((txtsubject.Text != "" && descriptionTextBox.Text != "") || btnSubmit.Text == "Send Reply")
        {
            User user = new User();
            string empEmail = Session["EmailId"].ToString();
            string descText = descriptionTextBox.Text;
            int empID = user.getEmployeeId(empEmail);
            string empName = user.getEmployeeName(empEmail);

            if (btnSubmit.Text == "Submit")
            {
                string subject = txtsubject.Text;
                string fileName = "";
                string filePath = "";

                if (fuattachment.PostedFile.FileName != "")
                {
                    fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + fuattachment.PostedFile.FileName;
                    filePath = @"~/QuerySupportFiles/" + fileName.Trim().ToString();
                    fuattachment.PostedFile.SaveAs(Server.MapPath(filePath));
                }

                int res = user.AddQuery(empID, empName, descText, subject, "Open", fileName, filePath);

                if (res > 0)
                {
                    ClientScriptManager CSM = Page.ClientScript;
                    string strconfirm = "<script>if(window.confirm('Query Submitted!')){window.location.href='QueryList.aspx'}</script>";
                    CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                }
            }
            else if (btnSubmit.Text == "Send Reply")
            {
                string qid = Request.QueryString["id"].ToString();
                //string descText2 = descriptionTextBox2.Text;

                int res = user.AddQueryReply(int.Parse(qid), descText, empName, empID, statusForSubmitClose);

                if (res > 0)
                {
                    ClientScriptManager CSM = Page.ClientScript;
                    string strconfirm = "<script>if(window.confirm('Reply Submitted!')){window.location.href='QueryList.aspx'}</script>";
                    CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                }
            }



        }
        else
        {
            ClientScriptManager CSM = Page.ClientScript;
            string strconfirm = "<script>alert('Subject and Content is Compulsory!')</script>";
            CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
        }
    }
    protected void btnSubmitClose_Click(object sender, EventArgs e)
    {
        statusForSubmitClose = "Close";
        btnSubmit_Click(sender, e);
    }
}
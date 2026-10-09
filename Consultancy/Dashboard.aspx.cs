using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Consultancy_Dashboard : System.Web.UI.Page
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
            bindjobapp();
        }
    }
    protected void bindjobapp()
    {
        string str = txtsearchcand.Text.Trim();
        string currUser = Session["EmailId"].ToString();
        DataTable dt;
        if (str != "")
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"select ja.id Id,InterviewStatus,ja.FinalResult, FName+' '+LName FullName,
        MobNo, Position, HQ,ja.CreatedDate
        from [dbo].[AvailableVacancies] ajv 
        join [dbo].[JobApplications] ja on ajv.Id = ja.VacancyId 
        where ja.RefferedBy = '" + currUser + "' and FName Like '%" + str + "%' or LName Like '%" + str + "%'");
        }
        else
        {
            dt = objSQLDataFile.ExecuteDataTableSQL(@"select ja.id Id,InterviewStatus,ja.FinalResult, FName+' '+LName FullName,
        MobNo, Position, HQ,ja.CreatedDate
        from [dbo].[AvailableVacancies] ajv 
        join [dbo].[JobApplications] ja on ajv.Id = ja.VacancyId 
        where ja.RefferedBy = '" + currUser + "'");
        }


        gdJobAppList.DataSource = dt;
        gdJobAppList.DataBind();

    }
    protected void gdJobAppList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //string FinalResult = e.Row.Cells[6].Text;
            //if (FinalResult == "Shortlisted" || FinalResult.Contains("Shortlisted"))
            //{
            //    Button button = e.Row.Cells[7].FindControl("btnmodel") as Button;
            //    button.Visible = true;
            //}
            e.Row.Cells[0].Text = Regex.Replace(e.Row.Cells[0].Text, txtsearchcand.Text.Trim(), delegate (Match match)
            {
                return string.Format("<span style = 'background-color:#D9EDF7'>{0}</span>", match.Value);
            }, RegexOptions.IgnoreCase);
        }
    }
    //protected void btnsubmit_click(object sender, EventArgs e)
    //{
    //    if (txtemail.Text != "" && txtpass.Text != "")
    //    {
    //        DataTable dt = objSQLDataFile.ExecuteDataTableSQL("SELECT * FROM UserMaster WHERE EmailId='" + txtemail.Text + "'");
    //        if (dt.Rows.Count > 0)
    //        {
    //            ClientScriptManager CSM = Page.ClientScript;
    //            string strconfirm = "<script>if(window.confirm('User already exists')){window.location.href='Dashboard.aspx'}</script>";
    //            CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);

    //            //lblerror.Text = "User Already Exists";
    //            //lblerror.ForeColor = Color.Red;
    //        }
    //        else
    //        {
    //            User user = new User();
    //            int res = user.AddUser("Candidate", txtemail.Text, txtpass.Text, "", Session["EmailId"].ToString(),0);
    //            if (res > 0)
    //            {
    //                ClientScriptManager CSM = Page.ClientScript;
    //                string strconfirm = "<script>if(window.confirm('Login Created, Please Login')){window.location.href='Dashboard.aspx'}</script>";
    //                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
    //            }

    //        }
    //    }
    //    else
    //    {
    //        ClientScriptManager CSM = Page.ClientScript;
    //        string strconfirm = "<script>if(window.confirm('Please check email & pasword')){window.location.href='Dashboard.aspx'}</script>";
    //        CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
    //    }
    //}
    protected void Search(object sender, EventArgs e)
    {
        this.bindjobapp();
    }
    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdJobAppList.PageIndex = e.NewPageIndex;
        this.bindjobapp();
    }
    protected void btnChangepass_click(object sender, EventArgs e)
    {
        Response.Redirect("ChangePassword.aspx");
    }

}
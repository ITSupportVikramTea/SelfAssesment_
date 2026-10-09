using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webpages_MasterPage : System.Web.UI.MasterPage
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    protected static string Username { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate");
            HttpContext.Current.Response.AddHeader("Pragma", "no-cache");
            HttpContext.Current.Response.AddHeader("Expires", "0");
            Response.ClearHeaders();
            Response.AddHeader("Cache-Control", "no-cache, no-store, max-age=0, must-revalidate");
            Response.AddHeader("Pragma", "no-cache");
            if (Session["EmailId"] == null || Session["UserType"].ToString().ToLower() != "candidate")
            {
                Response.Redirect("../Default.aspx");
            }
            getUsername();


        }
    }
    protected void getUsername()
    {
        DataTable res = objSQLDataFile.ExecuteDataTableSQL(@"SELECT EmpName from EmployeeMaster
                            WHERE EmpEmailId = '" + Session["EmailId"].ToString() + "'");
        if (res.Rows.Count > 0)
        {
            DataRow row2 = res.Rows[0];
            Username = row2["EmpName"].ToString();
        }
    }
}

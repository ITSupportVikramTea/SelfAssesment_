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

            if (Session["EmailId"] == null)
            {
                Response.Redirect("../Default.aspx");
            }
            if (Session["UserType"] != null)
            {
                if (Session["UserType"].ToString().ToLower().Equals("employeehr"))
                {
                    //temp changes made after contacting with nilesh sir
                    // delete below code and add folling line only.(today's date 28062022 time 6:16pm)
                    //hrhideunhide(true);

                    if (Session["EmailId"].Equals("nileshchudiwal1981@yahoo.com"))
                    {
                        hrhideunhide(true);
                    }
                    else
                    {
                        hraddRatings.Visible = false;
                        scores.Visible = false;

                    }
                }
                else
                {
                    hrhideunhide(false);
                    if (!(Session["EmailId"].ToString().Equals("hanifkhallikhan786@gmail.com")))
                    {
                        form16.Visible = false;
                    }
                }
            }
        }

    }

    public void hrhideunhide(bool obj)
    {
        try
        {
            DataTable res = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT IsHOD,IsRM from EmployeeMaster WHERE EmpEmailId = '" + Session["EmailId"].ToString() + "'");
            DataRow row = res.Rows[0];
            int ishod = int.Parse(row["IsHOD"].ToString());
            int isrm = int.Parse(row["IsRM"].ToString());
            HRRecrutment.Visible = obj;
            hraddRatings.Visible = obj;
            HRCons.Visible = obj;
            //HRReport.Visible = obj;
            assetLink.Visible = obj;
            lstkpi.Visible = obj;
            //assgnAssetsList.Visible = obj;
            if (obj)
            {
                empVacancies.Visible = false;
            }
            if (ishod > 0)
            {
                hraddRatings.Visible = true;
            }
            if (ishod > 0 || isrm > 0 || obj)
            {
                scores.Visible = true;
                assgnAssetsList.Visible = true;
            }
            else
            {
                assgnAssetsList.Visible = false;

            }
        }
        catch
        {

        }
        
    }
}

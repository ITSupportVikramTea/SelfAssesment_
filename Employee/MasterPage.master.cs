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
                   // if (Session["EmailId"].Equals("fake5@gmail.com"))
                   if (Session["EmailId"].Equals("vishalvk82@gmail.com"))
                   {
                        hrhideunhide(true);
                   }
                    else
                    {
                        hraddRatings.Visible = false;
                        scores.Visible = false;
                    }
                }
                //trial code to give permission to assign IT and admin assets according to emailid(Sachin Sir)
                else if (Session["EmailId"].ToString().Equals("sachinugale9890@gmail.com"))
              //  else if (Session["EmailId"].ToString().Equals("new@gmail.com"))
                {
                    hrhideunhide(false);
                    assetLink.Visible = true;
                    assgnAssetsList.Visible = true;
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
            assetLink.Visible = obj;
            lstkpi.Visible = obj;
            recrutmentReport.Visible = obj;
            resignationReport.Visible = obj;
            
            assgnAssetsList.Visible = obj;
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

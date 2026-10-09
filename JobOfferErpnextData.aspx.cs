using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class JobOfferErpnextData : System.Web.UI.Page
{
    static ERPNextService eRPNextService = new ERPNextService();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           
            getstockledger();
        }
    }

    [WebMethod]
    public static void GetJobOfferData()
    {
        string json = "";
        try
        {
            json = eRPNextService.getJobOfferData();
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Write(json);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

         [WebMethod]
    public static void getstockledger()
    {
        string json = "";
        try
        {
            json = eRPNextService.GetSalaryStructurer();
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Write(json);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

}
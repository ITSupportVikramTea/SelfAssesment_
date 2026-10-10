using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Admin_OrganizationChart : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    static SQLDataFileSIS objSQLDataFile2 = new SQLDataFileSIS();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["EmailId"] == null)
            {
                Response.Redirect("../Default.aspx");
            }
        }
        binddldepartments();
    }

    public void binddldepartments()
    {
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"select distinct EmpDepartment from [dbo].[EmployeeMaster] where EmpDepartment != ''");
        dldepartments.DataSource = dt;
        dldepartments.DataTextField = "EmpDepartment";
        dldepartments.DataTextField = "EmpDepartment";
        dldepartments.DataBind();
    }

    [WebMethod]
    public static string getCharData(string department)
    {
        List<ChartDetails> chartData = new List<ChartDetails>();

        DataTable dt = objSQLDataFile2.ExecuteDataTableSQL(@"select Id, HOD, RM, ASM, SDM, EmpName,EmpDesignation ,EmpDepartment
from EmployeeMaster WHERE EmpDepartment = '"+department+ "' and status = 'Active'");
        if (dt.Rows.Count > 0)
        {

            foreach(DataRow row in dt.Rows)
            {
                chartData.Add(new ChartDetails
                {
                    Id = int.Parse(row["Id"].ToString()),
                    HOD = row["HOD"].ToString(),
                    RM = row["RM"].ToString(),
                    ASM = row["ASM"].ToString(),
                    SDM = row["SDM"].ToString(),
                    EmpName = row["EmpName"].ToString(),
                    EmpDesignation = row["EmpDesignation"].ToString(),
                    EmpDepartment = row["EmpDepartment"].ToString()

                });

            }

        }

        string data = JsonConvert.SerializeObject(chartData);
        return (data);
    }

    [WebMethod]
    public static string getCharDataType(string type)
    {
        List<ChartDetails> chartData = new List<ChartDetails>();

        DataTable dt = objSQLDataFile2.ExecuteDataTableSQL(@"select top 400 Id, HOD, RM, ASM, SDM, EmpName,EmpDesignation ,EmpDepartment
from EmployeeMaster WHERE status = 'Active'");
        if (dt.Rows.Count > 0)
        {

            foreach (DataRow row in dt.Rows)
            {
                chartData.Add(new ChartDetails
                {
                    Id = int.Parse(row["Id"].ToString()),
                    HOD = row["HOD"].ToString(),
                    RM = row["RM"].ToString(),
                    ASM = row["ASM"].ToString(),
                    SDM = row["SDM"].ToString(),
                    EmpName = row["EmpName"].ToString(),
                    EmpDesignation = row["EmpDesignation"].ToString(),
                    EmpDepartment = row["EmpDepartment"].ToString()

                });

            }

        }

        string data = JsonConvert.SerializeObject(chartData);
        return (data);
    }

}


public class ChartDetails{
    public int Id { get; set; }
    public string HOD { get; set; }
    public string RM { get; set; }
    public string ASM { get; set; }
    public string SDM { get; set; }
    public string EmpName { get; set; }
    public string EmpDesignation { get; set; }
    public string EmpDepartment { get; set; }

}
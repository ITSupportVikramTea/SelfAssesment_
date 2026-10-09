using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_LeaveDashboard : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();

    protected void Page_Load(object sender, EventArgs e)
    {
        bindLeaves();
    }

    public void bindLeaves()
    {
        DataTable empname = objSQLDataFile.ExecuteDataTableSQL(@"SELECT EmpName FROM EmployeeMaster WHERE EmpEmailId='" + Session["EmailID"] + "'");
        DataRow name = empname.Rows[0];
        string masterEmp = name["empName"].ToString();
        DataTable leave = objSQLDataFile.ExecuteDataTableSQL(@"SELECT employeeName,FromDate,ToDate,totalLeaveDay FROM Leaves WHERE RM ='" + masterEmp + "'");
        if (leave.Rows.Count > 0)
        {
            DataRow emp = leave.Rows[0];
            string employee = emp["employeeName"].ToString();
            leaves.DataSource = leave;
            leaves.DataBind();
        }
    }
}
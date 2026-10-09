using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_TestAttendence : System.Web.UI.Page
{
    User objuser = new User();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["EmailId"] == null)
            {
                Response.Redirect("../Default.aspx");
            }

            Session["UserPF"] = "000000"; 
            DataTable getuser = objuser.GetEmployeeDetailsByEmail(Session["EmailId"].ToString());
            if (getuser.Rows.Count > 0)
            {
                string ERPID = getuser.Rows[0]["ERPID"].ToString().Trim();

                if (string.IsNullOrEmpty(ERPID))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(),
                    "alert",
                    "alert('Kindly Update Your Valid Employee Code');window.location ='DashboardEmployee.aspx';",
                    true);
                    return;
                }

               
                Session["UserPF"] = ERPID;

                string spreadsheetId = "1IA7Z_igo-fv2y-gKI4UDXJB8gXhQFQEtQSujDLpe3bM";
                string range = "Sheet2!A1:AS";

               
                var data = GoogleSheetsHelper.GetSheetData(spreadsheetId, range);

                if (data != null && data.Count > 0)
                {
                   
                    DataTable dt = ConvertToDataTable(data, Session["UserPF"].ToString());
                    

                    if (dt.Rows.Count > 2) 
                    {
                        
                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                    }
                    else
                    {
                        DataLabel.Text = "No data found for the specified ERPID.";
                    }
                }
                else
                {
                    DataLabel.Text = "No data found.";
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                "alert",
                "alert('Kindly Update Your Employee Code');window.location ='DashboardEmployee.aspx';",
                true);
            }
        }
    }

    public static DataTable ConvertToDataTable(IList<IList<object>> data, string sessionERPID)
    {
        DataTable dt = new DataTable();

       
        AddHeadersToDataTable(dt, data);

        for (int i = 2; i < data.Count; i++) 
        {
            if (data[i].Count == 0 || string.IsNullOrEmpty(data[i][0].ToString()))
            {
                continue;
            }

           
            string erpid = data[i][0].ToString().Trim();
            if (erpid == sessionERPID)
            {
                DataRow row = dt.NewRow();
                for (int j = 0; j < data[i].Count; j++)
                {
                    row[j] = data[i][j];
                }
                dt.Rows.Add(row);
            }
        }

        return dt;
    }

    private static void AddHeadersToDataTable(DataTable dt, IList<IList<object>> data)
    {
        if (data.Count > 0)
        {
           
            for (int i = 0; i < data[0].Count; i++)
            {
                dt.Columns.Add(data[0][i].ToString() ?? "Column" + (i + 1));
            }

           
            if (data.Count > 0)
            {
                
                DataRow headerRow1 = dt.NewRow();
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    headerRow1[i] = data[1].Count > i ? data[1][i] : DBNull.Value;
                }
                dt.Rows.Add(headerRow1);
            }
        }
    }
}

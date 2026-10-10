using DocumentFormat.OpenXml.Bibliography;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using static Telerik.Web.UI.ComboBox.ComboBoxStyles;

public partial class webpages_AddEmployees : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    protected async void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            if (Session["EmailId"] == null)
            {
                Response.Redirect("../Default.aspx");
            }
            await BindEmployees();
            // binddeparment();
            bindhodrm();
            //bindgetEmployeeList();


            if (Request.QueryString.AllKeys.Contains("id"))
            {

                DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from EmployeeMaster where id=" + Request.QueryString["id"].ToString());
                if (dt.Rows.Count > 0)
                {
                    txtfullname.Text = dt.Rows[0]["EmpName"].ToString();
                    txtemailid.Text = dt.Rows[0]["EmpEmailId"].ToString();
                    txtdoj.Text = getDateInDateFormat(dt.Rows[0]["DOJ"].ToString());
                    txtempcode.Text = dt.Rows[0]["EmpCode"].ToString();
                    // txtpromodate.Text = getDateInDateFormat(dt.Rows[0]["LastPromoYear"].ToString());
                    dlhod.Text = dt.Rows[0]["HOD"].ToString();
                    dlrm.Text = dt.Rows[0]["RM"].ToString();
                    dldesignation.Text = dt.Rows[0]["EmpDesignation"].ToString();
                    dlDepartment.Text = dt.Rows[0]["EmpDepartment"].ToString();
                    dlstatus.Text = dt.Rows[0]["Status"].ToString();
                    //desgchangedate.Text = getDateInDateFormat(dt.Rows[0]["DesgChangeDate"].ToString());
                    //existingctc.Text = dt.Rows[0]["ExistingCTC"].ToString();
                    txterpid.Text = dt.Rows[0]["ERPId"].ToString();
                    //dlsdm.Text = dt.Rows[0]["SDM"].ToString();
                    //dlasm.Text = dt.Rows[0]["ASM"].ToString();
                    txtlocation.Text = dt.Rows[0]["Location"].ToString();
                    txtdob.Text = getDateInDateFormat(dt.Rows[0]["DOB"].ToString());
                    dlnoticeperiod.Text = dt.Rows[0]["NoticePeriod"].ToString();
                    int IsHOD = 0;
                    int.TryParse(dt.Rows[0]["IsHOD"].ToString(), out IsHOD);
                    int IsRM = 0;
                    int.TryParse(dt.Rows[0]["IsRM"].ToString(), out IsRM);
                    int IsHR = 0;
                    int.TryParse(dt.Rows[0]["IsHR"].ToString(), out IsHR);
                    //update employee pending
                    if (IsHOD > 0)
                    {
                        chkHOD.Checked = true;
                    }
                    else
                    {
                        chkHOD.Checked = false;
                    }
                    if (IsRM > 0)
                    {
                        chkRM.Checked = true;
                    }
                    else
                    {
                        chkRM.Checked = false;
                    }
                    if (IsHR > 0)
                    {
                        chkHR.Checked = true;
                    }
                    else
                    {
                        chkHR.Checked = false;
                    }
                    btnsave.Text = "Update";
                    //if(dt.Rows[0]["EmpEmailId"].ToString() != "")
                    //{
                    //    txtemailid.ReadOnly = true ;
                    //}
                    txtempcode.ReadOnly = true;

                }
                else
                    btnsave.Text = "Save";
            }
        }
    }

    protected string getDateInDateFormat(string date)
    {
        if (!string.IsNullOrEmpty(date))
        {
            try
            {
                date = date.Substring(0, date.IndexOf(' '));
                DateTime dateTime = DateTime.Parse(date);
                date = dateTime.ToString("dd-MM-yyyy");
            }
            catch (Exception ex)
            {
                // Handle the exception if needed
            }
        }

        return date;
    }

    //public void binddeparment()
    //{
    //    try
    //    {
    //        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  * from Departments ");
    //        dlDepartment.DataSource = dt;
    //        dlDepartment.DataMember = "Departments";
    //        dlDepartment.DataTextField = "Departments";
    //        dlDepartment.DataValueField = "Departments";
    //        dlDepartment.DataBind();
    //    }
    //    catch(Exception e)
    //    {
    //        throw e;
    //    }

    //}

    public async Task BindEmployees()
    {
        txtlocation.Text = "";
        dlDepartment.Text = "";
        txtdob.Text = "";
        dldesignation.Text = "";
        txterpid.Text = "";
        dlhod.Text = "";
        txtemailid.Text = "";
        txtdoj.Text = "";
        dlnoticeperiod.Text = "";
        txtempcode.Text = "";
        dlrm.Text = "";

        try
        {
           

            string url = "https://vikramtea.erpnext.com/api/resource/Employee";
            string api = "59d4d40a9429113";
            string secrate = "381ce9223819c86";

            //string url = "https://vikramtea.m.frappe.cloud/api/resource/Employee";
            //string api = "59d4d40a9429113";
            //string secrate = "b7903a468291d4a";

            using (HttpClient client = new HttpClient())
            {

                client.DefaultRequestHeaders.Add(
                    "Authorization", "token " + api + ":" + secrate);



                string employeename = "?fields=[\"employee_name\"]&filters=[[\"custom_employee_current_status\",\"=\",\"Active\"]]&limit_start=0&limit_page_length=600";
                var response = await client.GetAsync(url + employeename);

                if (!response.IsSuccessStatusCode) return;

                var read = await response.Content.ReadAsStringAsync();
                dynamic data = JsonConvert.DeserializeObject(read);

                List<string> list = new List<string>();

                for (int i = 0; i < data.data.Count; i++)
                {
                    list.Add((string)data.data[i].employee_name);
                }

                txtfullname.DataSource = list;

                txtfullname.DataBind();

                txtfullname.Items.Insert(0, "-- Select Employee --");

            }
        }

        catch
        {

        }
    }


    protected async void ddlEmployee_SelectedIndexChanged(object sender, EventArgs e)
    {
        string url = "https://vikramtea.erpnext.com/api/resource/Employee";
        string api = "59d4d40a9429113";
        string secrate = "381ce9223819c86";

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", "token " + api + ":" + secrate);
            //date_of_joining,reporting_manager_name,name(erpid),designation,date_of_birth,department,location
            var email = "?fields=[\"prefered_email\",\"date_of_joining\",\"reporting_manager_name\",\"name\",\"designation\",\"date_of_birth\",\"department\",\"location\",\"notice_number_of_days\",\"provident_fund_account\",\"custom_reports_to_name\"]&filters=[[\"employee_name\",\"=\",\"" + txtfullname.SelectedValue.ToString() + "\"]]&limit_page_length=600";
            var response = await client.GetAsync(url + email);
            if (!response.IsSuccessStatusCode) return;
            var read = await response.Content.ReadAsStringAsync();
            dynamic deserilize = JsonConvert.DeserializeObject(read);

            string emailId = deserilize.data[0].prefered_email;
            string doj = deserilize.data[0].date_of_joining;
            string reporting_manager = deserilize.data[0].reporting_manager_name;
            string erpid = deserilize.data[0].name;
            string designation = deserilize.data[0].designation;
            string dob = deserilize.data[0].date_of_birth;
            string department = deserilize.data[0].department;
            string location = deserilize.data[0].location;
            string notice = deserilize.data[0].notice_number_of_days;
            string provident_fund_account = deserilize.data[0].provident_fund_account;
            string reports_to = deserilize.data[0].custom_reports_to_name;

            if (string.IsNullOrEmpty(emailId) || string.IsNullOrEmpty(reports_to) || string.IsNullOrEmpty(notice) || string.IsNullOrEmpty(provident_fund_account) || string.IsNullOrEmpty(doj) || string.IsNullOrEmpty(reporting_manager) || string.IsNullOrEmpty(erpid) || string.IsNullOrEmpty(designation) || string.IsNullOrEmpty(dob) || string.IsNullOrEmpty(department) || string.IsNullOrEmpty(location.ToString()))
            {
                ClientScriptManager script = Page.ClientScript;
                string client_script = "<script>alert('some of the fields data missing in ERP.')</script>";
                script.RegisterClientScriptBlock(this.GetType(), "alert", client_script);
                return;
            }

            txtlocation.Text = location.ToString();
            dlDepartment.Text = department.ToString();
            txtdob.Text = dob.ToString();
            dldesignation.Text = designation.ToString();
            txterpid.Text = erpid.ToString();
            dlhod.Text = reporting_manager.ToString();
            txtemailid.Text = emailId.ToString();
            txtdoj.Text = doj.ToString();
            dlnoticeperiod.Text = notice.ToString();
            txtempcode.Text = provident_fund_account.ToString();
            dlrm.Text = reports_to.ToString();

        }

    }

    public void bindhodrm()
    {

        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT  EmpName from EmployeeMaster ");
        //dlhod.DataSource = dt;
        //dlhod.DataMember = "EmpName";
        //dlhod.DataTextField = "EmpName";
        //dlhod.DataValueField = "EmpName";
        //dlhod.DataBind();

        //dlrm.DataSource = dt;
        //dlrm.DataMember = "EmpName";
        //dlrm.DataTextField = "EmpName";
        //dlrm.DataValueField = "EmpName";
        //dlrm.DataBind();

        //binding sdm and asm

        //after commenting fields
        //dlsdm.DataSource = dt;
        //dlsdm.DataMember = "EmpName";
        //dlsdm.DataTextField = "EmpName";
        //dlsdm.DataValueField = "EmpName";
        //dlsdm.DataBind();

        //dlasm.DataSource = dt;
        //dlasm.DataMember = "EmpName";
        //dlasm.DataTextField = "EmpName";
        //dlasm.DataValueField = "EmpName";
        //dlasm.DataBind();
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        User addusdr = new User();
        
        if (txtfullname.Text != "" && txtempcode.Text != "" && txtemailid.Text != "")
        {
            int checkStatus = 0;
            int IsHOD = 0;
            int IsRM = 0;
            int IsHR = 0;
            string role = dltype.Text;
            //  int notice = int.Parse(dlnoticeperiod.Text);
            int notice = 0; // Default value in case notice period is not selected

            if (!string.IsNullOrEmpty(dlnoticeperiod.SelectedValue))
            {
                notice = int.Parse(dlnoticeperiod.SelectedValue);
            }

            if (chkHOD.Checked)
            {
                IsHOD = 1;
            }
            if (chkRM.Checked)
            {
                IsRM = 1;
            }
            if (chkHR.Checked)
            {
                IsHR = 1;
                role = "EmployeeHR";
            }
            if (btnsave.Text == "Save")
            {

                checkStatus = addusdr.AddEmployee(txtfullname.Text, txtempcode.Text, txtemailid.Text, dldesignation.Text,
               dlDepartment.Text, dlstatus.Text, Session["EmailId"].ToString(), notice, dlhod.Text, dlrm.Text, txtdoj.Text,
               /*txtpromodate.Text, desgchangedate.Text, existingctc.Text,*/ role, IsHOD, IsRM, IsHR, txterpid.Text, txtlocation.Text, txtdob.Text);
                if (checkStatus > 0)
                {
                    ClientScriptManager CSM = Page.ClientScript;
                    string strconfirm = "<script>if(window.confirm('Employee Added Successfully')){window.location.href='EmployeeList.aspx'}</script>";
                    CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                }
                else
                {
                    ClientScriptManager CSM = Page.ClientScript;
                    string strconfirm = "<script>alert('There is some Error');</script>";
                    CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                }
            }
            else if (btnsave.Text == "Update")
            {
                int id = Convert.ToInt16(Request.QueryString["id"].ToString());

                checkStatus = addusdr.UpdateEmployee(id, txtfullname.Text, txtemailid.Text, dldesignation.Text,
                   dlDepartment.Text, dlstatus.Text, Session["EmailId"].ToString(), notice, dlhod.Text, dlrm.Text, txtdoj.Text,
                  /* txtpromodate.Text, desgchangedate.Text, existingctc.Text,*/ role, IsHOD, IsRM, IsHR, txterpid.Text, /*dlsdm.Text, dlasm.Text,*/ txtlocation.Text, txtdob.Text);
                if (checkStatus > 0)
                {
                    ClientScriptManager CSM = Page.ClientScript;
                    string strconfirm = "<script>if(window.confirm('Employee Updated Successfully')){window.location.href='EmployeeList.aspx'}</script>";
                    CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                }

                else
                {
                    ClientScriptManager CSM = Page.ClientScript;
                    string strconfirm = "<script>alert('There is some Error');</script>";
                    CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                }
            }

        }
        else
        {
            ClientScriptManager CSM = Page.ClientScript;
            string strconfirm = "<script>alert('EmpCode, Emaild & Name these fields are required!');</script>";
            CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
        }

    }
    protected void btncancel_click(object sender, EventArgs e)
    {
        Response.Redirect("EmployeeList.aspx");
    }
}

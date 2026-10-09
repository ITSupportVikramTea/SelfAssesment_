using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zip;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using iTextSharp.text.pdf;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;
using System.Net;
using System.Diagnostics;
using System.Threading.Tasks;
using iTextSharp.tool.xml.html;
using Telerik.Web.UI.Widgets;
using System.Data.SqlClient;
using DocumentFormat.OpenXml.ExtendedProperties;
using Org.BouncyCastle.Asn1.Ocsp;
using Newtonsoft.Json;
using System.Security.Cryptography;
using DocumentFormat.OpenXml.Bibliography;
using Newtonsoft.Json.Linq;

public partial class Employee_SalarySlip : System.Web.UI.Page
{
    #region Global Variable   
    string JsonContent = string.Empty;
    private static readonly string baseAddress = ConfigurationManager.AppSettings["baseAddress"].ToString();
    private static readonly string AccessTokenManish = ConfigurationManager.AppSettings["AccessToken"].ToString();
    SQLDataFileSIS SQLDataFile = new SQLDataFileSIS();

    #endregion
    User objuser = new User();
    
    string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

    protected async void Page_Load(object sender, EventArgs e)
    {



        if (!IsPostBack)
        {
           
            if (Session["EmailId"] != null && Session["EmailId"].ToString() == "janakjalna@gmail.com")
            {
              await LoadGrid();
            }
            else
            {
              
               
            }

           
            if (Session["EmailId"] == null)
            {
                Response.Redirect("../Default.aspx");
            }
            //Response.Redirect("../Default.aspx");
            Session["UserPF"] = "000000";
            DataTable getuser = objuser.GetEmployeeDetailsByEmail(Session["EmailId"].ToString());
            if (getuser.Rows.Count > 0)
            {

                string ERPID = getuser.Rows[0]["ERPID"].ToString();

                if (ERPID == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(),
                    "alert",
                    "alert('Kindly Update Your Valid Employee Code');window.location ='DashboardEmployee.aspx';",
                    true);

                }
                Session["UserPF"] = getuser.Rows[0]["ERPID"].ToString();

            }
            else
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(),
   "alert",
   "alert('Kindly Update Your Employee Code');window.location ='DashboardEmployee.aspx';",
   true);

            }
            binddropdownMonth(DateTime.Now.Year.ToString());
            bindyear();
          
        }
    }

    protected async void btnUpdate_Click(object sender, EventArgs e)
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            foreach (GridViewRow row in month.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("rbSelect");

                if (chk != null && chk.Checked)
                {
                    TextBox txtMonth = (TextBox)row.FindControl("txtMonth");
                    TextBox txtYear = (TextBox)row.FindControl("txtYear");

                    if (txtMonth != null && txtYear != null)
                    {
                        string selectedMonth = txtMonth.Text;
                        string selectedYear = txtYear.Text;

                        string query = @"UPDATE GlobalSettings 
                                     SET status = 'Released' 
                                     WHERE month = @month AND year = @year";

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@month", selectedMonth);
                            cmd.Parameters.AddWithValue("@year", selectedYear);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    break; 
                }
            }
        }

       await LoadGrid(); 
    }
    async Task LoadGrid()
    {
        //erplogic

        DataTable db = SQLDataFile.ExecuteDataTableSQL("SELECT * FROM GlobalSettings");
        HttpClientHandler _http = new HttpClientHandler();

        HttpClient _httpClient = new HttpClient(_http);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", AccessTokenManish);

        var send = await _httpClient.GetAsync("https://vikramtea.erpnext.com/api/resource/Salary Slip?fields=[\"posting_date\"]&filters=[[\"status\",\"=\",\"" + "Submitted" + "\"]]&limit_start=0&limit_page_length=4000");
        var read = await send.Content.ReadAsStringAsync();
        JObject json = JObject.Parse(read);

        var getDates = json["data"];

        var uniqueDates = getDates
      .Select(x => Convert.ToDateTime(x["posting_date"]).ToString("yyyy-MM"))
      .Distinct();

        foreach (var d in uniqueDates)
        {
            DateTime date = DateTime.Parse(d + "-01");

            string month = date.ToString("MMMM");
            string year = date.ToString("yyyy");

            bool exists = db.AsEnumerable().Any(row =>
                row["month"].ToString() == month &&
                row["year"].ToString() == year);

            if (!exists)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string insertQuery = @"INSERT INTO GlobalSettings(month,year,status)
                                   VALUES(@month,@year,@status)";

                    SqlCommand cmd = new SqlCommand(insertQuery, con);
                    cmd.Parameters.AddWithValue("@month", month);
                    cmd.Parameters.AddWithValue("@year", year);
                    cmd.Parameters.AddWithValue("@status", "Pending");

                    cmd.ExecuteNonQuery();
                }
            }
        }


        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = @"SELECT * FROM GlobalSettings";

            SqlDataAdapter sql = new SqlDataAdapter(query, con);

            DataTable dt = new DataTable();
            sql.Fill(dt);

            month.DataSource = dt;
            month.DataBind();
        }

    }
  
    protected void gvKRA_RowEditing(object sender, GridViewEditEventArgs e)
    {
        month.EditIndex = e.NewEditIndex;
        LoadGrid();
    }

    //bindyear() and binddropdownMonth() both are written to first fetch year and then depending on that fetch months from that year
    private void bindyear()
    {
        try
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            HttpClientHandler handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", AccessTokenManish);
            var APIResponse = client.GetAsync("https://vikramtea.erpnext.com/api/resource/Salary Slip?fields=[\"*\"]&filters=[[\"employee\",\"=\",\"" + Session["UserPF"].ToString() + "\"]]&limit_start=0&limit_page_length=200").Result;

            if (APIResponse.IsSuccessStatusCode)
            {
                JsonContent = APIResponse.Content.ReadAsStringAsync().Result;
                string getdata = JsonContent.ToString();
                RootPayrollList myDeserializedClass = new JavaScriptSerializer().Deserialize<RootPayrollList>(getdata);

                if (myDeserializedClass.data != null)
                {
                    var years = myDeserializedClass.data.Select(x => Convert.ToDateTime(x.start_date).Year).Distinct().ToList();

                    drpSalaryYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Year--"));
                    foreach (var year in years)
                    {
                        drpSalaryYear.Items.Add(new System.Web.UI.WebControls.ListItem(year.ToString(), year.ToString()));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            dvHtml.Controls.Add(new Literal { Text = ex.ToString() });
        }

    }

    //This is method written which will fetch salary slip based on only months dropdown (written by Manish Sir)
    /* private void BindDropdown()
     {
         try
         {
             ServicePointManager.Expect100Continue = true;
             ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

             using (HttpClientHandler handler = new HttpClientHandler())
             using (HttpClient client = new HttpClient(handler))
             {
                 client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", AccessTokenManish);

                 string userPF = Session["UserPF"].ToString();
                 // Filter to exclude drafts
                 string url = "https://vikramtea.erpnext.com/api/resource/Salary Slip?fields=[\"*\"]&filters=[[\"status\",\"!=\",\"Draft\"],[\"employee\",\"=\",\""
                              + Uri.EscapeDataString(userPF)
                              + "\"]]";

                 HttpResponseMessage response = client.GetAsync(url).Result;

                 // Log the URL for debugging
                 dvHtml.Controls.Add(new Literal { Text = "<br>Request URL: " + url });

                 if (response.IsSuccessStatusCode)
                 {
                     string jsonContent = response.Content.ReadAsStringAsync().Result;

                     // Log the raw JSON content for debugging
                     dvHtml.Controls.Add(new Literal { Text = "<br>Raw JSON Content: " + jsonContent });

                     var myDeserializedClass = new JavaScriptSerializer().Deserialize<RootPayrollList>(jsonContent);

                     if (myDeserializedClass.data != null)
                     {
                         drpSalarySlip.Items.Clear();
                         drpSalarySlip.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--"));

                         foreach (var item in myDeserializedClass.data)
                         {
                             // Debugging output for each item
                             dvHtml.Controls.Add(new Literal { Text = "<br>Item: " + item.name + ", Status: " + item.status + ", Start Date: " + item.start_date });

                             string dates = Convert.ToDateTime(item.start_date).ToString("MMMM-yyyy");
                             drpSalarySlip.Items.Add(new System.Web.UI.WebControls.ListItem(dates, item.name));
                         }
                     }
                     else
                     {
                         dvHtml.Controls.Add(new Literal { Text = "No data returned from the API." });
                     }
                 }
                 else
                 {
                     dvHtml.Controls.Add(new Literal { Text = "API request failed: " + response.ReasonPhrase });
                 }
             }
         }
         catch (Exception ex)
         {
             dvHtml.Controls.Add(new Literal { Text = "Exception: " + ex.ToString() });
         }
     }*/


    private void FetchDataAPI(string salaryslipID)
    {
        HttpClientHandler handler = new HttpClientHandler();
        HttpClient client = new HttpClient(handler);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", AccessTokenManish);

        var getErpMonth = client.GetStringAsync("https://vikramtea.erpnext.com/api/resource/Salary Slip/" + salaryslipID).Result;

        JObject json = JObject.Parse(getErpMonth);
        var rows = json["data"];

        string posting_date = rows["posting_date"].ToString();
        DateTime Date = Convert.ToDateTime(posting_date);
        string month = Date.ToString("MMMM");
        string years = Date.ToString("yyyy");
        int year = Convert.ToInt32(years);


        DataTable months = SQLDataFile.ExecuteDataTableSQL("SELECT * FROM GlobalSettings WHERE month='" + month + "' and year ='"+ year + "' and status='Released'");

        if (months.Rows.Count > 0)
        {

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;


                var APIResponse = client.GetAsync("https://vikramtea.erpnext.com/api/resource/Salary Slip/" + salaryslipID).Result;


                if (APIResponse.IsSuccessStatusCode)
                {
                    JsonContent = APIResponse.Content.ReadAsStringAsync().Result;
                    string getdata = JsonContent.ToString();
                    RootPayroll myDeserializedClass = new JavaScriptSerializer().Deserialize<RootPayroll>(getdata);

                    GenerateSalarySlip(myDeserializedClass);
                }
            }
            catch (Exception ex)
            {
                dvHtml.Controls.Add(new Literal { Text = ex.ToString() });

            }

        }

        else
        {
            string script = @"<script>alert('Salary Slip Not Generated Yet.')</script>";
            ClientScript.RegisterStartupScript(this.GetType(),"alert",script);
            return;
        }
    }
    private void GenerateSalarySlip(RootPayroll obj)
    {
        HttpClientHandler handler = new HttpClientHandler();
        HttpClient client = new HttpClient(handler);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", AccessTokenManish);
        var APIResponse = client.GetAsync("https://vikramtea.erpnext.com/api/resource/Employee/" + obj.data.employee).Result;

        RootEmployee myDeserializedClass = null;
        if (APIResponse.IsSuccessStatusCode)
        {
            JsonContent = APIResponse.Content.ReadAsStringAsync().Result;
            string getdata = JsonContent.ToString();
            myDeserializedClass = new JavaScriptSerializer().Deserialize<RootEmployee>(getdata);
        }

        string dates = Convert.ToDateTime(obj.data.end_date).ToString("MMMM-yyyy");

        #region File and folder create or delete sections

        string pdfFileName = Request.PhysicalApplicationPath + "SalarySlip/" + obj.data.employee_code + dates + ".pdf";

        if (File.Exists(pdfFileName))
            File.Delete(pdfFileName);
        #endregion

        #region Writer and doc open section

        Panel pnlPrintControl = new Panel();
        Document doc = new Document(PageSize.A4, 36f, 36f, 36f, 36f);//36f, 36f, 90f, 100f);
                                                                     // PdfWriter.GetInstance(doc, Response.OutputStream);
        PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(pdfFileName, FileMode.CreateNew));
        try
        {

            doc.Open();
            //var fontFamily = FontFactory.GetFont("TIMES ROMAN", 15, BaseColor.BLUE);
            // 1) Adding logo to right side top

            //                    Paragraph address = new Paragraph();
            //                    address.Add(new Chunk(@"Vikram Tea Processing Pvt Ltd.
            //                    Address: Gut No. 11, 12. Borkhedi, Near Malacha Ganpati, Sindhkhedraja Road, Jalna – 431203, Maharashtra", new Font(Font.FontFamily.COURIER, 10, 1, BaseColor.BLACK)));
            //                    address.Alignment = 1;
            //                    doc.Add(address);
            #endregion Writer and doc open section

            #region Header Section
            string imagePath = Server.MapPath("../Content") + "\\/vtppl_logo_2.jpg";
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imagePath);
            image.Alignment = Element.ALIGN_TOP;
            // set width and height
            image.ScaleToFit(100f, 150f);
            // doc.Add(image); 

            PdfPTable tabletop = new PdfPTable(2);
            int[] columnwidthtop = { 25, 75 };
            tabletop.SetWidths(columnwidthtop);
            tabletop.WidthPercentage = 100;
            tabletop.HorizontalAlignment = 0;

            PdfPCell titlecellColumnValueblank = new PdfPCell(new Phrase(@"", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111))));
            titlecellColumnValueblank.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            titlecellColumnValueblank.Border = 0;
            tabletop.AddCell(titlecellColumnValueblank);

            PdfPCell titlecellColumnValue = new PdfPCell(new Phrase(@"Vikram Tea Processor Pvt Ltd. ", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111))));
            titlecellColumnValue.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            titlecellColumnValue.Border = 0;

            tabletop.AddCell(titlecellColumnValue);

            PdfPCell imagecellColumnValue1 = new PdfPCell(image);
            imagecellColumnValue1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            imagecellColumnValue1.Border = 0;
            // titlecellColumnValue.Rowspan = 2;
            tabletop.AddCell(imagecellColumnValue1);


            PdfPCell titlecellColumnValue1 = new PdfPCell(new Phrase(@" Address: Gut No. 11, 12. Borkhedi, Near Malacha Ganpati, Sindhkhedraja Road, Jalna – 431203, Maharashtra ", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111))));
            titlecellColumnValue1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            titlecellColumnValue1.Border = 0;
            //titlecellColumnValue.Rowspan = 1;
            //titlecellColumnValue1.Rowspan = 2;
            tabletop.AddCell(titlecellColumnValue1);

            doc.Add(tabletop);

            #endregion

            #region Main Section

            #region Employee Detail Section

            // 2) Addling blank paragraph
            doc.Add(new Paragraph("  "));
            // 3) Adding title table
            Paragraph title = new Paragraph();
            title.Add(new Chunk("Pay Slip Month Of " + dates, new Font(Font.FontFamily.COURIER, 10, 1, BaseColor.BLACK)));
            title.Alignment = 1;
            doc.Add(title);
            // 4) Addling blank paragraph
            doc.Add(new Paragraph("  "));
            // 5) Creating 1st table with 4 column
            PdfPTable table1 = new PdfPTable(4);
            int[] columnwidth = { 20, 25, 20, 25 };
            table1.SetWidths(columnwidth);
            table1.WidthPercentage = 100;
            table1.HorizontalAlignment = 0;
            // 6) Adding employee data to table1

            if (obj.data.employee_code != "")
            {
                //"Employee Code"
                PdfPCell cellColumnNameemployee_code = new PdfPCell(new Phrase("Employee Code", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) };
                cellColumnNameemployee_code.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                                                     //Cellcolumnname.Border = 15;
                table1.AddCell(cellColumnNameemployee_code);

                PdfPCell cellColumnValueemployee_code = new PdfPCell(new Phrase(obj.data.employee_code, new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111))));
                cellColumnValueemployee_code.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                                                      //cellcolumnvalue.Border = 15;
                table1.AddCell(cellColumnValueemployee_code);

                //Working Days
                PdfPCell cellColumnNametotal_working_days = new PdfPCell(new Phrase("Working Days", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) };
                cellColumnNametotal_working_days.HorizontalAlignment = 0;
                table1.AddCell(cellColumnNametotal_working_days);

                PdfPCell cellColumnValuetotal_working_days = new PdfPCell(new Phrase(obj.data.total_working_days.ToString(), new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111))));
                cellColumnValuetotal_working_days.HorizontalAlignment = 0;
                table1.AddCell(cellColumnValuetotal_working_days);

                //"Employee Code"
                PdfPCell cellColumnNameemployee_name = new PdfPCell(new Phrase("Employee Name", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) };
                cellColumnNameemployee_name.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                                                     //Cellcolumnname.Border = 15;
                table1.AddCell(cellColumnNameemployee_name);
               
                PdfPCell cellColumnValueemployee_name = new PdfPCell(new Phrase(obj.data.employee_name, new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111))));
                cellColumnValueemployee_name.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                                                      //cellcolumnvalue.Border = 15;
                table1.AddCell(cellColumnValueemployee_name);

                //Absent Days
                PdfPCell cellColumnNameabsent_days = new PdfPCell(new Phrase("Absent Days", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) };
                cellColumnNameabsent_days.HorizontalAlignment = 0;
                table1.AddCell(cellColumnNameabsent_days);

                PdfPCell cellColumnValueabsent_days = new PdfPCell(new Phrase(obj.data.absent_days.ToString(), new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111))));
                cellColumnValueabsent_days.HorizontalAlignment = 0;
                table1.AddCell(cellColumnValueabsent_days);

                //Location
                PdfPCell cellColumnNameLocation = new PdfPCell(new Phrase("Location", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) };
                cellColumnNameLocation.HorizontalAlignment = 0;
                table1.AddCell(cellColumnNameLocation);

                PdfPCell cellColumnValueLocation = new PdfPCell(new Phrase(myDeserializedClass.data.location, new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111))));
                cellColumnValueLocation.HorizontalAlignment = 0;
                table1.AddCell(cellColumnValueLocation);


                //Payment Days
                PdfPCell cellColumnNamepayment_days = new PdfPCell(new Phrase("Payment Days", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) };
                cellColumnNamepayment_days.HorizontalAlignment = 0;
                table1.AddCell(cellColumnNamepayment_days);

                PdfPCell cellColumnValuepayment_days = new PdfPCell(new Phrase(obj.data.payment_days.ToString(), new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111))));
                cellColumnValuepayment_days.HorizontalAlignment = 0;
                table1.AddCell(cellColumnValuepayment_days);


                //"Department"
                PdfPCell cellColumnNameDepartment = new PdfPCell(new Phrase("Department", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) };
                cellColumnNameDepartment.HorizontalAlignment = 0;
                table1.AddCell(cellColumnNameDepartment);

                PdfPCell cellColumnValueDepartment = new PdfPCell(new Phrase(obj.data.department, new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111))));
                cellColumnValueDepartment.HorizontalAlignment = 0;
                table1.AddCell(cellColumnValueDepartment);

                // UAN
                PdfPCell cellColumnNameuan = new PdfPCell(new Phrase("UAN No", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) };
                cellColumnNameuan.HorizontalAlignment = 0;
                table1.AddCell(cellColumnNameuan);

                PdfPCell cellColumnValueuan = new PdfPCell(new Phrase(myDeserializedClass.data.uan, new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111))));
                cellColumnValueuan.HorizontalAlignment = 0;
                table1.AddCell(cellColumnValueuan);

                //Designation
                PdfPCell cellColumnNameDesignation = new PdfPCell(new Phrase("Designation", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) };
                cellColumnNameDesignation.HorizontalAlignment = 0;
                table1.AddCell(cellColumnNameDesignation);

                PdfPCell cellColumnValueDesignation = new PdfPCell(new Phrase(obj.data.designation, new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111))));
                cellColumnValueDesignation.HorizontalAlignment = 0;
                table1.AddCell(cellColumnValueDesignation);


                PdfPCell cellColumnValueStartDate = new PdfPCell(new Phrase(obj.data.start_date, new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111))));
                cellColumnValueStartDate.HorizontalAlignment = 0;
                

                //
                PdfPCell cellColumnName1 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) };
                cellColumnName1.HorizontalAlignment = 0;
                table1.AddCell(cellColumnName1);

                PdfPCell cellColumnValue1 = new PdfPCell(new Phrase("", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111))));
                cellColumnValue1.HorizontalAlignment = 0;
                table1.AddCell(cellColumnValue1);



                //Date Of Joining
                PdfPCell cellColumnNamedate_of_joining = new PdfPCell(new Phrase("Date Of Joining", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) };
                cellColumnNamedate_of_joining.HorizontalAlignment = 0;
                table1.AddCell(cellColumnNamedate_of_joining);

                PdfPCell cellColumnValuedate_of_joining = new PdfPCell(new Phrase(myDeserializedClass.data.date_of_joining, new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111))));
                cellColumnValuedate_of_joining.HorizontalAlignment = 0;
                table1.AddCell(cellColumnValuedate_of_joining);


                //esic
                PdfPCell cellColumnNamePF = new PdfPCell(new Phrase("Provident Fund Account", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) };
                cellColumnNamePF.HorizontalAlignment = 0;
                table1.AddCell(cellColumnNamePF);

                PdfPCell cellColumnValuePF = new PdfPCell(new Phrase(myDeserializedClass.data.provident_fund_account, new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111))));
                cellColumnValuePF.HorizontalAlignment = 0;
                table1.AddCell(cellColumnValuePF);

                //bank_ac_no
                PdfPCell cellColumnNamebank_ac_no = new PdfPCell(new Phrase("Bank Account No", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) };
                cellColumnNamebank_ac_no.HorizontalAlignment = 0;
                table1.AddCell(cellColumnNamebank_ac_no);

                PdfPCell cellColumnValuebank_ac_no = new PdfPCell(new Phrase(myDeserializedClass.data.bank_ac_no, new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111))));
                cellColumnValuebank_ac_no.HorizontalAlignment = 0;
                table1.AddCell(cellColumnValuebank_ac_no);


                //esic
                PdfPCell cellColumnNameesic = new PdfPCell(new Phrase("ESIC", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) };
                cellColumnNameesic.HorizontalAlignment = 0;
                table1.AddCell(cellColumnNameesic);

                PdfPCell cellColumnValueesic = new PdfPCell(new Phrase(myDeserializedClass.data.esic, new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111))));
                cellColumnValueesic.HorizontalAlignment = 0;
                table1.AddCell(cellColumnValueesic);

                //bank Name
                PdfPCell cellColumnNamebank_name = new PdfPCell(new Phrase("IFSC Code", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) };
                cellColumnNamebank_name.HorizontalAlignment = 0;
                table1.AddCell(cellColumnNamebank_name);

                PdfPCell cellColumnValuebank_name = new PdfPCell(new Phrase(myDeserializedClass.data.ifsc_code, new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111))));
                cellColumnValuebank_name.HorizontalAlignment = 0;
                table1.AddCell(cellColumnValuebank_name);


                //PAN
                PdfPCell cellColumnNamePAN = new PdfPCell(new Phrase("PAN", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) };
                cellColumnNamePAN.HorizontalAlignment = 0;
                table1.AddCell(cellColumnNamePAN);

                PdfPCell cellColumnValuePAN = new PdfPCell(new Phrase(myDeserializedClass.data.pan_number, new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111))));
                cellColumnValuePAN.HorizontalAlignment = 0;
                table1.AddCell(cellColumnValuePAN);


            }


            doc.Add(table1);
            // 6) Addling blank paragraphe
            doc.Add(new Paragraph("  "));


            #endregion

            #region Salary Section
            // 7) Creating 2nd table with 4 columns [which is main table]
            PdfPTable mainTable = new PdfPTable(4); //earnedTable1.TotalWidth = 500f;//earnedTable1.LockedWidth = true;
            int[] columnwidth1 = { 30, 20, 30, 20 }; //23, 20, 25, 32 };
            mainTable.SetWidths(columnwidth1);
            mainTable.WidthPercentage = 100;
            mainTable.HorizontalAlignment = 0;
            // a. adding 4 cells for header
            mainTable.AddCell(new PdfPCell(new Phrase("EARNINGS", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) }); //{ HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(System.Drawing.Color.Silver) };;
            mainTable.AddCell(new PdfPCell(new Phrase("RUPEES", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) }); //{ HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(System.Drawing.Color.Silver) };;
            mainTable.AddCell(new PdfPCell(new Phrase("DEDUCTIONS", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) }); //{ HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(System.Drawing.Color.Silver) };;
            mainTable.AddCell(new PdfPCell(new Phrase("RUPEES", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) }); //{ HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(System.Drawing.Color.Silver) };;


            // b. creating earning table with 2 columns [left side]
            PdfPTable earning = new PdfPTable(2);
            int[] columnwidth3 = { 30, 20 };
            earning.SetWidths(columnwidth3);
            earning.WidthPercentage = 80;
            earning.HorizontalAlignment = 0;

            #region Earning data Section

            // c. adding earning data
            foreach (var item in obj.data.earnings)
            {
                // string columnName = (ds.Tables[1].Columns[i].ColumnName);
                string columnName = (item.salary_component);
                string columnValue = (item.amount.ToString());
                PdfPCell cellColumnName = new PdfPCell(new Phrase(columnName, new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111))));
                cellColumnName.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                earning.AddCell(cellColumnName);
                PdfPCell cellColumnValue = new PdfPCell(new Phrase(AppendComma(columnValue), new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_RIGHT };
                earning.AddCell(cellColumnValue);
            }

            #endregion

            #region Deduction data Section
            // d. creating deduction table with 2 columns [Right side]
            PdfPTable deduction = new PdfPTable(2);
            int[] columnwidth4 = { 30, 20 };
            deduction.SetWidths(columnwidth3);
            deduction.WidthPercentage = 80;
            deduction.HorizontalAlignment = 0;
            // e. adding deduction data
            foreach (var item in obj.data.deductions)
            {
                // string columnName = (ds.Tables[1].Columns[i].ColumnName);
                string columnName = (item.salary_component);
                string columnValue = (item.amount.ToString());
                PdfPCell cellColumnName = new PdfPCell(new Phrase(columnName, new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111))));
                cellColumnName.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                deduction.AddCell(cellColumnName);
                PdfPCell cellColumnValue = new PdfPCell(new Phrase(AppendComma(columnValue), new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_RIGHT };
                deduction.AddCell(cellColumnValue);
            }
            foreach (var item in obj.data.loans)
            {
                // string columnName = (ds.Tables[1].Columns[i].ColumnName);
                string columnName = (item.loan_type);
                string columnValue = (item.principal_amount.ToString());
                PdfPCell cellColumnName = new PdfPCell(new Phrase(columnName, new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111))));
                cellColumnName.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                deduction.AddCell(cellColumnName);
                PdfPCell cellColumnValue = new PdfPCell(new Phrase(AppendComma(columnValue), new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_RIGHT };
                deduction.AddCell(cellColumnValue);
            }
            #endregion

            #region Gross and Net income Section
            // f. creating a new cell [cell1] with colspan=2
            //    adding earning table into cell1
            PdfPCell cell1 = new PdfPCell(earning);
            cell1.Colspan = 2;
            // adding cell1 into mainTable
            mainTable.AddCell(cell1);
            // g. creating a new cell [cell2] with colspan=2
            //    adding deduction table into cell2
            PdfPCell cell2 = new PdfPCell(deduction);
            cell2.Colspan = 2;
            // adding cell2 into mainTable
            mainTable.AddCell(cell2);
            mainTable.AddCell(new PdfPCell(new Phrase("Gross Earning", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_LEFT, BackgroundColor = new BaseColor(236, 236, 236) });
            mainTable.AddCell(new PdfPCell(new Phrase(AppendComma(obj.data.base_gross_pay.ToString()), new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 5 });
            mainTable.AddCell(new PdfPCell(new Phrase("Gross Deductions", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) });
            if (obj.data.with_loan_deduction > 0)
            {
                mainTable.AddCell(new PdfPCell(new Phrase(AppendComma(obj.data.with_loan_deduction.ToString()), new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 5 });
            }
            else
            {
                mainTable.AddCell(new PdfPCell(new Phrase(AppendComma(obj.data.total_deduction.ToString()), new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 5 });
            }
            PdfPCell netEarning = new PdfPCell(new Phrase("Net Salary", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_LEFT, BackgroundColor = new BaseColor(236, 236, 236) };
            mainTable.AddCell(netEarning);
            string NetSalary = obj.data.net_pay.ToString();
            PdfPCell netSalary = new PdfPCell(new Phrase(AppendComma(NetSalary), new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 5 };
            mainTable.AddCell(netSalary);
            PdfPCell blankCell = new PdfPCell();
            blankCell.Colspan = 2;
            mainTable.AddCell(blankCell);
            PdfPCell NetSalaryInWords = new PdfPCell(new Phrase("Net Salary In Word : " + GenerateWordsinRs(NetSalary), new Font(Font.FontFamily.COURIER, 8, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5 };
            NetSalaryInWords.Colspan = 4;
            mainTable.AddCell(NetSalaryInWords);
            // adding mainTable to document object
            doc.Add(mainTable);
            #endregion

            #endregion


            doc.Add(new Paragraph("  "));


            doc.Add(new Paragraph("  "));
            PdfPTable mainLeaveTablediff = new PdfPTable(5); //earnedTable1.TotalWidth = 500f;//earnedTable1.LockedWidth = true;
            int[] columnwidthLeavediff = { 20, 20, 20, 20, 20 }; //23, 20, 25, 32 };
            mainLeaveTablediff.SetWidths(columnwidthLeavediff);
            mainLeaveTablediff.WidthPercentage = 100;
            mainLeaveTablediff.HorizontalAlignment = 0;

            mainLeaveTablediff.AddCell(new PdfPCell(new Phrase("Leave Type", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) }); //{ HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(System.Drawing.Color.Silver) };;
            mainLeaveTablediff.AddCell(new PdfPCell(new Phrase("Total Allocated Leave(s)", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) }); //{ HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(System.Drawing.Color.Silver) };;
            mainLeaveTablediff.AddCell(new PdfPCell(new Phrase("Used Leave(s)", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) }); //{ HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(System.Drawing.Color.Silver) };;
            mainLeaveTablediff.AddCell(new PdfPCell(new Phrase("Leave(s) Pending Approval", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) }); //{ HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(System.Drawing.Color.Silver) };;
            mainLeaveTablediff.AddCell(new PdfPCell(new Phrase("Available Leave(s)", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) }); //{ HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(System.Drawing.Color.Silver) };;


            foreach (var item in obj.data.leave_details)
            {
                // string columnName = (ds.Tables[1].Columns[i].ColumnName);

                string leave_type = (item.leave_type.ToString());

                PdfPCell cellColumnValueleave_type = new PdfPCell(new Phrase(leave_type, new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_CENTER };
                mainLeaveTablediff.AddCell(cellColumnValueleave_type);

                string total_allocated_leaves = (item.total_allocated_leaves.ToString());
                PdfPCell cellColumnValuetotal_allocated_leaves = new PdfPCell(new Phrase(total_allocated_leaves, new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_CENTER };
                mainLeaveTablediff.AddCell(cellColumnValuetotal_allocated_leaves);

                string used_leaves = (item.used_leaves.ToString());
                PdfPCell cellColumnValueused_leaves = new PdfPCell(new Phrase(used_leaves, new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_CENTER };
                mainLeaveTablediff.AddCell(cellColumnValueused_leaves);

                string pending_leaves = (item.pending_leaves.ToString());
                PdfPCell cellColumnValuepending_leaves = new PdfPCell(new Phrase(pending_leaves, new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_CENTER };
                mainLeaveTablediff.AddCell(cellColumnValuepending_leaves);

                string available_leaves = (item.available_leaves.ToString());
                PdfPCell cellColumnValueavailable_leaves = new PdfPCell(new Phrase(available_leaves, new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_CENTER };
                mainLeaveTablediff.AddCell(cellColumnValueavailable_leaves);

            }

            //// a. adding 4 cells for header


            //for (int i = 0; i < ds.Tables[5].Columns.Count; i++)
            //{
            //    string columnValue1 = (ds.Tables[5].Rows[1][i].ToString());

            //    string columnValue2 = (ds.Tables[5].Rows[2][i].ToString());

            //    PdfPCell cellColumnName = new PdfPCell(new Phrase(columnValue1, new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111)))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(236, 236, 236) };
            //    cellColumnName.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //    //Cellcolumnname.Border = 15;
            //    mainLeaveTablediff.AddCell(cellColumnName);
            //    PdfPCell cellColumnValue = new PdfPCell(new Phrase(columnValue2, new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111))));
            //    cellColumnValue.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //    //cellcolumnvalue.Border = 15;
            //    mainLeaveTablediff.AddCell(cellColumnValue);
            //}
            doc.Add(mainLeaveTablediff);
            //// 6) Addling blank paragraphe
            doc.Add(new Paragraph("  "));

            #endregion

            #region Footer Section
            if (obj.data.employee_code != "")
            {
                Paragraph Note = new Paragraph();
                Note.Add(new Chunk("This is computer generated payslip of", new Font(Font.FontFamily.COURIER, 10, 1, BaseColor.BLACK)));
                Note.Add(new Chunk(" ", new Font(Font.FontFamily.COURIER, 10, 1, BaseColor.BLACK)));
                Note.Add(new Chunk(Convert.ToDateTime(obj.data.start_date).ToString("dd-MMMM-yyyy"), new Font(Font.FontFamily.COURIER, 10, 1, BaseColor.BLACK)));
                Note.Add(new Chunk(" ", new Font(Font.FontFamily.COURIER, 10, 1, BaseColor.BLACK)));
                Note.Add(new Chunk("To", new Font(Font.FontFamily.COURIER, 10, 1, BaseColor.BLACK)));
                Note.Add(new Chunk(" ", new Font(Font.FontFamily.COURIER, 10, 1, BaseColor.BLACK)));
                Note.Add(new Chunk(Convert.ToDateTime(obj.data.end_date).ToString("dd-MMMM-yyyy"), new Font(Font.FontFamily.COURIER, 10, 1, BaseColor.BLACK)));
                Note.Add(new Chunk(" ", new Font(Font.FontFamily.COURIER, 10, 1, BaseColor.BLACK)));
                Note.Add(new Chunk("and does not require signature or company seal.", new Font(Font.FontFamily.COURIER, 10, 1, BaseColor.BLACK))); 
               
                Note.Alignment = 0;
                doc.Add(Note);
            }

            #endregion

            #region Writer and doc Close section
            doc.Close();
            writer.Close();
            #endregion Writer and doc Close section
            showpdf.Src = "../SalarySlip/" + obj.data.employee_code + dates + ".pdf";

        }
        catch (Exception xe)
        {
            throw xe;
        }
        finally
        {
            doc.Close();
            writer.Close();
        }
    }

    #region SQlALLQuery
    public string GenerateWordsinRs(string inputRs)
    {
        string input = inputRs;
        string a = "";
        string b = "";
        // take decimal part of input. convert it to word. add it at the end of method.
        string decimals = "";
        if (input.Contains("."))
        {
            decimals = input.Substring(input.IndexOf(".") + 1);
            // remove decimal part from input
            input = input.Remove(input.IndexOf("."));
        }
        string strWords = NumbersToWords(Convert.ToInt32(input));
        if (!inputRs.Contains("."))
        {
            a = strWords + " Rupees Only";
        }
        else
        {
            a = strWords + " Rupees";
        }
        if (decimals.Length > 0)
        {
            // if there is any decimal part convert it to words and add it to strWords.
            string strwords2 = NumbersToWords(Convert.ToInt32(decimals));
            b = " and " + strwords2 + " Paisa Only ";
        }
        string final2 = "";
        final2 = a + b;
        return final2;
    }
    public static string NumbersToWords(int inputNumber)
    {
        int inputNo = inputNumber;
        if (inputNo == 0)
            return "Zero";
        int[] numbers = new int[4];
        int first = 0;
        int u, h, t;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        if (inputNo < 0)
        {
            sb.Append("Minus ");
            inputNo = -inputNo;
        }
        string[] words0 = {"" ,"One ", "Two ", "Three ", "Four ",
     "Five " ,"Six ", "Seven ", "Eight ", "Nine "};
        string[] words1 = {"Ten ", "Eleven " , "Twelve ", "Thirteen " , "Fourteen ",
     "Fifteen ", "Sixteen " ,"Seventeen ", "Eighteen " , "Nineteen "};
        string[] words2 = {"Twenty ", "Thirty " , "Forty ", "Fifty ", "Sixty ",
     "Seventy ", "Eighty " , "Ninety "};
        string[] words3 = { "Thousand ", "Lakh ", "Crore " };
        numbers[0] = inputNo % 1000; // units
        numbers[1] = inputNo / 1000;
        numbers[2] = inputNo / 100000;
        numbers[1] = numbers[1] - 100 * numbers[2]; // thousands
        numbers[3] = inputNo / 10000000; // crores
        numbers[2] = numbers[2] - 100 * numbers[3]; // lakhs
        for (int i = 3; i > 0; i--)
        {
            if (numbers[i] != 0)
            {
                first = i;
                break;
            }
        }
        for (int i = first; i >= 0; i--)
        {
            if (numbers[i] == 0) continue;
            u = numbers[i] % 10; // ones
            t = numbers[i] / 10;
            h = numbers[i] / 100; // hundreds
            t = t - 10 * h; // tens
            if (h > 0) sb.Append(words0[h] + "Hundred ");
            if (u > 0 || t > 0)
            {
                if (h > 0 || i == 0) sb.Append("");
                if (t == 0)
                    sb.Append(words0[u]);
                else if (t == 1)
                    sb.Append(words1[u]);
                else
                    sb.Append(words2[t - 2] + words0[u]);
            }
            if (i != 0) sb.Append(words3[i - 1]);
        }
        return sb.ToString().TrimEnd();
    }
    #endregion SQlALLQuery

    private string AppendComma(string value)
    {
        return String.Format("{0:#,0.00}", Convert.ToDecimal(value));
    }

    protected void drpSalarySlip_SelectedIndexChanged(object sender, EventArgs e)
    {
        FetchDataAPI(drpSalarySlip.SelectedValue);
    }

    protected void drpSalaryYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        string selectedYear = drpSalaryYear.SelectedItem.Value;
        binddropdownMonth(selectedYear);
    }

    protected void btnShowPreviousMonthSalary_Click(object sender, EventArgs e)
    {
        
      
        binddropdownMonth(DateTime.Now.Year.ToString());
    }

    protected void btnHidePreviousMonthSalary_Click(object sender, EventArgs e)
    {
     
      
        binddropdownMonth(DateTime.Now.Year.ToString());
    }

  
    private bool CheckMonth(DateTime month)
    {
        DataTable months = SQLDataFile.ExecuteDataTableSQL("SELECT * FROM GlobalSettings WHERE month='" + month + "'"); 
       
        if (months.Rows.Count > 0)
        {
            string currentMonth = months.Rows[0]["month"].ToString();
            Console.WriteLine("there is current month");
            return true;
        }

        else
        {
            return false;
        }
    }

    private void binddropdownMonth(string selectedYear)
    {
        try
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", AccessTokenManish);

            DateTime currentDate = DateTime.Now;
            DateTime previousMonthStart = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(-1);
            string previousMonthStartStr = previousMonthStart.ToString("yyyy-MM");

            
            if (Session["UserPF"] == null)
            {
                
                throw new Exception("Session 'UserPF' is not set.");
            }

            string userPF = Session["UserPF"].ToString();
            string apiUrl;
        
              
                apiUrl = "https://vikramtea.erpnext.com/api/resource/Salary Slip?fields=[\"*\"]&filters=[[\"employee\",\"=\",\""
                         + userPF
                         + "\"],[\"status\",\"!=\",\"Draft\"], [\"posting_date\", \">=\", \""
                         + selectedYear + "-01-01\"], [\"posting_date\", \"<\", \"\"]]";
        
           
            var APIResponse = client.GetAsync(apiUrl).Result;

            if (APIResponse.IsSuccessStatusCode)
            {
                string JsonContent = APIResponse.Content.ReadAsStringAsync().Result;
                RootPayrollList myDeserializedClass = new JavaScriptSerializer().Deserialize<RootPayrollList>(JsonContent);

                if (myDeserializedClass.data != null)
                {
                    drpSalarySlip.Items.Clear();
                    drpSalarySlip.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", ""));
                    foreach (var item in myDeserializedClass.data)
                    {
                        string monthYear = Convert.ToDateTime(item.end_date).ToString("MMMM-yyyy");

                        if (monthYear.Contains(selectedYear))
                        {
                            string month = Convert.ToDateTime(item.end_date).ToString("MMMM");

                            drpSalarySlip.Items.Add(new System.Web.UI.WebControls.ListItem(month, item.name));
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            
            dvHtml.Controls.Add(new Literal { Text = ex.ToString() });
        }
    }



    //private void binddropdownMonth(string selectedYear)
    //{
    //    try
    //    {
    //        ServicePointManager.Expect100Continue = true;
    //        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
    //        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

    //        HttpClientHandler handler = new HttpClientHandler();
    //        HttpClient client = new HttpClient(handler);
    //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", AccessTokenManish);

    //        DateTime currentDate = DateTime.Now;
    //        string currentMonthStart = new DateTime(currentDate.Year, currentDate.Month, 1).ToString("yyyy-MM-dd");
    //        string nextMonthStart = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(1).ToString("yyyy-MM-dd");

    //        string apiUrl = "https://vikramtea.erpnext.com/api/resource/Salary Slip?fields=[\"*\"]&filters=[[\"employee\",\"=\",\"" + Session["UserPF"].ToString() + "\"],[\"status\",\"!=\",\"Draft\"], [\"posting_date\", \">=\", \"" + selectedYear + "-01-01\"], [\"posting_date\", \"<\", \"" + currentMonthStart + "\"]]";
    //        var APIResponse = client.GetAsync(apiUrl).Result;

    //        if (APIResponse.IsSuccessStatusCode)
    //        {
    //            JsonContent = APIResponse.Content.ReadAsStringAsync().Result;
    //            string getdata = JsonContent.ToString();
    //            RootPayrollList myDeserializedClass = new JavaScriptSerializer().Deserialize<RootPayrollList>(getdata);

    //            if (myDeserializedClass.data != null)
    //            {
    //                drpSalarySlip.Items.Clear();
    //                drpSalarySlip.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", ""));
    //                foreach (var item in myDeserializedClass.data)
    //                {
    //                    string monthYear = Convert.ToDateTime(item.start_date).ToString("MMMM-yyyy");

    //                    if (monthYear.Contains(selectedYear))
    //                    {
    //                        string month = Convert.ToDateTime(item.start_date).ToString("MMMM");
    //                        drpSalarySlip.Items.Add(new System.Web.UI.WebControls.ListItem(month, item.name));
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        dvHtml.Controls.Add(new Literal { Text = ex.ToString() });
    //    }
    //}


    private void fetchPreviousMonthSalary()
    {
        try
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            HttpClientHandler handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", AccessTokenManish);

            DateTime currentDate = DateTime.Now;
            DateTime previousMonthStart = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(-1);
            DateTime previousMonthEnd = previousMonthStart.AddMonths(1).AddDays(-1);

            string previousMonthStartStr = previousMonthStart.ToString("yyyy-MM-dd");
            string previousMonthEndStr = previousMonthEnd.ToString("yyyy-MM-dd");

            string apiUrl = "https://vikramtea.erpnext.com/api/resource/Salary Slip?fields=[\"*\"]&filters=[[\"employee\",\"=\",\"" + Session["UserPF"].ToString() + "\"],[\"status\",\"!=\",\"Draft\"], [\"posting_date\", \">=\", \"" + previousMonthStartStr + "\"], [\"posting_date\", \"<=\", \"" + previousMonthEndStr + "\"]]";
            var APIResponse = client.GetAsync(apiUrl).Result;

            if (APIResponse.IsSuccessStatusCode)
            {
                string JsonContent = APIResponse.Content.ReadAsStringAsync().Result;
                RootPayrollList myDeserializedClass = new JavaScriptSerializer().Deserialize<RootPayrollList>(JsonContent);

                if (myDeserializedClass.data != null)
                {
                    foreach (var item in myDeserializedClass.data)
                    {
                        string month = Convert.ToDateTime(item.start_date).ToString("MMMM");
                        drpSalarySlip.Items.Add(new System.Web.UI.WebControls.ListItem(month, item.name));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            dvHtml.Controls.Add(new Literal { Text = ex.ToString() });
        }
    }


}
public partial class Footer : PdfPageEventHelper
{
    private iTextSharp.text.Image waterMark;
    //public Footer(iTextSharp.text.Image img)
    public Footer()
    {
        // waterMark = img;
    }
    public override void OnStartPage(PdfWriter writer, Document document)
    {
        #region Header Section
        string imagePath = @"D:\Manish\Live backup\ShreeNew_live_01042021\images" + "\\vikram logo1.jpg";
        iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imagePath);
        image.Alignment = Element.ALIGN_TOP;
        // set width and height
        image.ScaleToFit(110f, 170f);
        // doc.Add(image); 


        PdfPTable tabletop = new PdfPTable(2);
        int[] columnwidthtop = { 70, 30 };
        tabletop.SetWidths(columnwidthtop);
        tabletop.WidthPercentage = 100;
        tabletop.HorizontalAlignment = 0;

        PdfPCell titlecellColumnValue = new PdfPCell(new Phrase(@" ", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111))));
        titlecellColumnValue.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        titlecellColumnValue.Border = 0;

        tabletop.AddCell(titlecellColumnValue);


        PdfPCell titlecellColumnValueblank = new PdfPCell(new Phrase(@"", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111))));
        titlecellColumnValueblank.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        titlecellColumnValueblank.Border = 0;
        tabletop.AddCell(titlecellColumnValueblank);

        PdfPCell titlecellColumnValue1 = new PdfPCell(new Phrase(@" ", new Font(Font.FontFamily.COURIER, 10, 1, new BaseColor(50, 50, 111))));
        titlecellColumnValue1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        titlecellColumnValue1.Border = 0;
        //titlecellColumnValue.Rowspan = 1;
        //titlecellColumnValue1.Rowspan = 2;
        tabletop.AddCell(titlecellColumnValue1);

        PdfPCell imagecellColumnValue1 = new PdfPCell(image);
        imagecellColumnValue1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        imagecellColumnValue1.Border = 0;
        // titlecellColumnValue.Rowspan = 2;
        tabletop.AddCell(imagecellColumnValue1);



        tabletop.TotalWidth = 600;
        tabletop.WriteSelectedRows(0, -1, document.LeftMargin - 25, document.PageSize.Height + 10, writer.DirectContent);

        #endregion
    }

    public override void OnEndPage(PdfWriter writer, Document doc)
    {

        Font BookmanOldStyle = FontFactory.GetFont("Bookman Old Style", 10, BaseColor.BLACK);

        iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(@"D:\Manish\Live backup\ShreeNew_live_01042021\images" + "\\Pdffooter.png");
        image1.Alignment = Element.ALIGN_CENTER;
        // set width and height
        image1.ScaleToFit(600f, 500f);

        PdfPTable footerTbl = new PdfPTable(1);

        footerTbl.TotalWidth = 600;

        footerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

        PdfPCell cell = new PdfPCell(image1);

        cell.Border = 0;

        //cell.PaddingLeft = 10;

        footerTbl.AddCell(cell);

        footerTbl.WriteSelectedRows(0, -1, 1, 90, writer.DirectContent);

        // PdfContentByte content = writer.DirectContentUnder;

        // content.AddImage(waterMark);
    }


}

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class Earning
{
    public string name { get; set; }
    public string owner { get; set; }
    public string creation { get; set; }
    public string modified { get; set; }
    public string modified_by { get; set; }
    public string parent { get; set; }
    public string parentfield { get; set; }
    public string parenttype { get; set; }
    public int idx { get; set; }
    public int docstatus { get; set; }
    public string salary_component { get; set; }
    public string abbr { get; set; }
    public double amount { get; set; }
    public double year_to_date { get; set; }
    public int is_recurring_additional_salary { get; set; }
    public int statistical_component { get; set; }
    public int depends_on_payment_days { get; set; }
    public int exempted_from_income_tax { get; set; }
    public int is_tax_applicable { get; set; }
    public int is_flexible_benefit { get; set; }
    public int variable_based_on_taxable_salary { get; set; }
    public int do_not_include_in_total { get; set; }
    public int deduct_full_tax_on_selected_payroll_date { get; set; }
    public int amount_based_on_formula { get; set; }
    public double default_amount { get; set; }
    public double additional_amount { get; set; }
    public double tax_on_flexible_benefit { get; set; }
    public double tax_on_additional_salary { get; set; }
    public string doctype { get; set; }
}

public class Deduction
{
    public string name { get; set; }
    public string owner { get; set; }
    public string creation { get; set; }
    public string modified { get; set; }
    public string modified_by { get; set; }
    public string parent { get; set; }
    public string parentfield { get; set; }
    public string parenttype { get; set; }
    public int idx { get; set; }
    public int docstatus { get; set; }
    public string salary_component { get; set; }
    public string abbr { get; set; }
    public double amount { get; set; }
    public double year_to_date { get; set; }
    public int is_recurring_additional_salary { get; set; }
    public int statistical_component { get; set; }
    public int depends_on_payment_days { get; set; }
    public int exempted_from_income_tax { get; set; }
    public int is_tax_applicable { get; set; }
    public int is_flexible_benefit { get; set; }
    public int variable_based_on_taxable_salary { get; set; }
    public int do_not_include_in_total { get; set; }
    public int deduct_full_tax_on_selected_payroll_date { get; set; }
    public int amount_based_on_formula { get; set; }
    public double default_amount { get; set; }
    public double additional_amount { get; set; }
    public double tax_on_flexible_benefit { get; set; }
    public double tax_on_additional_salary { get; set; }
    public string doctype { get; set; }
}
public class Loan
{
    public string name { get; set; }
    public string owner { get; set; }
    public string creation { get; set; }
    public string modified { get; set; }
    public string modified_by { get; set; }
    public string parent { get; set; }
    public string parentfield { get; set; }
    public string parenttype { get; set; }
    public int idx { get; set; }
    public int docstatus { get; set; }
    public string loan { get; set; }
    public string loan_type { get; set; }
    public string loan_account { get; set; }
    public string interest_income_account { get; set; }
    public double principal_amount { get; set; }
    public double interest_amount { get; set; }
    public double total_payment { get; set; }
    public string loan_repayment_entry { get; set; }
    public string doctype { get; set; }
}

public class LeaveDetail
{
    public string name { get; set; }
    public string creation { get; set; }
    public string modified { get; set; }
    public string modified_by { get; set; }
    public string parent { get; set; }
    public string parentfield { get; set; }
    public string parenttype { get; set; }
    public int idx { get; set; }
    public int docstatus { get; set; }
    public string leave_type { get; set; }
    public double total_allocated_leaves { get; set; }
    public double expired_leaves { get; set; }
    public double used_leaves { get; set; }
    public double pending_leaves { get; set; }
    public double available_leaves { get; set; }
    public string doctype { get; set; }
}
public class DataPayroll
{
    public string name { get; set; }
    public string owner { get; set; }
    public string creation { get; set; }
    public string modified { get; set; }
    public string modified_by { get; set; }
    public int idx { get; set; }
    public int docstatus { get; set; }
    public string posting_date { get; set; }
    public string employee { get; set; }
    public string employee_name { get; set; }
    public string department { get; set; }
    public string designation { get; set; }
    public string payroll_cost_center { get; set; }
    public string status { get; set; }
    public string payroll_entry { get; set; }
    public string company { get; set; }
    public string currency { get; set; }
    public double exchange_rate { get; set; }
    public string letter_head { get; set; }
    public string start_date { get; set; }
    public string end_date { get; set; }
    public string salary_structure { get; set; }
    public int salary_slip_based_on_timesheet { get; set; }
    public string payroll_frequency { get; set; }
    public double total_working_days { get; set; }
    public double unmarked_days { get; set; }
    public double leave_without_pay { get; set; }
    public double absent_days { get; set; }
    public double payment_days { get; set; }
    public double total_working_hours { get; set; }
    public double hour_rate { get; set; }
    public double base_hour_rate { get; set; }
    public string bank_name { get; set; }
    public string bank_account_no { get; set; }
    public string mode_of_payment { get; set; }
    public int deduct_tax_for_unclaimed_employee_benefits { get; set; }
    public int deduct_tax_for_unsubmitted_tax_exemption_proof { get; set; }
    public double gross_pay { get; set; }
    public double base_gross_pay { get; set; }
    public double gross_year_to_date { get; set; }
    public double base_gross_year_to_date { get; set; }
    public double total_deduction { get; set; }
    public double base_total_deduction { get; set; }
    public double total_principal_amount { get; set; }
    public double total_interest_amount { get; set; }
    public double total_loan_repayment { get; set; }
    public double net_pay { get; set; }
    public double base_net_pay { get; set; }
    public double year_to_date { get; set; }
    public double base_year_to_date { get; set; }
    public double rounded_total { get; set; }
    public double base_rounded_total { get; set; }
    public double month_to_date { get; set; }
    public double base_month_to_date { get; set; }
    public string total_in_words { get; set; }
    public string base_total_in_words { get; set; }
    public double with_loan_deduction { get; set; }
    public string set_loan_calculated { get; set; }
    public string uan { get; set; }
    public string esic { get; set; }
    public string payroll_status { get; set; }
    public string employee_code { get; set; }
    public string ifsc_code { get; set; }
    public string employee_status { get; set; }
    public string employment_type { get; set; }
    public int applicable_for_abry { get; set; }
    public string abry_provident_fund { get; set; }
    public string doctype { get; set; }
    public List<object> timesheets { get; set; }
    public List<Earning> earnings { get; set; }
    public List<Deduction> deductions { get; set; }
    public List<Loan> loans { get; set; }
    public List<LeaveDetail> leave_details { get; set; }

}

public class RootPayroll
{
    public DataPayroll data { get; set; }
}

public class RootPayrollList
{


    public List<DataPayroll> data { get; set; }
}
public class DataEmployee
{
    public string name { get; set; }
    public string owner { get; set; }
    public string creation { get; set; }
    public string modified { get; set; }
    public string modified_by { get; set; }
    public int idx { get; set; }
    public int docstatus { get; set; }
    public string employee { get; set; }
    public string naming_series { get; set; }
    public string first_name { get; set; }
    public string middle_name { get; set; }
    public string salutation { get; set; }
    public string employee_name { get; set; }
    public string employment_type { get; set; }
    public string company { get; set; }
    public string status { get; set; }
    public string gender { get; set; }
    public string date_of_birth { get; set; }
    public string date_of_joining { get; set; }
    public int applicable_for_esic { get; set; }
    public int applicable_for_abry { get; set; }
    public int create_user_permission { get; set; }
    public string job_applicant { get; set; }
    public string extension_letter { get; set; }
    public int notice_number_of_days { get; set; }
    public string confirmation_letter { get; set; }
    public string date_of_retirement { get; set; }
    public string department { get; set; }
    public string designation { get; set; }
    public string reports_to { get; set; }
    public string location { get; set; }
    public string holiday_list { get; set; }
    public string salary_mode { get; set; }
    public string payroll_cost_center { get; set; }
    public string pan_number { get; set; }
    public string provident_fund_account { get; set; }
    public string uan { get; set; }
    public string esic { get; set; }
    public string adhar_card_no { get; set; }
    public string payroll_status { get; set; }
    public string bank_name { get; set; }
    public string bank_ac_no { get; set; }
    public string ifsc_code { get; set; }
    public string cell_number { get; set; }
    public string prefered_email { get; set; }
    public string personal_email { get; set; }
    public int unsubscribed { get; set; }
    public string permanent_accommodation_type { get; set; }
    public string permanent_address { get; set; }
    public string prefered_contact_email { get; set; }
    public string current_accommodation_type { get; set; }
    public string marital_status { get; set; }
    public string blood_group { get; set; }
    public string leave_encashed { get; set; }
    public int lft { get; set; }
    public int rgt { get; set; }
    public string old_parent { get; set; }
    public string doctype { get; set; }
    public List<object> education { get; set; }
    public List<object> external_work_history { get; set; }
    public List<object> internal_work_history { get; set; }
    public List<object> employee_increment_history { get; set; }
}

public class RootEmployee
{
    public DataEmployee data { get; set; }
}
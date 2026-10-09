
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_ClearanceForm : System.Web.UI.Page
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    static SQLDataFileSIS objSQLDataFile2 = new SQLDataFileSIS();
    ERPNextService eRPNextService = new ERPNextService();

    static string assetEmpId;
    static int isSubmitted = 0;
    protected static string imgsource { get; set; }
    protected static string thankingName { get; set; }
    protected static string thankingDate { get; set; }
    static bool res = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["EmailId"] == null)
            {
                Response.Redirect("../Default.aspx");
            }

            hideSubmitBtn();
            CustomizeFieldsBasedOnUserRole();
            bindgdDepartmentalNoc();
            SetInitialRow();

            res = isClearanceExixts();

            if (Request.QueryString.AllKeys.Contains("id"))
            {
                DataTable dt2 = objSQLDataFile.ExecuteDataTableSQL(@"
select resg.EmpId,emp.ERPID, emp.EmpName,emp.EmpEmailId,emp.EmpDesignation, emp.EmpDepartment, emp.DOJ, resg.DatenTime, resg.Remarks, emp.Location, 
emp.RM, emp.NoticePeriod, resg.DatenTime,
DateOfRelieving  from EmployeeMaster emp 
join Resignations resg
on emp.Id = resg.EmpId
where resg.ResignationId =" + Request.QueryString["id"].ToString() + " AND IsWithdrawn = 0");

                if (dt2.Rows.Count > 0)
                {
                    txtempname.Text = dt2.Rows[0]["EmpName"].ToString();
                    txterpid.Text = dt2.Rows[0]["ERPID"].ToString();
                    txtdesgnation.Text = dt2.Rows[0]["EmpDesignation"].ToString();
                    txtdepartment.Text = dt2.Rows[0]["EmpDepartment"].ToString();
                    txtdoj.Text = getDateInDateFormat(dt2.Rows[0]["DOJ"].ToString());
                    txtdor.Text = getDateInDateFormat(dt2.Rows[0]["DatenTime"].ToString());
                    txtlocation.Text = dt2.Rows[0]["Location"].ToString();
                    txtrm.Text = dt2.Rows[0]["RM"].ToString();
                    assetEmpId = dt2.Rows[0]["EmpId"].ToString();
                    txtremark.Text = dt2.Rows[0]["Remarks"].ToString();
                    int noticePeriod = int.Parse(dt2.Rows[0]["NoticePeriod"].ToString());

                    Dictionary<int, int> noticePeriodMapping = new Dictionary<int, int>
                    {
                          { 1, 30 },
                          { 2, 60 },
                          { 3, 90 },
                          { 4, 120 },
                    };

                    if (noticePeriodMapping.ContainsKey(noticePeriod))
                    {
                        int noticePeriodInDays = noticePeriodMapping[noticePeriod];
                        DateTime dateOfResignation = DateTime.Parse(dt2.Rows[0]["DatenTime"].ToString());
                        DateTime dateOfRelieving = dateOfResignation.AddDays(noticePeriodInDays);
                        txtdorelieving.Text = dateOfRelieving.ToString("dd-MM-yyyy");
                    }
                }
            }
            bindgdAssetList();
            // if record exists then fill the details
            if (res)
            {
                DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT resg.EmpId, emp.EmpName, emp.EmpEmailId, emp.EmpDesignation, emp.EmpDepartment, emp.DOJ, resg.DatenTime,
        emp.RM,cf.DateOfResignation, DateOfRelieving, ThankingSignIMGPATH, ThankingName, ThankingDate, ccf.id as assetId, ccf.AssesstName, ccf.ReturnBackStatus, ccf.ReturnedToWhom, dcc.id as dccId, dcc.DistributorName, dcc.IsNOCRecieved,dcc.Description, dcc.BalanceStatus,dcc.DistributorNocClearanceImg, dnc.id as dncId, dnc.department_name, dnc.description, dnc.is_noc_recieved, dnc.amount_if_payable
        FROM EmployeeMaster emp
        JOIN Resignations resg ON emp.Id = resg.EmpId
        join [dbo].[ClearanceForm] cf on resg.ResignationId = cf.ResignationId
        left JOIN [dbo].[AssetChildClearance] ccf ON resg.ResignationId = ccf.ResignationId
        left JOIN [dbo].[DistributorChildClearance] dcc ON resg.ResignationId = dcc.ResignationId
        left JOIN [dbo].[Departmental_noc_child_clearance] dnc ON resg.ResignationId = dnc.ResignationId
        WHERE resg.ResignationId = " + Request.QueryString["id"].ToString() + " AND resg.IsWithdrawn = 0");

                if (dt.Rows.Count > 0)
                {
                    txtdorelieving.Text = getDateInDateFormat(dt.Rows[0]["DateOfRelieving"].ToString());
                    try
                    {
                        imgsource = dt.Rows[0]["ThankingSignIMGPATH"].ToString().Substring(1);
                    }
                    catch
                    {
                        imgsource = "";
                    }
                    thankingName = dt.Rows[0]["ThankingName"].ToString();

                    if (imgsource != "" && thankingName != "")
                    {
                        bottomFields.Visible = false;
                        bottomFilledFields.Visible = true;
                    }

                    thankingDate = getDateInDateFormat(dt.Rows[0]["ThankingDate"].ToString());

                    // setAssetGridview();
                    setDistributoGridview();
                    isfileUploaded();
                }
            }
        }
    }
    protected void CustomizeFieldsBasedOnUserRole()
    {
        #region Making fields readonly for depthead and visible if it is rm or hod of employee

        User user = new User();
        bool isRMOfEmp = IsUserRMOrHODOfResignedEmployee();
        string resignationDepartment = GetResignationDepartment();
        string loggedInUserEmail = Session["EmailId"].ToString();
        string loggedInUserDepartment = user.GetDepartmentByEmail(loggedInUserEmail);

        DataTable departmentTable = objSQLDataFile.ExecuteDataTableSQL("SELECT DISTINCT department_name FROM department_head");
        List<string> departmentNames = new List<string>();

        foreach (DataRow row in departmentTable.Rows)
        {
            departmentNames.Add(row["department_name"].ToString());
        }

        bool isDepartmentHead = departmentNames.Contains(loggedInUserDepartment);
        if (isDepartmentHead && !isRMOfEmp)
        {
            SetFieldsAsReadOnlyForDepartmentHead();
        }
        #endregion
      //  if (resignationDepartment.IndexOf("sales", StringComparison.OrdinalIgnoreCase) >= 0 || resignationDepartment.IndexOf("marketing", StringComparison.OrdinalIgnoreCase) >= 0)
        if (resignationDepartment.IndexOf("sales", StringComparison.OrdinalIgnoreCase) >= 0)
        {
            gdSuperiorClearance.Visible = true;
        }
        else
        {
            gdSuperiorClearance.Visible = false;
        }
    }
    protected void SetFieldsAsReadOnlyForDepartmentHead()
    {
        txtempname.Enabled = false;
        txtlocation.Enabled = false;
        txtdepartment.Enabled = false;
        txtdesgnation.Enabled = false;
        txtdoj.Enabled = false;
        txtrm.Enabled = false;
        txtdorelieving.Enabled = false;
        txtdor.Enabled = false;
        txtremark.Enabled = false;
        txterpid.Enabled = false;
        bottomFields.Enabled = false;
        bottomFilledFields.Enabled = false;
        txtthankingname.Enabled = false;
        txtthankingdate.Enabled = false;
        fusign.Enabled = false;
    }
    protected string GetResignationDepartment()
    {
        string dept = string.Empty;
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT em.EmpDepartment FROM Resignations r JOIN EmployeeMaster em ON r.EmpId = em.Id WHERE r.ResignationId = " + Request.QueryString["id"].ToString() + "");
        if (dt.Rows.Count > 0)
        {
            dept = dt.Rows[0]["EmpDepartment"].ToString();
        }
        return dept;
    }

    #region Distributor Gridview Functions
    protected void setDistributoGridview()
    {
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT  dcc.id as dccId, dcc.DistributorName,dcc.Description,dcc.BalanceStatus, dcc.IsNOCRecieved, dcc.DistributorNocClearanceImg FROM Resignations resg
        JOIN [dbo].[DistributorChildClearance] dcc ON resg.ResignationId = dcc.ResignationId
		WHERE resg.ResignationId = " + Request.QueryString["id"].ToString() + " AND resg.IsWithdrawn = 0");

        //adding the number of rows according to rows of db 

        int count = dt.Rows.Count;

        for (int i = 0; i < count; i++)
        {
            if (i < dt.Rows.Count)
            {
                AddNewRowToGrid();
                Label dccId = (Label)gdSuperiorClearance.Rows[i].FindControl("dccId");
                DropDownList dstrname = (DropDownList)gdSuperiorClearance.Rows[i].FindControl("ddlDistributorList");
                DropDownList noc = (DropDownList)gdSuperiorClearance.Rows[i].FindControl("ddlnoc");
                TextBox desc = (TextBox)gdSuperiorClearance.Rows[i].FindControl("txtdescription");
                TextBox dstrStatus = (TextBox)gdSuperiorClearance.Rows[i].FindControl("txtbill");
                FileUpload fuImage = (FileUpload)gdSuperiorClearance.Rows[i].FindControl("fuNocImage");
                HiddenField IsNocImg = (HiddenField)gdSuperiorClearance.Rows[i].FindControl("IsNocImg");
                //Button viewDistNocBtn = (Button)gdSuperiorClearance.Rows[i].FindControl("viewNoc");

                string distClearanceImg = dt.Rows[i]["DistributorNocClearanceImg"].ToString();
                dccId.Text = dt.Rows[i]["dccId"].ToString();
                dstrname.SelectedValue = dt.Rows[i]["DistributorName"].ToString();
                noc.SelectedValue = dt.Rows[i]["IsNOCRecieved"].ToString();
                desc.Text = dt.Rows[i]["Description"].ToString();
                dstrStatus.Text = dt.Rows[i]["BalanceStatus"].ToString();

                if (!string.IsNullOrEmpty(distClearanceImg))
                {
                    IsNocImg.Value = distClearanceImg;
                    // viewDistNocBtn.Visible = true;
                }

            }
        }
    }

    private void AddNewRowToGrid()
    {
        //**add blank row logic to avoid blank entry
        if (ViewState["CurrentTable"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
            DataRow drCurrentRow = null;

            if (dtCurrentTable.Rows.Count > 0)
            {
                drCurrentRow = dtCurrentTable.NewRow();
                drCurrentRow["RowNumber"] = dtCurrentTable.Rows.Count + 1;
                drCurrentRow["Column1"] = "0";

                dtCurrentTable.Rows.Add(drCurrentRow);

                ViewState["CurrentTable"] = dtCurrentTable;

                for (int i = 0; i < dtCurrentTable.Rows.Count - 1; i++)
                {
                    Label lblDccId = (Label)gdSuperiorClearance.Rows[i].FindControl("dccId");
                    DropDownList ddl = (DropDownList)gdSuperiorClearance.Rows[i].Cells[2].FindControl("ddlDistributorList");
                    DropDownList box2 = (DropDownList)gdSuperiorClearance.Rows[i].Cells[3].FindControl("ddlnoc");
                    TextBox box4 = (TextBox)gdSuperiorClearance.Rows[i].Cells[4].FindControl("txtdescription");
                    TextBox box3 = (TextBox)gdSuperiorClearance.Rows[i].Cells[5].FindControl("txtbill");
                    HiddenField IsNocImg = (HiddenField)gdSuperiorClearance.Rows[i].Cells[6].FindControl("IsNocImg");
                    //FileUpload fuImage = (FileUpload)gdSuperiorClearance.Rows[i].Cells[6].FindControl("fuNocImage");


                    dtCurrentTable.Rows[i]["Column1"] = lblDccId.Text;
                    dtCurrentTable.Rows[i]["Column2"] = ddl.SelectedValue;
                    dtCurrentTable.Rows[i]["Column3"] = box2.Text;
                    dtCurrentTable.Rows[i]["Column5"] = box4.Text;
                    dtCurrentTable.Rows[i]["Column4"] = box3.Text;
                    dtCurrentTable.Rows[i]["Column6"] = IsNocImg.Value;
                    //if (IsNocImg.Visible)
                    //{
                    //    //fuImage.Visible = false;
                    //    dtCurrentTable.Rows[i]["Column6"] = IsNocImg;
                    //}
                    //else
                    //{
                    //    //fuImage.Visible = true;
                    //    ;
                    //}

                }

                gdSuperiorClearance.DataSource = dtCurrentTable;
                gdSuperiorClearance.DataBind();
            }
        }
        else
        {
            Response.Write("ViewState is null");
        }
        SetPreviousData();
    }
    private void SetPreviousData()
    {
        if (ViewState["CurrentTable"] != null)
        {
            bindDistributorList();
            DataTable dt = (DataTable)ViewState["CurrentTable"];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Label lblDccId = (Label)gdSuperiorClearance.Rows[i].FindControl("dccId");
                DropDownList ddl = (DropDownList)gdSuperiorClearance.Rows[i].FindControl("ddlDistributorList");
                DropDownList box2 = (DropDownList)gdSuperiorClearance.Rows[i].FindControl("ddlnoc");
                TextBox box4 = (TextBox)gdSuperiorClearance.Rows[i].FindControl("txtdescription");
                TextBox box3 = (TextBox)gdSuperiorClearance.Rows[i].FindControl("txtbill");
                HiddenField IsNocImg = (HiddenField)gdSuperiorClearance.Rows[i].FindControl("IsNocImg");
                FileUpload fuImage = (FileUpload)gdSuperiorClearance.Rows[i].FindControl("fuNocImage");
                Button btnViewNoc = (Button)gdSuperiorClearance.Rows[i].FindControl("viewNoc");

                lblDccId.Text = dt.Rows[i]["Column1"].ToString();
                string selectedValue = dt.Rows[i]["Column2"].ToString();
                ListItem selectedItem = ddl.Items.FindByValue(selectedValue);
                if (selectedItem != null)
                {
                    ddl.SelectedValue = selectedValue;
                }
                box2.Text = dt.Rows[i]["Column3"].ToString();
                box4.Text = dt.Rows[i]["Column5"].ToString();
                box3.Text = dt.Rows[i]["Column4"].ToString();
                IsNocImg.Value = dt.Rows[i]["Column6"].ToString();
            }
        }
    }
    private void SetInitialRow(int rowCount = 1)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
        dt.Columns.Add(new DataColumn("Column1", typeof(string)));
        dt.Columns.Add(new DataColumn("Column2", typeof(string)));
        dt.Columns.Add(new DataColumn("Column3", typeof(string)));//for TextBox value   
        dt.Columns.Add(new DataColumn("Column4", typeof(string)));
        dt.Columns.Add(new DataColumn("Column5", typeof(string)));//for TextBox value
        dt.Columns.Add(new DataColumn("Column6", typeof(string)));

        dr = dt.NewRow();
        dr["RowNumber"] = 1;
        dr["Column1"] = "0";
        dr["Column2"] = string.Empty;
        dr["Column3"] = string.Empty;
        dr["Column4"] = string.Empty;
        dr["Column5"] = string.Empty;
        dr["Column6"] = string.Empty;
        dt.Rows.Add(dr);

        ViewState["CurrentTable"] = dt;

        gdSuperiorClearance.DataSource = dt;
        gdSuperiorClearance.DataBind();

        bindDistributorList();
    }
    protected void insertOrUpdateDistributorChild()
    {
        User user = new User();
        try
        {
            int resgId = int.Parse(Request.QueryString["id"].ToString());
            if (gdSuperiorClearance.Rows.Count > 0)
            {
                foreach (GridViewRow row2 in gdSuperiorClearance.Rows)
                {
                    Label dccId = (Label)row2.FindControl("dccId");
                    DropDownList dstrName1 = (DropDownList)row2.FindControl("ddlDistributorList");
                    DropDownList dlNoc = (DropDownList)row2.FindControl("ddlnoc");
                    TextBox description = (TextBox)row2.FindControl("txtdescription");
                    TextBox dstrBalStatus = (TextBox)row2.FindControl("txtbill");
                    FileUpload fuNocImg = (FileUpload)row2.FindControl("fuNocImage");
                    HiddenField nocImgPath = (HiddenField)row2.FindControl("IsNocImg");

                    int dccid = int.Parse(dccId.Text);
                    string DistName = dstrName1.SelectedItem.Text;
                    string Noc = dlNoc.SelectedItem.Text;
                    string descr = description.Text;
                    string Balance = dstrBalStatus.Text;
                   
                    #region code to check if file already exists if so set it otherwise new file is uploaded then delete old file and update record with new file
                    string noc_img = nocImgPath.Value; // Initialize with the current file path

                    // Check if a new file is uploaded
                    if (fuNocImg.HasFile)
                    {
                        // Delete the old file (if it exists)
                        if (!string.IsNullOrEmpty(noc_img))
                        {
                            string oldFilePath = Server.MapPath(noc_img);
                            if (File.Exists(oldFilePath))
                            {
                                File.Delete(oldFilePath);
                            }
                        }

                        // Save the new file
                        string filePath = "~/DistributorNocClearance/";
                        string timestamp = DateTime.Now.ToString("yyyy-MM-dd");
                        noc_img = filePath + timestamp + fuNocImg.FileName;
                        fuNocImg.SaveAs(Server.MapPath(noc_img));

                    }
                    #endregion
                    if (!string.IsNullOrEmpty(DistName))
                    {
                        user.addOrUpdateDistributorChildClearance(dccid, resgId, DistName, Noc, descr, Balance, noc_img);

                    }

                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ButtonAdd_Click(object sender, EventArgs e)
    {
        AddNewRowToGrid();
    }
    private void ResetRowID(DataTable dt)
    {
        int rowNumber = 1;
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                row[0] = rowNumber;
                rowNumber++;
            }
        }
    }
    protected void RemoveRow_Command(object sender, CommandEventArgs e)
    {
        if (ViewState["CurrentTable"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTable"];

            if (dt.Rows.Count > 1)
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                if (rowIndex < dt.Rows.Count)
                {
                    GridViewRow row = gdSuperiorClearance.Rows[rowIndex];
                    Label lblDccId = (Label)gdSuperiorClearance.Rows[rowIndex].FindControl("dccId");
                    if (lblDccId != null)
                    {
                        int recordID = Convert.ToInt32(lblDccId.Text);
                        DeleteRecordFromDatabase(recordID);
                        dt.Rows.RemoveAt(rowIndex);
                        ResetRowID(dt);
                    }
                }
            }
            ViewState["CurrentTable"] = dt;
            gdSuperiorClearance.DataSource = dt;
            gdSuperiorClearance.DataBind();
        }
        Response.Redirect(Request.RawUrl);
    }
    private void DeleteRecordFromDatabase(int recordID)
    {
        int id;

        DataTable dt = objSQLDataFile.ExecuteDataTableSQL("SELECT * FROM [dbo].[DistributorChildClearance] WHERE id=" + recordID);
        if (dt.Rows.Count > 0)
        {
            id = int.Parse(dt.Rows[0]["id"].ToString());
            objSQLDataFile.ExecuteDataTableSQL(@"delete from [dbo].[DistributorChildClearance] where id=" + id);
        }
        SetPreviousData();
    }
    protected void bindDistributorList()
    {
        List<string> customerList = eRPNextService.getCustomersList();

        foreach (GridViewRow row in gdSuperiorClearance.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlDistributorList = row.FindControl("ddlDistributorList") as DropDownList;
                // List<string> customerList = eRPNextService.getCustomersList();
                ddlDistributorList.DataSource = customerList;
                ddlDistributorList.DataBind();
            }
        }
    }
    protected void isfileUploaded()
    {
        DataTable supiriorClearanceGd = (DataTable)gdSuperiorClearance.DataSource;
        int count = supiriorClearanceGd.Rows.Count;

        for (int i = 0; i < count; i++)
        {
            if (i < supiriorClearanceGd.Rows.Count)
            {
                FileUpload fuImage = (FileUpload)gdSuperiorClearance.Rows[i].FindControl("fuNocImage");
                HiddenField nocImgPath = (HiddenField)gdSuperiorClearance.Rows[i].FindControl("IsNocImg");
                Button viewDistNocBtn = (Button)gdSuperiorClearance.Rows[i].FindControl("viewNoc");

                if (!string.IsNullOrEmpty(nocImgPath.Value))
                {
                    viewDistNocBtn.Visible = true;
                }
            }

        }
    }
    protected void gdSuperiorClearance_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataTable dt = (DataTable)ViewState["CurrentTable"];
            LinkButton lb = (LinkButton)e.Row.FindControl("LinkButton1");

            if (lb != null)
            {
                if (dt.Rows.Count > 1)
                {
                    if (e.Row.RowIndex == dt.Rows.Count - 1)
                    {
                        lb.Visible = false;
                    }
                }
                else
                {
                    lb.Visible = false;
                }
            }
        }
    }
    #endregion

    #region Asset Gridview Functions
    protected void bindgdAssetList()
    {
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"select Id, AssesstName, ReturnBackStatus, ReturnedToWhom from AssetChildClearance where ResignationId = " + Request.QueryString["id"].ToString() +
    "union select 0 as Id, AssesstName, NULL ReturnBackStatus, NULL ReturnedToWhom from CompanyAssesstsAssigned " +
    "WHERE (EmpId =" + assetEmpId + " and AssesstName not in (select AssesstName from AssetChildClearance where ResignationId = " + Request.QueryString["id"].ToString() + "))");

        gdAssetList.DataSource = dt;
        gdAssetList.DataBind();
    }
    protected void setAssetGridview()
    {
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"SELECT  acc.Id as assetId, acc.AssesstName,acc.ReturnBackStatus,acc.ReturnedToWhom FROM Resignations resg
        JOIN [dbo].[AssetChildClearance] acc ON resg.ResignationId = acc.ResignationId
		WHERE resg.ResignationId = " + Request.QueryString["id"].ToString() + " AND resg.IsWithdrawn = 0");

        int count = dt.Rows.Count;

        for (int i = 0; i < count; i++)
        {
            if (i < dt.Rows.Count)
            {
                Label accId = (Label)gdAssetList.Rows[i].FindControl("assetId");
                TextBox returnStatus = (TextBox)gdAssetList.Rows[i].FindControl("txtrbs");
                TextBox returnTo = (TextBox)gdAssetList.Rows[i].FindControl("txtrtw");

                string assetName = dt.Rows[i]["AssesstName"].ToString();
                accId.Text = dt.Rows[i]["assetId"].ToString();
                returnStatus.Text = dt.Rows[i]["ReturnBackStatus"].ToString();
                returnTo.Text = dt.Rows[i]["ReturnedToWhom"].ToString();

            }
        }
    }
    protected void insertOrUpdateAssetNocChild()
    {
        User user = new User();
        try
        {
            int resgId = int.Parse(Request.QueryString["id"].ToString());
            if (gdDeptNOC.Rows.Count > 0)
            {
                foreach (GridViewRow row1 in gdAssetList.Rows)
                {
                    Label assetId = (Label)row1.FindControl("assetId");
                    string assetName = row1.Cells[1].Text;
                    TextBox rbs = (TextBox)row1.FindControl("txtrbs");
                    Label lblBs = (Label)row1.FindControl("lblBs");
                    TextBox rtw = (TextBox)row1.FindControl("txtrtw");
                    Label lblRtw = (Label)row1.FindControl("lblRtw");

                    int assetid = int.Parse(assetId.Text);
                    string returnedBack = lblBs.Visible ? lblBs.Text : rbs.Text;
                    string returnedToWhom = lblRtw.Visible ? lblRtw.Text : rtw.Text;

                    //string returnedBack = rbs.Text;
                    //string returnedToWhom = rtw.Text;
                    if (!string.IsNullOrEmpty(assetName))
                    {
                        assetName = (assetName.Contains("&amp;")) ? assetName.Replace("&amp;", "&") : assetName;
                    }

                    user.AddOrUpdateAssetNocChildClearance(assetid, resgId, assetName, returnedBack, returnedToWhom);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gdAssetList_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label assetId = e.Row.FindControl("assetId") as Label;
            string assetName = e.Row.Cells[1].Text;
            Label lblBs = e.Row.FindControl("lblBs") as Label;
            TextBox txtrbs = e.Row.FindControl("txtrbs") as TextBox;
            Label lblRtw = e.Row.FindControl("lblRtw") as Label;
            TextBox txtrtw = e.Row.FindControl("txtrtw") as TextBox;
            string AssetName = e.Row.Cells[1].Text;

            if (!string.IsNullOrEmpty(lblBs.Text))
            {
                lblBs.Visible = true;
                txtrbs.Visible = false;
            }
            else
            {
                lblBs.Visible = false;
                txtrbs.Visible = true;
            }
            if (!string.IsNullOrEmpty(lblRtw.Text))
            {
                lblRtw.Visible = true;
                txtrtw.Visible = false;
            }
            else
            {
                lblRtw.Visible = false;
                txtrtw.Visible = true;
            }
        }

    }
    //protected void bindgdAssetList()
    //{
    //    DataTable dt = objSQLDataFile.ExecuteDataTableSQL("SELECT * FROM CompanyAssesstsAssigned WHERE EmpId=" + assetEmpId);
    //    gdAssetList.DataSource = dt;
    //    gdAssetList.DataBind();
    //}
    #endregion

    #region Departmental NOC Gridview Functions
    protected void bindgdDepartmentalNoc()
    {
        User user = new User();
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL(@"select id,department_name, description, is_noc_recieved, amount_if_payable from Departmental_noc_child_clearance where ResignationId = " + Request.QueryString["id"].ToString() +
            " union select 0 as id, department_name, NULL description, NULL is_noc_recieved , NULL amount_if_payable from Department_Head where department_name not in (select department_name from Departmental_noc_child_clearance where ResignationId = " + Request.QueryString["id"].ToString() + ")");

        gdDeptNOC.DataSource = dt;
        gdDeptNOC.DataBind();

    }
    protected void insertOrUpdateDeptNocChild()
    {
        User user = new User();
        try
        {
            int resgId = int.Parse(Request.QueryString["id"].ToString());
            if (gdDeptNOC.Rows.Count > 0)
            {
                foreach (GridViewRow row in gdDeptNOC.Rows)
                {
                    string loggedInUserDepartment = user.GetDepartmentByEmail(Session["EmailId"].ToString());
                    string deptName = row.Cells[1].Text;

                    if (deptName == loggedInUserDepartment)
                    {
                        Label dncId = (Label)row.FindControl("dncId");
                        Label lblDesc = (Label)row.FindControl("lblDesc");
                        TextBox txtDesc = (TextBox)row.FindControl("txtDesc");
                        Label lblNoc = (Label)row.FindControl("lblNoc");
                        DropDownList txtNoc = (DropDownList)row.FindControl("ddlNoc");
                        Label lblAmt = (Label)row.FindControl("lblAmt");
                        TextBox txtAmt = (TextBox)row.FindControl("txtAmt");

                        int dncid = int.Parse(dncId.Text);
                        string description = lblDesc.Visible ? lblDesc.Text : txtDesc.Text;
                        string nocReceived = lblNoc.Visible ? lblNoc.Text : txtNoc.Text;

                        string amountStr = lblAmt.Visible ? lblAmt.Text : txtAmt.Text;
                        float amountPayable = (string.IsNullOrEmpty(amountStr)) ? -1 : float.Parse(amountStr);

                        user.AddOrUpdateDeptNocChildClearance(dncid, resgId, deptName, description, nocReceived, amountPayable);
                    }
                    else
                    {
                        row.Visible = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gdDeptNOC_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string loggedInUserEmail = Session["EmailId"].ToString();
            User user = new User();
            string loggedInUserDepartment = user.GetDepartmentByEmail(loggedInUserEmail);

            Label dncId = e.Row.FindControl("dncId") as Label;
            Label lblDesc = e.Row.FindControl("lblDesc") as Label;
            TextBox txtDesc = e.Row.FindControl("txtDesc") as TextBox;
            Label lblNoc = e.Row.FindControl("lblNoc") as Label;
            DropDownList txtNoc = e.Row.FindControl("ddlNoc") as DropDownList;
            Label lblAmt = e.Row.FindControl("lblAmt") as Label;
            TextBox txtAmt = e.Row.FindControl("txtAmt") as TextBox;

            float lblAmount = (string.IsNullOrEmpty(lblAmt.Text)) ? -1 : float.Parse(lblAmt.Text);
            int departmentNameColumnIndex = 1;
            string rowDepartment = e.Row.Cells[departmentNameColumnIndex].Text;

            if (string.IsNullOrEmpty(lblDesc.Text) && string.IsNullOrEmpty(lblNoc.Text) && (lblAmount == -1) && loggedInUserDepartment != rowDepartment)
            {
                e.Row.Visible = false;
            }
            else
            {
                if (!string.IsNullOrEmpty(lblDesc.Text))
                {
                    lblDesc.Visible = true;
                    txtDesc.Visible = false;
                }
                else
                {
                    if (loggedInUserDepartment == rowDepartment)
                    {
                        lblDesc.Visible = false;
                        txtDesc.Visible = true;
                    }
                    else
                    {
                        lblDesc.Visible = true;
                        txtDesc.Visible = false;
                    }
                }

                if (!string.IsNullOrEmpty(lblNoc.Text))
                {
                    lblNoc.Visible = true;
                    txtNoc.Visible = false;
                }
                else
                {
                    if (loggedInUserDepartment == rowDepartment)
                    {
                        lblNoc.Visible = false;
                        txtNoc.Visible = true;
                    }
                    else
                    {
                        lblNoc.Visible = true;
                        txtNoc.Visible = false;
                    }
                }
                if (lblAmount > -1)
                {
                    lblAmt.Visible = true;
                    txtAmt.Visible = false;
                }
                else
                {
                    if (loggedInUserDepartment == rowDepartment)
                    {
                        lblAmt.Visible = false;
                        txtAmt.Visible = true;
                    }
                    else
                    {
                        lblAmt.Visible = false;
                        txtAmt.Visible = false;
                    }
                }
            }
        }
    }
    #endregion

    protected bool isClearanceExixts()
    {
        bool res2 = false;
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL("select * from [dbo].[ClearanceForm] where ResignationId =" + Request.QueryString["id"].ToString());

        if (dt.Rows.Count > 0)
        {
            res2 = true;
        }
        return res2;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ResingList.aspx");
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        isSubmitted = 1;

        if (res)
        {
            updateClearanceForm();
        }
        else
        {
            firstEntryInClearance();
        }
        #region Send Final Clearance submission mail to HR

        int clearanceId = int.Parse(Request.QueryString["id"].ToString());
        ClearanceData clearanceData = new ClearanceData(clearanceId);
        string clearancePdfFile = clearanceData.clearancePdfFileName;

        string physicalDirectoryPath = Server.MapPath("~/clearance_files/");
        string physicalFilePath = Path.Combine(physicalDirectoryPath, clearancePdfFile);
        string erpid = "";
        string empname = "";
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL("select emp.EmpName, resg.ERPId from Resignations resg Join Employeemaster emp on resg.empid = emp.id where ResignationId = " + clearanceId);
        if (dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            erpid = row["ERPId"].ToString();
            empname = row["EmpName"].ToString();
        }

        PassNMail pm = new PassNMail();
        pm.SendEmailToHR(empname, physicalFilePath);

        #endregion 
        Response.Redirect("ResingList.aspx");
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        isSubmitted = 0;

        if (res)
        {
            updateClearanceForm();
        }
        else
        {
            firstEntryInClearance();
        }
        //  Response.Redirect("ResingList.aspx");
    }
    protected void firstEntryInClearance()
    {
        try
        {
            if (fusign.PostedFile.FileName != "" && txtthankingdate.Text != "" && txtdorelieving.Text != "")
            {
                User user = new User();
                string FileName = string.Empty;
                string FilePath = string.Empty;
                string resgId = Request.QueryString["id"].ToString();

                FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + fusign.PostedFile.FileName;
                FilePath = @"~/ClearanceSignatureImage/" + FileName.Trim().ToString();
                fusign.PostedFile.SaveAs(Server.MapPath(FilePath));

                user.AddClearanceForm(int.Parse(resgId), int.Parse(assetEmpId), txterpid.Text, txtdor.Text, txtdorelieving.Text, FileName, FilePath, txtthankingname.Text, txtthankingdate.Text, isSubmitted);
                objSQLDataFile.ExecuteDataTableSQL("update Resignations set DateOfRelieving = CONVERT(date,'" + txtdorelieving.Text + "',105) where ResignationId =" + Request.QueryString["id"].ToString());
                objSQLDataFile.ExecuteDataTableSQL("update ClearanceForm set DateOfRelieving = '" + txtdorelieving.Text + "', ThankingSignIMGPATH = ' " + FilePath + "' , ThankingName = '" + FileName + "', ThankingDate = '" + txtthankingdate.Text + "' where ResignationId =" + Request.QueryString["id"].ToString());
                if (gdAssetList.Rows.Count > 0)
                {
                    insertOrUpdateAssetNocChild();
                    //foreach (GridViewRow row1 in gdAssetList.Rows)
                    //{
                    //    string assetName = row1.Cells[0].Text;
                    //    TextBox rbs = (TextBox)row1.FindControl("txtrbs").FindControl("txtrbs");
                    //    TextBox rtw = (TextBox)row1.FindControl("txtrtw").FindControl("txtrtw");

                    //    string returnedBack = rbs.Text;
                    //    string returnedToWhom = rtw.Text;

                    //    user.AddAssetChildClearance(resgId, assetName, returnedBack, returnedToWhom);
                    //}
                }

                if (gdSuperiorClearance.Rows.Count > 0)
                {
                    insertOrUpdateDistributorChild();
                }

                if (gdDeptNOC.Rows.Count > 0)
                {
                    insertOrUpdateDeptNocChild();
                }

                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>if(window.confirm('Details Added')){window.location.href='ClearanceForm.aspx?id=" + resgId + "'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
            }
            else
            {
                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>alert('These Fields are compulsory sign image, date of relieving & date.');</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
            }
        }
        catch (Exception ex)
        {
            //ClientScriptManager CSM = Page.ClientScript;
            //string strconfirm = "<script>alert('Something went wrong, Please try again.');</script>";
            //CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
        }
    }
    protected void updateClearanceForm()
    {
        try
        {
            User user = new User();
            string resgId = Request.QueryString["id"].ToString();
            DataTable dt = null;
            int id = 0;
            int rw = 0;

            if (isSubmitted == 1)
            {
                objSQLDataFile.ExecuteDataTableSQL("update [dbo].[ClearanceForm] set isSubmitted=1 where ResignationId =" + Request.QueryString["id"].ToString());
            }
            objSQLDataFile.ExecuteDataTableSQL("update [dbo].[ClearanceForm] set ThankingDate = CONVERT(date,'" + txtthankingdate.Text + "',105), DateofRelivieng = '" + txtdorelieving.Text + "'  where ResignationId =" + Request.QueryString["id"].ToString());
            objSQLDataFile.ExecuteDataTableSQL("update Resignations set DateOfRelieving = CONVERT(date,'" + txtdorelieving.Text + "',105) where ResignationId =" + Request.QueryString["id"].ToString());

            if (gdAssetList.Rows.Count > 0)
            {
                insertOrUpdateAssetNocChild();
            }

            if (gdSuperiorClearance.Rows.Count > 0)
            {
                insertOrUpdateDistributorChild();
            }

            if (gdDeptNOC.Rows.Count > 0)
            {
                insertOrUpdateDeptNocChild();
            }

            if (isSubmitted == 1)
            {
                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>if(window.confirm('Details Updated')){window.location.href='ResingList.aspx'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
            }
            else
            {
                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>if(window.confirm('Details Updated')){window.location.href='ClearanceForm.aspx?id=" + resgId + "'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
            }
        }
        catch (Exception ex)
        {
            
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
            }
        }
        return date;
    }
    protected void hideSubmitBtn()
    {
        User user = new User();
        int resignationId = int.Parse(Request.QueryString["id"].ToString());
        DataTable res = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT IsHOD, IsRM, ISHR FROM EmployeeMaster WHERE EmpEmailId = '" + Session["EmailId"].ToString() + "'");

        if (res.Rows.Count > 0)
        {
            DataRow row = res.Rows[0];
            int isHOD = 0;
            int isRM = 0;
            int isHR = 0;

            int.TryParse(row["IsHOD"].ToString(), out isHOD);
            int.TryParse(row["IsRM"].ToString(), out isRM);
            int.TryParse(row["ISHR"].ToString(), out isHR);

            if (isRM > 0 || isHOD > 0)
            {
                string rmName = user.getEmployeeName(Session["EmailId"].ToString());

                DataTable dt2 = objSQLDataFile.ExecuteDataTableSQL(@"SELECT resg.EmpId FROM Resignations resg
                JOIN EmployeeMaster emp ON resg.EmpId = emp.Id
                WHERE (emp.RM = '" + rmName + "' OR emp.HOD = '" + rmName + "') AND resg.ResignationId = " + resignationId + "");

                if (dt2.Rows.Count > 0)
                {
                    btnSubmit.Visible = true;
                }
                else
                {
                    btnSubmit.Visible = false;
                }
            }
            else
            {
                btnSubmit.Visible = false;
            }
        }
    }
    protected bool IsUserRMOrHODOfResignedEmployee()
    {
        int resignationId = int.Parse(Request.QueryString["id"].ToString());
        DataTable res = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT IsHOD, IsRM, ISHR FROM EmployeeMaster WHERE EmpEmailId = '" + Session["EmailId"].ToString() + "'");
        bool isRMOrHODOfEmp = false;

        if (res.Rows.Count > 0)
        {
            DataRow row = res.Rows[0];
            int isRM = 0;
            int.TryParse(row["IsRM"].ToString(), out isRM);

            int isHOD = 0;
            int.TryParse(row["IsHOD"].ToString(), out isHOD);

            if (isRM > 0 || isHOD > 0)
            {
                User user = new User();
                string rmName = user.getEmployeeName(Session["EmailId"].ToString());

                DataTable dt2 = objSQLDataFile.ExecuteDataTableSQL(@"SELECT resg.EmpId FROM Resignations resg
                JOIN EmployeeMaster emp ON resg.EmpId = emp.Id
                WHERE (emp.RM = '" + rmName + "' OR emp.HOD = '" + rmName + "') AND resg.ResignationId = " + resignationId + "");

                isRMOrHODOfEmp = dt2.Rows.Count > 0;
            }
        }
        return isRMOrHODOfEmp;
    }

}
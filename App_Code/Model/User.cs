using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for EmployeeMaster
/// </summary>

public class User
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    PassNMail pm = new PassNMail();
    public List<ListItem> pages = new List<ListItem>();
    public User()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public DataTable GetEmployeeDetailsByEmail(string empEmail)
    {
        DataTable dtemp = null;
        try
        {
            dtemp = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT * from EmployeeMaster WHERE EmpEmailId = '" + empEmail + "'");
        }
        catch (Exception ex) { }
        return dtemp;
    }
    public DataTable CheckUserAuth(string userid, string password)
    {
        try
        {
            string SPName = "CheckUserAuth";
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;
            #region Parameters
            cmd.Parameters.Add("@EmailId", SqlDbType.VarChar).Value = userid;
            cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = password;
            #endregion

            //Get data
            DataTable userList = objSQLDataFile.ExecuteDataTable(cmd, SPName);


            return userList;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    public int getEmployeeId(string currEmp, string EmpCode = "")
    {
        int empId = 0;
        if (EmpCode == "") //only send empid back by using email only
        {
            DataTable res = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT Id from EmployeeMaster WHERE EmpEmailId = '" + currEmp + "'");
            if (res.Rows.Count > 0)
            {
                DataRow row = res.Rows[0];
                empId = int.Parse(row["Id"].ToString());
            }
        }
        else if (EmpCode != "")
        {
            DataTable res = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT Id from EmployeeMaster WHERE EmpEmailId = '" + currEmp + "' or EmpCode='" + EmpCode + "'");
            if (res.Rows.Count > 0)
            {
                DataRow row = res.Rows[0];
                empId = int.Parse(row["Id"].ToString());
            }
        }

        return empId;
    }
    public string getEmployeeName(string empEmail)
    {
        string empName = "";

        if (empEmail != "")
        {

            DataTable res = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT EmpName from EmployeeMaster WHERE EmpEmailId = '" + empEmail + "'");
            if (res.Rows.Count > 0)
            {
                DataRow row = res.Rows[0];
                empName = row["EmpName"].ToString();
            }
        }
        return empName;
    }
    public int checkDuplicate(string EmpCode = "", string empMail = "")
    {
        int res = 0;
        if (EmpCode != "")
        {
            DataTable dt = objSQLDataFile.ExecuteDataTableSQL("select EmpName from EmployeeMaster where EmpCode='" + EmpCode + "' or EmpEmailId ='" + empMail + "'");
            if (dt.Rows.Count > 0)
            {
                res = 1;
            }
        }
        else if (empMail != "")
        {
            DataTable dt = objSQLDataFile.ExecuteDataTableSQL("select EmailId from UserMaster where EmailId'" + empMail + "'");
            if (dt.Rows.Count > 0)
            {
                res = 1;
            }
        }

        return res;

    }
    public int checkDuplicateResignEntry(int empid)
    {
        int res = 0;
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL("Select * from Resignations where EmpId=" + empid+" and status = 'Approved'");
        if(dt.Rows.Count > 0)
        {
            res = 1;
        }
        return res;
    }
    public int checkDuplicateHiddenVacancy(int empdId, int vacId)
    {
        int res = 0;
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL("select * from HiddenVacancies where EmpId=" + empdId + " and VacancyId=" + vacId);
        if (dt.Rows.Count > 0)
        {
            res = 1;
        }

        return res;

    }
    public int checkDuplicateVacancies(string pos, string department, string ZONE, string SDM, string HQ)
    {
        int res = 0;
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL("select Position from [dbo].[AvailableVacancies] where Position = '" + pos + "' and Department = '" + department + "' and ZONE ='" + ZONE + "' and SDM='" + SDM + "' and HQ ='" + HQ + "'");
        if (dt.Rows.Count > 0)
        {
            res = 1;
        }
        return res;

    }

    public int UpdateEmployee(int id, string EmpName, string EmpEmailId, string EmpDesignation,
   string EmpDepartment, string Status, string ModifiedBy, int NoticePeriod, string HOD, string RM, string DOJ,
   string dltype, int IsHOD = 0, int IsRM = 0, int IsHR = 0, string ERPId = "",
   string Location = "", string DOB = null)
    {
        try
        {
            string connstr = string.Empty, _EmailID = string.Empty;
            string SPName = "UpdateEmployee";
            connstr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (var conn = new SqlConnection(connstr))
            using (var cmd = new SqlCommand(SPName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@EmpName", SqlDbType.VarChar, 50).Value = (object)EmpName ?? DBNull.Value;
                cmd.Parameters.Add("@EmpEmailId", SqlDbType.VarChar, 50).Value = (object)EmpEmailId ?? DBNull.Value;
                cmd.Parameters.Add("@EmpDesignation", SqlDbType.VarChar, 50).Value = (object)EmpDesignation ?? DBNull.Value;
                cmd.Parameters.Add("@EmpDepartment", SqlDbType.VarChar, 50).Value = (object)EmpDepartment ?? DBNull.Value;
                cmd.Parameters.Add("@Status", SqlDbType.VarChar, 50).Value = (object)Status ?? DBNull.Value;
                cmd.Parameters.Add("@ModifiedBy", SqlDbType.VarChar, 50).Value = (object)ModifiedBy ?? DBNull.Value;
                cmd.Parameters.Add("@NoticePeriod", SqlDbType.Int).Value = NoticePeriod;
                cmd.Parameters.Add("@HOD", SqlDbType.VarChar, 50).Value = (object)HOD ?? DBNull.Value;
                cmd.Parameters.Add("@RM", SqlDbType.VarChar, 50).Value = (object)RM ?? DBNull.Value;
                cmd.Parameters.Add("@IsHOD", SqlDbType.Int).Value = IsHOD;
                cmd.Parameters.Add("@IsRM", SqlDbType.Int).Value = IsRM;
                cmd.Parameters.Add("@IsHR", SqlDbType.Int).Value = IsHR;
                cmd.Parameters.Add("@ERPId", SqlDbType.VarChar, 50).Value = (object)ERPId ?? DBNull.Value;
                cmd.Parameters.Add("@Location", SqlDbType.VarChar, 50).Value = (object)Location ?? DBNull.Value;


                DateTime tmp;
                if (string.IsNullOrWhiteSpace(DOJ))
                    cmd.Parameters.Add("@DOJ", SqlDbType.Date).Value = DBNull.Value;
                else if (DateTime.TryParseExact(DOJ, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out tmp) ||
                         DateTime.TryParseExact(DOJ, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out tmp) ||
                         DateTime.TryParse(DOJ, out tmp))
                    cmd.Parameters.Add("@DOJ", SqlDbType.Date).Value = tmp.Date;
                else
                    cmd.Parameters.Add("@DOJ", SqlDbType.Date).Value = DBNull.Value;

                if (string.IsNullOrWhiteSpace(DOB))
                    cmd.Parameters.Add("@DOB", SqlDbType.Date).Value = DBNull.Value;
                else if (DateTime.TryParseExact(DOB, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out tmp) ||
                         DateTime.TryParseExact(DOB, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out tmp) ||
                         DateTime.TryParse(DOB, out tmp))
                    cmd.Parameters.Add("@DOB", SqlDbType.Date).Value = tmp.Date;
                else
                    cmd.Parameters.Add("@DOB", SqlDbType.Date).Value = DBNull.Value;

                if (dltype == "EmployeeHR")
                {
                    objSQLDataFile.ExecuteDataTableSQL(@"update UserMaster set Status = '" + Status + "', " +
                    "UserType = '" + dltype + "', EmailId = '" + EmpEmailId + "', " +
                    "EmpId=" + id + " where Id=(select top 1 Id from [dbo].[UserMaster] where EmailId = '' and UserType = 'EmployeeHR')" +
                    " or Id = (select top 1 Id from[dbo].[UserMaster] where EmailId = '" + EmpEmailId + "') or EmpId = " + id);
                }
                else
                {
                    objSQLDataFile.ExecuteDataTableSQL(@"update UserMaster set Status = '" + Status + "', " +
                    "UserType = '" + dltype + "', EmailId = '" + EmpEmailId + "', " +
                    "EmpId=" + id + " where Id=(select top 1 Id from [dbo].[UserMaster] where EmailId = '' and UserType = 'Employee')" +
                    " or Id = (select top 1 Id from [dbo].[UserMaster] where EmailId = '" + EmpEmailId + "') or EmpId = " + id);
                }

                var returnParam = new SqlParameter();
                returnParam.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(returnParam);

                conn.Open();
                cmd.ExecuteNonQuery();

                int rows = Convert.ToInt32(returnParam.Value);
                return rows;
            }


        }
        catch
        {

            throw;
        }
    }
   public int AddEmployee(
    string empName,
    string empCode,
    string empEmailId,
    string empDesignation,
    string empDepartment,
    string status,
    string createdBy,
    int noticePeriod,
    string hod = "",
    string rm = "",
    string doj = null,
    string dlType = "",
    int isHod = 0,
    int isRm = 0,
    int isHr = 0,
    string erpId = "",
    string location = "",
    string dob = null)
{
    int result = 0;
    int check = checkDuplicate(empCode, empEmailId);

    if (check == 0)
    {
        try
        {
            string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("AddEmployee", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

              
                cmd.Parameters.Add("@EmpName", SqlDbType.NVarChar).Value = empName;
                cmd.Parameters.Add("@EmpCode", SqlDbType.NVarChar).Value = empCode;
                cmd.Parameters.Add("@EmpEmailId", SqlDbType.NVarChar).Value = empEmailId;
                cmd.Parameters.Add("@EmpDesignation", SqlDbType.NVarChar).Value = empDesignation;
                cmd.Parameters.Add("@EmpDepartment", SqlDbType.NVarChar).Value = empDepartment;
                cmd.Parameters.Add("@Status", SqlDbType.NVarChar).Value = status;
                cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = createdBy;
                cmd.Parameters.Add("@NoticePeriod", SqlDbType.Int).Value = noticePeriod;
                cmd.Parameters.Add("@HOD", SqlDbType.NVarChar).Value = hod;
                cmd.Parameters.Add("@RM", SqlDbType.NVarChar).Value = rm;
              
                cmd.Parameters.Add("@IsHOD", SqlDbType.Int).Value = isHod;
                cmd.Parameters.Add("@IsRM", SqlDbType.Int).Value = isRm;
                cmd.Parameters.Add("@IsHR", SqlDbType.Int).Value = isHr;
                cmd.Parameters.Add("@ERPId", SqlDbType.NVarChar).Value = erpId;
                cmd.Parameters.Add("@Location", SqlDbType.NVarChar).Value = location;

                    DateTime tmp;
                    if (string.IsNullOrWhiteSpace(doj))
                        cmd.Parameters.Add("@DOJ", SqlDbType.Date).Value = DBNull.Value;
                    else if (DateTime.TryParseExact(doj, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out tmp) ||
                             DateTime.TryParseExact(doj, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out tmp) ||
                             DateTime.TryParse(doj, out tmp))
                        cmd.Parameters.Add("@DOJ", SqlDbType.Date).Value = tmp.Date;
                    else
                        cmd.Parameters.Add("@DOJ", SqlDbType.Date).Value = DBNull.Value;

                    if (string.IsNullOrWhiteSpace(dob))
                        cmd.Parameters.Add("@DOB", SqlDbType.Date).Value = DBNull.Value;
                    else if (DateTime.TryParseExact(dob, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out tmp) ||
                             DateTime.TryParseExact(dob, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out tmp) ||
                             DateTime.TryParse(dob, out tmp))
                        cmd.Parameters.Add("@DOB", SqlDbType.Date).Value = tmp.Date;
                    else
                        cmd.Parameters.Add("@DOB", SqlDbType.Date).Value = DBNull.Value;

                    conn.Open();
                result = cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {

            throw new ApplicationException("Error adding employee", ex);
        }
    }

    return result;
}


    public int AddKRA(string KPI, string Target, float Weightage, string UOM, string CreatedBy)
    {
        try
        {
            string SPName = "AddKRA";
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;
            #region Parameters
            cmd.Parameters.Add("@KPI", SqlDbType.VarChar).Value = KPI;
            cmd.Parameters.Add("@Target", SqlDbType.VarChar).Value = Target;
            cmd.Parameters.Add("@Weightage", SqlDbType.Float).Value = Weightage;
            cmd.Parameters.Add("@UOM", SqlDbType.VarChar).Value = UOM;
            cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = CreatedBy;


            #endregion

            //Get data
            int userList = objSQLDataFile.ExecuteQuery(cmd, SPName);

            return userList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public int UploadSingleKPI(string KPI, string CreatedBy)
    {
        try
        {
            string SPName = "UploadSingleKPI";
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;
            #region Parameters
            cmd.Parameters.Add("@KPI", SqlDbType.VarChar).Value = KPI;
            cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = CreatedBy;


            #endregion

            //Get data
            int userList = objSQLDataFile.ExecuteQuery(cmd, SPName);

            return userList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public int UpdateKRA(string KPI, string Target, float Weightage, string UOM, string ModifiedBy, string id)
    {
        try
        {
            string SPName = "UpdateKRA";
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;
            #region Parameters
            cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = id;
            cmd.Parameters.Add("@KPI", SqlDbType.VarChar).Value = KPI;
            cmd.Parameters.Add("@Target", SqlDbType.VarChar).Value = Target;
            cmd.Parameters.Add("@Weightage", SqlDbType.Float).Value = Weightage;
            cmd.Parameters.Add("@UOM", SqlDbType.VarChar).Value = UOM;
            cmd.Parameters.Add("@ModifiedBy", SqlDbType.VarChar).Value = ModifiedBy;


            #endregion

            //Get data
            int userList = objSQLDataFile.ExecuteQuery(cmd, SPName);

            return userList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public int AddLeave(string EmpCode, string FromDate, string ToDate, string LeaveType, string RM, string Reason, string CreatedDate, string employeeName ,string dayTyped,string halfDaySession,double TotalLeaveDay,double TotalLeaveBalance)
    {
        try
        {
            
            if (string.IsNullOrEmpty(EmpCode) || string.IsNullOrEmpty(FromDate) || string.IsNullOrEmpty(ToDate) ||
                string.IsNullOrEmpty(LeaveType) || string.IsNullOrEmpty(RM) || string.IsNullOrEmpty(Reason) ||
                string.IsNullOrEmpty(CreatedDate) || string.IsNullOrEmpty(employeeName))
            {
                throw new ArgumentException("One or more input parameters are null or empty.");
            }

            DateTime fromDateValue;
            DateTime toDateValue;
            DateTime createdDateValue;

            if (!DateTime.TryParse(FromDate, out fromDateValue) || !DateTime.TryParse(ToDate, out toDateValue) || !DateTime.TryParse(CreatedDate, out createdDateValue))
            {
                throw new ArgumentException("Invalid date format.");
            }

            string SPName = "AddLeaves";

            using (SqlCommand cmd = new SqlCommand(SPName))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                #region Parameters
                cmd.Parameters.Add("@EmpCode", SqlDbType.VarChar).Value = EmpCode;
                cmd.Parameters.Add("@FromDate", SqlDbType.Date).Value = fromDateValue.Date;
                cmd.Parameters.Add("@ToDate", SqlDbType.Date).Value = toDateValue.Date;
                cmd.Parameters.Add("@LeaveType", SqlDbType.VarChar).Value = LeaveType;
                cmd.Parameters.Add("@RM", SqlDbType.VarChar).Value = RM;
                cmd.Parameters.Add("@Reason", SqlDbType.VarChar).Value = Reason;
                cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = createdDateValue;
                cmd.Parameters.Add("@employeeName", SqlDbType.VarChar).Value = employeeName;
                cmd.Parameters.Add("@dayTyped", SqlDbType.VarChar).Value = dayTyped;
                cmd.Parameters.Add("@halfDaySession", SqlDbType.VarChar).Value = halfDaySession;
                cmd.Parameters.Add("@TotalLeaveDay", SqlDbType.Float).Value = TotalLeaveDay;
                cmd.Parameters.Add("@TotalLeaveBalance", SqlDbType.Float).Value = TotalLeaveBalance;
                #endregion

                int result = objSQLDataFile.ExecuteQuery(cmd, SPName);

                return result;
            }
        }
        catch (Exception ex)
        {
        
            Console.WriteLine(ex.Message);
            throw;
        }
    }




    public void CheckIfKPIExists(string KPI, float Target, float Weightage, string UOM, string CreatedBy)
    {
        //removed first condition which will check all values like target weightage uom 
        //old query
       // DataTable dt = objSQLDataFile.ExecuteDataTableSQL("Select * from KRAMaster Where KPI = '" + KPI + "' and Target = '" + Target + "' and Weightage = '" + Weightage + "' and UOM = '" + UOM + "'");
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL("Select * from KRAMaster Where KPI = '" + KPI + "'");
        if (dt.Rows.Count < 1)
        {
            //old condition removed 
            //if ((Target == 0.00 || Target.ToString() == null) && (Weightage == 0.00 || Weightage.ToString() == null) && (UOM == null) )
              //  {
                    AddKRA(KPI, Target.ToString(), Weightage, UOM, CreatedBy);

                //}

        }
    }

    public int CheckDuplicateKPIsUW(string EmpId, string KPI, float Target, float Weightage, string UOM)
    {
        int res = 0;
        //old query
        //DataTable dt = objSQLDataFile.ExecuteDataTableSQL("select * from [dbo].[KRAUserwise] where EmpId= '" + EmpId + "' and KPI = '" + KPI + "'" +
        //                                                    " and Target = '" + Target + "' and Weightage ='" + Weightage + "' and UOM ='" + UOM + "'");

       
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL("select * from [dbo].[KRAUserwise] where EmpId = '" + EmpId + "' and KPI = '" + KPI + "' and Year = YEAR(GETDATE())");
        if (dt.Rows.Count > 0)
        {

            bool isDuplicate = false;
            foreach (DataRow row in dt.Rows)
            {
                float targetValue = 0;
                float.TryParse(row["Target"].ToString(), out targetValue);
                float weightageValue = 0;
                float.TryParse(row["Weightage"].ToString(), out weightageValue);

                if (targetValue == 0 && weightageValue == 0 && string.IsNullOrEmpty(row["UOM"].ToString()))
                {
                    isDuplicate = true;
                    break;
                }
            }

            if (!isDuplicate)
            {
                res = 1;
            }
           
         /* if ((Target == 0.00 || Target.ToString() == null) && (Weightage == 0.00 || Weightage.ToString() == null) && (UOM == null))
            {
                res = 1;
            }*/
                
        }

        return res;

    }

    public int AddAssetChildClearance(string resgId, string assetName, string returnedBack, string returnedToWhom)
    {
       
        try
        {
            string SPName = "AddAssetChildClearance";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;

            #region Paramteres
            cmd.Parameters.Add("@ResignationId", SqlDbType.Int).Value = resgId;
            cmd.Parameters.Add("@AssetName", SqlDbType.VarChar).Value = assetName;
            cmd.Parameters.Add("@ReturnBackStatus", SqlDbType.VarChar).Value = returnedBack;
            cmd.Parameters.Add("@ReturnedToWhom", SqlDbType.VarChar).Value = returnedToWhom;
            //cmd.Parameters.Add("@amount_if_payable", SqlDbType.Float).Value = balanceStatus;

            #endregion

            int userList = objSQLDataFile.ExecuteQuery(cmd, SPName);
            return userList;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    public int AddDistributorChildClearance(string resgId, string distName, string noc, string balance)
    {
        try
        {
            string SPName = "AddDistributorChildClearance";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;

            #region Paramteres
            cmd.Parameters.Add("@ResignationId", SqlDbType.Int).Value = resgId;
            cmd.Parameters.Add("@DistributorName", SqlDbType.VarChar).Value = distName;
            cmd.Parameters.Add("@IsNOCRecieved", SqlDbType.VarChar).Value = noc;
            cmd.Parameters.Add("@BalanceStatus", SqlDbType.VarChar).Value = balance;
            //cmd.Parameters.Add("@amount_if_payable", SqlDbType.Float).Value = balanceStatus;

            #endregion

            int userList = objSQLDataFile.ExecuteQuery(cmd, SPName);
            return userList;
        }
        catch (Exception ex)
        {
            throw ex;
        }


    }

    public int AddKRAUW(string EmpId, string KPI, float Target, float Weightage, string UOM, string CreatedBy, float Achievement = 0)
    {

        CheckIfKPIExists(KPI, Target, Weightage, UOM, CreatedBy);

        int res = CheckDuplicateKPIsUW(EmpId, KPI, Target, Weightage, UOM);

        if (res == 0)
        {

            try
            {
                string SPName = "AddKRAUW";
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = SPName;
                cmd.CommandType = CommandType.StoredProcedure;
                #region Parameters
                cmd.Parameters.Add("@EmpId", SqlDbType.VarChar).Value = EmpId;
                cmd.Parameters.Add("@KPI", SqlDbType.VarChar).Value = KPI;
                cmd.Parameters.Add("@Target", SqlDbType.Int).Value = Target;
                cmd.Parameters.Add("@Weightage", SqlDbType.Float).Value = Weightage;
                cmd.Parameters.Add("@UOM", SqlDbType.VarChar).Value = UOM;
                cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = CreatedBy;
                cmd.Parameters.Add("@Achievement", SqlDbType.Int).Value = Achievement;


                #endregion

                //Get data
                int userList = objSQLDataFile.ExecuteQuery(cmd, SPName);

                return userList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        else
        {
            res = -1;
            return res;
        }

    }
    public int checkDuplicateSkillsUW(string EmpId, string Competency, string ManagementSkills, string Description)
    {
        int res = 0;
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL("select * from [dbo].[SkillsUserWise] where EmpId= '" + EmpId + "' and Competency = '" + Competency + "'" +
                                                            " and ManagementSkills = '" + ManagementSkills + "' and Description ='" + Description + "'");
        if (dt.Rows.Count > 0)
        {
            res = 1;
        }

        return res;
    }

    //added to check skill is present in skill table or not before assigning to employee via importing excel 
    //if not present then first it will add that skill to skills table and then assigned to employee.
    public void CheckSkillPresentOrNot(string Competency, string ManagementSkills, string Description, string createdBy)
    {
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL("Select * from Skills Where Competency = '" + Competency + "' and ManagementSkills = '" + ManagementSkills + "' and Description = '" + Description + "'");
        if (dt.Rows.Count < 1)
        {
            AddSkills(Competency, ManagementSkills, Description, createdBy);

        }

    }
    public int AddSkillsUW(string EmpId, string Competency, string ManagementSkills, string Description, string CreatedBy)
    {
        CheckSkillPresentOrNot(Competency, ManagementSkills, Description, CreatedBy);

        int res = checkDuplicateSkillsUW(EmpId, Competency, ManagementSkills, Description);

        if (res == 0)
        {

            try
            {
                string SPName = "AddSkillsUW";
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = SPName;
                cmd.CommandType = CommandType.StoredProcedure;
                #region Parameters
                cmd.Parameters.Add("@EmpId", SqlDbType.VarChar).Value = EmpId;
                cmd.Parameters.Add("@Competency", SqlDbType.VarChar).Value = Competency;
                cmd.Parameters.Add("@ManagementSkills", SqlDbType.VarChar).Value = ManagementSkills;
                cmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = Description;
                cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = CreatedBy;



                #endregion

                //Get data
                int userList = objSQLDataFile.ExecuteQuery(cmd, SPName);

                return userList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        else
        {
            res = -1;
            return res;
        }
    }
    public int AddSurveyUW(int SurveyId, int EmpId, string CreatedBy)
    {
        try
        {
            string SPName = "AddSurveyUW";
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;
            #region Parameters
            cmd.Parameters.Add("@SurveyId", SqlDbType.Int).Value = SurveyId;
            cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = EmpId;
            cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = CreatedBy;



            #endregion

            //Get data
            int userList = objSQLDataFile.ExecuteQuery(cmd, SPName);

            return userList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public int AddUser(string UserType, string EmailId, string Password, string Status, string CreatedBy, int EmpId)
    {
        int userList = 0;
        int check = checkDuplicate("", EmailId);
        if (check == 0)
        {
            try
            {
                string SPName = "AddUser";
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = SPName;
                cmd.CommandType = CommandType.StoredProcedure;
                #region Parameters
                cmd.Parameters.Add("@UserType", SqlDbType.VarChar).Value = UserType;
                cmd.Parameters.Add("@EmailId", SqlDbType.VarChar).Value = EmailId;
                cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = Password;
                cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = Status;
                cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = CreatedBy;
                cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = EmpId;

                #endregion

                //Get data
                userList = objSQLDataFile.ExecuteQuery(cmd, SPName);

                return userList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        else
        {
            return userList;
        }

    }
    public int AddSkills(string Competency, string ManagementSkills, string Description, string CreatedBy)
    {
        try
        {
            string SPName = "AddSkills";
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;
            #region Parameters
            cmd.Parameters.Add("@Competency", SqlDbType.VarChar).Value = Competency;
            cmd.Parameters.Add("@ManagementSkills", SqlDbType.VarChar).Value = ManagementSkills;
            cmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = Description;
            //cmd.Parameters.Add("@EmpEmailId", SqlDbType.VarChar).Value = EmpEmailId;
            cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = CreatedBy;

            #endregion

            //Get data
            int userList = objSQLDataFile.ExecuteQuery(cmd, SPName);

            return userList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public int AddIntvwSchedule(int Id, string InterviewStatus, string Summary, string Interviewer, string HR)
    {
        try
        {
            string SPName = "AddIntvwSchedule";
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;
            #region Parameters
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Id;
            cmd.Parameters.Add("@InterviewStatus", SqlDbType.VarChar).Value = InterviewStatus;
            cmd.Parameters.Add("@Summary", SqlDbType.VarChar).Value = Summary;
            cmd.Parameters.Add("@Interviewer", SqlDbType.VarChar).Value = Interviewer;
            cmd.Parameters.Add("@HR", SqlDbType.VarChar).Value = HR;

            #endregion

            //Get data
            int userList = objSQLDataFile.ExecuteQuery(cmd, SPName);

            return userList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public int AddVacancies(string Department, string Position, int NoOfVC, string HQ, string CreatedBy, string ZONE, string SDM)
    {
        int res = checkDuplicateVacancies(Position, Department, ZONE, SDM, HQ);
        if (res == 0)
        {
            try
            {
                string SPName = "AddVacancies";
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = SPName;
                cmd.CommandType = CommandType.StoredProcedure;
                #region Parameters
                cmd.Parameters.Add("@Department", SqlDbType.VarChar).Value = Department;
                cmd.Parameters.Add("@Position", SqlDbType.VarChar).Value = Position;
                cmd.Parameters.Add("@NoOfVC", SqlDbType.Int).Value = NoOfVC;
                cmd.Parameters.Add("@HQ", SqlDbType.VarChar).Value = HQ;
                cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = CreatedBy;
                cmd.Parameters.Add("@ZONE", SqlDbType.VarChar).Value = ZONE;
                cmd.Parameters.Add("@SDM", SqlDbType.VarChar).Value = SDM;

                #endregion

                //Get data
                int userList = objSQLDataFile.ExecuteQuery(cmd, SPName);

                return userList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        else
        {
            res = -1;
            return res;
        }

    }
    public int UpdateVacancies(int Id, string Department, string Position, int NoOfVC, string HQ, string ZONE, string SDM)
    {
        try
        {
            string SPName = "UpdateVacancies";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;
            #region parameters
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Id;
            cmd.Parameters.Add("@Department", SqlDbType.VarChar).Value = Department;
            cmd.Parameters.Add("@Position", SqlDbType.VarChar).Value = Position;
            cmd.Parameters.Add("@NoOfVC", SqlDbType.Int).Value = NoOfVC;
            cmd.Parameters.Add("@HQ", SqlDbType.VarChar).Value = HQ;
            cmd.Parameters.Add("@ZONE", SqlDbType.VarChar).Value = ZONE;
            cmd.Parameters.Add("@SDM", SqlDbType.VarChar).Value = SDM;
            #endregion
            int userList = objSQLDataFile.ExecuteQuery(cmd, SPName);

            return userList;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
   
    public int UpdateUser(int Id, string UserType, string EmailId, string Status)
    {
        try
        {
            string SPName = "UpdateUser";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;
            #region parameters
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Id;
            cmd.Parameters.Add("@UserType", SqlDbType.VarChar).Value = UserType;
            cmd.Parameters.Add("@EmailId", SqlDbType.VarChar).Value = EmailId;
            cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = Status;

            #endregion
            int userList = objSQLDataFile.ExecuteQuery(cmd, SPName);

            return userList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public string GetDepartmentByEmail(string loggedInUserEmail)
    {
        string deptName = "";

        if (loggedInUserEmail != "")
        {

            DataTable res = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT department_name from [dbo].[department_head] WHERE department_head_email = '" + loggedInUserEmail + "'");
            if (res.Rows.Count > 0)
            {
                DataRow row = res.Rows[0];
                deptName = row["department_name"].ToString();
            }
        }
        return deptName;

    }

    public int UpdateSkills(int Id, string Competency, string ManagementSkills, string Description, string EmpEmailId = "")
    {

        try
        {
            string SPName = "UpdateSkills";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;
            #region parameters
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Id;
            cmd.Parameters.Add("@Competency", SqlDbType.VarChar).Value = Competency;
            cmd.Parameters.Add("@ManagementSkills", SqlDbType.VarChar).Value = ManagementSkills;
            cmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = Description;
           // cmd.Parameters.Add("@EmpEmailId", SqlDbType.VarChar).Value = EmpEmailId;

            #endregion
            int userList = objSQLDataFile.ExecuteQuery(cmd, SPName);

            return userList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public int AddJobApplication(string FName, string MName, string LName, string MobNo, string EmailId, int VacancyId,
        string FileName, string FileSize, string FilePath, string FileExt, string RefferedBy, string RefferedPerson)
    {
        try
        {
            string SPName = "AddJobApplication";
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;
            #region Parameters
            cmd.Parameters.Add("@FName", SqlDbType.VarChar).Value = FName;
            cmd.Parameters.Add("@MName", SqlDbType.VarChar).Value = MName;
            cmd.Parameters.Add("@LName", SqlDbType.VarChar).Value = LName;
            cmd.Parameters.Add("@MobNo", SqlDbType.VarChar).Value = MobNo;
            cmd.Parameters.Add("@EmailId", SqlDbType.VarChar).Value = EmailId;
            cmd.Parameters.Add("@VacancyId", SqlDbType.Int).Value = VacancyId;
            cmd.Parameters.Add("@FileName", SqlDbType.VarChar).Value = FileName;
            cmd.Parameters.Add("@FileSize", SqlDbType.VarChar).Value = FileSize;
            cmd.Parameters.Add("@FilePath", SqlDbType.VarChar).Value = FilePath;
            cmd.Parameters.Add("@FileExt", SqlDbType.VarChar).Value = FileExt;
            cmd.Parameters.Add("@RefferedBy", SqlDbType.VarChar).Value = RefferedBy;
            cmd.Parameters.Add("@RefferedPerson", SqlDbType.VarChar).Value = RefferedPerson;

            #endregion

            //Get data
            int userList = objSQLDataFile.ExecuteQuery(cmd, SPName);

            return userList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public int AddConsultancy(string ConsName, string EmailId, string Location, string CreatedBy, string dltype, string status = "")
    {
        try
        {
            string SPName = "AddConsultancy";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;
            #region Parameters
            cmd.Parameters.Add("@ConsName", SqlDbType.VarChar).Value = ConsName;
            cmd.Parameters.Add("@EmailId", SqlDbType.VarChar).Value = EmailId;
            cmd.Parameters.Add("@Location", SqlDbType.VarChar).Value = Location;
            cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = CreatedBy;
            #endregion
            int userlist = objSQLDataFile.ExecuteQuery(cmd, SPName);

            if (userlist > 0)
            {
                DataTable res = objSQLDataFile.ExecuteDataTableSQL(@"SELECT DISTINCT Id from ConsultancyMaster where EmailId = '" + EmailId + "'");
                int empId = 0;
                if (res.Rows.Count > 0)
                {
                    DataRow row = res.Rows[0];
                    empId = int.Parse(row["Id"].ToString());
                }
                AddUser(dltype, EmailId, "111", status, CreatedBy, empId);
            }
            return userlist;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public int AddAssessment(int CandidateId, string RatingType, string Appearance, string JobKnowledge,
        string RelavantExp, string Matuarity, string CommSkills, string HRFeedback, string IntFeedback,
        string FinalResult, string PositivePoints, string NegativePoints)
    {
        try
        {
            string SPName = "AddAssessment";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;
            #region Parameters
            cmd.Parameters.Add("@CandidateId", SqlDbType.Int).Value = CandidateId;
            cmd.Parameters.Add("@RatingType", SqlDbType.VarChar).Value = RatingType;
            cmd.Parameters.Add("@Appearance", SqlDbType.VarChar).Value = Appearance;
            cmd.Parameters.Add("@JobKnowledge", SqlDbType.VarChar).Value = JobKnowledge;
            cmd.Parameters.Add("@RelavantExp", SqlDbType.VarChar).Value = RelavantExp;
            cmd.Parameters.Add("@Matuarity", SqlDbType.VarChar).Value = Matuarity;
            cmd.Parameters.Add("@CommSkills", SqlDbType.VarChar).Value = CommSkills;
            cmd.Parameters.Add("@HRFeedback", SqlDbType.VarChar).Value = HRFeedback;
            cmd.Parameters.Add("@IntFeedback", SqlDbType.VarChar).Value = IntFeedback;
            cmd.Parameters.Add("@FinalResult", SqlDbType.VarChar).Value = FinalResult;
            cmd.Parameters.Add("@PositivePoints", SqlDbType.VarChar).Value = PositivePoints;
            cmd.Parameters.Add("@NegativePoints", SqlDbType.VarChar).Value = NegativePoints;
            #endregion
            int userlist = objSQLDataFile.ExecuteQuery(cmd, SPName);
            return userlist;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

   
    public string GetResigningEmployeeRmEmail(string assetEmpId)
    {
        string rmEmail = string.Empty;

        DataTable res = objSQLDataFile.ExecuteDataTableSQL("SELECT RM FROM [dbo].[Resignations] WHERE EmpId = '" + assetEmpId + "' AND IsWithdrawn = 0");
        if (res.Rows.Count > 0)
        {
            DataRow row = res.Rows[0];
            string rmName = row["RM"].ToString();

            DataTable rmData = objSQLDataFile.ExecuteDataTableSQL("SELECT EmpEmailId FROM [dbo].[EmployeeMaster] WHERE EmpName = '" + rmName + "'");
            if (rmData.Rows.Count > 0)
            {
                DataRow rmRow = rmData.Rows[0];
                rmEmail = rmRow["EmpEmailId"].ToString();
            }
        }

        return rmEmail;
    }
    public string GetEmployeeEmailByResignationId(int resignationId)
    {
        string email = string.Empty;
        DataTable dt = objSQLDataFile.ExecuteDataTableSQL("SELECT emp.EmpEmailId,resg.DatenTime FROM Resignations resg JOIN EmployeeMaster emp ON resg.EmpId = emp.Id WHERE resg.ResignationId = " + resignationId);

        if(dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            email = row["EmpEmailId"].ToString();
        }

        return email;
    }
    public string GetEmployeeEmailAndResignationDateByResignationId(int resignationId)
    {
        string emailAndDate = string.Empty;

        DataTable dt = objSQLDataFile.ExecuteDataTableSQL("SELECT emp.EmpEmailId, emp.NoticePeriod, emp.EmpName, resg.DatenTime FROM Resignations resg JOIN EmployeeMaster emp ON resg.EmpId = emp.Id WHERE resg.ResignationId = " + resignationId);

        if (dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            string employeeEmail = row["EmpEmailId"].ToString();
            string empname = row["EmpName"].ToString();
            DateTime resignationDate = Convert.ToDateTime(row["DatenTime"]);
            string resignationDateString = resignationDate.ToString("dd-MM-yyyy"); // Convert DateTime to string in "yyyy-MM-dd" format
            int noticePeriod = Convert.ToInt32(row["noticePeriod"]);

            emailAndDate = empname + "|" + employeeEmail + "|" + resignationDateString + "|" + noticePeriod;
        }

        return emailAndDate;
    }
    public int UpdateAddAssessment(int CandidateId, string RatingType, string Appearance, string JobKnowledge,
       string RelavantExp, string Matuarity, string CommSkills, string HRFeedback, string IntFeedback,
       string FinalResult, string PositivePoints, string NegativePoints)
    {
        try
        {
            string SPName = "UpdateAddAssessment";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;
            #region Parameters
            cmd.Parameters.Add("@CandidateId", SqlDbType.Int).Value = CandidateId;
            cmd.Parameters.Add("@RatingType", SqlDbType.VarChar).Value = RatingType;
            cmd.Parameters.Add("@Appearance", SqlDbType.VarChar).Value = Appearance;
            cmd.Parameters.Add("@JobKnowledge", SqlDbType.VarChar).Value = JobKnowledge;
            cmd.Parameters.Add("@RelavantExp", SqlDbType.VarChar).Value = RelavantExp;
            cmd.Parameters.Add("@Matuarity", SqlDbType.VarChar).Value = Matuarity;
            cmd.Parameters.Add("@CommSkills", SqlDbType.VarChar).Value = CommSkills;
            cmd.Parameters.Add("@HRFeedback", SqlDbType.VarChar).Value = HRFeedback;
            cmd.Parameters.Add("@IntFeedback", SqlDbType.VarChar).Value = IntFeedback;
            cmd.Parameters.Add("@FinalResult", SqlDbType.VarChar).Value = FinalResult;
            cmd.Parameters.Add("@PositivePoints", SqlDbType.VarChar).Value = PositivePoints;
            cmd.Parameters.Add("@NegativePoints", SqlDbType.VarChar).Value = NegativePoints;
            #endregion
            int userlist = objSQLDataFile.ExecuteQuery(cmd, SPName);
            return userlist;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public int AddCandidateDocs(int CandidateId, string TenthCert, string TwevleCert, string GraduationCert,
        string ExpCert, string SalarySlip, string DrivingLicence, string AadharCard)
    {
        try
        {
            string SPName = "AddCandidateDocs";
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;
            #region Parameters
            cmd.Parameters.Add("@CandidateId", SqlDbType.Int).Value = CandidateId;
            cmd.Parameters.Add("@TenthCert", SqlDbType.VarChar).Value = TenthCert;
            cmd.Parameters.Add("@TwevleCert", SqlDbType.VarChar).Value = TwevleCert;
            cmd.Parameters.Add("@GraduationCert", SqlDbType.VarChar).Value = GraduationCert;
            cmd.Parameters.Add("@AadharCard", SqlDbType.VarChar).Value = AadharCard;
            cmd.Parameters.Add("@ExpCert", SqlDbType.VarChar).Value = ExpCert;
            cmd.Parameters.Add("@SalarySlip", SqlDbType.VarChar).Value = SalarySlip;
            cmd.Parameters.Add("@DrivingLicence", SqlDbType.VarChar).Value = DrivingLicence;

            #endregion

            //Get data
            int userList = objSQLDataFile.ExecuteQuery(cmd, SPName);

            return userList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public int AddHiddenVacancy(int EmpId, int VacancyId, string CreatedBy)
    {
        int userList = 0;
        int res = checkDuplicateHiddenVacancy(EmpId, VacancyId);
        if (res == 0)
        {
            try
            {
                string SPName = "AddHiddenVacancy";
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = SPName;
                cmd.CommandType = CommandType.StoredProcedure;
                #region Parameters
                cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = EmpId;
                cmd.Parameters.Add("@VacancyId", SqlDbType.Int).Value = VacancyId;
                cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = CreatedBy;


                #endregion

                //Get data
                userList = objSQLDataFile.ExecuteQuery(cmd, SPName);

                return userList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        else
        {
            return userList;
        }

    }
    public int AddHideSurvey(int EmpId, int SurveyId, string CreatedBy)
    {
        int userList = 0;
        try
        {
            string SPName = "AddHideSurvey";
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;
            #region Parameters
            cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = EmpId;
            cmd.Parameters.Add("@SurveyId", SqlDbType.Int).Value = SurveyId;
            cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = CreatedBy;


            #endregion

            //Get data
            userList = objSQLDataFile.ExecuteQuery(cmd, SPName);

            return userList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public int AddQuery(int EmpId, string EmpName, string Query, string Subject, string Status, string FileName = "", string FilePath = "")
    {
        try
        {
            string SPName = "AddQuery";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;

            #region Parameters
            cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = EmpId;
            cmd.Parameters.Add("@EmpName", SqlDbType.VarChar).Value = EmpName;
            cmd.Parameters.Add("@Query", SqlDbType.NVarChar).Value = Query;
            cmd.Parameters.Add("@Subject", SqlDbType.VarChar).Value = Subject;
            cmd.Parameters.Add("@FileName", SqlDbType.VarChar).Value = FileName;
            cmd.Parameters.Add("@FilePath", SqlDbType.VarChar).Value = FilePath;
            cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = Status;


            #endregion
            int userList = objSQLDataFile.ExecuteQuery(cmd, SPName);

            return userList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public int AddQueryReply(int QueryId, string ReplyQuery, string RepliedByName, int RepliedById, string Status)
    {
        try
        {
            string SPName = "AddQueryReply";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;

            #region Parameters
            cmd.Parameters.Add("@QueryId", SqlDbType.Int).Value = QueryId;
            cmd.Parameters.Add("@ReplyQuery", SqlDbType.NVarChar).Value = ReplyQuery;
            cmd.Parameters.Add("@RepliedByName", SqlDbType.VarChar).Value = RepliedByName;
            cmd.Parameters.Add("@RepliedById", SqlDbType.Int).Value = RepliedById;
            cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = Status;


            #endregion
            int userList = objSQLDataFile.ExecuteQuery(cmd, SPName);

            return userList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public int AddResignations(int EmpId, string ErpId, string Reason, string Details, string RM, string HOD, DateTime DatenTime)
    {
        int userList = 0;
        int res = checkDuplicateResignEntry(EmpId);
        if (res == 0)
        {
            try
            {
                string SPName = "AddResignations";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = SPName;
                cmd.CommandType = CommandType.StoredProcedure;

                #region Paramteres
                cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = EmpId;
                cmd.Parameters.Add("@ERPId", SqlDbType.VarChar).Value = ErpId;
                cmd.Parameters.Add("@Reason", SqlDbType.VarChar).Value = Reason;
                cmd.Parameters.Add("@Details", SqlDbType.NVarChar).Value = Details;
                cmd.Parameters.Add("@RM", SqlDbType.VarChar).Value = RM;
                cmd.Parameters.Add("@HOD", SqlDbType.VarChar).Value = HOD;
                cmd.Parameters.Add("@DatenTime", SqlDbType.DateTime).Value = DatenTime;
                
                #endregion

                userList = objSQLDataFile.ExecuteQuery(cmd, SPName);
                return userList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        else
        {
            return userList;
        }

    }

    //public int AddClearanceForm(int ResignationId,int EmpId,string DateofResignation, string DateofRelivieng, string ThankingSignIMGName, string ThankingSignIMGPath, string ThankingName, 
    //    string ThankingDate, string AssetName="", string ReturnBackStatus="", string ReturnedToWhom = "", string DistributorName = "", string NOCRecieved = "",
    //    string BalanceNillOrPayable = "", int IsSubmitted=0)
    //{
    //    try
    //    {
    //        string SPName = "AddClearanceForm";
    //        SqlCommand cmd = new SqlCommand();
    //        cmd.CommandText = SPName;
    //        cmd.CommandType = CommandType.StoredProcedure;

    //        #region Paramteres
    //        cmd.Parameters.Add("@EmpResignationIdId", SqlDbType.Int).Value = ResignationId;
    //        cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = EmpId;
    //        cmd.Parameters.Add("@DateofResignation", SqlDbType.VarChar).Value = DateofResignation;
    //        cmd.Parameters.Add("@DateofRelivieng", SqlDbType.VarChar).Value = DateofRelivieng;
    //        cmd.Parameters.Add("@AssetName", SqlDbType.VarChar).Value = AssetName;
    //        cmd.Parameters.Add("@ReturnBackStatus", SqlDbType.VarChar).Value = ReturnBackStatus;
    //        cmd.Parameters.Add("@ReturnedToWhom", SqlDbType.VarChar).Value = ReturnedToWhom;
    //        cmd.Parameters.Add("@DistributorName", SqlDbType.VarChar).Value = DistributorName;
    //        cmd.Parameters.Add("@NOCRecieved", SqlDbType.VarChar).Value = NOCRecieved;
    //        cmd.Parameters.Add("@BalanceNillOrPayable", SqlDbType.VarChar).Value = BalanceNillOrPayable;
    //        cmd.Parameters.Add("@ThankingSignIMGName", SqlDbType.VarChar).Value = ThankingSignIMGName;
    //        cmd.Parameters.Add("@ThankingSignIMGPath", SqlDbType.VarChar).Value = ThankingSignIMGPath;
    //        cmd.Parameters.Add("@ThankingName", SqlDbType.VarChar).Value = ThankingName;
    //        cmd.Parameters.Add("@ThankingDate", SqlDbType.VarChar).Value = ThankingDate;
    //        cmd.Parameters.Add("@IsSubmitted", SqlDbType.Bit).Value = IsSubmitted;
    //        #endregion

    //        int userList = objSQLDataFile.ExecuteQuery(cmd, SPName);
    //        return userList;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    
    public int AddClearanceForm(int ResignationId, int EmpId, string ErpId, string DateofResignation, string DateofRelivieng, string ThankingSignIMGName, string ThankingSignIMGPath, string ThankingName,
        string ThankingDate, int IsSubmitted = 0)
    {
        int userList = 0;
        ////check for duplicate clearanceform entry of same employee in clearance table
        //int check = checkDuplicateClearanceEntry(EmpId, ResignationId);
        //if (check == 0)
        //{

            try
            {
                string SPName = "AddClearanceForm";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = SPName;
                cmd.CommandType = CommandType.StoredProcedure;

                #region Paramteres
                cmd.Parameters.Add("@ResignationId", SqlDbType.Int).Value = ResignationId;
                cmd.Parameters.Add("@EmpId", SqlDbType.VarChar).Value = EmpId;
                cmd.Parameters.Add("@ERPId", SqlDbType.VarChar).Value = ErpId;
                cmd.Parameters.Add("@DateofResignation", SqlDbType.VarChar).Value = DateofResignation;
                cmd.Parameters.Add("@DateofRelivieng", SqlDbType.VarChar).Value = DateofRelivieng;
                cmd.Parameters.Add("@ThankingSignIMGName", SqlDbType.VarChar).Value = ThankingSignIMGName;
                cmd.Parameters.Add("@ThankingSignIMGPath", SqlDbType.VarChar).Value = ThankingSignIMGPath;
                cmd.Parameters.Add("@ThankingName", SqlDbType.VarChar).Value = ThankingName;
                cmd.Parameters.Add("@ThankingDate", SqlDbType.VarChar).Value = ThankingDate;
                cmd.Parameters.Add("@IsSubmitted", SqlDbType.Bit).Value = IsSubmitted;
                #endregion

                userList = objSQLDataFile.ExecuteQuery(cmd, SPName);
                return userList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        //}
        //else
        //{
        //    return userList;
        //}

    }

    public int AddChildClearanceForm(int ResignationId, string AssetName, string ReturnBackStatus, string ReturnedToWhom)
    {
        try
        {
            string SPName = "AddChildClearanceForm";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;

            #region Paramteres
            cmd.Parameters.Add("@ResignationId", SqlDbType.Int).Value = ResignationId;
            cmd.Parameters.Add("@AssetName", SqlDbType.VarChar).Value = AssetName;
            cmd.Parameters.Add("@ReturnBackStatus", SqlDbType.VarChar).Value = ReturnBackStatus;
            cmd.Parameters.Add("@ReturnedToWhom", SqlDbType.VarChar).Value = ReturnedToWhom;
            #endregion

            int userList = objSQLDataFile.ExecuteQuery(cmd, SPName);
            return userList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public int AddBlankKPI(int Id, string Target, float Weightage, string UOM, float Achievment)
    {
        try
        {
            string SPName = "AddBlankKPI";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;

            #region Paramteres
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Id;
            cmd.Parameters.Add("@Target", SqlDbType.VarChar).Value = Target;
            cmd.Parameters.Add("@Weightage", SqlDbType.VarChar).Value = Weightage;
            cmd.Parameters.Add("@UOM", SqlDbType.VarChar).Value = UOM;
            cmd.Parameters.Add("@Achievement", SqlDbType.VarChar).Value = Achievment;
            #endregion

            int userList = objSQLDataFile.ExecuteQuery(cmd, SPName);
            return userList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public int ImportIdealRating(int Id, int IdealRating, int ConsentRating)
    {
        try
        {
            string SPName = "ImportIdealRating";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;

            #region Paramteres
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Id;
            cmd.Parameters.Add("@IdealRating", SqlDbType.Int).Value = IdealRating;
            cmd.Parameters.Add("@ConsentRating", SqlDbType.Int).Value = ConsentRating;
            #endregion

            int userList = objSQLDataFile.ExecuteQuery(cmd, SPName);
            return userList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public int SCKPIAvg(int EmpId, int JCType, string Year)
    {
        try
        {
            string SPName = "SCKPIAvg";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;

            #region Paramteres
            cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = EmpId;
            cmd.Parameters.Add("@JCType", SqlDbType.Int).Value = JCType;
            cmd.Parameters.Add("@Year", SqlDbType.VarChar).Value = Year;
            #endregion

            int userList = objSQLDataFile.ExecuteQuery(cmd, SPName);
            return userList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public int SCSkillAVG(int EmpId)
    {
        try
        {
            string SPName = "SCSkillAVG";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;

            #region Paramteres
            cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = EmpId;
            #endregion

            int userList = objSQLDataFile.ExecuteQuery(cmd, SPName);
            return userList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public int AdminScores(int Id, float ORGScore, float DepScore)
    {
        try
        {
            string SPName = "AdminScores";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;

            #region Paramteres
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Id;
            cmd.Parameters.Add("@ORGScore", SqlDbType.Decimal).Value = ORGScore;
            cmd.Parameters.Add("@DepScore", SqlDbType.Decimal).Value = DepScore;
            #endregion

            int userList = objSQLDataFile.ExecuteQuery(cmd, SPName);
            return userList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public int UpdateKPIAVG(int EmpId, int JCType, string year)
    {
        try
        {
            string SPName = "UpdateKPIAVG";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;

            #region Paramteres
            cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = EmpId;
            cmd.Parameters.Add("@JCType", SqlDbType.Int).Value = JCType;
            cmd.Parameters.Add("@year", SqlDbType.VarChar).Value = year;
            #endregion

            int userList = objSQLDataFile.ExecuteQuery(cmd, SPName);
            return userList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public int AddForm16(int EmpId, string PartA, string PartB, string CreatedBy)
    {
        try
        {
            string SPName = "AddForm16";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;

            #region Paramteres
            cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = EmpId;
            cmd.Parameters.Add("@PartA", SqlDbType.NVarChar).Value = PartA;
            cmd.Parameters.Add("@PartB", SqlDbType.NVarChar).Value = PartB;
            cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = CreatedBy;
            #endregion

            int userList = objSQLDataFile.ExecuteQuery(cmd, SPName);
            return userList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public int AddJCKPI(int EmpId, string KPI, string CreatedBy, string JCType)
    {
        try
        {
            string SPName = "AddJCKPI";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;

            #region Paramteres
            cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = EmpId;
            cmd.Parameters.Add("@KPI", SqlDbType.VarChar).Value = KPI;
            cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = CreatedBy;
            cmd.Parameters.Add("@JCType", SqlDbType.VarChar).Value = JCType;
            #endregion

            int userList = objSQLDataFile.ExecuteQuery(cmd, SPName);
            return userList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public int AddJCKPIDepartmentWise(string Department, string CreatedBy, int JCType, int OfJC)
    {
        try
        {
            string SPName = "AddJCKPIDepartmentWise";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;

            #region Paramteres
            cmd.Parameters.Add("@Department", SqlDbType.VarChar).Value = Department;
            cmd.Parameters.Add("@JCType", SqlDbType.Int).Value = JCType;
            cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = CreatedBy;
            cmd.Parameters.Add("@OfJC", SqlDbType.Int).Value = OfJC;
            #endregion

            int userList = objSQLDataFile.ExecuteQuery(cmd, SPName);
            return userList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public int AddIncrementLetter(int EmpId, string IncrementLetter, string CreatedBy)
    {
        try
        {
            string SPName = "AddIncrementLetter";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;

            #region Paramteres
            cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = EmpId;
            cmd.Parameters.Add("@IncrementLetter", SqlDbType.NVarChar).Value = IncrementLetter;
            cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = CreatedBy;
            #endregion

            int userList = objSQLDataFile.ExecuteQuery(cmd, SPName);
            return userList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public int AddESICLetter(int EmpId,string ESICLetter,string CreatedBy) 
    {
        try
        {
            string SPName = "AddESICLetter";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SPName;
            #region Parameters
            cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = EmpId;
            cmd.Parameters.Add("@ESICLetter", SqlDbType.NVarChar).Value = ESICLetter;
            cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = CreatedBy;
            #endregion

            int userList = objSQLDataFile.ExecuteQuery(cmd, SPName);
            return userList;
        }
        catch(Exception ex) 
        {      
            throw ex;
        }
    }

    public int AddOrUpdateAssetNocChildClearance(int id, int resgId, string assetName, string returnedBack, string returnedToWhom)
    {
        try
        {
            string SPName = "AddOrUpdateAssetNocChildClearance";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;

            #region Paramteres
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            cmd.Parameters.Add("@ResignationId", SqlDbType.Int).Value = resgId;
            cmd.Parameters.Add("@AssesstName", SqlDbType.VarChar).Value = assetName;
            cmd.Parameters.Add("@ReturnBackStatus", SqlDbType.VarChar).Value = returnedBack;
            cmd.Parameters.Add("@ReturnedToWhom", SqlDbType.VarChar).Value = returnedToWhom;
            #endregion

            int userList = objSQLDataFile.ExecuteQuery(cmd, SPName);
            return userList;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    public int addOrUpdateDistributorChildClearance(int id, int resgId, string distName, string noc, string desc, string balance, string funocimg)
    {
        try
        {
            string SPName = "AddOrUpdateDistributorChildClearance";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;

            #region Parameters
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            cmd.Parameters.Add("@ResignationId", SqlDbType.Int).Value = resgId;
            cmd.Parameters.Add("@DistributorName", SqlDbType.VarChar).Value = distName;
            cmd.Parameters.Add("@IsNOCRecieved", SqlDbType.VarChar).Value = noc;
            cmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = desc;
            cmd.Parameters.Add("@BalanceStatus", SqlDbType.VarChar).Value = balance;
            cmd.Parameters.Add("@DistributorNocClearanceImg", SqlDbType.NVarChar).Value = funocimg;
           
            #endregion

            int userList = objSQLDataFile.ExecuteQuery(cmd, SPName);
            return userList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public int AddOrUpdateDeptNocChildClearance(int id, int resgId, string deptName, string description, string nocRecieved, float balanceStatus)
    {
        try
        {
            string SPName = "AddOrUpdateDeptNocChildClearance";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;

            #region Paramteres
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            cmd.Parameters.Add("@ResignationId", SqlDbType.Int).Value = resgId;
            cmd.Parameters.Add("@department_name", SqlDbType.VarChar).Value = deptName;
            cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = description;
            cmd.Parameters.Add("@is_noc_recieved", SqlDbType.VarChar).Value = nocRecieved;
            cmd.Parameters.Add("@amount_if_payable", SqlDbType.Float).Value = balanceStatus;

            #endregion

            int userList = objSQLDataFile.ExecuteQuery(cmd, SPName);
            return userList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public int AddOrUpdatePayregisterEmployees(string erpid, string emp_abbr, string employee_name, string emp_email_id, string department, string hod_name, string hod_email, string increment_letter_path, int is_mail_sent)
    {
        try
        {
            string SPName = "AddOrUpdatePayregisterEmployees";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;

            #region Paramteres
            // cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            cmd.Parameters.Add("@erp_id", SqlDbType.VarChar).Value = erpid;
            cmd.Parameters.Add("@emp_abbrivation", SqlDbType.VarChar).Value = emp_abbr;
            cmd.Parameters.Add("@employee_name", SqlDbType.VarChar).Value = employee_name;
            cmd.Parameters.Add("@emp_email_id", SqlDbType.VarChar).Value = emp_email_id;
            cmd.Parameters.Add("@department", SqlDbType.VarChar).Value = department;
            cmd.Parameters.Add("@hod_name", SqlDbType.VarChar).Value = hod_name;
            cmd.Parameters.Add("@hod_email", SqlDbType.VarChar).Value = hod_email;
            cmd.Parameters.Add("@increment_letter_path", SqlDbType.VarChar).Value = increment_letter_path;
            cmd.Parameters.Add("@is_mail_sent", SqlDbType.Int).Value = is_mail_sent;

            #endregion

            int userList = objSQLDataFile.ExecuteQuery(cmd, SPName);
            return userList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public int AddEmployeeSignature(int EmpId, string empName, string imgfile)
    {
        int userList = 0;
        try
        {
            string SPName = "AddEmployeeSignature";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;

            #region Paramteres
           
            cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = EmpId;
            cmd.Parameters.Add("@EmpName", SqlDbType.VarChar).Value = empName;
            cmd.Parameters.Add("@SignImg", SqlDbType.NVarChar).Value = imgfile;
           
            #endregion

            userList = objSQLDataFile.ExecuteQuery(cmd, SPName);
            return userList;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    public int AddEmployeePhoto(int EmpId, string empName, string imgfile)
    {
        int userList = 0;
        try
        {
            string SPName = "AddEmployeePhoto";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SPName;
            cmd.CommandType = CommandType.StoredProcedure;

            #region Paramteres

            cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = EmpId;
            cmd.Parameters.Add("@EmpName", SqlDbType.VarChar).Value = empName;
            cmd.Parameters.Add("@PhotoImg", SqlDbType.NVarChar).Value = imgfile;

            #endregion

            userList = objSQLDataFile.ExecuteQuery(cmd, SPName);
            return userList;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

}
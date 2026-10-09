using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using System.Web;

/// <summary>
/// Summary description for SqlOperation
/// </summary>
public class SqlOperation
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    public SqlOperation()
    {
        //
        // TODO: Add constructor logic here
        //


    }


    public int Fn_CHeckUser(String EmailId, String Password)
    {

        int result = -1;
        try
        {

            DataTable dt = objSQLDataFile.ExecuteDataTableSQL("select * from UserMaster  where EmailId='" + EmailId + "' and Password='" + Password + "'");


            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                string status = row["Status"].ToString();
                if (status == "InActive")
                {
                    result = -2;
                }
                else
                {
                    result = 1;

                }
            }
            else
            {
                result = -1;
            }

        }
        catch { }

        return result;
    }



}
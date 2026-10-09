using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Configuration; 
using System.IO;
using System.Reflection; 
/// <summary>
/// Summary description for SQLDataFile
/// </summary>
public class SQLDataFileSIS
{ 
    static string connstr = string.Empty, _EmailID = string.Empty;
       
    // SqlConnection con = new SqlConnection(connstr);
    public SQLDataFileSIS()
    {
       

        connstr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    }

    public int ExecuteQuery(SqlTransaction trans, string strQuery)
    {
        int res = 0;
        try
        {
            SqlConnection con = new SqlConnection(connstr);
            SqlCommand com = new SqlCommand(strQuery, con);
            com.CommandType = CommandType.StoredProcedure;
            res = com.ExecuteNonQuery();
        }
        catch (Exception ex) { }
        return res;
    }

    public bool Execute_Cmd_list( List<SqlCommand> cmdL)
    {
        int intExecuteNonQuery = 0;
        bool bIsSavedFlag = false;
        using (SqlConnection connection = new SqlConnection(connstr))
        {
            connection.Open();
          
            SqlTransaction transaction = connection.BeginTransaction("InsertUpdateDate");
            try
            {
                foreach (SqlCommand cmd in cmdL)
                {
                    cmd.Connection = connection;
                    cmd.Transaction = transaction;
                    intExecuteNonQuery = cmd.ExecuteNonQuery();
                    bIsSavedFlag = true;
                }
                // Attempt to commit the transaction.
                transaction.Commit();
                Console.WriteLine("Both records are written to database.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                Console.WriteLine("  Message: {0}", ex.Message);

                // Attempt to roll back the transaction. 
                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex2)
                {
                    // This catch block will handle any errors that may have occurred 
                    // on the server that would cause the rollback to fail, such as 
                    // a closed connection.
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                }
                return false;
            }
        }
        return bIsSavedFlag;
    }

    public void StartTransation()
    {
        SqlConnection con = new SqlConnection(connstr);
        con.Open();
        SqlTransaction trans = con.BeginTransaction(IsolationLevel.ReadCommitted);
    }

    public void EndTransation()
    {
        SqlConnection con = new SqlConnection(connstr);
        //if(con.

    }

    public int ExecuteQuery(string strQuery)
    {
        int res = 0;
        SqlConnection con = new SqlConnection(connstr);
        try
        {
            SqlCommand com = new SqlCommand(strQuery, con);
            com.CommandType = CommandType.StoredProcedure;
            con.Open();
            res = com.ExecuteNonQuery();
        }
        catch (Exception ex) { }
        finally
        {
            con.Close();
        }

        return res;
    }

    public DataTable ExecuteDataTable(string strQuery)
    {
        DataTable dt = new DataTable();
        SqlConnection con = new SqlConnection(connstr);
        try
        {
            SqlCommand com = new SqlCommand(strQuery, con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(com);
            com.CommandType = CommandType.StoredProcedure;
            da.Fill(dt);
        }
        catch (Exception ex)
        {
            //HelperSIS.SendEmail("", "dnv1987@live.com", "", "", ex.Message, "Simply Sucess " + strQuery, "");
        }
        finally
        {
            con.Close();
        }
        return dt;
    }

    public DataTable ExecuteDataTable(SqlCommand com, string strQuery)
    {
        DataTable dt = new DataTable();
        SqlConnection con = new SqlConnection(connstr);
        try
        {
            //SqlCommand com = new SqlCommand(strQuery, con);
            com.Connection = con;
            con.Open();
            com.CommandText = strQuery;
            SqlDataAdapter da = new SqlDataAdapter(com);
            com.CommandType = CommandType.StoredProcedure;
            da.Fill(dt);
        }
        catch (Exception ex)
        {
            //HelperSIS.SendEmail("", "dnv1987@live.com", "", "", ex.Message, "Simply Sucess " + strQuery, "");
        }
        finally
        {
            con.Close();
        }
        return dt;
    }

    public string ExecuteScalar(SqlCommand com, string strQuery)
    {
        string result = string.Empty;
        SqlConnection con = new SqlConnection(connstr);
        try
        {
            //SqlCommand com = new SqlCommand(strQuery, con);
            com.Connection = con;
            con.Open();
            com.CommandText = strQuery;
            SqlDataAdapter da = new SqlDataAdapter(com);
            com.CommandType = CommandType.StoredProcedure;
            result = Convert.ToString(com.ExecuteScalar());
        }
        catch (Exception ex) { }
        finally
        {
            con.Close();
        }
        return result;
    }

    public static string ExecuteScalarBool(SqlCommand com, string strQuery)
    {
        string result = string.Empty;
        SqlConnection con = new SqlConnection(connstr);
        try
        {
            com.Connection = con;
            con.Open();
            com.CommandText = strQuery;
            SqlDataAdapter da = new SqlDataAdapter(com);
            com.CommandType = CommandType.StoredProcedure;
            result = Convert.ToString(com.ExecuteScalar());
        }
        catch (Exception ex) { }
        finally
        {
            con.Close();
        }
        return result;
    }
    public int ExecuteScalarint(SqlCommand com, string strQuery)
    {
        int result = -1;
        SqlConnection con = new SqlConnection(connstr);
        try
        {
            //SqlCommand com = new SqlCommand(strQuery, con);
            com.Connection = con;
            con.Open();
            com.CommandText = strQuery;
            SqlDataAdapter da = new SqlDataAdapter(com);
            com.CommandType = CommandType.StoredProcedure;
            result = Convert.ToInt32(com.ExecuteScalar());
        }
        catch (Exception ex) { }
        finally
        {
            con.Close();
        }
        return result;
    }
    public int ExecuteQuery(SqlCommand com, string ProcName)
    {
        int res = 0;
        SqlConnection con = new SqlConnection(connstr);
        try
        {
            //SqlCommand com = new SqlCommand(strQuery, con);
            com.Connection = con;
            com.CommandText = ProcName;
            con.Open();
            com.CommandType = CommandType.StoredProcedure;
            res = com.ExecuteNonQuery();
        }
        catch (Exception ex) { }
        finally
        {
            con.Close();
        }
        return res;
    }

    public int ExecuteQuerySql(string StrSql)
    {
        int res = 0;
        SqlConnection con = new SqlConnection(connstr);
        try
        {
            SqlCommand com = new SqlCommand(StrSql, con);
            com.Connection = con;
            con.Open();
            res = com.ExecuteNonQuery();
        }
        catch (Exception ex) { }
        finally
        {
            con.Close();
        }
        return res;
    }
    public static DataTable ExecuteDataTableSQLname(string strQuery)
    {
        DataTable dt = new DataTable();
        SqlConnection con = new SqlConnection(connstr);
        try
        {
            SqlCommand com = new SqlCommand(strQuery, con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(com);
            da.Fill(dt);
        }
        catch (Exception ex) { }
        finally
        {
            con.Close();
        }
        return dt;
    }

    public DataTable ExecuteDataTableSQL(string strQuery)
    {
        DataTable dt = new DataTable();
        SqlConnection con = new SqlConnection(connstr);
        try
        {
            SqlCommand com = new SqlCommand(strQuery, con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(com);
            da.Fill(dt);
        }
        catch (Exception ex) { }
        finally
        {
            con.Close();
        }
        return dt;
    }

    public DataTable ExecuteDataTableText(SqlCommand com, string strQuery)
    {
        DataTable dt = new DataTable();
        SqlConnection con = new SqlConnection(connstr);
        try
        {
            //SqlCommand com = new SqlCommand(strQuery, con);
            com.Connection = con;
            con.Open();
            com.CommandText = strQuery;
            SqlDataAdapter da = new SqlDataAdapter(com);
            com.CommandType = CommandType.Text;
            da.Fill(dt);
        }
        catch (Exception ex)
        {
            //HelperSIS.SendEmail("", "dnv1987@live.com", "", "", ex.Message, "Simply Sucess " + strQuery, "");
        }
        finally
        {
            con.Close();
        }
        return dt;
    }
    public static List<T> ConvertDataTable<T>(DataTable dt)
    {
        List<T> data = new List<T>();
        foreach (DataRow row in dt.Rows)
        {
            T item = GetItem<T>(row);
            data.Add(item);
        }
        return data;
    }
    public static T GetItem<T>(DataRow dr)
    {
        Type temp = typeof(T);
        T obj = Activator.CreateInstance<T>();

        foreach (DataColumn column in dr.Table.Columns)
        {
            foreach (PropertyInfo pro in temp.GetProperties())
            {
                if (pro.Name == column.ColumnName)
                    pro.SetValue(obj, dr[column.ColumnName], null);
                else
                    continue;
            }
        }
        return obj;
    }  

}
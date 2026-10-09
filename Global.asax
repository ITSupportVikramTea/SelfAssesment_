<%@ Application Language="C#" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="System" %>
<script runat="server">

    void Application_Start(object sender, EventArgs e)
    {
        // Load state from file on application start
        Application["ShowPreviousMonthSalary"] = LoadStateFromFile();
    }

    private bool LoadStateFromFile()
    {
        string filePath = HttpContext.Current.Server.MapPath("~/ShowPreviousMonthSalary.txt");
        // Test if file can be created
        try
        {
            if (!File.Exists(filePath))
            {
                // Create the file if it doesn't exist
                File.WriteAllText(filePath, "false");
                return false;
            }
            return bool.Parse(File.ReadAllText(filePath));
        }
        catch (Exception ex)
        {
            // Log or handle the error
            HttpContext.Current.Response.Write("Error: " + ex.Message);
            return false;
        }
    }

    private void SaveStateToFile(bool state)
    {
        string filePath = HttpContext.Current.Server.MapPath("~/ShowPreviousMonthSalary.txt");
        try
        {
            File.WriteAllText(filePath, state.ToString());
        }
        catch (Exception ex)
        {
            // Log or handle the error
            HttpContext.Current.Response.Write("Error: " + ex.Message);
        }
    }

    void Application_End(object sender, EventArgs e)
    {
        // Code that runs on application shutdown
    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs
    }
</script>

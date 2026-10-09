using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.IO;

public class GoogleSheetsHelper
{
    static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
    static string ApplicationName = "Attendence";

    public static IList<IList<object>> GetSheetData(string spreadsheetId, string range)
    {
        GoogleCredential credential;

      
        using (var stream = new FileStream(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/strong-harbor-427510-q4-e89dd4cc1297.json"), FileMode.Open, FileAccess.Read))
        {
            credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
        }

       
        var service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName,
        });


        var request = service.Spreadsheets.Values.Get(spreadsheetId, range);
        ValueRange response = request.Execute();

        IList<IList<object>> values = response.Values;
        return values;
    }
}

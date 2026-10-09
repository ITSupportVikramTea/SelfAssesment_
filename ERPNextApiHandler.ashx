<%@ WebHandler Language="C#" Class="ERPNextApiHandler" %>

using System;
using System.Web;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
public class ERPNextApiHandler : IHttpHandler
{
    #region Global Variable   
    string JsonContent = string.Empty;
    private static readonly string baseAddress = ConfigurationManager.AppSettings["baseAddress"].ToString();
    private static readonly string AccessTokenManish = ConfigurationManager.AppSettings["AccessToken"].ToString();
    #endregion

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        try
        {
            string doctype = context.Request.QueryString["doctype"];
            if (string.IsNullOrWhiteSpace(doctype))
            {
                context.Response.Write("error");
                return;
            }

            FetchDataFromErpNext(doctype);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FetchDataFromErpNext(string doctype)
    {

        string apiUrl = baseAddress + "resource/" + doctype + "?fields=[\"*\"]&limit_page_length=None";
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization","token " + AccessTokenManish);
            HttpResponseMessage response = client.GetAsync(apiUrl).Result;

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = response.Content.ReadAsStringAsync().Result;
                HttpContext.Current.Response.ContentType = "application/json";
                HttpContext.Current.Response.Write(jsonResponse);
            }
            else
            {
                HttpContext.Current.Response.Write("error");
            }
        }
    }
       
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
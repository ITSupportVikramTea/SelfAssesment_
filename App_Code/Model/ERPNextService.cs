
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;


/// <summary>
/// Summary description for ERPNextService
/// </summary>
/// 

public class ERPNextService
{
    #region Global Variable   
    string JsonContent = string.Empty;
    private static readonly string baseAddress = ConfigurationManager.AppSettings["baseAddress"].ToString();
    private static readonly string AccessTokenManish = ConfigurationManager.AppSettings["AccessToken"].ToString();
    #endregion

    public ERPNextService()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public List<string> getCustomersList()
    {
        try
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            HttpClientHandler handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", AccessTokenManish);
          
            //DA = Distributor Appointment
            // Retrieve customers from default form
            string defaultUrl = baseAddress + "resource/Customer?limit_page_length=None";
            List<string> customerList = new List<string>();
            customerList.Add("");
            var defaultAPIResponse = client.GetAsync(defaultUrl).Result;
            if (defaultAPIResponse.IsSuccessStatusCode)
            {
                JsonContent = defaultAPIResponse.Content.ReadAsStringAsync().Result;
                string getdata = JsonContent.ToString();
                dynamic data = JObject.Parse(getdata);

                foreach (dynamic names in data["data"])
                {
                    string name = names["name"];
                    customerList.Add(name);
                }
            }
            // Retrieve customers from DA form
            string daUrl = baseAddress + "method/DACustomers?limit_page_length=None";
            var daAPIResponse = client.GetAsync(daUrl).Result;
            if (daAPIResponse.IsSuccessStatusCode)
            {
                JsonContent = daAPIResponse.Content.ReadAsStringAsync().Result;
                string getdata = JsonContent.ToString();
                dynamic data = JObject.Parse(getdata);

                foreach (dynamic names in data["message"])
                {
                    string name = names["customer_name"];
                    customerList.Add(name);
                }
            }
            return customerList;

        }
        catch (Exception ex)
        {
            //throw ex;
        }
        return null;
    }
    public string getJobOfferData()
    {
        string response = string.Empty;
        try
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            HttpClientHandler handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", AccessTokenManish);

            string defaultUrl = baseAddress + "resource/Job Offer?fields=[\"*\"]&limit_page_length=None";
            List<string> JobOfferData = new List<string>();
            JobOfferData.Add("");
            var defaultAPIResponse = client.GetAsync(defaultUrl).Result;
            if (defaultAPIResponse.IsSuccessStatusCode)
            {
                JsonContent = defaultAPIResponse.Content.ReadAsStringAsync().Result;
                response = JsonContent.ToString();
                
            }
        }
        catch (Exception ex)
        {
            //throw ex;
        }
        return response;
    }



    public string GetSalaryStructurer()
    {
        string response = string.Empty;
        try
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            HttpClientHandler handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", AccessTokenManish);

            string defaultUrl = baseAddress + "resource/Salary Structurer?fields=[\"*\"]&limit_page_length=None";
            List<string> stockledger = new List<string>();
            stockledger.Add("");
            var defaultAPIResponse = client.GetAsync(defaultUrl).Result;
            if (defaultAPIResponse.IsSuccessStatusCode)
            {
                JsonContent = defaultAPIResponse.Content.ReadAsStringAsync().Result;
                response = JsonContent.ToString();
                
            }
        }
        catch (Exception ex)
        {
            //throw ex;
        }
        return response;
    }



}
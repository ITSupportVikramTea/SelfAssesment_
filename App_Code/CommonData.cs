using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



   public class CommonData
    {
        public static string user_code { get; set; }
        public static string user_type { get; set; }
        public static string comp_code { get; set; }
        public static string vc_company_name { get; set; }
        public static string vc_license_no { get; set; }
        public static string VC_STATE_CODE { get; set; }
        public static string Comp_Abbr { get; set; }
        public static string OutletId { get; set; }
        public static string OutletName { get; set; }



    }

public class Payload
{
    public string UserId { get; set; }
    public string Password { get; set; }
    public string SendMethod { get; set; }
    public string MsgType { get; set; }
    public string SenderId { get; set; }
    public string Msg { get; set; }
    public string Mobile { get; set; }
}

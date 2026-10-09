using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for PassNMail
/// </summary>
public class PassNMail
{
    SQLDataFileSIS objSQLDataFile = new SQLDataFileSIS();
    private DateTime yourISTTime;
    public PassNMail()
    {
        //
        // TODO: Add constructor logic here
        //
        ConvertToIST();
    }
    //test code to convert time zone
    private void ConvertToIST()
    {
        DateTime dateTime = DateTime.Now;
        DateTime utcTime = dateTime.ToUniversalTime();
        TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        yourISTTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, istZone);
    }
    public void sendEmail(string to)
    {
        #region aws mail code

        var SMTP_USERNAME = ConfigurationManager.AppSettings["AWS_SMTP_USERNAME"].ToString();
        var SMTP_PASSWORD = ConfigurationManager.AppSettings["AWS_SMTP_PASSWORD"].ToString();
        var HOST = ConfigurationManager.AppSettings["AWS_SMTP_HOST"].ToString();
        int PORT = int.Parse(ConfigurationManager.AppSettings["AWS_SMTP_PORT"].ToString());
        var FROM = ConfigurationManager.AppSettings["AWS_FROM_EMAIL"].ToString();
        string emppass = CreatePassword(6);

        using (SmtpClient client = new SmtpClient(HOST, PORT))
        {

            var SUBJECT = "Login Credentials for ESS.";
            var BODY = @"<p>Dear Employee,</p></br><p>Please Login Using Following Credentials.</p><br/>
                       <p>Link: https://ess.vikramtea.com/ <br/> Email: " + to + "<br/> Password: " + emppass + "</p><br/><br/><br/> Regards," +
                       "<br/>Team HR,<br/>Vikram Tea Processor Pvt. Ltd.";

            client.Credentials = new NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);
            client.EnableSsl = true;

            try
            {
                var mailMessage = new MailMessage()
                {
                    Body = BODY,
                    Subject = SUBJECT,
                    From = new MailAddress(FROM)
                };
                mailMessage.To.Add(new MailAddress(to));
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.IsBodyHtml = true;

                //REMOVE THIS CODE
                //System.IO.MemoryStream ms = new System.IO.MemoryStream(attchmentRec.ssSTAttachment.ssBinary);
                //Attachment attach = new Attachment(ms, attchmentRec.ssSTAttachment.ssFilename);
                //mailMessage.Attachments.Add(attach);

                //System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType();
                //contentType.MediaType = System.Net.Mime.MediaTypeNames.Application.Octet;
                //contentType.Name = "Lebowski.docx";
                //mailMessage.Attachments.Add(new Attachment("C:\\users\\luis\\Documents\\Lebowski.docx", contentType));

                client.Send(mailMessage);
                objSQLDataFile.ExecuteDataTableSQL(@"UPDATE UserMaster SET Password = '" + emppass + "' WHERE EmailId = '" + to + "'");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        #endregion

        #region old code
        //MailMessage message = null;
        ////ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //var fromEmailAddress = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
        //var fromEmailDisplayName = ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
        //var fromEmailPassword = ConfigurationManager.AppSettings["FromEmailPassword"].ToString();
        //var smtpHost = ConfigurationManager.AppSettings["SMTPHost"].ToString();
        //var smtpPort = ConfigurationManager.AppSettings["SMTPPort"].ToString();
        //string emppass = CreatePassword(6);


        //message = new MailMessage(fromEmailAddress, to);

        //message.Subject = "Login Credentials for ESS.";
        //message.Body = @"<p>Dear Employee,</p></br><p>Please Login Using Following Credentials.</p><br/>
        //    <p>Link: http://ess.vikramtea.com/ <br/> Email: " + to + "<br/> Password:" + emppass + "</p><br/><br/><br/> Thanks & Regards," +
        //    "<br/>Team HR,<br/>Vikram Tea Processor Pvt. Ltd.";
        //message.BodyEncoding = Encoding.UTF8;
        //message.IsBodyHtml = true;
        //SmtpClient client = new SmtpClient();
        ////client.TargetName = "STARTTLS/smtp.office365.com";
        //client.UseDefaultCredentials = false;
        //client.Credentials = new NetworkCredential(fromEmailAddress, fromEmailPassword);
        //client.Host = smtpHost;
        //client.EnableSsl = true;
        //client.Port = !string.IsNullOrEmpty(smtpPort) ? Convert.ToInt32(smtpPort) : 0;
        //client.DeliveryMethod = SmtpDeliveryMethod.Network;

        //try
        //{
        //    client.Send(message);
        //    objSQLDataFile.ExecuteDataTableSQL(@"UPDATE UserMaster SET Password = '" + emppass + "' WHERE EmailId = '" + to + "'");
        //}

        //catch (Exception ex)
        //{
        //    //throw ex;
        //}
        #endregion
    }

    public void SendMailToRM(List<string> tos, string empname, string location, string rmname)
    {
        var ISTTime = yourISTTime.ToString("dd/MM/yyyy HH:mm:ss tt");

        #region aws mail code

        var SMTP_USERNAME = ConfigurationManager.AppSettings["AWS_SMTP_USERNAME"].ToString();
        var SMTP_PASSWORD = ConfigurationManager.AppSettings["AWS_SMTP_PASSWORD"].ToString();
        var HOST = ConfigurationManager.AppSettings["AWS_SMTP_HOST"].ToString();
        int PORT = int.Parse(ConfigurationManager.AppSettings["AWS_SMTP_PORT"].ToString());
        var FROM = ConfigurationManager.AppSettings["AWS_FROM_EMAIL"].ToString();
      
        using (SmtpClient client = new SmtpClient(HOST, PORT))
        {
            var SUBJECT = "Resignation Submitted - ESS Portal.";
            var BODY = @"<p>Dear Sir,</p><p>Resignation is added by Employee<b> " + empname + "</b> on <b>" + ISTTime + " </b></p>" +
                "<p>Employee Location : <b>"+location+"</b> <p>Reporting To: <b>"+rmname+ " </b></p><p>Login : http://uat.vikramtea.com </p> Regards, <br/><br/>Vikram Tea Processor Pvt. Ltd.";
            
            client.Credentials = new NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);
            client.EnableSsl = true;

            try
            {
                var mailMessage = new MailMessage()
                {
                    Body = BODY,
                    Subject = SUBJECT,
                    From = new MailAddress(FROM)
                };
                if (tos != null)
                {
                    foreach (string addr in tos)
                    {
                        mailMessage.To.Add(addr);
                    }
                }
           //   mailMessage.To.Add(new MailAddress(tos));
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.IsBodyHtml = true;
                mailMessage.CC.Add("itsupport@vikramtea.com");
                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
               // throw ex;
            }
        }

        #endregion
    }
    public void SendEmailToHR(string empname, string physicalFilePath)
    {
        var ISTTime = yourISTTime.ToString("dd/MM/yyyy HH:mm:ss tt");
        #region aws mail code

        var SMTP_USERNAME = ConfigurationManager.AppSettings["AWS_SMTP_USERNAME"].ToString();
        var SMTP_PASSWORD = ConfigurationManager.AppSettings["AWS_SMTP_PASSWORD"].ToString();
        var HOST = ConfigurationManager.AppSettings["AWS_SMTP_HOST"].ToString();
        int PORT = int.Parse(ConfigurationManager.AppSettings["AWS_SMTP_PORT"].ToString());
        var FROM = ConfigurationManager.AppSettings["AWS_FROM_EMAIL"].ToString();

        using (SmtpClient client = new SmtpClient(HOST, PORT))
        {
            var SUBJECT = "Clearance Submitted - ESS Portal.";
            var BODY = @"<p>Dear Sir,</p><p>Clearance Form of Employee<b> " + empname + "</b> is submitted on " + ISTTime + "</p> Regards, <br/><br/>Vikram Tea Processor Pvt. Ltd.";
            client.Credentials = new NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);
            client.EnableSsl = true;
            string hrMail = "itsupport@vikramtea.com";//add hr email 
            try
            {
                var mailMessage = new MailMessage()
                {
                    Body = BODY,
                    Subject = SUBJECT,
                    From = new MailAddress(FROM)
                };
               
                mailMessage.To.Add(new MailAddress(hrMail));
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.IsBodyHtml = true;
                Attachment attach = new Attachment(physicalFilePath);
                mailMessage.Attachments.Add(attach);

                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                // throw ex;
            }
        }

        #endregion

    }
    public void SendSeperationMailToEmp(string employeeEmail, string resignationDateString, int noticePeriod)
    {
        var ISTTime = yourISTTime.ToString("dd/MM/yyyy HH:mm:ss tt");
        #region aws mail code

        var SMTP_USERNAME = ConfigurationManager.AppSettings["AWS_SMTP_USERNAME"].ToString();
        var SMTP_PASSWORD = ConfigurationManager.AppSettings["AWS_SMTP_PASSWORD"].ToString();
        var HOST = ConfigurationManager.AppSettings["AWS_SMTP_HOST"].ToString();
        int PORT = int.Parse(ConfigurationManager.AppSettings["AWS_SMTP_PORT"].ToString());
        var FROM = ConfigurationManager.AppSettings["AWS_FROM_EMAIL"].ToString();
       
        using (SmtpClient client = new SmtpClient(HOST, PORT))
        {
            var SUBJECT = "Separation from the service of the company";
            var BODY = @"<p><b>Dear Employee,</b></p>
<p><strong>With reference to your resignation from the services of the company dated <strong>" + resignationDateString + @"</strong> & as per appointment letter clasue / revised clause, your Last working date will be after " + noticePeriod + @" Months from your date of resignation,
Note : After acceptance of your resignation from your superiors, you are advised to complete followings procedure:</strong></p>
<p>Click on the below Exit Interview Form link and fill in the details:</p> 
<p><a href=""https://docs.google.com/forms/d/e/1FAIpQLSd31kFuiPN8OvJdKpZ5ysckmgMKBZas1oa92komWIda9q2jbA/viewform?vc=0&c=0&w=1&flr=0&usp=mail_form_link"" style=""cursor: pointer;"">Exit Interview Form</a></p>

<p>• Consult your superior / HR  and ensure your complete and correct attendance till last date of working. </p>
<p>• Ensure with your superior about approval status of your leave applied during last month. </p>
<p>• Submit all properties issued to you during your tenure with the company. Submit the same to your superior or to the person on his advice. (Property should be in working conditions). </p>
<p>• In case of not submitting of company property on  last date of working, deduction will be made equal to the cost of property from your payable dues.</p>
<p>• Collect NOC from all concerned and submit to HR through your reporting Manager within 7 days from last day of your working in the company, or 7 days before end of notice period if tendered the same. </p>
<p>• Clear your recoverable balance amount of loans and advances if any, with accounts.</p>
<p>• Submit your all pending expenses to your superior, within seven days from the last day of working, in case notice of resignation is tendered then 7 days before end of notice period.</p>

<p>Please complete above all,  for smooth handing over, taking over of charges and clearing of your dues at the earliest.
<p>Your settlement of account at the earliest, is subject to submission all above in stipulated time, responsibility of delay in settlement of your dues, due to not submission of above all in time would be entirely yours and company will not be responsible in any manner. </p>
<p>Your relieving letter will be issued to you on completion of above formalities and settling of your full and final settlement of account.</p>

<p>Thanking you</p>
<p>For Vikram Tea Processor Private Limited.</p>

<p>HR Department.</p>";

            client.Credentials = new NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);
            client.EnableSsl = true;

            try
            {
                var mailMessage = new MailMessage()
                {
                    Body = BODY,
                    Subject = SUBJECT,
                    From = new MailAddress(FROM)
                };
                mailMessage.To.Add(new MailAddress(employeeEmail));
                string ccEmail = "itsupport@vikramtea.com";//add hr emailid here
                if (!string.IsNullOrEmpty(ccEmail))
                {
                    mailMessage.CC.Add(new MailAddress(ccEmail));
                }
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.IsBodyHtml = true;

                //REMOVE THIS CODE
                //System.IO.MemoryStream ms = new System.IO.MemoryStream(attchmentRec.ssSTAttachment.ssBinary);
                //Attachment attach = new Attachment(ms, attchmentRec.ssSTAttachment.ssFilename);
                //mailMessage.Attachments.Add(attach);

                //System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType();
                //contentType.MediaType = System.Net.Mime.MediaTypeNames.Application.Octet;
                //contentType.Name = "Lebowski.docx";
                //mailMessage.Attachments.Add(new Attachment("C:\\users\\luis\\Documents\\Lebowski.docx", contentType));

                 client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                // throw ex;
            }
        }
        #endregion
    }

    public void SendEmailToDepartmentHeads(string empName, List<string> departmentHeadEmails, string rdate)
    {
        var ISTTime = yourISTTime.ToString("dd/MM/yyyy HH:mm:ss tt");
        #region AWS mail code

        var SMTP_USERNAME = ConfigurationManager.AppSettings["AWS_SMTP_USERNAME"].ToString();
        var SMTP_PASSWORD = ConfigurationManager.AppSettings["AWS_SMTP_PASSWORD"].ToString();
        var HOST = ConfigurationManager.AppSettings["AWS_SMTP_HOST"].ToString();
        int PORT = int.Parse(ConfigurationManager.AppSettings["AWS_SMTP_PORT"].ToString());
        var FROM = ConfigurationManager.AppSettings["AWS_FROM_EMAIL"].ToString();

        using (SmtpClient client = new SmtpClient(HOST, PORT))
        {
            var SUBJECT = "Resignation of Employee Submitted and Approved by RM - ESS Portal.";
            var BODY = @"<p>Dear All,</p><p>Resignation of Employee <b> "+empName+" </b> was submitted on <b> "+rdate+ " </b> and Approved on <b>" + ISTTime + " </b><br/></p><p> Please fill the Clearance Details of your department </p> Regards, <br/><br/>Vikram Tea Processor Pvt. Ltd.";

            client.Credentials = new NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);
            client.EnableSsl = true;

            try
            {
                var mailMessage = new MailMessage
                {
                    Body = BODY,
                    Subject = SUBJECT,
                    From = new MailAddress(FROM),
                    BodyEncoding = Encoding.UTF8,
                    IsBodyHtml = true
                };

                foreach (var email in departmentHeadEmails)
                {
                    mailMessage.To.Add(email);
                }
                mailMessage.CC.Add("itsupport@vikramtea.com");
               // client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                // Handle the exception or log it.
            }
        }

        #endregion
    }

    public string CreatePassword(int length)
    {
        //const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        const string valid = "1234567890";
        StringBuilder res = new StringBuilder();
        Random rnd = new Random();
        while (0 < length--)
        {
            res.Append(valid[rnd.Next(valid.Length)]);
        }
        return res.ToString();
    }

    //old mail code
    //public void SendIncrementLetter(string toEmailId, string emailContent, string subject, string attachedFile)
    //{

    //    MailMessage message = null;

    //    var fromEmailAddress = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
    //    var fromEmailDisplayName = ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
    //    var fromEmailPassword = ConfigurationManager.AppSettings["FromEmailPassword"].ToString();
    //    var smtpHost = ConfigurationManager.AppSettings["SMTPHost"].ToString();
    //    var smtpPort = ConfigurationManager.AppSettings["SMTPPort"].ToString();
    //    string emppass = CreatePassword(6);


    //    message = new MailMessage(fromEmailAddress, toEmailId);

    //    message.Subject = subject;
    //    message.Body = emailContent;
    //    Attachment attach = new Attachment(attachedFile);
    //    message.Attachments.Add(attach);
    //    message.BodyEncoding = Encoding.UTF8;
    //    message.IsBodyHtml = true;

    //    SmtpClient client = new SmtpClient();
    //    client.UseDefaultCredentials = false;
    //    client.Credentials = new NetworkCredential(fromEmailAddress, fromEmailPassword);
    //    client.Host = smtpHost;
    //    client.EnableSsl = true;
    //    client.Port = !string.IsNullOrEmpty(smtpPort) ? Convert.ToInt32(smtpPort) : 0;
    //    client.DeliveryMethod = SmtpDeliveryMethod.Network;
    //    try
    //    {
    //       client.Send(message);
    //    }

    //    catch (Exception ex)
    //    {
    //        //throw ex;
    //    }
    //}

    //new aws mail code 
    public void SendIncrementLetter(string toEmailId, string emailContent, string subject, string attachedFile)
    {
        MailMessage message = null;

        var SMTP_USERNAME = ConfigurationManager.AppSettings["AWS_SMTP_USERNAME"].ToString();
        var SMTP_PASSWORD = ConfigurationManager.AppSettings["AWS_SMTP_PASSWORD"].ToString();
        var HOST = ConfigurationManager.AppSettings["AWS_SMTP_HOST"].ToString();
        int PORT = int.Parse(ConfigurationManager.AppSettings["AWS_SMTP_PORT"].ToString());
        var FROM = ConfigurationManager.AppSettings["AWS_FROM_EMAIL"].ToString();

        message = new MailMessage(FROM, toEmailId);

        string ccEmail = "hr@vikramtea.com"; 
        message.CC.Add(ccEmail);

        message.Subject = subject;
        message.Body = emailContent;
        Attachment attach = new Attachment(attachedFile);
        message.Attachments.Add(attach);
        message.BodyEncoding = Encoding.UTF8;
        message.IsBodyHtml = true;

        SmtpClient client = new SmtpClient();
        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);
        client.Host = HOST;
        client.EnableSsl = true;
        client.Port = PORT;
        client.DeliveryMethod = SmtpDeliveryMethod.Network;

        //try
        //{
        //    client.Send(message);
        //}
        //catch (Exception ex)
        //{
        //    // Handle the exception appropriately
        //}

        string logFilePath = @"C:\Logs\Emaillogs.xlsx";

        try 
        {
            client.Send(message);
            LogEmailStatusToExcel(logFilePath, toEmailId, subject, "success");
        }
        catch (Exception ex) 
        {
            LogEmailStatusToExcel(logFilePath,toEmailId, subject,"Failed", ex.Message);
        }
    }

    public void LogEmailStatusToExcel(string filepath,string toEmail,string subject,string status,string error = "")
    {

        using (var workbook = File.Exists(filepath) ? new XLWorkbook(filepath) : new XLWorkbook()) 
        {
            var worksheet = workbook.Worksheets.FirstOrDefault() ?? workbook.AddWorksheet("EmailLogs");

            int lastRow = worksheet.LastRowUsed() != null ? worksheet.LastRowUsed().RowNumber() : 0;

            if(lastRow == 0)
            {
                worksheet.Cell(1, 1).Value = "TimeStamp";
                worksheet.Cell(1, 2).Value = "To Email";
                worksheet.Cell(1, 3).Value = "subject";
                worksheet.Cell(1, 4).Value = "Status";
                worksheet.Cell(1, 5).Value = "Error";
                lastRow = 1;

            }

            var newRow = lastRow + 1;

            worksheet.Cell(newRow, 1).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            worksheet.Cell(newRow,2).Value = toEmail;
            worksheet.Cell(newRow, 3).Value = subject;
            worksheet.Cell(newRow,4).Value = status;
            worksheet.Cell(newRow, 5).Value = error;
            workbook.SaveAs(filepath);
        
        }
          
    }
}
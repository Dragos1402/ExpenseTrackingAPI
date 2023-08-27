using ExpenseTrackingAPI.DataContext;
using ExpenseTrackingAPI.DbModels;
using NuGet.Common;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Extensions.Hosting.Internal;
using System.Net.Mail;
using System.Net;
using Newtonsoft.Json;

namespace ExpenseTrackingAPI.Helpers
{
    public class Globals
    {

        public static int CheckToken(string token)
        {
        int tkID = 0;

            using (IDbConnection connection = new SqlConnection("Server=.\\DRAGOSSERVER;Initial Catalog=ExpenseTracking;MultipleActiveResultSets=True;Encrypt=False;TrustServerCertificate=True;Trusted_Connection=true"))
            {
                connection.Open();

                string query = "SELECT tk_id FROM dbo.Token WHERE tk_value = @Token";
                tkID = connection.QueryFirstOrDefault<int>(query, new { Token = token });
            }
            return tkID;
        }
        public static String CreateJSON(object item)
        {
            return JsonConvert.SerializeObject(item);
        }
        public static string ReadHtmlTemplate(string file)
        {
            string path = "";
            //string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Template") + @"\";
            StreamReader Output = new StreamReader(path + file, false);
            string result = Output.ReadToEnd();
            Output.Close();
            return result;
        }
        public static void SendEmail(string toAdress, string bccAddress,string mailSubject, string mailBody, string fileAttach)
        {
            string fromEmail = "De selectat din Appsettings si de modificat cu datele care trebuiesc";
            int port = 0;
            string host = "";
            string username = "";
            string password = "";

            MailMessage mail = new MailMessage(fromEmail, toAdress);
            if (bccAddress !="")
            {
                mail.Bcc.Add(bccAddress);
            }
            SmtpClient client = new SmtpClient();
            client.Port= port;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host= host;
            //client.UseDefaultCredentials = false;
            client.Credentials= new NetworkCredential(username, password);
            mail.IsBodyHtml = true; // to make message body as html
            mail.Subject = mailSubject;
            if (fileAttach!="")
            {
                mail.Attachments.Add(new Attachment(fileAttach));
            }
            mail.Body += mailBody;
            client.Send(mail);
        }
    }
}

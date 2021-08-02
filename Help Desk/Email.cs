using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Configuration;



namespace Help_Desk
{
    public class Email
    {
        
        public bool sendEmail(string ID, string subject, string data)
        {
            string Email = "";
            string SystemAttendant = ConfigurationManager.AppSettings["SystemEmail"];
            string Helpdesk = ConfigurationManager.AppSettings["HelpdeskEmail"];
            if (ID != "-1")
            {
                Email = getEmailfromID(ID);
            }
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient("smtp.office365.com");
            message.From = new MailAddress(SystemAttendant);
            if (ID != "-1")
                message.To.Add(new MailAddress(Email));
            else
                message.To.Add(new MailAddress(Helpdesk));
            message.Subject = subject;
            message.IsBodyHtml = true;

            message.Body = data;
            smtp.Send(message);

            return true;
        }

        public string getEmailfromID(string id)
        {
            string email = null;
            sqlTypeCommands sql = new sqlTypeCommands();
            DataTable dt = new DataTable();
            string query = "select emailaddress from person where personid = " + id;
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                email = dt.Rows[0]["Emailaddress"].ToString();
            }
            return email;
        }
    }


}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Projekat.Services
{
    public class EmailService : IEmailService
    {
        
        public void SendMail(string subject, string body, string mailAdd)
        {
            string mailSubject = subject;
            string mailBody = body;
            
            string FromMail = ConfigurationManager.AppSettings["from"];
            string emailTO = mailAdd;
           
            MailMessage mail = new MailMessage();

            SmtpClient SmtpServer = new SmtpClient(ConfigurationManager.AppSettings["smtpServer"]);

            mail.From = new MailAddress(FromMail);
            //kolekcija, jedan mail mozemo poslati na vise adresa
            mail.To.Add(emailTO);
            mail.Subject = mailSubject;
            mail.Body = mailBody;
            
            //mail.Attachments.Add(new Attachment(HttpContext.Current.Server.MapPath("~/logs/app-log.txt")));

            SmtpServer.Port = int.Parse(ConfigurationManager.AppSettings["smtpPort"]);
            SmtpServer.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["from"], ConfigurationManager.AppSettings["password"]);
            //ssl da li treba da se kriptuje sadrzaj maila
            SmtpServer.EnableSsl = bool.Parse(ConfigurationManager.AppSettings["smtpSsl"]);
            SmtpServer.Send(mail);

        }
    }
}
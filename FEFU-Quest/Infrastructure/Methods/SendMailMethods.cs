using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;

namespace FEFU_Quest.Infrastructure.Methods
{
    public static class SendMailMethods
    {
        public static bool CheckEmail(string email)=>
          new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").IsMatch(email);

        public static async Task<bool> SendEmailAsync(string senderEmail, string SenderName, string senderPassword, string recicerEmail, string subj, string body)
        {
            try
            {
                var from = new MailAddress(senderEmail, SenderName);
                var to = new MailAddress(recicerEmail);
                var m = new MailMessage(from, to)
                {
                    Subject = subj,
                    Body = $"<div> <p>{body}</p> \n{DateTime.Now:f}</div>" //<p></p>
                };
                m.IsBodyHtml = true;
                var smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential(senderEmail, senderPassword);
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(m);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}

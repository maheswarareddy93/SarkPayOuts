using Microsoft.Extensions.Options;
using SarkPayOuts.Interface;
using SarkPayOuts.Models;
using SarkPayOuts.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SarkPayOuts.MailsAndMessages
{
    public class EmailService: IEmailSender
    {
        private readonly smtp ec;
        private readonly ApplicationDBContext _context;
        public EmailService(IOptions<smtp> emailConfig, ApplicationDBContext context)
        {
            this.ec = emailConfig.Value;
            _context = context;
        }
        public void SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                //var emailMessage = new MimeMessage();
                //emailMessage.From.Add(new MailboxAddress(ec.FromName, ec.FromAddress));
                //emailMessage.To.Add(new MailboxAddress("", email));
                //emailMessage.Subject = subject;
                //emailMessage.Body = new TextPart(TextFormat.Html) { Text = message };

                //using (var client = new MailKit.Net.Smtp.SmtpClient())
                //{
                //    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                //    client.LocalDomain = ec.LocalDomain;
                //    client.ConnectAsync(ec.MailServerAddress, ec.MailServerPort, false);
                //    client.AuthenticateAsync(new System.Net.NetworkCredential(ec.UserId, ec.UserPassword));
                //    client.SendAsync(emailMessage);
                //    client.DisconnectAsync(true);
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        public void SendEmail(string email, string Cc, string subject, string message, byte[] bytes)
        {
            try
            {
                using (MailMessage mm = new MailMessage(new MailAddress(ec.UserName), new MailAddress(email)))
                {
                    mm.Subject = subject;
                    if (!string.IsNullOrEmpty(Cc))
                    {
                        mm.CC.Add(Cc);
                    }
                    mm.Body = message;
                    mm.IsBodyHtml = true;
                    using (System.Net.Mail.SmtpClient smtpc = new System.Net.Mail.SmtpClient())
                    {
                        CommonSetting settings = SmtpServerData();
                        if (settings != null)
                        {
                            smtpc.Host = settings.SMTPServer;
                            smtpc.EnableSsl = true;
                            NetworkCredential NetworkCred = new NetworkCredential(settings.Email, settings.Password);
                            smtpc.UseDefaultCredentials = true;
                            smtpc.Credentials = NetworkCred;
                            smtpc.Port = settings.Port;
                        }
                        else
                        {
                            smtpc.Host = ec.Host;
                            smtpc.EnableSsl = true;
                            NetworkCredential NetworkCred = new NetworkCredential(ec.UserName, ec.Password);
                            smtpc.UseDefaultCredentials = true;
                            smtpc.Credentials = NetworkCred;
                            smtpc.Port = ec.Port;
                        }
                        if (bytes != null && bytes.Length > 0)
                        {
                            mm.Attachments.Add(new Attachment(new MemoryStream(bytes), "DigitalReceipt.pdf"));
                        }
                        smtpc.Send(mm);
                    }
                }
            }
            catch (Exception ex)
            {
                SendMail("mahiavr025@gmail.com", "Mail Exception", ex.ToString(), "");
            }
        }
        public void SendSMS(string name, string message, string mobile)
        {
            string result = string.Empty;
            try
            {
                using (var wb = new WebClient())
                {
                    byte[] response = wb.UploadValues("http://api.textlocal.in/send/", new NameValueCollection()
                {
                {"username" ,"mahiavr025@gmail.com"},
                {"hash" , "675b07d9b40bc692f5b158de5c0c8ed31765566a432d434490eaf8ae0477ca84"},
                {"numbers" , mobile},
                {"message" , message},
                {"sender" , ""}
                });
                    result = System.Text.Encoding.UTF8.GetString(response);
                }
            }
            catch (Exception ex)
            {
            }
        }
        public void SendMail(string email, string subject, string message, string data)
        {
            try
            {
                using (MailMessage mm = new MailMessage(new MailAddress(ec.UserName), new MailAddress(email)))
                {
                    mm.Subject = subject;
                    mm.Body = message;
                    mm.IsBodyHtml = true;

                    using (System.Net.Mail.SmtpClient smtpc = new System.Net.Mail.SmtpClient())
                    {
                        smtpc.Host = ec.Host;
                        smtpc.EnableSsl = true;
                        NetworkCredential NetworkCred = new NetworkCredential(ec.UserName, ec.Password);
                        smtpc.UseDefaultCredentials = true;
                        smtpc.Credentials = NetworkCred;
                        smtpc.Port = ec.Port;
                        if (!string.IsNullOrEmpty(data))
                        {
                            Attachment attachment = new Attachment(data);
                            mm.Attachments.Add(attachment);
                        }
                        smtpc.Send(mm);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Getting the Mails from DB To Send Approved Mails and Leads to Mails 
        public AutharityData GettingMailFromDB()
        {
            AutharityData ObjtblAutharityData = (from mails in _context.AutharityData select mails).FirstOrDefault();
            return ObjtblAutharityData;
        }

        public CommonSetting SmtpServerData()
        {
            CommonSetting ObjtblCommonSetting = (from mails in _context.CommonSetting select mails).FirstOrDefault();
            return ObjtblCommonSetting;
        }
    }
}

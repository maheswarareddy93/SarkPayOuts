using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SarkPayOuts.Interface
{
    public interface  IEmailSender
    {
        void SendEmailAsync(string email, string subject, string message);
        void SendEmail(string email, string Cc, string subject, string message, byte[] data);
        void SendSMS(string name, string message, string mobile);
        void SendMail(string email, string subject, string message, string data);
       // tblAutharityData GettingMailFromDB();
    }
}

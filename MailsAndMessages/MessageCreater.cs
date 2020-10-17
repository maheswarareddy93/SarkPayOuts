using SarkPayOuts.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SarkPayOuts.MailsAndMessages
{
    public class MessageCreater
    {
        StringBuilder sb = new StringBuilder();
        ApplicationDBContext _db = new ApplicationDBContext();
        string htmlBodyContent;
        public string GetHtmlBody(string msg)
        {
            var htmlBody = (from template in _db.MailTemplate  where template.TemplateName == msg select template.Body).FirstOrDefault();
            if (!string.IsNullOrEmpty(htmlBody))
            {
                return htmlBody;
            }
            else { return null; }
        }

        public string ForgotPasswordSMS(AdminDetails details,string url)
        {
            htmlBodyContent = GetHtmlBody("ForgotSMS");
            if (!string.IsNullOrEmpty(htmlBodyContent))
            {
                htmlBodyContent = string.Format("Forgot{0}", htmlBodyContent.Replace("$User$", details.Name));
                htmlBodyContent = string.Format("Pass{0}", htmlBodyContent.Replace("$Password$", details.password));
                htmlBodyContent = string.Format("word{0}",htmlBodyContent.Replace("$url",url));
                sb.Append(htmlBodyContent);
                return sb.ToString();
            }
            return null;
        }
    }
}

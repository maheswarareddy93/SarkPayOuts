using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SarkPayOuts.Helper
{
    public class CurrentAgentSession
    {
        private static HttpContextAccessor _HttpContextAccessor = new HttpContextAccessor();
        public static CurrentAgent Agent
        {
            get
            {
                CurrentAgent Agent = _HttpContextAccessor.HttpContext.Session.GetObject<CurrentAgent>("CurrentAgent");
                return Agent;
            }
            set
            {
                _HttpContextAccessor.HttpContext.Session.SetObject("CurrentAgent", value);
            }
        }
        public static string AgentId
        {
            get
            {
                if (Agent != null)
                    return Agent.AgentId;
                else
                    return null;
            }
        }
        public static string AgentName
        {
            get
            {
                if (AgentName != null)
                    return Agent.AgentName;
                else
                    return string.Empty;
            }
        }
        public static string Mobile
        {
            get
            {
                if (Mobile != null)
                    return Agent.Mobile;
                else
                    return string.Empty;
            }
        }
        public static string Email
        {
            get
            {
                if (Email != null)
                    return Agent.Email;
                else
                    return string.Empty;
            }
        }
       
        public static bool? IsActive
        {
            get
            {
                if (IsActive!=null)
                    return Agent.IsActive;
                else
                    return false;
            }
        }
    }
   
    [Serializable]
    public class CurrentAgent
    {
        public string AgentId { get; set; }
        public string AgentName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public bool? IsActive { get; set; }
    }
}
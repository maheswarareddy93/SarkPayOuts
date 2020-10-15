using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SarkPayOuts.Helper
{
    public static class SessionHelper
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetObject(key, value);
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            return session.GetObject<T>(key);
        }
    }
}

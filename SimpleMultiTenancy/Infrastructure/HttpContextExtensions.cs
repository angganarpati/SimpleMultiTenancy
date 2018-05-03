using SimpleMultiTenancy.Constants;
using SimpleMultiTenancy.Data;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;

namespace SimpleMultiTenancy.Infrastructure
{
    public static class HttpContextExtensions
    {
        public static void SetTenantCookies(
            this HttpContextBase context,
            string tenantName,
            int cookieExpireDate = 1)
        {
            #region Expired cookies

            RemoveTenantCookies(context);

            #endregion Expired cookies

            Assembly assembly = Assembly.GetExecutingAssembly();
            var attribute = (GuidAttribute)assembly.GetCustomAttributes(typeof(GuidAttribute), true)[0];
            HttpCookie tenantCookies = new HttpCookie(CookieKeys.TenantEnvironmetKey);
            tenantCookies.Value = tenantName;
            tenantCookies.Expires = DateTime.Now.AddDays(cookieExpireDate);
            context.Response.Cookies.Add(tenantCookies);
            context.Session.Add(CookieKeys.TenantEnvironmetKey, tenantName);
        }

        public static void SetTenantCookiesByTenantId(
            this HttpContextBase context,
            string tenantId)
        {
            var dbFactory = DependencyResolver.Current.GetService<ITenantDBFactory>();
            var dbContext = dbFactory.Get();

            #region Expired cookies

            RemoveTenantCookies(context);

            #endregion Expired cookies

            var tenant = dbContext.Tenants.Find(new Guid(tenantId));

            Assembly assembly = Assembly.GetExecutingAssembly();
            var attribute = (GuidAttribute)assembly.GetCustomAttributes(typeof(GuidAttribute), true)[0];
            HttpCookie tenantCookies = new HttpCookie(CookieKeys.TenantEnvironmetKey);
            tenantCookies.Value = tenant.TenantName;
            tenantCookies.Expires = DateTime.Now.AddMinutes(30);
            context.Response.Cookies.Add(tenantCookies);
        }

        public static void RemoveTenantCookies(
            this HttpContextBase context)
        {
            HttpCookie tenantCookies = new HttpCookie(CookieKeys.TenantEnvironmetKey);
            tenantCookies.Expires = DateTime.Now.AddDays(-1);
            context.Response.Cookies.Add(tenantCookies);
            context.Session.Remove(CookieKeys.TenantEnvironmetKey);
        }

        public static void RemoveTenantSession(
            this HttpContextBase context)
        {
            context.Session.Remove(CookieKeys.TenantEnvironmetKey);
        }
    }
}
using SimpleMultiTenancy.Constants;
using SimpleMultiTenancy.Data;
using SimpleMultiTenancy.Data.Abstract;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;

namespace SimpleMultiTenancy.Infrastructure
{
    public class TenantResolver : ITenantResolver
    {
        private readonly HttpContextBase httpContextBase;

        private readonly TenantContext tenantContext;

        public TenantResolver(
            ITenantDBFactory tenantDbFactory,
            HttpContextBase httpContextBase)
        {
            tenantContext = tenantDbFactory.Get();
            this.httpContextBase = httpContextBase;
        }

        private ITenant tenantData = null;

        public ITenant GetCurrentTenant
        {
            get
            {
                if (tenantData == null)
                {
                    if (httpContextBase.Session[CookieKeys.TenantEnvironmetKey] != null)
                    {
                        string tenant = httpContextBase.Session[CookieKeys.TenantEnvironmetKey].ToString();
                        if (!string.IsNullOrEmpty(tenant))
                        {
                            tenantData = tenantContext.Tenants.FirstOrDefault(x => x.TenantName == tenant) == null ? tenantContext.Tenants.FirstOrDefault(x => x.TenantCode == tenant) : null;
                        }
                    }

                    if (httpContextBase.Request.Cookies.Count > 0)
                    {
                        if (httpContextBase.Request.Cookies[CookieKeys.TenantEnvironmetKey] != null)
                        {
                            Assembly assembly = Assembly.GetExecutingAssembly();
                            var attribute = (GuidAttribute)assembly.GetCustomAttributes(typeof(GuidAttribute), true)[0];
                            string tenant = httpContextBase.Request.Cookies[CookieKeys.TenantEnvironmetKey].Value;

                            if (httpContextBase.Session[CookieKeys.TenantEnvironmetKey] != null)
                            {
                                httpContextBase.Session.Remove(CookieKeys.TenantEnvironmetKey);
                            }

                            if (!string.IsNullOrEmpty(tenant))
                            {
                                tenantData = tenantContext.Tenants.FirstOrDefault(x => x.TenantName == tenant) == null ? tenantContext.Tenants.FirstOrDefault(x => x.TenantCode == tenant) : null;
                            }
                        }
                    }
                }
                return tenantData;
            }
        }

        private IDBTenantConnectionString tenantConnectionString = null;

        public IDBTenantConnectionString GetTenantDBConnectionString
        {
            get
            {
                var tenant =  this.GetCurrentTenant;
                if (tenant != null)
                {
                    if (tenantConnectionString == null)
                    {
                        tenantConnectionString = tenantContext.DBTenantConnectionStrings.FirstOrDefault(x => x.TenantID == GetCurrentTenant.TenantID);
                    }
                    return tenantConnectionString;
                }
                return tenantConnectionString;
            }
        }

        private ITenantProfile tenantProfile = null;

        public ITenantProfile GetTenantProfile
        {
            get
            {
                var tenant = this.GetCurrentTenant;
                if (tenant != null)
                {
                    if (tenantProfile == null)
                    {
                        tenantProfile = tenantContext.TenantProfile.FirstOrDefault(x => x.TenantId == GetCurrentTenant.TenantID);
                    }
                    return tenantProfile;
                }
                return tenantProfile;
            }
        }
    }
}
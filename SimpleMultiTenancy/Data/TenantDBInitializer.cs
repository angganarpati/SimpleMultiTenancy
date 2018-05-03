using SimpleMultiTenancy.Data.Domain;
using System;
using System.Data.Entity;

namespace SimpleMultiTenancy.Data
{
    public class TenantDBInitializer : DropCreateDatabaseIfModelChanges<TenantContext>
    {
        protected override void Seed(TenantContext context)
        {
            //Create dummy tenant
            for (int i = 0; i < 5; i++)
            {
                var tenant = new Tenant()
                {
                    ApplicationTitle = string.Format("Tenant {0} App", i.ToString()),
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    TenantCode = string.Format("CMP0{0}", i.ToString()),
                    TenantName = string.Format("Company {0}", i.ToString())
                };

                var dbSetting = new DBTenantConnectionString()
                {
                    DataSource = ".\\",
                    IntegratedSecurity = true,
                    MultipleActiveResultSets = true,
                    InitialCatalog = "Tenant_" + tenant.TenantCode,
                    UserID = "",
                    Password = "",
                    TenantID = tenant.TenantID
                };

                context.Tenants.Add(tenant);
                context.DBTenantConnectionStrings.Add(dbSetting);
            }

            context.SaveChanges();

            base.Seed(context);
        }
    }
}
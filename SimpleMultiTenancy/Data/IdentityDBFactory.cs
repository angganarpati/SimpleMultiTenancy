using SimpleMultiTenancy.Infrastructure;
using System;
using System.Data.SqlClient;

namespace SimpleMultiTenancy.Data
{
    public class IdentityDBFactory : IDisposable, IIdentityDBFactory
    {
        private ApplicationDbContext dbContext;

        private ITenantResolver tenantResolver;

        public IdentityDBFactory(
            ITenantResolver tenantResolver)
        {
            this.tenantResolver = tenantResolver;
        }

        public void Dispose()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }

        public ApplicationDbContext Get()
        {
            return dbContext ?? (ChangeConnection());
        }

        public ApplicationDbContext ChangeConnection()
        {
            var connectionString = tenantResolver.GetTenantDBConnectionString;

            //avoid system error while connection string not present
            if (connectionString == null)
            {
                return new ApplicationDbContext();
            }

            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder
            {
                DataSource = connectionString.DataSource,
                InitialCatalog = connectionString.InitialCatalog,
                PersistSecurityInfo = true,
                IntegratedSecurity = connectionString.IntegratedSecurity,
                MultipleActiveResultSets = connectionString.MultipleActiveResultSets,
                UserID = connectionString.UserID,
                Password = connectionString.Password,
            };

            // get default connection string
            dbContext = new ApplicationDbContext(sqlBuilder.ConnectionString);

            return dbContext;
        }
    }
}
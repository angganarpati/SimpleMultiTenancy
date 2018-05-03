using SimpleMultiTenancy.Data.Abstract;
using System;

namespace SimpleMultiTenancy.Data.Domain
{
    public class DBTenantConnectionString : IDBTenantConnectionString
    {
        public DBTenantConnectionString()
        {
            IntegratedSecurity = false;
        }

        public Guid TenantID { get; set; }

        public string DataSource { get; set; }

        public string InitialCatalog { get; set; }

        public bool IntegratedSecurity { get; set; }

        public bool MultipleActiveResultSets { get; set; }

        public string UserID { get; set; }

        public string Password { get; set; }
    }
}
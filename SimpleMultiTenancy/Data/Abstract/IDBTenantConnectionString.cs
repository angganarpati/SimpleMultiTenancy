using System;

namespace SimpleMultiTenancy.Data.Abstract
{
    public interface IDBTenantConnectionString
    {
        Guid TenantID { get; set; }

        string DataSource { get; set; }

        string InitialCatalog { get; set; }

        bool IntegratedSecurity { get; set; }

        bool MultipleActiveResultSets { get; set; }

        string UserID { get; set; }

        string Password { get; set; }
    }
}
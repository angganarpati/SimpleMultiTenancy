using SimpleMultiTenancy.Data.Abstract;

namespace SimpleMultiTenancy.Infrastructure
{
    public interface ITenantResolver
    {
        ITenant GetCurrentTenant { get; }

        ITenantProfile GetTenantProfile { get; }

        IDBTenantConnectionString GetTenantDBConnectionString { get; }
    }
}
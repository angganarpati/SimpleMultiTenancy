namespace SimpleMultiTenancy.Data
{
    public interface ITenantDBFactory : IDbFactory<TenantContext>
    {
    }
}
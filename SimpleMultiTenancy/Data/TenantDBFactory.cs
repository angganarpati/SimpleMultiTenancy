using System;

namespace SimpleMultiTenancy.Data
{
    public class TenantDBFactory : IDisposable, ITenantDBFactory
    {
        private TenantContext dataContext;

        public void Dispose()
        {
            dataContext.Dispose();
        }

        public TenantContext Get()
        {
            return dataContext ?? (dataContext = new TenantContext());
        }
    }
}
using SimpleMultiTenancy.Data.Abstract;
using System;

namespace SimpleMultiTenancy.Data.Domain
{
    public class TenantProfile : Entity, ITenantProfile
    {
        public TenantProfile()
        {
            TenantProfileId = Guid.NewGuid();
        }

        public Guid TenantProfileId { get; set; }

        public Guid TenantId { get; set; }

        public string Email { get; set; }
    }
}
using System;

namespace SimpleMultiTenancy.Data.Abstract
{
    public interface ITenantProfile
    {
        Guid TenantProfileId { get; set; }

        Guid TenantId { get; set; }

        string Email { get; set; }
    }
}
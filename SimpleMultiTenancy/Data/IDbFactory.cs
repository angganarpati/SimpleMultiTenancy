using System;
using System.Data.Entity;

namespace SimpleMultiTenancy.Data
{
    public interface IDbFactory<TContext> : IDisposable
        where TContext : DbContext
    {
        TContext Get();
    }
}

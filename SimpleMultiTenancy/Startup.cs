using Microsoft.Owin;
using Owin;
using SimpleMultiTenancy.Data;
using SimpleMultiTenancy.Infrastructure;

[assembly: OwinStartupAttribute(typeof(SimpleMultiTenancy.Startup))]
namespace SimpleMultiTenancy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            RegisterContainer.Register(app);
            ConfigureAuth(app);

            System.Data.Entity.Database.SetInitializer(new TenantDBInitializer());
        }
    }
}

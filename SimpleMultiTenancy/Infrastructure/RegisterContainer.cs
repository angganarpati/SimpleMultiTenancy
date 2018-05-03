using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using SimpleMultiTenancy.Data;
using System.Web;
using System.Web.Mvc;

namespace SimpleMultiTenancy.Infrastructure
{
    public class RegisterContainer
    {
        public static void Register(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            #region Register all controllers for the assembly

            // Note that ASP.NET MVC requests controllers by their concrete types,
            // so registering them As<IController>() is incorrect.
            // Also, if you register controllers manually and choose to specify
            // lifetimes, you must register them as InstancePerDependency() or
            // InstancePerHttpRequest() - ASP.NET MVC will throw an exception if
            // you try to reuse a controller instance for multiple requests.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            #endregion Register all controllers for the assembly

            #region Register modules

            builder.RegisterAssemblyModules(typeof(MvcApplication).Assembly);

            #endregion Register modules

            #region Model binder providers - excluded - not sure if need

            //builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            //builder.RegisterModelBinderProvider();

            #endregion Model binder providers - excluded - not sure if need

            #region Inject HTTP Abstractions

            /*
             The MVC Integration includes an Autofac module that will add HTTP request
             lifetime scoped registrations for the HTTP abstraction classes. The
             following abstract classes are included:
            -- HttpContextBase
            -- HttpRequestBase
            -- HttpResponseBase
            -- HttpServerUtilityBase
            -- HttpSessionStateBase
            -- HttpApplicationStateBase
            -- HttpBrowserCapabilitiesBase
            -- HttpCachePolicyBase
            -- VirtualPathProvider

            To use these abstractions add the AutofacWebTypesModule to the container
            using the standard RegisterModule method.
            */

            //HTTP context and other related stuff
            builder.RegisterModule(new AutofacWebTypesModule());

            #endregion Inject HTTP Abstractions

            //Register identity stuff

            builder.RegisterType<TenantDBFactory>().As<ITenantDBFactory>().InstancePerRequest();
            builder.RegisterType<TenantResolver>().As<ITenantResolver>().InstancePerRequest();
            builder.RegisterType<IdentityDBFactory>().As<IIdentityDBFactory>().InstancePerRequest();
            builder.RegisterType<ApplicationUserStore>().As<IUserStore<ApplicationUser>>().InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
            builder.Register<IAuthenticationManager>(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            builder.Register<IDataProtectionProvider>(c => app.GetDataProtectionProvider()).InstancePerRequest();

            #region Set the MVC dependency resolver to use Autofac

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            #endregion Set the MVC dependency resolver to use Autofac
        }
    }
}
using System.Web.Http;
using AspNetIdentity.Dal;
using AspNetIdentity.Mapper.Identity.Claim;
using AspNetIdentity.Mapper.Identity.Role;
using AspNetIdentity.Mapper.Identity.User;
using AspNetIdentity.WebApi.IdentityConfig;
using AspNetIdentity.WebApi.Utility.RequestMessageProvider;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;

namespace AspNetIdentity.WebApi
{
    public static class SimpleInjectorInitializer
    {
        public static void Initialize(HttpConfiguration config)
        {
            var container = new Container();

            ConfigureContainer(container);

            // This is an extension method from the integration package.
            container.RegisterWebApiControllers(config);
            container.EnableHttpRequestMessageTracking(config);

            container.Verify();

            config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }

        private static void ConfigureContainer(Container container)
        {
            container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();

            //configure
            container.Register(() => new AspNetIdentityDbContext(), Lifestyle.Scoped);

            ConfigureIdentityService(container);

            container.RegisterSingleton<IRequestMessageProvider>(new RequestMessageProvider(container));

            ConfigureMappers(container);
        }

        private static void ConfigureIdentityService(Container container)
        {
            container.Register(
                () => new UserStore(container.GetInstance<AspNetIdentityDbContext>()), Lifestyle.Scoped);
            container.Register<UserManager>(Lifestyle.Scoped);

            container.Register(
                () => new RoleStore(container.GetInstance<AspNetIdentityDbContext>()), Lifestyle.Scoped);
            container.Register<RoleManager>(Lifestyle.Scoped);
        }

        private static void ConfigureMappers(Container container)
        {
            container.Register<IClaimFactory, ClaimFactory>(Lifestyle.Scoped);
            container.Register<IUserFactory, UserFactory>(Lifestyle.Scoped);
            container.Register<IRoleFactory, RoleFactory>(Lifestyle.Scoped);
        }
    }
}
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Owin;
using SecretSanta.Data;
using SecretSanta.Data.Interfaces;
using SecretSanta.Services;
using SecretSanta.Services.Interfaces;

[assembly: OwinStartup(typeof(SecretSanta.API.Startup))]

namespace SecretSanta.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
           
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<UsersService>().As<IUsersService>().InstancePerRequest();
            builder.RegisterType<GroupsService>().As<IGroupsService>().InstancePerRequest();
            builder.RegisterType<RequestsService>().As<IRequestsService>().InstancePerRequest();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<SecretSantaDbContext>().AsSelf().InstancePerRequest();
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IRepository<>)).InstancePerRequest();

            var container = builder.Build();

            var config = GlobalConfiguration.Configuration;
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            ConfigureAuth(app);
        }
    }
}

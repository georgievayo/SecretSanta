using Ninject.Modules;
using Ninject.Web.Common;
using SecretSantaData;
using SecretSantaData.Contracts;
using SecretSantaServices;
using SecretSantaServices.Contracts;
using SecretSantaServices.Providers;

namespace SecretSantaWeb
{
    public class SecretSantaNinjectModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<SecretSantaDbContext>().ToSelf().InRequestScope();
            this.Bind(typeof(IRepository<>)).To(typeof(GenericRepository<>)).InRequestScope();
            this.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();
            this.Bind<IHttpContextProvider>().To<HttpContextProvider>().InSingletonScope();
            this.Bind<IGroupsService>().To<GroupsService>().InRequestScope();
            this.Bind<IAuthenticationService>().To<AuthenticationService>().InRequestScope();
        }
    }
}
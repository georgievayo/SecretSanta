using Ninject.Modules;
using Ninject.Web.Common;
using SecretSantaData;
using SecretSantaData.Contracts;

namespace SecretSantaWeb
{
    public class SecretSantaNinjectModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<SecretSantaDbContext>().ToSelf();
            this.Bind(typeof(IRepository<>)).To(typeof(GenericRepository<>)).InRequestScope();
            this.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();
        }
    }
}
using System.Security.Principal;
using Microsoft.Owin;

namespace SecretSantaServices.Providers
{
    public interface IHttpContextProvider
    {
        IOwinContext CurrentOwinContext { get; }

        IIdentity CurrentIdentity { get; }

        TManager GetUserManager<TManager>();
    }
}

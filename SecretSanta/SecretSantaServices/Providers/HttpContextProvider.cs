using System.Security.Principal;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace SecretSantaServices.Providers
{
    public class HttpContextProvider : IHttpContextProvider
    {
        public IOwinContext CurrentOwinContext
        {
            get
            {
                return HttpContext.Current.GetOwinContext();
            }
        }

        public IIdentity CurrentIdentity
        {
            get
            {
                return HttpContext.Current.User.Identity;
            }
        }

        public TManager GetUserManager<TManager>()
        {
            return HttpContext.Current.GetOwinContext().GetUserManager<TManager>();
        }
    }
}

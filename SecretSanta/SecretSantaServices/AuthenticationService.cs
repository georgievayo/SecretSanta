using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SecretSantaModels;
using SecretSantaServices.Contracts;
using SecretSantaServices.Managers;
using SecretSantaServices.Providers;

namespace SecretSantaServices
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpContextProvider httpContextProvider;

        public AuthenticationService(IHttpContextProvider httpContextProvider)
        {
            this.httpContextProvider = httpContextProvider;
        }

        protected ApplicationSignInManager SignInManager
        {
            get
            {
                return this.httpContextProvider.GetUserManager<ApplicationSignInManager>();
            }
        }

        protected ApplicationUserManager UserManager
        {
            get
            {
                return this.httpContextProvider.GetUserManager<ApplicationUserManager>();
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return this.httpContextProvider.CurrentIdentity.IsAuthenticated;
            }
        }

        public string CurrentUserId
        {
            get
            {
                return this.httpContextProvider.CurrentIdentity.GetUserId();
            }
        }

        public string CurrentUserUsername
        {
            get
            {
                return this.httpContextProvider.CurrentIdentity.GetUserName();
            }
        }

        public IdentityResult RegisterAndLoginUser(User user, string password, bool isPersistent, bool rememberBrowser)
        {
            var result = this.UserManager.Create(user, password);

            if (result.Succeeded)
            {
                this.SignInManager.SignIn(user, isPersistent, rememberBrowser);
            }

            return result;
        }

        public SignInStatus SignInWithPassword(string email, string password, bool rememberMe, bool shouldLockout)
        {
            return this.SignInManager.PasswordSignIn(email, password, rememberMe, shouldLockout);
        }

        public void SignOut()
        {
            this.httpContextProvider.CurrentOwinContext.Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}

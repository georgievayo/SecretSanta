using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SecretSantaModels;

namespace SecretSantaServices.Contracts
{
    public interface IAuthenticationService
    {
        bool IsAuthenticated { get; }

        string CurrentUserId { get; }

        string CurrentUserUsername { get; }

        IdentityResult RegisterAndLoginUser(User user, string password, bool isPersistent, bool rememberBrowser);

        SignInStatus SignInWithPassword(string email, string password, bool rememberMe, bool shouldLockout);

        void SignOut();
    }
}

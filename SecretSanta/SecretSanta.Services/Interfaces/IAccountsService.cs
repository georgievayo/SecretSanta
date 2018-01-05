using SecretSanta.Models;

namespace SecretSanta.Services.Interfaces
{
    public interface IAccountsService
    {
        void CreateUserSession(string username, string authToken);

        bool InvalidateUserSession();

        UserSession ReValidateSession(string authToken);

        void DeleteExpiredSessions();
    }
}

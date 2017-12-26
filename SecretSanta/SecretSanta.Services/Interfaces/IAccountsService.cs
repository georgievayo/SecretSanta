using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecretSanta.Models;

namespace SecretSanta.Services.Interfaces
{
    public interface IAccountsService
    {
        void CreateUserSession(string username, string authToken);

        void InvalidateUserSession();

        UserSession ReValidateSession(string authToken);

        void DeleteExpiredSessions();
    }
}

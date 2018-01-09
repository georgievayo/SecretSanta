using SecretSanta.Data.Interfaces;
using SecretSanta.Models;
using SecretSanta.Services.Interfaces;
using System;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace SecretSanta.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly IRepository<UserSession> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsersService _usersService;

        public AccountsService(IRepository<UserSession> repository, IUnitOfWork unitOfWork, IUsersService usersService)
        {
            this._repository = repository;
            this._unitOfWork = unitOfWork;
            this._usersService = usersService;
        }

        public void CreateUserSession(string username, string authToken)
        {
            var user = this._usersService.GetUserByUsername(username);

            if(user == null)
            {
                throw new Exception();
            }

            var userId = user.Id;

            var userSession = new UserSession()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                AuthToken = authToken,
                ExpirationDateTime = DateTime.Now + new TimeSpan(0, 30, 0)
            };

            this._repository.Add(userSession);
            this._unitOfWork.SaveChanges();
        }

        public void DeleteExpiredSessions()
        {
            var userSessions = this._repository
                .All
                .Where(session => session.ExpirationDateTime < DateTime.Now);

            foreach (var session in userSessions)
            {
                this._repository.Delete(session);
            }

            this._unitOfWork.SaveChanges();
        }

        public UserSession ReValidateSession(string authToken)
        {
            var userSession = this._repository
                .All
                .FirstOrDefault(session => session.AuthToken == authToken);

            if (userSession == null || userSession.ExpirationDateTime < DateTime.Now)
            {
                return null;
            }

            userSession.ExpirationDateTime = DateTime.Now + new TimeSpan(0, 30, 0);
            this._unitOfWork.SaveChanges();

            return userSession;
        }

        public bool InvalidateUserSession()
        {
            string authToken = GetCurrentBearerAuthrorizationToken();

            if (authToken == null)
            {
                throw new ArgumentNullException();
            }

            var userSession = this._repository
                .All
                .FirstOrDefault(session =>
                session.AuthToken == authToken);

            if (userSession != null)
            {
                this._repository.Delete(userSession);
                this._unitOfWork.SaveChanges();

                return true;
            }

            return false;
        }

        private HttpRequestMessage CurrentRequest
        {
            get
            {
                return (HttpRequestMessage)HttpContext.Current.Items["MS_HttpRequestMessage"];
            }
        }

        private string GetCurrentBearerAuthrorizationToken()
        {
            string authToken = null;
            if (CurrentRequest.Headers.Authorization != null && CurrentRequest.Headers.Authorization.Scheme == "Bearer")
            {
                authToken = CurrentRequest.Headers.Authorization.Parameter;
            }

            return authToken;
        }
    }
}

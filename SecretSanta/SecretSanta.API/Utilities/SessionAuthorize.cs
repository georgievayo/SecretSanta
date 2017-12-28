using SecretSanta.Data;
using SecretSanta.Models;
using SecretSanta.Services;
using SecretSanta.Services.Interfaces;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace SecretSanta.API.Utilities
{
    public class SessionAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly IAccountsService _accountsService;

        public SessionAuthorizeAttribute()
        {
            var dbContext = new SecretSantaDbContext();
            var userSessionRepository = new GenericRepository<UserSession>(dbContext);
            var userSessionunitOfWork = new UnitOfWork(dbContext);
            var userRepository = new GenericRepository<User>(dbContext);
            var usersService = new UsersService(userRepository, userSessionunitOfWork);
            this._accountsService = new AccountsService(userSessionRepository, userSessionunitOfWork, usersService);
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (SkipAuthorization(actionContext))
            {
                return;
            }

            if (this._accountsService.ReValidateSession())
            {
                base.OnAuthorization(actionContext);
            }
            else
            {
                actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(
                    HttpStatusCode.Unauthorized, "Session token expried or not valid.");
            }
        }

        private static bool SkipAuthorization(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                   || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }
    }
}
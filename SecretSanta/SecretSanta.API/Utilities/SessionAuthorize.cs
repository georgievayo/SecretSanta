using System.Collections.Generic;
using SecretSanta.Services.Interfaces;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Autofac.Integration.WebApi;
using SecretSanta.API.Controllers;

namespace SecretSanta.API.Utilities
{
    public class SessionAuthorizeAttribute : ActionFilterAttribute, IAutofacActionFilter
    {
        private readonly IAccountsService _accountsService;

        public SessionAuthorizeAttribute(IAccountsService accountsService)
        {
            this._accountsService = accountsService;
        }

        public override async Task OnActionExecutingAsync(HttpActionContext actionContext,
            CancellationToken cancellationToken)
        {
            if (SkipAuthorization(actionContext))
            {
                return;
            }

            IEnumerable<string> authValues;
            if (!actionContext.Request.Headers.TryGetValues("Authorization", out authValues))
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            var authToken = authValues.First().Substring(7);
            var userSession = _accountsService.ReValidateSession(authToken);

            if (userSession == null)
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            if (actionContext.ControllerContext.Controller.GetType() == typeof(GroupsController))
            {
                ((GroupsController)actionContext.ControllerContext.Controller)
                    .SetCurrentUserId(userSession.UserId);
            }
            else
            {
                ((UsersController)actionContext.ControllerContext.Controller)
                    .SetCurrentUser(userSession.UserId, userSession.User.UserName);
            }

            await base.OnActionExecutingAsync(actionContext, cancellationToken);
        }

        private static bool SkipAuthorization(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                   || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }
    }
}
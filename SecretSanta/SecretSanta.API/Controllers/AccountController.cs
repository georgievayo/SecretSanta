using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Testing;
using SecretSanta.API.Models;
using SecretSanta.Services.Interfaces;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace SecretSanta.API.Controllers
{
    [RoutePrefix("api/login")]
    public class AccountController : ApiController
    {
        private readonly IAccountsService _accountsService;

        private IAuthenticationManager Authentication
        {
            get
            {
                return Request.GetOwinContext().Authentication;
            }
        }

        public AccountController(IAccountsService accountsService)
        {
            this._accountsService = accountsService;
        }

        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> LoginUser(LoginViewModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Invalid user data");
            }

            var testServer = TestServer.Create<Startup>();
            var requestParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", model.Username),
                new KeyValuePair<string, string>("password", model.Password)
            };
            var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
            var tokenServiceResponse = await testServer.HttpClient.PostAsync(
                Startup.TokenEndpointPath, requestParamsFormUrlEncoded);

            if (tokenServiceResponse.StatusCode == HttpStatusCode.OK)
            {
                var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
                var jsSerializer = new JavaScriptSerializer();
                var responseData =
                    jsSerializer.Deserialize<Dictionary<string, string>>(responseString);
                var authToken = responseData["access_token"];
                var username = responseData["userName"];

                this._accountsService.CreateUserSession(username, authToken);

                this._accountsService.DeleteExpiredSessions();
            }

            return this.ResponseMessage(tokenServiceResponse);
        }

        [HttpDelete]
        [Route("")]
        public IHttpActionResult Logout()
        {
            this.Authentication.SignOut(DefaultAuthenticationTypes.ExternalBearer);
            this._accountsService.InvalidateUserSession();

            return this.Ok();
        }
    }
}
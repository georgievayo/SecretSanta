using System;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Testing;
using SecretSanta.API.Models;
using SecretSanta.Services.Interfaces;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace SecretSanta.API.Controllers
{
    [RoutePrefix("api/login")]
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class AccountController : ApiController
    {
        private readonly IAccountsService _accountsService;

        public AccountController(IAccountsService accountsService)
        {
            this._accountsService = accountsService;
        }

        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Login(LoginViewModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return this.BadRequest("Username or password is not correct.");
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

            if (tokenServiceResponse.IsSuccessStatusCode)
            {
                var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
                var jsSerializer = new JavaScriptSerializer();
                var responseData =
                    jsSerializer.Deserialize<Dictionary<string, string>>(responseString);
                var authToken = responseData["access_token"];
                var username = responseData["userName"];

                try
                {
                    this._accountsService.CreateUserSession(username, authToken);
                }
                catch (Exception)
                {
                    return NotFound();
                }

                this._accountsService.DeleteExpiredSessions();
            }
            else
            {
                return NotFound();
            }


            return this.ResponseMessage(tokenServiceResponse);
        }

        [HttpDelete]
        [Route("")]
        public IHttpActionResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ExternalBearer);

            try
            {
                var isInvalidated = this._accountsService.InvalidateUserSession();
                if (!isInvalidated)
                {
                    return this.NotFound();
                }
            }
            catch (ArgumentNullException)
            {
                return this.BadRequest();
            }

            return this.Ok();
        }
    }
}
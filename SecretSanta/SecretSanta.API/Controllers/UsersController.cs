using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using SecretSanta.API.Models;
using SecretSanta.Models;
using SecretSanta.Services.Interfaces;

namespace SecretSanta.API.Controllers
{

    [RoutePrefix("api/users")]
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class UsersController : ApiController
    {
        private ApplicationUserManager _userManager;
        private readonly IUsersService _usersService;
        private readonly IGroupsService _groupsService;

        public UsersController(IUsersService usersService, IGroupsService groupsService)
        {
            this._usersService = usersService;
            this._groupsService = groupsService;
        }

        //public AccountController(ApplicationUserManager userManager,
        //    ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        //{
        //    UserManager = userManager;
        //    AccessTokenFormat = accessTokenFormat;
        //}

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        [HttpDelete]
        [Authorize]
        [Route("logins")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User()
            {
                UserName = model.Username,
                Email = model.Email,
                DisplayName = model.DisplayName,
                Age = model.Age,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                Interests = model.Interests,
            };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Created("~api/users", user);
        }

        [HttpGet]
        [Authorize]
        [Route("{username}")]
        public IHttpActionResult GetProfile(string username)
        {
            if (username == null)
            {
                return BadRequest();
            }

            var user = this._usersService.GetUserByUsername(username);
            if (user == null)
            {
                return NotFound();
            }

            var model = new UserProfileViewModel()
            {
                Username = user.UserName,
                DisplayName = user.DisplayName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                Age = user.Age,
                Interests = user.Interests,
                PhotoUrl = user.PhotoUrl
            };

            return Ok(model);
        }

        [HttpGet]
        [Authorize]
        [Route("")]
        public IHttpActionResult GetAllUsers([FromUri]ViewCriteria criteria)
        {
            if (criteria.Order.ToLower() != "asc" && criteria.Order.ToLower() != "desc")
            {
                return BadRequest();
            }

            var users = this._usersService
                .GetUsers(criteria.Skip, criteria.Take, criteria.Order, criteria.Search)
                .ToList();

            return Ok(users);
        }

        [HttpGet]
        [Authorize]
        [Route("{username}/groups")]
        public IHttpActionResult GetUserGroups(string username, [FromUri]PagingCriteria criteria)
        {
            if (username == null)
            {
                return BadRequest();
            }

            try
            {
                var groups = this._usersService.GetUserGroups(username, criteria.Skip, criteria.Take);
                // should be mapped
                return Ok(groups);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound();
            }

        }

        [HttpGet]
        [Authorize]
        [Route("{username}/requests")]
        public IHttpActionResult GetAllRequests(string username, [FromUri] ViewCriteria criteria)
        {
            if (criteria.Order.ToLower() != "asc" && criteria.Order.ToLower() != "desc")
            {
                return BadRequest();
            }

            var user = this._usersService.GetUserByUsername(username);
            if (user == null)
            {
                return NotFound();
            }

            var currentUserId = RequestContext.Principal.Identity.GetUserId();
            if (currentUserId != user.Id)
            {
                return Content(HttpStatusCode.Forbidden, "You cannot see other's requests.");
            }

            if (criteria.Order.ToLower() == "asc")
            {
                var requests = user.Requests
                    .OrderBy(r => r.ReceivedAt)
                    .Skip(criteria.Skip)
                    .Take(criteria.Take)
                    .Select(r => new RequestViewModel()
                    {
                        Date = r.ReceivedAt,
                        GroupName = r.Group.Name,
                        OwnerName = r.Group.Owner.DisplayName,
                        Id = r.Id
                    })
                    .ToList();

                return Ok(requests);
            }
            else
            {
                var requests = user.Requests
                    .OrderByDescending(r => r.ReceivedAt)
                    .Skip(criteria.Skip)
                    .Take(criteria.Take)
                    .Select(r => new RequestViewModel()
                    {
                        Date = r.ReceivedAt,
                        GroupName = r.Group.Name,
                        OwnerName = r.Group.Owner.DisplayName,
                        Id = r.Id
                    })
                    .ToList();

                return Ok(requests);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("{username}/requests")]
        public IHttpActionResult SendRequest(string username, [FromBody] RequestViewModel request)
        {
            if (username == null || request.GroupName == null || request.OwnerName == null)
            {
                return BadRequest();
            }

            var user = this._usersService.GetUserByUsername(username);
            var group = this._groupsService.GetGroupByName(request.GroupName);
            if (user == null || group.Name == null)
            {
                return NotFound();
            }

            var currentUserUsername = RequestContext.Principal.Identity.Name;
            if (currentUserUsername != request.OwnerName)
            {
                return Content(HttpStatusCode.Forbidden, "You cannot send requests for this group.");
            }

            var foundRequst = user.Requests.First(r => r.ReceivedAt == request.Date && r.Group.Name == request.GroupName);
            if (foundRequst != null)
            {
                return Content(HttpStatusCode.Conflict, "You have already sent request to this user.");
            }

            var requestToSend = new Request()
            {
                ReceivedAt = request.Date,
                To = user,
                Id = Guid.NewGuid(),
                Group = group
            };

            this._usersService.AddRequest(requestToSend, user);
            return Created("requests", requestToSend);
        }

        [HttpDelete]
        [Authorize]
        [Route("{username}/requests/{id}")]
        public IHttpActionResult DeleteRequest(string username, string id)
        {
            if (username == null || id == null)
            {
                return BadRequest();
            }

            var user = this._usersService.GetUserByUsername(username);
            if (user == null)
            {
                return NotFound();
            }

            var currentUserUsername = RequestContext.Principal.Identity.Name;
            if (user.UserName != currentUserUsername)
            {
                return Content(HttpStatusCode.Forbidden, "You cannot delete this request.");
            }

            var request = user.Requests.First(r => r.Id == Guid.Parse(id));
            if (request == null)
            {
                return NotFound();
            }

            this._usersService.DeleteRequest(request, user);
            return Content(HttpStatusCode.NoContent, "Deleted");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}

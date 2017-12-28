using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
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
        private readonly IRequestsService _requestsService;
        private readonly IConnectionsService _connectionsService;
        private string _currentUserUsername;
        private string _currentUserId;

        public UsersController(IUsersService usersService, 
            IGroupsService groupsService, 
            IRequestsService requestsService,
            IConnectionsService connectionsService)
        {
            this._usersService = usersService;
            this._groupsService = groupsService;
            this._requestsService = requestsService;
            this._connectionsService = connectionsService;
        }

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

        public void SetCurrentUser(string id, string username)
        {
            this._currentUserId = id;
            this._currentUserUsername = username;
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
            if (result.Succeeded)
            {
                var resultModel = new UserProfileViewModel(user.Email, user.UserName, user.Age, user.Interests,
                    user.PhoneNumber, user.DisplayName, user.Address);

                return Content(HttpStatusCode.Created, resultModel);
            }
            else
            {
                return Content(HttpStatusCode.Conflict, "There is user with the same username!");
            }
            
        }

        [HttpGet]
        [Route("{username}")]
        public IHttpActionResult GetProfile([FromUri] string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest();
            }

            var user = this._usersService.GetUserByUsername(username);
            if (user == null)
            {
                return NotFound();
            }

            var model = new UserProfileViewModel(user.Email, user.UserName, user.Age, 
                user.Interests, user.PhoneNumber, user.DisplayName, user.Address);

            return Ok(model);
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllUsers([FromUri]ViewCriteria criteria)
        {
            if (criteria.Order.ToLower() != "asc" && criteria.Order.ToLower() != "desc")
            {
                return BadRequest();
            }

            var users = this._usersService
                .GetUsers(criteria.Skip, criteria.Take, criteria.Order, criteria.Search)
                .Select(u => new UserShortViewModel(u.UserName, u.DisplayName, u.PhoneNumber, u.Email))
                .ToList();

            return Ok(users);
        }

        [HttpGet]
        [Route("{username}/groups")]
        public IHttpActionResult GetUserGroups(string username, [FromUri]PagingCriteria criteria)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest();
            }

            try
            {
                var groups = this._usersService.GetUserGroups(username, criteria.Skip, criteria.Take)
                     .Select(g => new { GroupName = g.Name });

                return Ok(groups);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }

        }

        [HttpGet]
        [Route("{username}/groups/{groupName}/connections")]
        public IHttpActionResult GetUserConnectionInGroup([FromUri] string username, [FromUri] string groupName)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(groupName))
            {
                return BadRequest();
            }

            var connection = this._connectionsService.GetUserConnection(username, groupName);
            if (connection == null)
            {
                return NotFound();
            }

            var result = new SecretSantaViewModel(connection.To.UserName);

            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        [Route("{username}/requests")]
        public IHttpActionResult GetAllRequests([FromUri] string username, [FromUri] ViewCriteria criteria)
        {
            if (string.IsNullOrEmpty(username) || 
                (criteria.Order.ToLower() != "asc" && criteria.Order.ToLower() != "desc"))
            {
                return BadRequest();
            }

            var user = this._usersService.GetUserByUsername(username);
            if (user == null)
            {
                return NotFound();
            }

            if (this._currentUserId != user.Id)
            {
                return Content(HttpStatusCode.Forbidden, "You can see only your requests.");
            }

            var requests = this._requestsService.GetUserRequests(this._currentUserId, criteria.Order, criteria.Skip, criteria.Take)
                .Select(r => new RequestViewModel(r.Id, r.Group.Name, r.Group.Owner.DisplayName, r.ReceivedAt));

            return Ok(requests);
        }

        [HttpPost]
        [Route("{username}/requests")]
        public IHttpActionResult SendRequest([FromUri] string username, [FromBody] RequestViewModel request)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(request.GroupName))
            {
                return BadRequest();
            }

            var user = this._usersService.GetUserByUsername(username);
            var group = this._groupsService.GetGroupByName(request.GroupName);
            if (user == null || group.Name == null)
            {
                return NotFound();
            }

            if (this._currentUserUsername != request.OwnerName)
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
            var result = new RequestViewModel(requestToSend.Id, requestToSend.Group.Name, requestToSend.To.UserName,
                requestToSend.ReceivedAt);

            return Content(HttpStatusCode.Created, result);
        }

        [HttpDelete]
        [Route("{username}/requests/{id}")]
        public IHttpActionResult DeleteRequest([FromUri] string username, [FromUri] string id)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var user = this._usersService.GetUserByUsername(username);
            if (user == null)
            {
                return NotFound();
            }

            if (user.UserName != this._currentUserUsername)
            {
                return Content(HttpStatusCode.Forbidden, "You can delete only your requests.");
            }

            var request = user.Requests.First(r => r.Id == Guid.Parse(id));
            if (request == null)
            {
                return NotFound();
            }

            this._requestsService.DeleteRequest(request);
            return Content(HttpStatusCode.NoContent, "The request was deleted!");
        }
    }
}

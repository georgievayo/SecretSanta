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

            return Created("~api/users", user);
        }

        [HttpGet]
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
        [Route("")]
        public IHttpActionResult GetAllUsers([FromUri]ViewCriteria criteria)
        {
            if (criteria.Order.ToLower() != "asc" && criteria.Order.ToLower() != "desc")
            {
                return BadRequest();
            }

            var users = this._usersService
                .GetUsers(criteria.Skip, criteria.Take, criteria.Order, criteria.Search)
                .Select(u => new { Id = u.Id, UserName = u.UserName,  DisplayName = u.DisplayName, PhoneNumber = u.PhoneNumber, Email = u.Email})
                .ToList();

            return Ok(users);
        }

        [HttpGet]
        [Route("{username}/groups")]
        public IHttpActionResult GetUserGroups(string username, [FromUri]PagingCriteria criteria)
        {
            if (username == null)
            {
                return BadRequest();
            }

            try
            {
                var groups = this._usersService.GetUserGroups(username, criteria.Skip, criteria.Take)
                     .Select(g => new {GroupName = g.Name});

                return Ok(groups);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }

        }

        [HttpGet]
        [Route("{username}/groups/{groupName}/connections")]
        public IHttpActionResult GetUserConnectionInGroup(string username, string groupName)
        {
            var connection = this._connectionsService.GetUserConnection(username, groupName);
            if (connection == null)
            {
                return NotFound();
            }

            var result = new {receiver = connection.To.UserName};
            return Ok(result);
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

            if (this._currentUserId != user.Id)
            {
                return Content(HttpStatusCode.Forbidden, "You cannot see other's requests.");
            }

            var requests = this._requestsService.GetUserRequests(this._currentUserId)
                .Skip(criteria.Skip)
                .Take(criteria.Take)
                .Select(r => new RequestViewModel()
                {
                    Date = r.ReceivedAt,
                    GroupName = r.Group.Name,
                    OwnerName = r.Group.Owner.DisplayName,
                    Id = r.Id
                });

            if (criteria.Order.ToLower() == "asc")
            {
                var ordered = requests
                    .OrderBy(r => r.Date)
                    .ToList();

                return Ok(ordered);
            }
            else
            {
                var ordered = requests
                    .OrderByDescending(r => r.Date)
                    .ToList();

                return Ok(ordered);
            }
        }

        [HttpPost]
        [Route("{username}/requests")]
        public IHttpActionResult SendRequest(string username, [FromBody] RequestViewModel request)
        {
            if (username == null || request.GroupName == null)
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
            return Created("requests", requestToSend);
        }

        [HttpDelete]
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

            if (user.UserName != this._currentUserUsername)
            {
                return Content(HttpStatusCode.Forbidden, "You cannot delete this request.");
            }

            var request = user.Requests.First(r => r.Id == Guid.Parse(id));
            if (request == null)
            {
                return NotFound();
            }

            this._requestsService.DeleteRequest(request);
            return Content(HttpStatusCode.NoContent, "Deleted");
        }
    }
}

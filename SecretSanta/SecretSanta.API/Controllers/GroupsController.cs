using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using SecretSanta.API.Models;
using SecretSanta.Services.Interfaces;

namespace SecretSanta.API.Controllers
{

    [RoutePrefix("api/groups")]
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class GroupsController : ApiController
    {
        private readonly IGroupsService _groupsService;
        private readonly IRequestsService _requestsService;
        private readonly IUsersService _usersService;
        private readonly IConnectionsService _connectionsService;
        private string _currentUserId;

        public GroupsController(IGroupsService groupsService,
            IUsersService usersService,
            IRequestsService requestsService,
            IConnectionsService connectionsService)
        {
            this._groupsService = groupsService;
            this._usersService = usersService;
            this._requestsService = requestsService;
            this._connectionsService = connectionsService;
        }

        public void SetCurrentUserId(string id)
        {
            this._currentUserId = id;
        }

        [HttpGet]
        [Route("{groupName}")]
        public IHttpActionResult GetGroup([FromUri] string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                return BadRequest();
            }

            var group = this._groupsService.GetGroupByName(groupName);
            var model = new GroupViewModel(group.Name, group.Owner.DisplayName);
            if (group.OwnerId == this._currentUserId)
            {
                var participants = group.Users
                    .Select(u => new UserShortViewModel(u.UserName, u.DisplayName, u.PhoneNumber, u.Email))
                    .ToList();
                model.Participants = participants;
            }

            return Ok(model);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateGroup([FromBody] CreateGroupViewModel groupModel)
        {
            if (groupModel == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentUser = this._usersService.GetUserById(this._currentUserId);

            try
            {
                var group = this._groupsService.CreateGroup(groupModel.Name, currentUser);
                var participants = group.Users.Select(
                    u => new UserShortViewModel(u.UserName, u.DisplayName, u.PhoneNumber, u.Email))
                    .ToList();

                var model = new GroupViewModel(group.Name, group.Owner.DisplayName, participants);

                return Content(HttpStatusCode.Created, model);
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.Conflict, "The name should be unique!");
            }
        }

        [HttpGet]
        [Route("{groupName}/participants")]
        public IHttpActionResult GetParticipants(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                return BadRequest();
            }

            var group = this._groupsService.GetGroupByName(groupName);

            if (group == null)
            {
                return NotFound();
            }

            if (group.OwnerId != this._currentUserId)
            {
                return Content(HttpStatusCode.Forbidden, "Only the owner of the group can see participants!");
            }

            var participants = group.Users
                .Select(p => new ParticipantViewModel(p.UserName, p.DisplayName, p.Email));

            return Ok(participants);
        }

        [HttpPost]
        [Route("{groupName}/participants")]
        public IHttpActionResult PostParticipant([FromUri] string groupName, [FromBody] AddParticipantViewModel userModel)
        {
            if (string.IsNullOrEmpty(groupName) || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = this._usersService.GetUserByUsername(userModel.Username);
            if (user == null)
            {
                return NotFound();
            }

            var hasRequest = this._requestsService.AlreadyHasRequest(user.Id, groupName);
            if (!hasRequest)
            {
                return Content(HttpStatusCode.Forbidden, "You do not have request for this group.");
            }

            this._groupsService.AddUserToGroup(groupName, user);

            return Content(HttpStatusCode.Created, "The user was added!");
        }

        [HttpDelete]
        [Route("{groupName}/participants/{username}")]
        public IHttpActionResult DeleteParticipant([FromUri]string groupName, [FromUri] string username)
        {
            if (string.IsNullOrEmpty(groupName) || string.IsNullOrEmpty(username))
            {
                return BadRequest();
            }

            var user = this._usersService.GetUserByUsername(username);
            if (user == null)
            {
                return NotFound();
            }

            var group = this._groupsService.GetGroupByName(groupName);
            if (group == null)
            {
                return NotFound();
            }

            if (group.OwnerId != this._currentUserId)
            {
                return Content(HttpStatusCode.Forbidden, "Only the owner of group can remove participants!");
            }

            if (group.IsProcessStarted)
            {
                return Content(HttpStatusCode.Forbidden,
                    "You cannot remove user because the process of connections is already started!");
            }

            this._groupsService.RemoveUserFromGroup(group, user);
            return Content(HttpStatusCode.NoContent, "Deleted");
        }

        [HttpPut]
        [Route("{groupName}/connections")]
        public IHttpActionResult ConnectPeople([FromUri] string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                return BadRequest();
            }

            var group = this._groupsService.GetGroupByName(groupName);
            if (group == null)
            {
                return NotFound();
            }

            if (group.OwnerId != this._currentUserId)
            {
                return Content(HttpStatusCode.Forbidden, "Only the owner of group can start the process of connection!");
            }

            var participants = this._groupsService.GetParticipantsOfGroup(groupName);
            if (participants.Count == 1 || group.IsProcessStarted)
            {
                return Content(HttpStatusCode.PreconditionFailed, "The process of connection cannot be started!");
            }

            this._connectionsService.SaveConnections(participants, group);
            this._groupsService.SetThatProcessIsStarted(group);

            return Content(HttpStatusCode.Created, "The process of connection was started!");
        }
    }
}

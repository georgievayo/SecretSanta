using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using SecretSanta.API.Models;
using SecretSanta.Models;
using SecretSanta.Services.Interfaces;

namespace SecretSanta.API.Controllers
{

    [RoutePrefix("api/groups")]
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class GroupsController : ApiController
    {
        private readonly IGroupsService _groupsService;
        private readonly IUsersService _usersService;
        private readonly IConnectionsService _connectionsService;
        private string _currentUserId;

        public GroupsController(IGroupsService groupsService, 
            IUsersService usersService, 
            IConnectionsService connectionsService)
        {
            this._groupsService = groupsService;
            this._usersService = usersService;
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
            if (groupName == null)
            {
                return BadRequest();
            }

            var group = this._groupsService.GetGroupByName(groupName);

            if (group.OwnerId == this._currentUserId)
            {
                var model = new GroupViewModel(group.Name, group.Owner.DisplayName, group.Users);
                return Ok(model);
            }
            else
            {
                var model = new GroupViewModel(group.Name, group.Owner.DisplayName);
                return Ok(model);
            }
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateGroup([FromBody] CreateGroupViewModel groupModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var currentUser = this._usersService.GetUserById(this._currentUserId);
                var group = this._groupsService.CreateGroup(groupModel.Name, currentUser);

                if (group == null)
                {
                    return BadRequest();
                }

                var model = new GroupViewModel(group.Name, group.Owner.DisplayName);

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
            if (participants.Count == 1)
            {
                return Content(HttpStatusCode.PreconditionFailed, "The process of connection cannot be started!");
            }

            var connections = this.GenerateConnections(participants);

            foreach (var pair in connections)
            {
                this._connectionsService.AddConnection(participants.ElementAt(pair.Key),
                    participants.ElementAt(pair.Value), group);
            }

            return Content(HttpStatusCode.Created, "The process of connection was started!");
        }

        private IDictionary<int, int> GenerateConnections(ICollection<User> participants)
        {
            var count = participants.Count;

            Dictionary<int, int> pairs = new Dictionary<int, int>();
            bool[] used = new bool[count];

            var rand = new Random();

            for (int i = 0; i < count; i++)
            {
                var connectToIndex = rand.Next(0, count);
                while (connectToIndex == i || used[connectToIndex])
                {
                    connectToIndex = rand.Next(0, count);
                }

                pairs[i] = connectToIndex;
                used[connectToIndex] = true;
            }

            return pairs;
        }
    }
}

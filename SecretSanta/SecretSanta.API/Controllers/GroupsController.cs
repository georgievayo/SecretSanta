using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.AspNet.Identity;
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
                var model = new { GroupName = group.Name, Owner = group.Owner.DisplayName, Participants = group.Users };
                return Ok(model);
            }
            else
            {
                var model = new { GroupName = group.Name, Owner = group.Owner.DisplayName };
                return Ok(model);
            }
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateGroup([FromBody] string groupName)
        {
            try
            {
                var currentUser = this._usersService.GetUserById(this._currentUserId);
                var group = this._groupsService.CreateGroup(groupName, currentUser);

                if (group == null)
                {
                    return BadRequest();
                }

                return Created("~api/groups", group);
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
            if (groupName == null)
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
                return Content(HttpStatusCode.Forbidden, new List<ParticipantViewModel>());
            }

            var participants = group.Users
                .Select(p => new ParticipantViewModel()
                {
                    Username = p.UserName,
                    DisplayName = p.DisplayName,
                    Email = p.Email,
                    PhotoUrl = p.PhotoUrl
                }
                );

            return Ok(participants);
        }

        [HttpPost]
        [Route("{groupName}/participants")]
        public IHttpActionResult PostParticipant([FromUri] string groupName, [FromBody] string username)
        {
            if (groupName == null || username == null)
            {
                return BadRequest();
            }

            var user = this._usersService.GetUserByUsername(username);
            if (user == null)
            {
                return NotFound();
            }
            // Should return 403 if user has not such request
            this._groupsService.AddUserToGroup(groupName, user);
            return Created("participants", user);
        }

        [HttpDelete]
        [Route("{groupName}/participants/{username}")]
        public IHttpActionResult DeleteParticipant(string groupName, string username)
        {
            if (groupName == null || username == null)
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
                return Content(HttpStatusCode.Forbidden, "Not an owner");
            }

            this._groupsService.RemoveUserFromGroup(group, user);
            return Content(HttpStatusCode.NoContent, "Deleted");
        }

        [HttpPut]
        [Route("{groupName}/connections")]
        public IHttpActionResult ConnectPeople([FromUri] string groupName)
        {
            if (groupName == null)
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
                return Content(HttpStatusCode.Forbidden, "Not an owner");
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

            return Created($"~/api/groups/{groupName}/connections", "The process of connection was started!");
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

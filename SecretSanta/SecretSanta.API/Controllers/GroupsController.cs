using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.AspNet.Identity;
using SecretSanta.API.Models;
using SecretSanta.Services.Interfaces;

namespace SecretSanta.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/groups")]
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class GroupsController : ApiController
    {
        private readonly IGroupsService _groupsService;
        private readonly IUsersService _usersService;

        public GroupsController(IGroupsService groupsService, IUsersService usersService)
        {
            this._groupsService = groupsService;
            this._usersService = usersService;
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateGroup([FromBody] string groupName)
        {
            var currentUserId = RequestContext.Principal.Identity.GetUserId();
            try
            {
                var group = this._groupsService.CreateGroup(groupName, currentUserId);

                if (group == null)
                {
                    return BadRequest();
                }

                return Created("~api/groups", group);
            }
            catch (Exception ex)
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

            var currentUserId = RequestContext.Principal.Identity.GetUserId();
            var group = this._groupsService.GetGroupByName(groupName);

            if (group == null)
            {
                return NotFound();
            }

            if (group.OwnerId != currentUserId)
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

            var currentUserId = RequestContext.Principal.Identity.GetUserId();
            if (group.OwnerId != currentUserId)
            {
                return Content(HttpStatusCode.Forbidden, "Not an owner");
            }

            this._groupsService.RemoveUserFromGroup(group, user);
            return Content(HttpStatusCode.NoContent, "Deleted");
        }
    }
}

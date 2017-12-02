using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using SecretSanta.API.Models;
using SecretSanta.Models;
using SecretSanta.Services.Interfaces;

namespace SecretSanta.API.Controllers
{
    [RoutePrefix("api/groups")]
    public class GroupsController : ApiController
    {
        private readonly IGroupsService _groupsService;
        private readonly IUsersService _usersService;

        public GroupsController(IGroupsService groupsService, IUsersService usersService)
        {
            this._groupsService = groupsService;
            this._usersService = usersService;
        }

        [Authorize]
        [HttpPost]
        [Route("")]
        public Group CreateGroup([FromBody] string groupName)
        {
            var currentUserId = RequestContext.Principal.Identity.GetUserId();
            var group = this._groupsService.CreateGroup(groupName, currentUserId);

            return group;
        }

        [Authorize]
        [HttpGet]
        [Route("{groupName}/participants")]
        public IEnumerable<ParticipantViewModel> GetParticipants([FromUri]string groupName)
        {
            var participants = this._groupsService.GetAllParticipantsOfGroup(groupName).Select(p => new ParticipantViewModel()
                {
                    Username = p.UserName,
                    DisplayName = p.DisplayName,
                    Email = p.Email,
                    PhotoUrl = p.PhotoUrl
                }
            );

            return participants;
        }

        [HttpPost]
        [Route("{groupName}/participants")]
        public IHttpActionResult PostParticipant([FromUri] string groupName, [FromBody] string username)
        {
            var user = this._usersService.GetUserByUsername(username);
            this._groupsService.AddUserToGroup(groupName, user);
            return Ok();
        }

        [HttpDelete]
        [Route("{groupName}/participants")]
        public IHttpActionResult DeleteParticipant([FromUri] string groupName, [FromBody] string username)
        {
            var user = this._usersService.GetUserByUsername(username);
            this._groupsService.RemoveUserFromGroup(groupName, user);
            return Ok();
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using SecretSantaModels;
using SecretSantaServices.Contracts;
using SecretSantaWeb.Models;

namespace SecretSantaWeb.Controllers
{
    [RoutePrefix("groups")]
    public class GroupsController : ApiController
    {
        private readonly IGroupsService _groupsService;
        private readonly IAuthenticationService _authService;

        public GroupsController(IGroupsService groupsService, IAuthenticationService authService)
        {
            this._groupsService = groupsService;
            this._authService = authService;
        }

        [HttpPost]
        [Route("")]
        public Group CreateGroup([FromBody] string groupName)
        {
            var currentUserId = this._authService.CurrentUserId;
            var group = this._groupsService.CreateGroup(groupName, currentUserId);

            return group;
        }

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
    }
}

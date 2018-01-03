using System.Collections.Generic;
using SecretSanta.Models;

namespace SecretSanta.API.Models
{
    public class GroupViewModel
    {
        public GroupViewModel(string groupName, string ownerName)
        {
            GroupName = groupName;
            OwnerName = ownerName;
        }

        public GroupViewModel(string groupName, string ownerName, ICollection<UserShortViewModel> participants)
            :this(groupName, ownerName)
        {
            Participants = participants;
        }

        public string GroupName { get; set; }

        public string OwnerName { get; set; }

        public ICollection<UserShortViewModel> Participants { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using SecretSantaModels;

namespace SecretSantaServices.Contracts
{
    public interface IGroupsService
    {
        Group CreateGroup(string name, string ownerId);

        void AddUserToGroup(string groupName, User user);

        void RemoveUserFromGroup(string groupName, User user);

        ICollection<User> GetAllParticipantsOfGroup(string name);
    }
}

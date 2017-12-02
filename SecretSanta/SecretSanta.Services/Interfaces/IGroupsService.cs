﻿using System.Collections.Generic;
using SecretSanta.Models;

namespace SecretSanta.Services.Interfaces
{
    public interface IGroupsService
    {
        Group CreateGroup(string name, string ownerId);

        void AddUserToGroup(string groupName, User user);

        void RemoveUserFromGroup(Group group, User user);

        Group GetGroupByName(string name);
    }
}

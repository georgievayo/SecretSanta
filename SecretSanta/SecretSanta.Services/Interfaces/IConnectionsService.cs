using System;
using System.Collections.Generic;
using SecretSanta.Models;

namespace SecretSanta.Services.Interfaces
{
    public interface IConnectionsService
    {
        ICollection<Connection> GetGroupConnections(Guid groupId);

        Connection AddConnection(User from, User to, Group group);

        Connection GetUserConnection(string username, string groupName);

        void SaveConnections(ICollection<User> participants, Group group);
    }
}

using System.Collections.Generic;
using SecretSanta.Models;

namespace SecretSanta.Services.Interfaces
{
    public interface IUsersService
    {
        User GetUserByUsername(string username);

        User GetUserById(string id);

        IEnumerable<User> GetUsers(int skip, int take, string order, string search);

        ICollection<Group> GetUserGroups(string username, int skip, int take);

        void AddRequest(Request requestToSend, User user);

        void DeleteRequest(Request request, User user);
    }
}

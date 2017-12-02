using System.Collections.Generic;
using SecretSanta.Models;

namespace SecretSanta.Services.Interfaces
{
    public interface IUsersService
    {
        User GetUserByUsername(string username);

        IEnumerable<User> GetUsers(int skip, int take, string order, string search);
    }
}

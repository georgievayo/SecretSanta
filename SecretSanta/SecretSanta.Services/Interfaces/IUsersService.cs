using SecretSanta.Models;

namespace SecretSanta.Services.Interfaces
{
    public interface IUsersService
    {
        User GetUserByUsername(string username);

        User FindUser(string userName, string password);
    }
}

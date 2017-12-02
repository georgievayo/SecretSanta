using SecretSantaModels;

namespace SecretSantaServices.Contracts
{
    public interface IUsersService
    {
        User GetUserByUsername(string username);

        User FindUser(string userName, string password);
    }
}

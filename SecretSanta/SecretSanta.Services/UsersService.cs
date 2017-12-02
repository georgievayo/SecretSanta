using System.Linq;
using SecretSanta.Data.Interfaces;
using SecretSanta.Models;
using SecretSanta.Services.Interfaces;

namespace SecretSanta.Services
{
    public class UsersService : IUsersService
    {
        private readonly IRepository<User> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UsersService(IRepository<User> repository, IUnitOfWork unitOfWork)
        {
            this._repository = repository;
            this._unitOfWork = unitOfWork;
        }

        public User GetUserByUsername(string username)
        {
            return this._repository
                .All
                .FirstOrDefault(u => u.UserName == username);
        }

        public User FindUser(string userName, string password)
        {
            var user = _repository.All.FirstOrDefault(u => u.UserName == userName && u.PasswordHash == password);
            return user;
        }
    }
}

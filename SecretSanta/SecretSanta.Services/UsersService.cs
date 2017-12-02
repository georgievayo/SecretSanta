using System.Collections.Generic;
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

        public IEnumerable<User> GetUsers(int skip, int take, string order, string search)
        {
            if (order.ToLower() == "asc")
            {
                return this._repository
                    .All
                    .Where(u => u.UserName == search || u.DisplayName == search)
                    .OrderBy(u => u.DisplayName)
                    .Skip(skip)
                    .Take(take);
            }
            else
            {
                return this._repository
                    .All
                    .Where(u => u.UserName == search || u.DisplayName == search)
                    .OrderByDescending(u => u.DisplayName)
                    .Skip(skip)
                    .Take(take);
            }
            
        }
    }
}

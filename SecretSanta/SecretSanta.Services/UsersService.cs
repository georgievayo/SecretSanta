using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                if (string.IsNullOrEmpty(search))
                {
                    return this._repository
                        .All
                        .OrderBy(u => u.DisplayName)
                        .Skip(skip)
                        .Take(take);
                }
                return this._repository
                    .All
                    .Where(u => u.UserName.Contains(search) || u.DisplayName.Contains(search))
                    .OrderBy(u => u.DisplayName)
                    .Skip(skip)
                    .Take(take);
            }
            else
            {
                if (string.IsNullOrEmpty(search))
                {
                    return this._repository
                        .All
                        .OrderByDescending(u => u.DisplayName)
                        .Skip(skip)
                        .Take(take);
                }

                return this._repository
                    .All
                    .Where(u => u.UserName.Contains(search) || u.DisplayName.Contains(search))
                    .OrderByDescending(u => u.DisplayName)
                    .Skip(skip)
                    .Take(take);
            }

        }

        public ICollection<Group> GetUserGroups(string username, int skip, int take)
        {
            var user = this.GetUserByUsername(username);
            if (user == null)
            {
                throw new ArgumentNullException();
            }

            return user.Groups.Skip(skip).Take(take).ToList();
        }

        public void AddRequest(Request requestToSend, User user)
        {
            user.Requests.Add(requestToSend);
            this._unitOfWork.SaveChanges();
        }

        public void DeleteRequest(Request request, User user)
        {
            user.Requests.Remove(request);
            this._unitOfWork.SaveChanges();
        }
    }
}

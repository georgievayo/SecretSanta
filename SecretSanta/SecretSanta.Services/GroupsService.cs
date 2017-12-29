using System;
using System.Data.Entity;
using System.Linq;
using SecretSanta.Models;
using SecretSanta.Services.Interfaces;
using SecretSanta.Data.Interfaces;
using System.Collections.Generic;

namespace SecretSanta.Services
{
    public class GroupsService: IGroupsService
    {
        private readonly IRepository<Group> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public GroupsService(IRepository<Group> repository, IUnitOfWork unitOfWork)
        {
            this._repository = repository;
            this._unitOfWork = unitOfWork;
        }

        public Group CreateGroup(string name, User owner)
        {
            var group = new Group()
            {
                Id = Guid.NewGuid(),
                IsProcessStarted = false,
                Name = name,
                Owner = owner,
                Users = new List<User>() { owner }
            };

            this._repository.Add(group);
            this._unitOfWork.SaveChanges();

            return group;
        }

        public void SetThatProcessIsStarted(Group group)
        {
            group.IsProcessStarted = true;
            this._unitOfWork.SaveChanges();
        }

        public void AddUserToGroup(string groupName, User user)
        {
            var group = this._repository.All.FirstOrDefault(g => g.Name == groupName);
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            group.Users.Add(user);
            this._unitOfWork.SaveChanges();
        }

        public void RemoveUserFromGroup(Group group, User user)
        {
            group.Users.Remove(user);
            this._unitOfWork.SaveChanges();
        }

        public Group GetGroupByName(string name)
        {
            return this._repository
                .All
                .Include(g => g.Users)
                .FirstOrDefault(g => g.Name == name);
        }

        public ICollection<User> GetParticipantsOfGroup(string groupName)
        {
            var group = this._repository.All.FirstOrDefault(g => g.Name == groupName);
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            return group.Users;
        }
    }
}

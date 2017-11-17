using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SecretSantaData.Contracts;
using SecretSantaModels;
using SecretSantaServices.Contracts;

namespace SecretSantaServices
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

        public Group CreateGroup(string name, string ownerId)
        {
            var group = new Group()
            {
                Name = name,
                OwnerId = ownerId
            };

            this._repository.Add(group);
            this._unitOfWork.SaveChanges();

            return group;
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

        public void RemoveUserFromGroup(string groupName, User user)
        {
            var group = this._repository.All.FirstOrDefault(g => g.Name == groupName);
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            group.Users.Remove(user);
            this._unitOfWork.SaveChanges();
        }

        public ICollection<User> GetAllParticipantsOfGroup(string name)
        {
            var group = this._repository
                .All
                .Include(g => g.Users)
                .FirstOrDefault(g => g.Name == name);

            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            return group.Users;
        }
    }
}

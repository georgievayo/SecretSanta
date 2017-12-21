using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SecretSanta.Data.Interfaces;
using SecretSanta.Models;
using SecretSanta.Services.Interfaces;

namespace SecretSanta.Services
{
    public class ConnectionsService : IConnectionsService
    {
        private readonly IRepository<Connection> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ConnectionsService(IRepository<Connection> repository, IUnitOfWork unitOfWork)
        {
            this._repository = repository;
            this._unitOfWork = unitOfWork;
        }

        public ICollection<Connection> GetGroupConnections(Guid groupId)
        {
            return this._repository
                .All
                .Where(c => c.Group.Id == groupId)
                .ToList();
        }

        public Connection AddConnection(User from, User to, Group group)
        {
            var connection = new Connection()
            {
                Id = Guid.NewGuid(),
                From = from,
                To = to,
                Group = group
            };

            this._repository.Add(connection);
            this._unitOfWork.SaveChanges();

            return connection;
        }

        public Connection GetUserConnection(string username, string groupName)
        {
            return this._repository
                .All
                .Include(c => c.From)
                .Include(c => c.To)
                .FirstOrDefault(c => c.From.UserName == username && c.Group.Name == groupName);
        }
    }
}

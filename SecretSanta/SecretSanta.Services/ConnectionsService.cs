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

        public void SaveConnections(ICollection<User> participants, Group group)
        {
            var connections = this.GenerateConnections(participants);

            foreach (var pair in connections)
            {
                this.AddConnection(participants.ElementAt(pair.Key),
                    participants.ElementAt(pair.Value), group);
            }
        }

        private IDictionary<int, int> GenerateConnections(ICollection<User> participants)
        {
            var count = participants.Count;

            Dictionary<int, int> pairs = new Dictionary<int, int>();
            bool[] used = new bool[count];

            var rand = new Random();

            for (int i = 0; i < count; i++)
            {
                var connectToIndex = rand.Next(0, count);
                while (connectToIndex == i || used[connectToIndex])
                {
                    connectToIndex = rand.Next(0, count);
                }

                pairs[i] = connectToIndex;
                used[connectToIndex] = true;
            }

            return pairs;
        }
    }
}

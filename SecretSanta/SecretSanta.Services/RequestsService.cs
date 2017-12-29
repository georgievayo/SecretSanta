using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SecretSanta.Data.Interfaces;
using SecretSanta.Models;
using SecretSanta.Services.Interfaces;

namespace SecretSanta.Services
{
    public class RequestsService : IRequestsService
    {
        private readonly IRepository<Request> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public RequestsService(IRepository<Request> repository, IUnitOfWork unitOfWork)
        {
            this._repository = repository;
            this._unitOfWork = unitOfWork;
        }

        public ICollection<Request> GetUserRequests(string userId, string order, int skip, int take)
        {
            if (order.ToLower() == "asc")
            {
                return this._repository
                    .All
                    .Where(r => r.To.Id == userId)
                    .Include(r => r.Group)
                    .OrderBy(r => r.ReceivedAt)
                    .Skip(skip)
                    .Take(take)
                    .ToList();
            }
            else
            {
                return this._repository
                    .All
                    .Where(r => r.To.Id == userId)
                    .Include(r => r.Group)
                    .OrderByDescending(r => r.ReceivedAt)
                    .Skip(skip)
                    .Take(take)
                    .ToList();
            }
        }

        public bool AlreadyHasRequest(string userId, string groupName)
        {
            var foundCount = this._repository
                .All
                .Count(r => r.To.Id == userId && r.Group.Name == groupName);

            return foundCount > 0;
        }

        public void DeleteRequest(Request request)
        {
            this._repository.Delete(request);
            this._unitOfWork.SaveChanges();
        }
    }
}

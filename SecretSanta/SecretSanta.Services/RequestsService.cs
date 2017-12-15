using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public ICollection<Request> GetUserRequests(string userId)
        {
            return this._repository
                .All
                .Where(r => r.To.Id == userId)
                .Include(r => r.Group)
                .ToList();
        }

        public void DeleteRequest(Request request)
        {
            this._repository.Delete(request);
            this._unitOfWork.SaveChanges();
        }
    }
}

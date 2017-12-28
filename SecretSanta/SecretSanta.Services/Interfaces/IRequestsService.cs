using System.Collections.Generic;
using SecretSanta.Models;

namespace SecretSanta.Services.Interfaces
{
    public interface IRequestsService
    {
        ICollection<Request> GetUserRequests(string userId, string order, int skip, int take);

        void DeleteRequest(Request request);
    }
}

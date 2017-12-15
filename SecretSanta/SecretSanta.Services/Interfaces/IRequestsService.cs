using System.Collections.Generic;
using SecretSanta.Models;

namespace SecretSanta.Services.Interfaces
{
    public interface IRequestsService
    {
        ICollection<Request> GetUserRequests(string userId);

        void DeleteRequest(Request request);
    }
}

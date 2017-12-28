using System;

namespace SecretSanta.API.Models
{
    public class RequestViewModel
    {
        public RequestViewModel(Guid id, string groupName, string ownerName, DateTime date)
        {
            Id = id;
            GroupName = groupName;
            OwnerName = ownerName;
            Date = date;
        }

        public Guid Id { get; set; }

        public string GroupName { get; set; }

        public string OwnerName { get; set; }

        public DateTime Date { get; set; }
    }
}
using System;

namespace SecretSanta.Models
{
    public class Request
    {
        public Guid Id { get; set; }

        public User To { get; set; }

        public Group Group { get; set; }

        public DateTime ReceivedAt { get; set; }
    }
}

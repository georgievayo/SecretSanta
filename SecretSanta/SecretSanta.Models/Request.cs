using System;

namespace SecretSanta.Models
{
    public class Request
    {
        public Guid Id { get; set; }

        public User To { get; set; }

        public User From { get; set; }

        public Group Group { get; set; }

        public RequestState State { get; set; }
    }
}

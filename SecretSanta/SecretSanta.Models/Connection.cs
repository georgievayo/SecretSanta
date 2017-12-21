using System;

namespace SecretSanta.Models
{
    public class Connection
    {
        public Guid Id { get; set; }

        public Group Group { get; set; }

        public User From { get; set; }

        public User To { get; set; }
    }
}

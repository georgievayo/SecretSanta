using System;

namespace SecretSantaModels
{
    public class Present
    {
        public Guid Id { get; set; }

        public Group Group { get; set; }

        public User From { get; set; }

        public User To { get; set; }
    }
}

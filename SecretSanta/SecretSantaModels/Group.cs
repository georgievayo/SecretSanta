using System;
using System.Collections.Generic;

namespace SecretSantaModels
{
    public class Group
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public User Owner { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}

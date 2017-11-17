using System;
using System.Collections.Generic;

namespace SecretSantaModels
{
    public class Group
    {
        public Group()
        {
            this.Users = new HashSet<User>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string OwnerId { get; set; }

        public virtual User Owner { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}

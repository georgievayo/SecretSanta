using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecretSanta.Models
{
    public class Group
    {
        public Group()
        {
            this.Users = new HashSet<User>();
        }

        public Guid Id { get; set; }

        [MaxLength(100)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        public string OwnerId { get; set; }

        public virtual User Owner { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}

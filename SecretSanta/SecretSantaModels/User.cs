using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SecretSantaModels
{
    public class User: IdentityUser, IUser
    {
        public string DisplayName { get; set; }

        public string PhotoUrl { get; set; }

        public int Age { get; set; }

        public string Address { get; set; }

        public string Interests { get; set; }

        public virtual ICollection<Present> Presents { get; set; }

        public virtual ICollection<Request> Requests { get; set; }

        public virtual ICollection<Group> Groups { get; set; }
    }
}

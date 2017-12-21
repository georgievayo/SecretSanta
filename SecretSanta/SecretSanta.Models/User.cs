using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SecretSanta.Models
{
    public class User: IdentityUser, IUser
    {
        public User()
        {
            this.Connections = new HashSet<Connection>();
            this.Requests = new HashSet<Request>();
            this.Groups = new HashSet<Group>();
        }
        public string DisplayName { get; set; }

        public string PhotoUrl { get; set; }

        public int Age { get; set; }

        public string Address { get; set; }

        public string Interests { get; set; }

        public virtual ICollection<Connection> Connections { get; set; }

        public virtual ICollection<Request> Requests { get; set; }

        public virtual ICollection<Group> Groups { get; set; }

        public async Task<ClaimsIdentity>
            GenerateUserIdentityAsync(UserManager<User> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            return userIdentity;
        }
    }
}

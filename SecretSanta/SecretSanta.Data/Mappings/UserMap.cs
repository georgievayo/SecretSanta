using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using SecretSanta.Models;

namespace SecretSanta.Data.Mappings
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            this.HasMany(x => x.Connections).WithRequired(x => x.From);
            this.HasMany(x => x.Connections).WithRequired(x => x.To);
            this.HasMany(x => x.Groups).WithMany(x => x.Users);
            this.HasMany(x => x.Requests).WithRequired(x => x.To);
        }
    }
}

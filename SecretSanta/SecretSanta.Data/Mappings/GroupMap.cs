using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using SecretSanta.Models;

namespace SecretSanta.Data.Mappings
{
    public class GroupMap : EntityTypeConfiguration<Group>
    {
        public GroupMap()
        {
            this.HasMany(x => x.Users).WithMany(x => x.Groups);
        }
    }
}

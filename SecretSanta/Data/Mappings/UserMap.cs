using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using SecretSantaModels;

namespace SecretSantaData.Mappings
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            this.Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.HasMany(x => x.Presents).WithRequired(x => x.From);
            this.HasMany(x => x.Presents).WithRequired(x => x.To);
            this.HasMany(x => x.Groups).WithMany(x => x.Users);
            this.HasMany(x => x.Requests).WithRequired(x => x.From);
            this.HasMany(x => x.Requests).WithRequired(x => x.To);
        }
    }
}

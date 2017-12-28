using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using SecretSanta.Data.Mappings;
using SecretSanta.Models;

namespace SecretSanta.Data
{
    public partial class SecretSantaDbContext : IdentityDbContext<User>
    {
        public SecretSantaDbContext()
            : base("SecretSanta")
        {
            Database.SetInitializer<SecretSantaDbContext>(null);
        }

        public static SecretSantaDbContext Create()
        {
            return new SecretSantaDbContext();
        }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Request> Requests { get; set; }

        public DbSet<Connection> Presents { get; set; }

        public DbSet<UserSession> UserSessions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new GroupMap());
            base.OnModelCreating(modelBuilder);
        }

        public IDbSet<TEntity> DbSet<TEntity>() where TEntity : class
        {
            return this.Set<TEntity>();
        }

        public void SetAdded<TEntry>(TEntry entity) where TEntry : class
        {
            var entry = this.Entry(entity);
            entry.State = EntityState.Added;
        }

        public void SetDeleted<TEntry>(TEntry entity) where TEntry : class
        {
            var entry = this.Entry(entity);
            entry.State = EntityState.Deleted;
        }

        public void SetUpdated<TEntry>(TEntry entity) where TEntry : class
        {
            var entry = this.Entry(entity);
            entry.State = EntityState.Modified;
        }
    }
}

using SecretSanta.Data.Interfaces;

namespace SecretSanta.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SecretSantaDbContext _dbContext;

        public UnitOfWork(SecretSantaDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public void SaveChanges()
        {
            this._dbContext.SaveChanges();
        }
    }
}

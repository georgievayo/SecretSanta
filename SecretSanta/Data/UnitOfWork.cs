using System;
using SecretSantaData.Contracts;

namespace SecretSantaData
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SecretSantaDbContext dbContext;

        public UnitOfWork(SecretSantaDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            this.dbContext = dbContext;
        }

        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }
    }
}

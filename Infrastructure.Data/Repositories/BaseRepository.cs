using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class BaseRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationsDbContext context;
        protected readonly DbSet<T> dbSet;

        public BaseRepository(ApplicationsDbContext dbContext)
        {
            context = dbContext;
            dbSet = context.Set<T>();
        }

        public void Delete(T item)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        public virtual T GetById(int id)
        {
            return dbSet.Find(id);
        }

        public void Insert(T item)
        {
            dbSet.Add(item);
        }

        public void Update(T item)
        {
            throw new NotImplementedException();
        }
    }
}

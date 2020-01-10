using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Core.Model;
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
            dbSet.Remove(item);
        }

        public async virtual Task<IEnumerable<T>> GetAll()
        {
            return await dbSet.ToListAsync();
        }
		

        public async virtual Task<T> GetById(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task Insert(T item)
        {
            await dbSet.AddAsync(item);
        }

        public void Update(T item)
        {
            dbSet.Update(item);
        }
    }
}
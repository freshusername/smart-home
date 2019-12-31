﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            _context = dbContext;
            _dbSet = _context.Set<T>();
        }

        public void Delete(T item)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public virtual T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public void Insert(T item)
        {
	        _dbSet.Add(item);
        }

        public void Update(T item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<History> GetAllHistories()
        {
	        return _context.Histories.Include(h => h.Sensor).ToList();
        }

        public History GetHistoryById(int id)
        {
	        return _context.Histories.Include(h => h.Sensor).FirstOrDefault(s => s.Id == id);
        }
	}
}
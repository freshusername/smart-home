using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        Task<T> GetById(int id);
        Task Insert(T item);
        Task Update(T item);
        Task Delete(T item);
    }
}

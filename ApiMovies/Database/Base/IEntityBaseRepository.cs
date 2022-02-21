using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiMovies.Database.Base
{
    public interface IEntityBaseRepository<T> where T : class, IEntityBase
    {
        Task<List<T>> GetAllAsync();        
        Task<T> GetByIdAsync(int id);        
        Task AddAsync(T entity);
        Task UpdateAsync(int id, T entity);
        Task DeleteAsync(int id);
    }
}

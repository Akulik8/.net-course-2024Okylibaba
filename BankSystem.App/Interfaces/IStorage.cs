using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Interfaces
{
    public interface IStorage<T, R>
    {
        public Task<R> GetByIdAsync(Guid id);

        public Task<List<T>> GetAsync(int pageSize, int pageNumber, Expression<Func<T, bool>>? filter);

        public Task AddAsync(T item);
        
        public Task UpdateAsync(Guid id, T newItem);

        public Task DeleteAsync(Guid id);
    }
}

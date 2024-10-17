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
        public R GetById(Guid id);

        public List<T> Get(int pageSize, int pageNumber, Func<T, bool>? filters);

        public void Add(T item);
        
        public void Update(Guid id, T newItem);

        public void Delete(Guid id);
    }
}

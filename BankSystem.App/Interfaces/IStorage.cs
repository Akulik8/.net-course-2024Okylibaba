using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Interfaces
{
    public interface IStorage<T, R>
    {
        public R Get(Func<T, bool>? filter);
        
        public void Add(T item);
        
        public void Update(T item, T newItem);

        public void Delete(T item);
    }
}

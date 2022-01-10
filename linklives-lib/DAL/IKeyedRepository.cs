using Linklives.Domain;
using System.Collections.Generic;

namespace Linklives.DAL
{
    public interface IKeyedRepository<T> where T: KeyedItem
    {
        T GetByKey(string key);
        IEnumerable<T> GetByKeys(IList<string> keys);
    }
}
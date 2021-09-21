using System.Collections.Generic;

namespace Linklives.DAL
{
    public interface ISourceRepository
    {
        public IEnumerable<dynamic> GetAll();
        public dynamic GetById(int id);
        public IEnumerable<dynamic> GetByIds(IList<int> ids);
    }
}

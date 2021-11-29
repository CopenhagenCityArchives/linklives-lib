using Linklives.Domain;
using System.Collections.Generic;

namespace Linklives.DAL
{
    public interface ISourceRepository
    {
        public IEnumerable<dynamic> GetAll();
        public Source GetById(int id);
        public IEnumerable<dynamic> GetByIds(IList<int> ids);
    }
}

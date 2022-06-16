using Linklives.Domain;
using System.Collections.Generic;

namespace Linklives.DAL
{
    public interface ISourceRepository
    {
        public IEnumerable<Source> GetAll();
        public Source GetById(int id);
        public IEnumerable<Source> GetByIds(IList<int> ids);
    }
}

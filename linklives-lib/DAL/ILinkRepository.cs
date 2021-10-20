using Linklives.Domain;
using System.Collections.Generic;

namespace Linklives.DAL
{
    public interface ILinkRepository
    {
        int Count();
        IEnumerable<Link> GetAll();
        Link GetByKey(string linkKey);
        void Insert(Link link);
        void Insert(IEnumerable<Link> links);
        void Upsert(IEnumerable<Link> entitties);
        void Delete(string linkKey);
        void Save();
    }
}

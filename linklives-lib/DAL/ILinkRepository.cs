using Linklives.Domain;
using System.Collections.Generic;

namespace Linklives.DAL
{
    public interface ILinkRepository : IKeyedRepository<Link>
    {
        int Count();
        IEnumerable<Link> GetAll();
        void Insert(Link link);
        void Insert(IEnumerable<Link> links);
        void Upsert(IEnumerable<Link> entitties);
        void Delete(string linkKey);
        void Save();
    }
}

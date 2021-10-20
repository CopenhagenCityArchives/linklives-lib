using Linklives.Domain;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Linklives.DAL
{
    public interface IPersonAppearanceRepository
    {
        BasePA GetById(string Id);
        IEnumerable<BasePA> GetByIds(List<string> Ids);
    }
}

using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Linklives.DAL
{
    public interface IPersonAppearanceRepository
    {
        dynamic GetById(string Id);
        IEnumerable<dynamic> GetByIds(List<string> Ids);
    }
}

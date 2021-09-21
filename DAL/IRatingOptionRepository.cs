using Linklives.Domain;
using System.Collections.Generic;

namespace Linklives.DAL
{
    public interface IRatingOptionRepository
    {
        IEnumerable<RatingOption> GetAll();
        RatingOption GetById(int id);
    }
}

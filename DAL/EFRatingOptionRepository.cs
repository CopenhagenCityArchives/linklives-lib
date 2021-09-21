using Linklives.Domain;
using System.Linq;

namespace Linklives.DAL
{
    public class EFRatingOptionRepository : DBRepository<RatingOption>, IRatingOptionRepository
    {

        public EFRatingOptionRepository(LinklivesContext context) : base(context)
        {
        }

        public RatingOption GetById(int id)
        {
            return context.RatingOptions.Single(x => x.Id == id);
        }
    }
}

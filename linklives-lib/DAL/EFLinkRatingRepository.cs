using Linklives.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Linklives.DAL
{
    public class EFLinkRatingRepository : DBRepository<LinkRating>, ILinkRatingRepository
    {

        public EFLinkRatingRepository(LinklivesContext context) : base(context)
        {
        }
        public List<LinkRating> GetbyLinkId(int linkId)
        {
            return context.LinkRatings.IncludeAll().Where(x => x.LinkId == linkId).ToList();
        }
        public void Delete(int id)
        {
            var entity = context.LinkRatings.Find(id);
            context.LinkRatings.Remove(entity);
        }
        public LinkRating GetById(int id)
        {
            return context.LinkRatings.IncludeAll().SingleOrDefault(x => x.Id == id);
        }

        public void Insert(IEnumerable<LinkRating> entitties)
        {
            context.LinkRatings.AddRange(entitties);
        }
    }
}

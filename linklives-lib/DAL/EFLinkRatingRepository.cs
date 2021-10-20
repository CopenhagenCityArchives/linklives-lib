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
        public List<LinkRating> GetbyLinkKey(string linkKey)
        {
            return context.LinkRatings.IncludeAll().Where(x => x.LinkKey == linkKey).ToList();
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

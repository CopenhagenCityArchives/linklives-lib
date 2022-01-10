using Linklives.Domain;
using System.Collections.Generic;

namespace Linklives.DAL
{
    public interface ILinkRatingRepository
    {
        int Count();
        IEnumerable<LinkRating> GetAll();
        LinkRating GetById(int id);
        List<LinkRating> GetbyLinkId(int linkId);
        void Insert(LinkRating linkRating);
        void Insert(IEnumerable<LinkRating> linkRatings);
        void Delete(int id);
        void Save();
    }
}

using Linklives.Domain;

namespace Linklives.DAL
{
    public class EFLinkRepository : EFKeyedRepository<Link>, ILinkRepository
    {
        public EFLinkRepository(LinklivesContext context) : base(context)
        {
        }
    }
}

using Linklives.Domain;
using Microsoft.EntityFrameworkCore;

namespace Linklives.DAL
{
    public interface IEFDownloadHistoryRepository {
        void RegisterDownload(DownloadHistoryEntry entry);
        void Save();
    }

    public class EFDownloadHistoryRepository : DBRepository<DownloadHistoryEntry>, IEFDownloadHistoryRepository
    {
        private readonly DbContextOptions<LinklivesContext> contextOptions;
        private readonly int batchSize = 1000;
        public EFDownloadHistoryRepository(LinklivesContext context, DbContextOptions<LinklivesContext> options) : base(context)
        {
            contextOptions = options;
        }

        public void RegisterDownload(DownloadHistoryEntry entry)  {
            context.DownloadHistoryEntries.Add(entry);
        }
    }
}

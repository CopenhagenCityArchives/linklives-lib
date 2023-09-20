using Linklives.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Linklives.DAL
{
    public interface ITranscribedPARepository
    {
        TranscribedPA GetById(string id);
        IEnumerable<TranscribedPA> GetByIds(List<string> ids);
        IList<TranscribedPA> GetBySource(int sourceId);
    }
}

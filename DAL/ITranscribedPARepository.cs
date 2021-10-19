using Linklives.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Linklives.DAL
{
    public interface ITranscribedPARepository
    {
        IEnumerable<TranscribedPA> GetBySource(int sourceId);
    }
}

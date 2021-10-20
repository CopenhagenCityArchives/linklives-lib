using Linklives.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Linklives.DAL
{
    public interface ITranscribedPARepository
    {
        IList<TranscribedPA> GetBySource(int sourceId);
    }
}

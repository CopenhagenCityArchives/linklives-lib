using Linklives.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Linklives.DAL
{
    public interface ITranscribedPARepository
    {
        TranscribedPA GetById(string id);
        IList<TranscribedPA> GetBySource(int sourceId);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Linklives.Domain
{
    /// <summary>
    /// Represents a Person Appearance formed from burial records
    /// </summary>
    public class BurialPA : BasePA
    {
        public BurialPA()
        {
            
        }
        public BurialPA(StandardPA standardPA, int sourceId) : base(standardPA, sourceId)
        {
            Type = SourceType.burial_protocol;
        }
        protected override void InitSourceSpecificFields()
        {
            //TODO: Implement census mapping
        }
    }
}

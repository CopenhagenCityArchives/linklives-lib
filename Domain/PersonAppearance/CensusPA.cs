namespace Linklives.Domain
{
    /// <summary>
    /// Represents a Person Appearance formed from census records
    /// </summary>
    public class CensusPA : BasePA
    {
        public CensusPA()
        {

        }
        public CensusPA(StandardPA standardPA, TranscribedPA transcribedPA, int sourceId) : base(standardPA, transcribedPA, sourceId)
        {
            Type = SourceType.census;
        }
        protected override void InitSourceSpecificFields()
        {
            //TODO: Implement census mapping
        }
    }
}

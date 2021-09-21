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
        public CensusPA(StandardPA standardPA, int sourceId) : base(standardPA, sourceId)
        {
            Type = SourceType.census;
        }
        protected override void InitFields()
        {
            base.InitFields();
            //TODO: Implement census mapping
        }
    }
}

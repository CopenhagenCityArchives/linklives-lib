namespace Linklives.Domain
{
    /// <summary>
    /// Represents a Person Appearance fromed from parish register records
    /// </summary>
    public class ParishPA : BasePA
    {
        public ParishPA()
        {

        }
        public ParishPA(StandardPA standardPA, int sourceId) : base(standardPA, sourceId)
        {
            Type = SourceType.parish_register;
        }
        protected override void InitSourceSpecificFields()
        {
            //TODO: Implement parish mapping
        }
    }
}

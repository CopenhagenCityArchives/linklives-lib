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
            InitFields();
        }
        private void InitFields()
        {
            //TODO: Implement parish mapping
        }
    }
}

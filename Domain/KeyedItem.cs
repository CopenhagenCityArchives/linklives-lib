namespace Linklives.Domain
{
    public abstract class KeyedItem
    {
        [CsvHelper.Configuration.Attributes.Ignore]
        public string Key { get; protected set; }        
        public abstract void InitKey();
    }
}

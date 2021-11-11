using System.ComponentModel.DataAnnotations.Schema;

namespace Linklives.Domain
{
    public abstract class KeyedItem
    {
        [CsvHelper.Configuration.Attributes.Ignore]
        [Column(TypeName ="Varchar(350)")] //If we dont specify the field size we cant use composite keys because they become too big
        [Nest.Keyword]
        public string Key { get; protected set; }        
        public abstract void InitKey();
    }
}

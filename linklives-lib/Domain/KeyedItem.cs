using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Linklives.Domain
{
    [Index(nameof(Key), IsUnique = true)]
    [Index(nameof(Id))]

    public abstract class KeyedItem
    {
        [CsvHelper.Configuration.Attributes.Ignore]
        [Column(TypeName ="Varchar(350)")] //If we dont specify the field size we cant use composite keys because they become too big
        [Nest.Keyword]
        public string Key { get; protected set; }
        public int Id { get; set; }
        [Nest.Boolean]
        [CsvHelper.Configuration.Attributes.Ignore]
        public bool Is_historic { get; set; }
        [Nest.Keyword]
        [CsvHelper.Configuration.Attributes.Ignore]
        public string Data_version { get; set; }
        public abstract void InitKey();
    }
}

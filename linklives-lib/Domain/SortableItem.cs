using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Linklives.Domain
{
    public abstract class SortableItem : KeyedItem
    {
        // Sortables
        [CsvHelper.Configuration.Attributes.Ignore]
        [NotMapped]
        public abstract int? Sourceyear_sortable { get; }
        [CsvHelper.Configuration.Attributes.Ignore]
        [NotMapped]
        [Nest.Keyword]
        public abstract string First_names_sortable { get; }
        [CsvHelper.Configuration.Attributes.Ignore]
        [NotMapped]
        [Nest.Keyword]
        public abstract string Family_names_sortable { get; }
        [CsvHelper.Configuration.Attributes.Ignore]
        [NotMapped]
        public abstract int? Birthyear_sortable { get; }
        [CsvHelper.Configuration.Attributes.Ignore]
        [NotMapped]
        public abstract int? Event_year_sortable { get; }
        [CsvHelper.Configuration.Attributes.Ignore]
        [NotMapped]
        public abstract int? Deathyear_sortable { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Linklives.Domain
{
    public abstract class SortableItem : KeyedItem
    {
        // Sortables
        [CsvHelper.Configuration.Attributes.Ignore]
        public abstract int? Sourceyear_sortable { get; }
        [CsvHelper.Configuration.Attributes.Ignore]
        public abstract string First_names_sortable { get; }
        [CsvHelper.Configuration.Attributes.Ignore]
        public abstract string Family_names_sortable { get; }
        [CsvHelper.Configuration.Attributes.Ignore]
        public abstract int? Birthyear_sortable { get; }
        [CsvHelper.Configuration.Attributes.Ignore]
        public abstract int? Event_year_sortable { get; }
        [CsvHelper.Configuration.Attributes.Ignore]
        public abstract int? Deathyear_sortable { get; }
    }
}

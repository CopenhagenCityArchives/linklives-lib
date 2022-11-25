using CsvHelper.Configuration.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using Nest;
using Linklives.Serialization;

namespace Linklives.Domain
{
    [ElasticsearchType(IdProperty = nameof(Key))]
    public partial class LifeCourse : SortableItem
    {
        public LifeCourse()
        {
            PersonAppearances = new List<BasePA>();
        }
        [Name("life_course_id")]
        [Exportable(FieldCategory.Identification)]
        public int Life_course_id { get; set; }
        [Name("pa_ids")]
        public string Pa_ids { get; set; }
        [Name("source_ids")]
        [Exportable(FieldCategory.Identification)]
        public string Source_ids { get; set; }
        [Name("link_ids")]
        public string Link_ids { get; set; }
        [Name("n_sources")]
        public string N_sources { get; set; }

        [CsvHelper.Configuration.Attributes.Ignore]
        public ICollection<Link> Links { get; set; }
        [CsvHelper.Configuration.Attributes.Ignore]
        [NotMapped] //Tells entity framework to ignore this property since we are adding it from another source
        [Nest.PropertyName("person_appearance")]
        [NestedExportable()]
        public List<BasePA> PersonAppearances { get; set; }
        
        #region sortables - not given in CSV files
        public override int? Sourceyear_sortable { get { return 0; } }
        public override string First_names_sortable { get { return ""; } }
        public override string Family_names_sortable { get { return ""; } }
        public override int? Birthyear_sortable { get { return 0; } }
        public override int? Event_year_sortable { get { return 0; } }
        public override int? Deathyear_sortable { get { return 0; } }
        #endregion
        public override void InitKey()
        {
            var sb = new StringBuilder();
            foreach (var link in Links)
            {
                sb.Append(link.Key);
                sb.Append("..");
            }
            Key = sb.ToString(0, sb.Length - 2); //Exclude the extra .. that always gets tagged on the end.
        }

        public IEnumerable<string> GetPAKeys()
        {
            return Source_ids.Split(",").Zip(Pa_ids.Split(","), (first, second) => first + "-" + second);
        }
    }
}

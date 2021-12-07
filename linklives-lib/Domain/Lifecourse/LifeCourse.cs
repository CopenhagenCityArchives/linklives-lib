using CsvHelper.Configuration.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Nest;

namespace Linklives.Domain
{
    [ElasticsearchType(IdProperty = nameof(Life_course_id))]
    public partial class LifeCourse : KeyedItem
    {
        public LifeCourse()
        {
            PersonAppearances = new List<BasePA>();
        }
        [Name("life_course_id")]
        public int Life_course_id { get; set; }
        [Name("pa_ids")]
        public string Pa_ids { get; set; }
        [Name("source_ids")]
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
        public List<BasePA> PersonAppearances { get; set; }

        public override void InitKey()
        {
            var sb = new StringBuilder();
            foreach (var link in Links)
            {
                sb.Append(link.Key);
                sb.Append("..");
            }
            Key = sb.ToString(0, sb.Length - 3); //Exclude the extra .. that always gets tagged on the end.
        }
        }
    }
}

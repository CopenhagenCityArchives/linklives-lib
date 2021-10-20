using CsvHelper.Configuration.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Linklives.Domain
{
    public partial class Link : KeyedItem
    {
        [Name("link_id")]
        public string Link_id { get; set; }
        [Name("iteration")]
        public string Iteration { get; set; }
        [Name("iteration_inner")]
        public string Iteration_inner { get; set; }
        [Name("method_id")]
        public string Method_id { get; set; }
        [Name("pa_id1")]
        public int Pa_id1 { get; set; }
        [Name("score")]
        public string Score { get; set; }
        [Name("pa_id2")]
        public int Pa_id2 { get; set; }
        [Name("source_id1")]
        public int Source_id1 { get; set; }
        [Name("source_id2")]
        public int Source_id2 { get; set; }
        public string[] PaKeys { get => new string[] { $"{Source_id1}-{Pa_id1}", $"{Source_id2}-{Pa_id2}" }; }
        [JsonIgnore] //Ignore when serializing to avoid endless recursion in json
        [Nest.Ignore]
        public virtual ICollection<LifeCourse> LifeCourses { get; set; }
        [Nest.Ignore]
        public virtual ICollection<LinkRating> Ratings { get; set; }
        public override void InitKey()
        {
            Key = $"{PaKeys[0]}_{PaKeys[1]}";
        }
    }
}

using CsvHelper.Configuration.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Linklives.Domain
{
    public partial class Link : KeyedItem
    {
        [Name("link_id")]
        public int Link_id { get; set; }
        [Name("iteration")]
        public int Iteration { get; set; }
        [Name("iteration_inner")]
        public int Iteration_inner { get; set; }
        [Name("method_id")]
        public int Method_id { get; set; }
        [Name("pa_id1")]
        public int Pa_id1 { get; set; }
        [Name("score")]
        public double Score { get; set; }
        [Name("pa_id2")]
        public int Pa_id2 { get; set; }
        [Name("source_id1")]
        public int Source_id1 { get; set; }
        [Name("source_id2")]
        public int Source_id2 { get; set; }
        [Ignore]
        public string LifeCourseKey { get; set; }
        [JsonIgnore]
        public virtual LifeCourse LifeCourse { get; set; }

        public override void InitKey()
        {
            Key = $"{Source_id1}-{Pa_id1}_{Source_id2}-{Pa_id2}";
        }
    }
}

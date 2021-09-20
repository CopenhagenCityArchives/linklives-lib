using Newtonsoft.Json;
using System.Collections.Generic;

namespace Linklives.Domain
{
    public partial class Link : KeyedItem
    {
        public int Link_id { get; set; }
        public int Iteration { get; set; }
        public int Iteration_inner { get; set; }
        public int Method_id { get; set; }
        public int Pa_id1 { get; set; }
        public double Score { get; set; }
        public int Pa_id2 { get; set; }
        public int Source_id1 { get; set; }
        public int Source_id2 { get; set; }
        public string Method_type { get; set; }
        public string Method_subtype1 { get; set; }
        public string Method_description { get; set; }
        public string LifeCourseKey { get; set; }
        [JsonIgnore]
        public virtual LifeCourse LifeCourse { get; set; }

        public override void InitKey()
        {
            Key = $"{Source_id1}-{Pa_id1}_{Source_id2}-{Pa_id2}";
        }
    }
}

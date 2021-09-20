using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Linklives.Domain
{
    public partial class LifeCourse : KeyedItem
    {
        public LifeCourse()
        {
            PersonAppearances = new List<BasePA>();
        }
        public int Life_course_id { get; set; }
        public ICollection<Link> Links { get; set; }
        [NotMapped] //Tells entity framework to ignore this property since we are adding it from another source
        public List<BasePA> PersonAppearances { get; set; }

        public override void InitKey()
        {
            var sb = new StringBuilder();
            foreach (var link in Links)
            {
                sb.Append(link.Key);
                sb.Append("..");
            }
            sb.ToString(0, sb.Length - 2); //Exclude the extra .. that always gets tagged on the end.
        }
    }
}

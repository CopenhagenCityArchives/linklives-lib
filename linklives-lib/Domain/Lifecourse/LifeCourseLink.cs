using System;
using System.Collections.Generic;
using System.Text;

namespace Linklives.Domain
{
    public class LifeCourseLink : KeyedItem
    {
        public string LifeCoursesKey { get; set; }
        public string LinksKey { get; set; }
        public virtual LifeCourse LifeCourse { get; set; }
        public virtual Link Link { get; set; }

        public override void InitKey() { }
    }
}

namespace Linklives.Domain
{
    public class Stats
    {
        public long EsPersonAppearanceCount { get; set; }
        public long EsLifecourseCount { get; set; }
        public long EsLinkCount { get; set; }
        public long EsSourceCount { get; set; }
        public long DbLifecourseCount { get; set; }
        public long DbLinkCount { get; set; }
        public long DbLinkRatingCount { get; set; }
        public long LifecourseDiff { get { return DbLifecourseCount - EsLifecourseCount; } }
        public long Linkdiff { get { return DbLinkCount - EsLinkCount; } }

    }
}

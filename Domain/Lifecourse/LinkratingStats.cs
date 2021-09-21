using System.Collections.Generic;

namespace Linklives.Domain
{
    public class LinkRatingStats
    {
        public LinkRatingStats()
        {
            HeadingRatings = new Dictionary<string, int>();
        }
        public Dictionary<string, int> HeadingRatings { get; set; }
        public int TotalRatings { get; set; }
    }
}

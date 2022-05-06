using System.ComponentModel.DataAnnotations.Schema;

namespace Linklives.Domain
{
    public class RatingOption
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Heading { get; set; }
        [Column(TypeName = "CHAR(10)")]
        public string Category { get; set; }
    }
}

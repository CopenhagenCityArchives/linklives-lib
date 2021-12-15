using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Linklives.Domain
{
    public class PostLinkRating
    {
        [DataMember, Required(AllowEmptyStrings = false, ErrorMessage = "Must have valid rating id")]
        public int RatingId { get; set; }
        [DataMember, Required(AllowEmptyStrings = false, ErrorMessage = "Must have valid link key")]
        public int LinkId { get; set; }
        public LinkRating ToLinkRating(string User)
        {
            return new LinkRating { LinkId = LinkId, RatingId = RatingId, User = User };
        }
    }
    public class LinkRating : PostLinkRating
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        [DataMember()]
        public virtual RatingOption Rating { get; set; }
        [DataMember, Required(AllowEmptyStrings = false, ErrorMessage = "Must have valid user id")]
        public string User { get; set; }
        [JsonIgnore]
        public virtual Link Link { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace ApiTFG.Models
{
    public partial class UserToFollows
    {
        public int Id { get; set; }

        public virtual AppUser Follower { get; set; }
        public string FollowerId { get; set; }

        public virtual AppUser Followed { get; set; }
        public string FollowedId { get; set; }

    }
}

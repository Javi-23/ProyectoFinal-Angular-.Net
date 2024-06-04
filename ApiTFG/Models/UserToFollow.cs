using System.ComponentModel.DataAnnotations.Schema;

namespace ApiTFG.Models
{
    [Table("T_UserToFollows")]
    public partial class UserToFollows
    {
        [Column("C_pk_UserToFollows")]
        public int Id { get; set; }

        [ForeignKey("FollowerId")]
        public virtual AppUser Follower { get; set; }

        [Column("C_FollowerId")]
        public string FollowerId { get; set; }

        [ForeignKey("FollowedId")]
        public virtual AppUser Followed { get; set; }

        [Column("C_FollowedId")]
        public string FollowedId { get; set; }
    }
}
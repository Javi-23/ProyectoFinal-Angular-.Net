using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiTFG.Models
{
    public class AppUser : IdentityUser
    {
        public string? Description { get; set; }
        public ICollection<Posts> UserPosts { get; set; }

        [InverseProperty("Follower")]
        public ICollection<UserToFollows> UserFollower { get; set; }

        [InverseProperty("Followed")]
        public ICollection<UserToFollows> UserFollowed { get; set; }

        public ICollection<Posts> UserComment{ get; set; }

    }
}

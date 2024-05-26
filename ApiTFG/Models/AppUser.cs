using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiTFG.Models
{
    public class AppUser : IdentityUser
    {

        public AppUser()
        {
            UserPosts = new HashSet<Posts>();
            UserFollower = new HashSet<UserToFollows>();
            UserFollowed = new HashSet<UserToFollows>();
            UserComment = new HashSet<Comment>();
            UserLikes = new HashSet<Likes>();
        }

        public string? Description { get; set; }

        public byte[]? Image { get; set; }
        public ICollection<Posts> UserPosts { get; set; }

        [InverseProperty("Follower")]
        public ICollection<UserToFollows> UserFollower { get; set; }

        [InverseProperty("Followed")]
        public ICollection<UserToFollows> UserFollowed { get; set; }

        public ICollection<Comment> UserComment{ get; set; }

        public ICollection<Likes> UserLikes { get; set; }

    }
}

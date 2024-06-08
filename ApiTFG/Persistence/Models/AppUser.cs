using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiTFG.Models
{
    [Table("T_AppUser")]
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

        [Column("C_Description")]
        [MaxLength(20)]
        public string? Description { get; set; }

        [Column("C_Image")]
        public byte[]? Image { get; set; }

        [InverseProperty("User")]
        public ICollection<Posts> UserPosts { get; set; }

        [InverseProperty("Follower")]
        public ICollection<UserToFollows> UserFollower { get; set; }

        [InverseProperty("Followed")]
        public ICollection<UserToFollows> UserFollowed { get; set; }

        [InverseProperty("User")]
        public ICollection<Comment> UserComment { get; set; }

        [InverseProperty("User")]
        public ICollection<Likes> UserLikes { get; set; }
    }
}
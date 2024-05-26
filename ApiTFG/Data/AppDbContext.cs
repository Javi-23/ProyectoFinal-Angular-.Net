using ApiTFG.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiTFG.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Posts> Posts { get; set; }
        public DbSet<UserToFollows> UserToFollows { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Likes> Likes { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Posts>()
                .HasOne(p => p.User)
                .WithMany(u => u.UserPosts)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<Likes>()
                .HasOne(l => l.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.PostId);

            modelBuilder.Entity<Likes>()
                .HasOne(l => l.User)
                .WithMany(u => u.UserLikes)
                .HasForeignKey(l => l.UserId);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.postsId);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.UserComment)
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<UserToFollows>()
                .HasOne(ut => ut.Follower)
                .WithMany(u => u.UserFollower)
                .HasForeignKey(ut => ut.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserToFollows>()
                .HasOne(ut => ut.Followed)
                .WithMany(u => u.UserFollowed)
                .HasForeignKey(ut => ut.FollowedId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
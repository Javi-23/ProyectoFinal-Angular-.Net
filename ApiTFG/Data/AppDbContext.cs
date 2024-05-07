using ApiTFG.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiTFG.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Posts> Posts { get; set; }
        public DbSet<UserToFollows> UserToFollows { get; set; }
        public ICollection<UserToFollows> Follower { get; set; }
        public ICollection<UserToFollows> Followed { get; set; }
        public DbSet<Comment> Comments { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


    }
}
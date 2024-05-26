using Microsoft.EntityFrameworkCore;

namespace ApiTFG.Models
{
    public partial class Posts
    {
        public Posts()
        {
            Comments = new HashSet<Comment>();
            Likes = new HashSet<Likes>();
        }

        public int Id { get; set; }
        public string UserId { get; set; } 
        public DateTime CreationDate { get; set; }
        public DateTime EditDate { get; set; }
        public string Text { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Likes> Likes { get; set; }

        public virtual AppUser User { get; set; }
    }
}

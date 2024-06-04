using System.ComponentModel.DataAnnotations.Schema;

namespace ApiTFG.Models
{
    [Table("T_Posts")]
    public partial class Posts
    {
        public Posts()
        {
            Comments = new HashSet<Comment>();
            Likes = new HashSet<Likes>();
        }

        [Column("C_pk_Posts")]
        public int Id { get; set; }

        [Column("C_UserId")]
        public string UserId { get; set; }

        [Column("C_CreationDate")]
        public DateTime CreationDate { get; set; }

        [Column("C_EditDate")]
        public DateTime EditDate { get; set; }

        [Column("C_Text")]
        public string Text { get; set; }

        [Column("C_Image")]
        public byte[]? Image { get; set; }

        [InverseProperty("Post")]
        public ICollection<Comment> Comments { get; set; }

        [InverseProperty("Post")]
        public ICollection<Likes> Likes { get; set; }

        [ForeignKey("UserId")]
        public virtual AppUser User { get; set; }
    }
}

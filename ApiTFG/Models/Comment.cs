using System.ComponentModel.DataAnnotations.Schema;

namespace ApiTFG.Models
{
    [Table("T_Comments")]
    public class Comment
    {
        [Column("C_pk_Comments")]
        public int Id { get; set; }

        [Column("C_postsId")]
        public int postsId { get; set; }

        [Column("C_UserId")]
        public string UserId { get; set; }

        [Column("C_UserName")]
        public string UserName { get; set; }

        [Column("C_Text")]
        public string Text { get; set; }

        [Column("C_CreationDate")]
        public DateTime CreationDate { get; set; }

        [ForeignKey("postsId")]
        public virtual Posts Post { get; set; }

        [ForeignKey("UserId")]
        public virtual AppUser User { get; set; }
    }
}
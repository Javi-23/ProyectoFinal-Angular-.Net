using System.ComponentModel.DataAnnotations.Schema;

namespace ApiTFG.Models
{
    [Table("T_Likes")]
    public class Likes
    {
        [Column("C_pk_Likes")]
        public int Id { get; set; }

        [Column("C_PostId")]
        public int PostId { get; set; }

        [Column("C_UserId")]
        public string UserId { get; set; }

        [ForeignKey("PostId")]
        public virtual Posts Post { get; set; }

        [ForeignKey("UserId")]
        public virtual AppUser User { get; set; }
    }
}
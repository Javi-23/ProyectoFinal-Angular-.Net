using ApiTFG.Migrations;
using System.Runtime.InteropServices;

namespace ApiTFG.Dtos
{
    public class PostDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public byte[] Image { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<CommentDTO> Comments { get; set; }
        public ICollection<LikesDTO> Likes { get; set; }

        public PostDTO()
        {
            Comments = new HashSet<CommentDTO>();
            Likes = new HashSet<LikesDTO>();
        }
    }
}

using System.Runtime.InteropServices;

namespace ApiTFG.Dtos
{
    public class PostDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<CommentDTO> Comments { get; set; }
    }
}

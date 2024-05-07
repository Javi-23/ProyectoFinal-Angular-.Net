using ApiTFG.Models;

namespace ApiTFG.Dtos
{
    public class UserAndPostDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public ICollection<PostDTO> Posts { get; set; }

    }
}


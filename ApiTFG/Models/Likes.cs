namespace ApiTFG.Models
{
    public class Likes
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }

        public virtual Posts Post { get; set; }
        public virtual AppUser User { get; set; }
    }
}

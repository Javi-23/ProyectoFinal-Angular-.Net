namespace ApiTFG.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int postsId { get; set; }
        public string UserId{ get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
    }
}

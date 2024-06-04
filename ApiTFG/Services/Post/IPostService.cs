using ApiTFG.Dtos;

namespace ApiTFG.Services.Post
{
    public interface IPostService
    {
        Task<PostDTO> CreatePost(string text, IFormFile imageFile);
        Task<PostDTO> UpdatePost(int id, string text);
        Task<bool> DeletePost(int id);
        Task<CommentDTO> CreateComment(int postsId, string text);
        Task<bool> DeleteComment(int id);
        Task<bool> LikePost(int postId);
        Task<byte[]> GetPostImage(int postId);
    }
}

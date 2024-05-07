using ApiTFG.Dtos;

namespace ApiTFG.Services.Post
{
    public interface IPostService
    {
        Task<PostDTO> CreatePost(string text);
        Task<PostDTO> UpdatePost(int id, string text);
        Task<bool> DeletePost(int id);
        Task<CommentDTO> CreateComment(int postsId, string text);
        Task<bool> DeleteComment(int id);

    }
}

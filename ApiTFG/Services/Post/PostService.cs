using ApiTFG.Dtos;
using ApiTFG.Models;
using ApiTFG.Repository;
using ApiTFG.Repository.Contracts;
using ApiTFG.Utils;
using AutoMapper;

namespace ApiTFG.Services.Post
{
    public class PostService : IPostService
    {
        private readonly IGenericRepository<Posts> _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostService(IGenericRepository<Posts> postRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public async Task<PostDTO> CreatePost(string text)
        {
            EnsureUserAuthenticated();

            var userId = await GetUserId();

            var post = new Posts
            {
                CreationDate = DateTime.Now,
                UserId = userId,
                Text = text,
            };

            var createdPost = await _postRepository.Create(post);

            if (createdPost == null)
            {
                throw new TaskCanceledException("No se pudo crear la publicación");
            }

            return _mapper.Map<PostDTO>(createdPost);
        }

        public async Task<bool> DeletePost(int id)
        {
            var post = await GetPostById(id);
            EnsureUserAuthorized(post.UserId);

            return await _postRepository.Delete(post);
        }

        public async Task<PostDTO> UpdatePost(int id, string text)
        {
            var post = await GetPostById(id);
            EnsureUserAuthorized(post.UserId);

            post.Text = text;
            post.EditDate = DateTime.Now;

            var updatedPost = await _postRepository.Update(post);

            return _mapper.Map<PostDTO>(updatedPost);
        }

        public async Task<CommentDTO> CreateComment(int postId, string text)
        {
            EnsureUserAuthenticated();

            var post = await GetPostById(postId);

            var comment = new Comment
            {
                postsId = postId,
                UserId = await GetUserId(),
                UserName = GetUsername(),
                Text = text,
                CreationDate = DateTime.Now
            };

            post.Comments.Add(comment);
            await _postRepository.Update(post);

            return _mapper.Map<CommentDTO>(comment);
        }

        public async Task<bool> DeleteComment(int id)
        {
            var post = await GetPostByCommentId(id);
            var comment = post.Comments.FirstOrDefault(c => c.Id == id);

            EnsureUserAuthorized(comment?.UserId);

            post.Comments.Remove(comment);

            await _postRepository.Update(post);

            return true;
        }

        private async Task<Posts> GetPostById(int id)
        {
            var post = await _postRepository.Get(p => p.Id == id);
            if (post == null)
            {
                throw new KeyNotFoundException("No se encontró la publicación");
            }
            return post;
        }

        private async Task<Posts> GetPostByCommentId(int id)
        {
            var post = await _postRepository.Get(filter: p => p.Comments.Any(c => c.Id == id));
            if (post == null)
            {
                throw new KeyNotFoundException("No se encontró la publicación asociada al comentario");
            }
            return post;
        }

        private void EnsureUserAuthenticated()
        {
            if (string.IsNullOrEmpty(GetUsername()))
            {
                throw new UnauthorizedAccessException("Usuario no autenticado");
            }
        }

        private void EnsureUserAuthorized(string userId)
        {
            var currentUserId = GetUserId().Result;

            if (userId != currentUserId)
            {
                throw new UnauthorizedAccessException("Usuario no autorizado");
            }
        }

        private string GetUsername()
        {
            var jwtToken = JwtUtils.ExtractJwtToken(_httpContextAccessor.HttpContext);
            return JwtUtils.ExtractUsernameFromToken(jwtToken);
        }

        private async Task<string> GetUserId()
        {
            var username = GetUsername();
            return await _userRepository.GetUserIdByUsername(username);
        }
    }
}
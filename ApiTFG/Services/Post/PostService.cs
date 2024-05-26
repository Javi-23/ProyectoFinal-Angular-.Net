using ApiTFG.Dtos;
using ApiTFG.Models;
using ApiTFG.Repository;
using ApiTFG.Repository.Contracts;
using ApiTFG.Utils;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiTFG.Services.Post
{
    public class PostService : IPostService
    {
        private readonly IGenericRepository<Posts> _postRepository;
        private readonly IGenericRepository<Likes> _likeRepository;
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
            try
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
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> DeletePost(int id)
        {
            try
            {
                var post = await GetPostById(id);
                EnsureUserAuthorized(post.UserId);

                return await _postRepository.Delete(post);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<PostDTO> UpdatePost(int id, string text)
        {
            try
            {
                var post = await GetPostById(id);
                EnsureUserAuthorized(post.UserId);

                post.Text = text;
                post.EditDate = DateTime.Now;

                var updatedPost = await _postRepository.Update(post);

                return _mapper.Map<PostDTO>(updatedPost);
            } 
            catch(Exception ex) 
            {
                throw;
            }         
        }

        public async Task<CommentDTO> CreateComment(int postId, string text)
        {
            try
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
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> DeleteComment(int id)
        {
            try
            {
                EnsureUserAuthenticated();

                var userId = await GetUserId();
                var userPostsQuery = await _postRepository.Query(p => p.UserId == userId);
                var userPosts = await userPostsQuery.Include(p => p.Comments).ToListAsync();

                foreach (var post in userPosts)
                {
                    var commentToRemove = post.Comments.FirstOrDefault(c => c.Id == id);
                    if (commentToRemove != null && commentToRemove.UserId == userId)
                    {
                        post.Comments.Remove(commentToRemove);
                        await _postRepository.Update(post);
                        return true;
                    }
                }

                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> LikePost(int postId)
        {
            try
            {
                EnsureUserAuthenticated();

                var post = await GetPostById(postId);
                var userId = await GetUserId();
                var like = new Likes
                {
                    PostId = postId,
                    UserId = userId
                };

                post.Likes.Add(like);
                await _postRepository.Update(post);

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UnlikePost(int postId)
        {
            try
            {
                EnsureUserAuthenticated();

                var userId = await GetUserId();
                var userPostsQuery = await _postRepository.Query(p => p.UserId == userId);
                var post = await userPostsQuery.Include(p => p.Likes).FirstOrDefaultAsync(p => p.Id == postId);

                if (post != null)
                {
                    var likeToRemove = post.Likes.FirstOrDefault(l => l.PostId == postId && l.UserId == userId);
                    if (likeToRemove != null)
                    {
                        post.Likes.Remove(likeToRemove);
                        await _postRepository.Update(post);
                        return true;
                    }
                }

                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }


        private async Task<Posts> GetPostById(int id)
        {
            try
            {
                var post = await _postRepository.Get(p => p.Id == id);
                if (post == null)
                {
                    throw new KeyNotFoundException("No se encontró la publicación");
                }
                return post;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<Posts> GetPostByCommentId(int id)
        {
            try
            {
                var post = await _postRepository.Get(filter: p => p.Comments.Any(c => c.Id == id));
                if (post == null)
                {
                    throw new KeyNotFoundException("No se encontró la publicación asociada al comentario");
                }
                return post;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void EnsureUserAuthenticated()
        {
            try
            {
                if (string.IsNullOrEmpty(GetUsername()))
                {
                    throw new UnauthorizedAccessException("Usuario no autenticado");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void EnsureUserAuthorized(string userId)
        {
            try
            {
                var currentUserId = GetUserId().Result;

                if (userId != currentUserId)
                {
                    throw new UnauthorizedAccessException("Usuario no autorizado");
                }
            }
            catch (Exception)
            {

                throw;
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
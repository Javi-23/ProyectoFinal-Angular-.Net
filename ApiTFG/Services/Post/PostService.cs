using ApiTFG.Dtos;
using ApiTFG.Exceptions;
using ApiTFG.Models;
using ApiTFG.Repository;
using ApiTFG.Repository.Contracts;
using ApiTFG.Utils;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<PostDTO> CreatePost(string text, IFormFile imageFile)
        {
            try
            {
                EnsureUserAuthenticated();

                var userId = await GetUserId();
                byte[]? image = null;

                if (imageFile != null && imageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imageFile.CopyToAsync(memoryStream);
                        image = memoryStream.ToArray();
                    }
                }

                var post = new Posts
                {
                    CreationDate = DateTime.Now,
                    UserId = userId,
                    Text = text,
                    Image = image,
                };

                var createdPost = await _postRepository.Create(post);

                if (createdPost == null)
                {
                    throw new PostException("No se pudo crear la publicación");
                }

                return _mapper.Map<PostDTO>(createdPost);
            }
            catch (Exception ex)
            {
                throw new PostException("Error al crear la publicación", ex);
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
                throw new PostException("Error deleting the post", ex);
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
            catch (Exception ex)
            {
                throw new PostException("Error updating the post", ex);
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
            catch (Exception ex)
            {

                throw new PostException("Error creating the comment", ex);
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
            catch (Exception ex)
            {
                throw new PostException("Error deleting the comment", ex);
            }
        }

        public async Task<bool> LikePost(int postId)
        {
            try
            {
                EnsureUserAuthenticated();

                var userId = await GetUserId();
                var userPostsQuery = await _postRepository.Query(p => p.Id == postId);
                var post = await userPostsQuery.Include(p => p.Likes).FirstOrDefaultAsync();

                if (post != null)
                {
                    var existingLike = post.Likes.FirstOrDefault(l => l.UserId == userId);
                    if (existingLike != null)
                    {
                        post.Likes.Remove(existingLike);
                        await _postRepository.Update(post);
                        return false; 
                    }
                    else
                    {
                        var like = new Likes
                        {
                            PostId = postId,
                            UserId = userId
                        };

                        post.Likes.Add(like);
                        await _postRepository.Update(post);
                        return true; 
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new PostException("Error liking the post", ex);
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
            catch (Exception ex)
            {
                throw new PostException("Error unliking the post", ex);
            }
        }
        public async Task<byte[]> GetPostImage(int postId)
        {
            try
            {
                var post = await GetPostById(postId);
                if (post?.Image == null)
                {
                    throw new PostException("No image found for the specified post.");
                }
                return post.Image;
            }
            catch (Exception ex)
            {
                throw new PostException("Error retrieving the post image", ex);
            }
        }



        private async Task<Posts> GetPostById(int id)
        {
            try
            {
                var post = await _postRepository.Get(p => p.Id == id);
                if (post == null)
                {
                    throw new KeyNotFoundException("Post not found");
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
                    throw new UnauthorizedAccessException("User not authenticated");
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
                    throw new UnauthorizedAccessException("User not authorized");
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
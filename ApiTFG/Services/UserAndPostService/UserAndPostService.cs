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
using System.Linq;
using System.Threading.Tasks;

namespace ApiTFG.Services.UserAndPostService
{
    public class UserAndPostService : IUserAndPostService
    {
        private readonly IGenericRepository<Posts> _postRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IUserToFollowRepository _userToFollowRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserAndPostService(IGenericRepository<Posts> postRepository, IMapper mapper, IUserRepository userRepository, IUserToFollowRepository userToFollowRepository, IHttpContextAccessor httpContextAccessor)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _userToFollowRepository = userToFollowRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserAndPostDto> GetUserPosts(string username)
        {
            try
            {
                var user = await _userRepository.Get(u => u.UserName == username);
                if (user == null)
                {
                    throw new UserAndPostException($"No user found with the username {username}");
                }

                var userId = user.Id;
                var userPostsQuery = await _postRepository.Query(p => p.UserId == userId);
                var userPosts = await userPostsQuery
                .Include(p => p.Comments)
                .Include(p => p.Likes) 
                .ToListAsync();

                return MapToUserAndPostDto(userId, username, user.Description, user.Image, userPosts);
            }
            catch (Exception ex)
            {
                throw new UserAndPostException("Error retrieving user posts", ex);
            }
        }

        public async Task<List<UserAndPostDto>> GetAllPosts()
        {
            try
            {
                var users = await _userRepository.GetAll();
                var userAndPostList = new List<UserAndPostDto>();

                foreach (var user in users)
                {
                    var userPostsQuery = await _postRepository.Query(p => p.UserId == user.Id);
                    var userPosts = await userPostsQuery
                        .Include(p => p.Comments)
                        .Include(p => p.Likes)
                        .ToListAsync();

                    userAndPostList.Add(MapToUserAndPostDto(user.Id, user.UserName, user.Description, user.Image, userPosts));
                }

                return userAndPostList;
            }
            catch (Exception ex)
            {
                throw new UserAndPostException("Error retrieving all posts", ex);
            }
        }

        public async Task<List<UserAndPostDto>> GetFollowedPosts()
        {
            try
            {
                var username = GetUsername();
                var userIds = await _userToFollowRepository.GetFollowedId(username);
                var userAndPostList = new List<UserAndPostDto>();

                foreach (var followedUserId in userIds)
                {
                    var followed = await _userRepository.Get(u => u.Id == followedUserId);

                    var userPostsQuery = await _postRepository.Query(p => p.UserId == followedUserId);
                    var userPosts = await userPostsQuery
                    .Include(p => p.Comments)
                    .Include(p => p.Likes)
                    .ToListAsync();

                    userAndPostList.Add(MapToUserAndPostDto(followed.Id, followed.UserName, followed.Description, followed.Image, userPosts));
                }

                return userAndPostList;
            }
            catch (Exception ex)
            {
                throw new UserAndPostException("Error retrieving followed posts", ex);
            }
        }

        private UserAndPostDto MapToUserAndPostDto(string userId, string username, string description, byte[] image, List<Posts> posts)
        {
            return new UserAndPostDto
            {
                UserId = userId,
                UserName = username,
                Description = description,
                Image = image,
                Posts = _mapper.Map<List<PostDTO>>(posts)
            };
        }

        private string GetUsername()
        {
            var jwtToken = JwtUtils.ExtractJwtToken(_httpContextAccessor.HttpContext);
            return JwtUtils.ExtractUsernameFromToken(jwtToken);
        }
    }
}
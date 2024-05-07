using ApiTFG.Data;
using ApiTFG.Repository;
using ApiTFG.Repository.Contracts;
using ApiTFG.Services;
using ApiTFG.Services.Post;
using ApiTFG.Services.User;
using ApiTFG.Services.UserAndPostService;
using ApiTFG.Services.UserToFollow;
using ApiTFG.Utils;
using Microsoft.EntityFrameworkCore;

namespace ApiTFG.Dependencies
{
    public static class Dependecies
    {
        public static void DependenciesInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(configuration.GetConnectionString("Default"),
                    new MySqlServerVersion(new Version(8, 0, 2)));
            });

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IUserToFollowService, UserToFollowService>();
            services.AddScoped<IUserToFollowRepository, UserToFollowRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserAndPostService, UserAndPostService>();
            services.AddAutoMapper(typeof(AutoMapperProfile));

        }
    }
}

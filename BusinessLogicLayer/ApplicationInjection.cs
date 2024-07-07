using BusinessLogicLayer.Services.Algorithms;
using BusinessLogicLayer.Services.Implementations;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Data;
using DataAccessLayer.Data.Implementations;
using DataAccessLayer.Data.Interfaces;
using DataAccessLayer.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace BusinessLogicLayer
{
    public static class ApplicationInjection
    {
        public static IServiceCollection AddApplication
            (this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AutoMapperConfigure()
                .RepositoriesConfigure()
                .ServicesConfigure()
                .DbConfigure(configuration)
                .ValidationConfigure()
                .AuthenticationConfigure(configuration);

            return services;
        }

        private static IServiceCollection RepositoriesConfigure(this IServiceCollection services)
        {
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            return services;
        }

        private static IServiceCollection ServicesConfigure(this IServiceCollection services)
        {
            services.AddTransient<IPasswordHasher, PasswordHasher>();

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderItemService, OrderItemService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            return services;
        }

        private static IServiceCollection DbConfigure(this IServiceCollection services, IConfiguration configuration)
        {
            var sqlConnectionBuilder = new SqlConnectionStringBuilder();
            sqlConnectionBuilder.ConnectionString = configuration.GetConnectionString("SQLDbConnection");
            services.AddDbContext<AppDbContext>(options => options
                                                    .UseLazyLoadingProxies()
                                                    .UseSqlServer(sqlConnectionBuilder.ConnectionString));
            return services;
        }

        private static IServiceCollection AutoMapperConfigure(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }

        private static IServiceCollection ValidationConfigure(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddFluentValidationAutoValidation();
            return services;
        }

        private static IServiceCollection AuthenticationConfigure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!)),
                    ValidateActor = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true
                };
            });
            return services;
        }

        public static IServiceProvider StartApplication(this IServiceProvider services)
        {
            services
                .AddSeed();
            return services;
        }

        private static IServiceProvider AddSeed(this IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<AppDbContext>())
                {
                    if (!context.Roles.Any())
                    {
                        var admin = new Role { Id = Guid.NewGuid(), Name = RoleConstants.Admin };
                        context.Roles.AddRange(
                            admin,
                            new Role { Id = Guid.NewGuid(), Name = RoleConstants.User }
                        );

                        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
                        context.Users.Add(
                            new User { Id = Guid.NewGuid(), UserName = "Admin", HashedPassword = passwordHasher.HashPassword("Pa55w0rd!"), Role = admin, RoleId = admin.Id }
                        );
                        context.SaveChanges();
                    }
                }
            }
            return services;
        }
    }
}

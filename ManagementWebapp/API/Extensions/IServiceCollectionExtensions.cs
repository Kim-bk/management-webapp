using System;
using API.Context;
using Domain;
using Domain.Accounts;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Infrastructure;
using Infrastructure.Custom;
using Infrastructure.CustomIdentity;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Service;
using Service.Authenticators;
using Service.Interfaces;
using Service.TokenGenratorServices;
using Service.TokenValidators;

namespace API.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure DbContext with Scoped lifetime
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("API"));
                options.UseLazyLoadingProxies();
            }
            );

            services.AddScoped((Func<IServiceProvider, Func<AppDbContext>>)((provider) => () => provider.GetService<AppDbContext>()));
            // TODO : Test Transion
            services.AddScoped<DbFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserStore<ApplicationUser>, CustomUserStore>();
            services.AddScoped<IRoleStore<IdentityRole>, CustomRoleStore>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddScoped(typeof(IRepository<>), typeof(Repository<>))
                .AddScoped<IAccountRepository, AccountRepository>()
                .AddScoped<IRefreshTokenRepository, RefreshTokenRepository>()
                .AddScoped<IProjectRepository, ProjectRepository>()
                .AddScoped<IListTaskRepository, ListTaskRepository>()
                .AddScoped<ITaskRepository, TaskRepository>()
                .AddScoped<ILabelRepository, LabelRepostiory>()
                .AddScoped<IToDoRepository, ToDoRepository>();
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IAccountService, AccountService>()
                .AddScoped<AccessTokenService>()
                .AddScoped<RefreshTokenService>()
                .AddScoped<IProjectService, ProjectService>()
                .AddScoped<IListTaskService, ListTaskService>()
                .AddScoped<ITaskService, TaskService>()
                .AddScoped<IRoleService, RoleService>()
                .AddScoped<TokenGenerator>()
                .AddScoped<RefreshTokenValidator>()
                .AddScoped<IMapperCustom, Mapper>()
                .AddScoped<Authenticator>();
        }
    }
}

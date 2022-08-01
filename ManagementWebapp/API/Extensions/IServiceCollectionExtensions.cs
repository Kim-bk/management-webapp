using System;
using API.Context;
using Domain;
using Domain.Accounts;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Service;
using Service.Interfaces;
using Service.TokenGenratorServices;

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

            services.AddScoped<Func<AppDbContext>>((provider) => () => provider.GetService<AppDbContext>());
            services.AddScoped<DbFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

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
                .AddScoped<ITaskRepository, TaskRepository>();
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IAccountService, AccountService>()
                .AddScoped<AccessTokenGenerator>()
                .AddScoped<RefreshTokenGenerator>()
                .AddScoped<IProjectService, ProjectService>()
                .AddScoped<IListTaskService, ListTaskService>();
        }
    }
}

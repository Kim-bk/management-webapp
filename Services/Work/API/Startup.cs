using System;
using System.Text;
using Infrastructure.Context;
using API.Extensions;
using API.Services.Mapping;
using AutoMapper;
using Domain.AggregateModels.UserAggregate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MediatR;
using API.DomainEventHandlers;
using EventBus.Abstractions;
using EventBusRabbitMQ;
using Microsoft.Extensions.Logging;
using EventBus;
using API.IntegrationEvents;
using API.IntegrationEvents.EventHandlers;
using RabbitMQ.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Autofac;

namespace API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Regist Domain Event Handler / Mediator
            services.AddMediatR(typeof(ListTaskDeletedDomainEventHandler).Assembly);
            services.AddMediatR(typeof(ProjectCreatedDomainEventHandler).Assembly);
            //                     typeof(SomeOtherHandler).Assembly);

            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);

            // Requirements in Password
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = true;
            })

            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            services.AddControllers();
            services.AddControllers().AddNewtonsoftJson();
            services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services
               .AddDatabase(Configuration)
               .AddRepositories()
               .AddServices();

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["AuthSettings:Audience"],
                    ValidIssuer = Configuration["AuthSettings:Issuer"],
                    RequireExpirationTime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AuthSettings:AccessTokenSecret"])),
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services
            .AddCustomIntegrations(Configuration)
            .AddEventBus(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            ConfigureEventBus(app);
        }
        private void RegisterRabbitMQ(IServiceCollection services)
        {
            /*services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                var rabbitMQ = Configuration.GetSection("RabbitMQ");
                var factory = new ConnectionFactory()
                {
                    HostName = rabbitMQ.GetSection("Hostname").Value,
                    DispatchConsumersAsync = true,
                    UserName = rabbitMQ.GetSection("Username").Value,
                    Password = rabbitMQ.GetSection("Password").Value,
                };

                var retryCount = Int32.Parse(Configuration.GetSection("EventBusRetryCount").Value);

                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            });*/

            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@locahost:5672")
            };
            using var connection = factory.CreateConnection();
            using var chanel = connection.CreateModel();

        }
        private void RegisterEventBus(IServiceCollection services)
        {
            services.AddSingleton<IEventBus, EventBusRabbitMQServices>(sp =>
            {
                /* var subscriptionClientName = "queue-test";
                 var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                 var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                 var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQServices>>();
                 var eventBusSubscriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
                 var retryCount = 5;

                 var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
                 var serviceScopeFactory = sp.GetRequiredService<IServiceScopeFactory>();


                 return new EventBusRabbitMQServices(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubscriptionsManager, subscriptionClientName, retryCount);
                */
                var subscriptionClientName = "queue_test";
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQServices>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var serviceScopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                var retryCount = 5;

                return new EventBusRabbitMQServices(rabbitMQPersistentConnection, logger, eventBusSubcriptionsManager, serviceScopeFactory, subscriptionClientName, retryCount);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            services.AddTransient<IIntegrationEventHandler<UserUpdatedIntegrationEvent>, UserUpdatedIntegrationEventHandler>();
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<UserUpdatedIntegrationEvent, UserUpdatedIntegrationEventHandler>();
        }  
    }
    static class CustomExtensionsMethods
    {

        /* public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
         {
             var hcBuilder = services.AddHealthChecks();

             hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

             hcBuilder
                 .AddSqlServer(
                     configuration["ConnectionString"],
                     name: "OrderingDB-check",
                     tags: new string[] { "orderingdb" });



             hcBuilder
                 .AddRabbitMQ(
                     $"amqp://{configuration["EventBusConnection"]}",
                     name: "ordering-rabbitmqbus-check",
                     tags: new string[] { "rabbitmqbus" });


             return services;
         }*/

        public static IServiceCollection AddCustomIntegrations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

           services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();


                var factory = new ConnectionFactory()
                {
                    /* Uri = new Uri("amqp://guest:guest@locahost:5672"),*/
                    UserName = "guest",
                    Password = "guest",
                    HostName = "localhost",
                    //AutomaticRecoveryEnabled = true

                };

                var retryCount = 5;


                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            });

            return services;
        }

        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {


            services.AddSingleton<IEventBus, EventBusRabbitMQServices>(sp =>
            {
                var subscriptionClientName = "queue_test";
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQServices>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var serviceScopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                var retryCount = 5;

                return new EventBusRabbitMQServices(rabbitMQPersistentConnection, logger, eventBusSubcriptionsManager, serviceScopeFactory, subscriptionClientName, retryCount);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            services.AddTransient<IIntegrationEventHandler<UserUpdatedIntegrationEvent>, UserUpdatedIntegrationEventHandler>();
            return services;
        }
    }
}

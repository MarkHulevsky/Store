using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Store.BuisnessLogic.Common;
using Store.BuisnessLogic.Common.Interfaces;
using Store.BuisnessLogic.Helpers;
using Store.BuisnessLogic.Helpers.Interfaces;
using Store.BuisnessLogic.Services;
using Store.BuisnessLogic.Services.Interfaces;
using Store.DataAccess.AppContext;
using Store.DataAccess.Entities;
using Store.DataAccess.Initialization;
using Store.DataAccess.Repositories.DapperRepositories;
using Store.DataAccess.Repositories.Interfaces;
using Store.Presentation.Middlewares;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using StripeConfiguration = Stripe.StripeConfiguration;

namespace Store.Presentation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ILogger, Logger>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IPrintingEditionRepository, PrintingEditionRepository>();
            services.AddScoped<IAuthorInPrintingEditionRepository, AuthorInPrintingEditionRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IEmailProvider, EmailProvider>();
            services.AddScoped<IJwtProvider, JwtProvider>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IPrintingEditionService, PrintingEditionService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IHttpProvider, HttpProvider>();

            services.AddDbContext<ApplicationContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole<Guid>>()
               .AddEntityFrameworkStores<ApplicationContext>()
                 .AddDefaultTokenProviders();

            StripeConfiguration.ApiKey = Configuration.GetSection("Stripe")["SecretKey"];

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = Configuration.GetSection("JwtSettings")["Issuer"],

                        ValidateAudience = true,
                        ValidAudience = Configuration.GetSection("JwtSettings")["Audience"],
                        ValidateLifetime = true,

                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
                            Configuration.GetSection("JwtSettings")["Key"])),
                        ValidateIssuerSigningKey = true,
                    };
                });

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
                options.User.RequireUniqueEmail = true;
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                    builder.SetIsOriginAllowed(_ => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddControllers();

            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Store API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<User> userManager,
            RoleManager<IdentityRole<Guid>> roleManager, ApplicationContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var swaggerSection = Configuration.GetSection("SwaggerSettings");
            app.UseSwagger(option =>
            {
                option.RouteTemplate = swaggerSection["JsonRoute"];
            });
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint(swaggerSection["UIEndpoint"], swaggerSection["Description"]);
            });

            app.UseCors();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseMiddleware<LoggerMiddleware>();

            var dataBaseInitializer = new DataBaseInitializer(userManager, roleManager, dbContext);

            dataBaseInitializer.InitializeDb();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

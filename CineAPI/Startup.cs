using CineAPI.Business.Entities;
using CineAPI.Models;
using CineAPI.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using CineAPI.Business.Helpers;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;

namespace CineAPI
{
    public class Startup
    {
        private readonly string corsPolicy = "MyPolicy";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddCors(options => options.AddPolicy(corsPolicy, builder =>
            {
                builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
            }));

            var settingsOptionsSection = Configuration.GetSection("SettingsOptions");
            services.Configure<SettingsOptions>(settingsOptionsSection);

            var settingsOptions = settingsOptionsSection.Get<SettingsOptions>();
            var key = Encoding.ASCII.GetBytes(settingsOptions.Secret);
            services.AddAuthentication(item =>
            {
                item.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                item.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(item =>
            {
                item.RequireHttpsMetadata = false;
                item.SaveToken = true;
                item.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("ConnectionMysql"), options =>
                {
                    options.ServerVersion(new Version(5, 7, 17), ServerType.MySql)
                        .EnableRetryOnFailure(
                            maxRetryCount: 10,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null
                        );
                    options.MigrationsAssembly("CineAPI");
                }));

            services.AddSwaggerGen(item =>
            {
                item.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "CineAPI - .Net Core",
                        Description = "API in .Net Core 3.0 with Swagger",
                        Contact = new OpenApiContact
                        {
                            Name = "Luiz Anthero Gama",
                            Email = "luizanthero@icloud.com",
                            Url = new Uri("https://github.com/luizanthero")
                        }
                    });

                item.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                item.IncludeXmlComments(xmlPath);

                item.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                item.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            services.AddScoped<SettingsOptions>();

            services.AddScoped<ExhibitionsBusiness>();
            services.AddScoped<FilmsBusiness>();
            services.AddScoped<RoomsBusiness>();
            services.AddScoped<RoomTypesBusiness>();
            services.AddScoped<SchedulesBusiness>();
            services.AddScoped<ScreensBusiness>();
            services.AddScoped<UsersBusiness>();
            services.AddScoped<TokensBusiness>();
            services.AddScoped<RolesBusiness>();
            services.AddScoped<UserRolesBusiness>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            var swagger = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swagger);

            app.UseSwagger(options => { options.RouteTemplate = swagger.JsonRoute; });

            app.UseSwaggerUI(options => { options.SwaggerEndpoint(swagger.UIEndpoint, swagger.Description); });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors(corsPolicy);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.Run(async context =>
            {
                context.Response.Redirect("/swagger/index.html");
            });
        }
    }
}

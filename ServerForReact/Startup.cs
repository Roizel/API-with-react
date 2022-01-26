using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using ServerForReact.Abstract;
using ServerForReact.Data;
using ServerForReact.Data.Identity;
using ServerForReact.Mapper;
using ServerForReact.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog.Extensions.Logging;
using NLog.Web;
using Hangfire;
using Hangfire.SQLite;
using ServerForReact.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using ServerForReact.Pagination;
using ServerForReact.Extensions;

namespace ServerForReact
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
            services.AddCors();
            services.AddDbContext<AppEFContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
               .AddEntityFrameworkStores<AppEFContext>()
               .AddDefaultTokenProviders();
            services.AddFluentValidation(x =>
              x.RegisterValidatorsFromAssemblyContaining<Startup>());
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    //options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize; //якщо щось не так з JSON то видали цю строчку кода, в н≥й шось не так
                });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ServerForReact", Version = "v1" });
            });
            services.AddAutoMapper(typeof(AppMapProfile));

            services.AddHangfire(configuration => configuration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSQLiteStorage(Configuration.GetConnectionString("HangfireConnection")));
            services.AddHangfireServer();

            #region DI
            services.AddScoped<IJwtTokenService, JwtTokenServices>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IEmailSenderService, EmailSenderService>();
            services.AddScoped<FacebookService>();
            services.AddScoped<CoursePagination>();
            services.AddScoped<StudentPagination>();
            #endregion
            var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<String>("JwtKey")));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = FacebookDefaults.AuthenticationScheme;
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = signinKey,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RoleManager<AppRole> roleManager)
        {
            app.UseCors(options =>
             options.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ServerForReact v1"));
            }
            if (roleManager.Roles.Count() == 0)
            {
                var result = roleManager.CreateAsync(new AppRole
                {
                    Name = "Admin"
                }).Result;

                result = roleManager.CreateAsync(new AppRole
                {
                    Name = "Student"
                }).Result;
            }
            //app.ConfigureExceptionhandler(logger);
            app.ConfigureCustomExceptionMiddleware();
            app.UseHangfireDashboard();

            var dir = Path.Combine(Directory.GetCurrentDirectory(), "images");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(dir),
                RequestPath = "/images"
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });
        }
    }
}

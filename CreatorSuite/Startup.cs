using System;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CreatorSuite
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAutoMapper();
            services.AddScoped<IUserService, UserService>();
            services.AddDbContext<DataContext>(x => x.UseInMemoryDatabase("TestDb"));

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();

            var authBuilder = services.AddAuthentication(options =>
            {
                options.DefaultScheme = "MyScheme";
            });
            authBuilder.AddScheme<BasicAuthenticationOptions, BasicAuthenticationHandler>("MyScheme", options =>
            {
                /* configure options */
            });

            //authBuilder.AddJwtBearer(x =>
            //{
            //    x.RequireHttpsMetadata = true;
            //    x.SaveToken = true;
            //    x.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateAudience = true,
            //        ValidateIssuer = true,
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Secret)),
            //    };
            //    x.Events = new JwtBearerEvents
            //    {
            //        OnTokenValidated = context =>
            //        {
            //            Console.WriteLine();
            //            Console.WriteLine("vali");
            //            Console.WriteLine();
            //
            //            var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
            //            var userId = int.Parse(context.Principal.Identity.Name);
            //            var user = userService.GetById(userId);
            //            if (user == null)
            //                context.Fail("Unauthorized");
            //
            //            return Task.CompletedTask;
            //        }
            //    };
            //});

            var signalRBuilder = services.AddSignalR();
            signalRBuilder.AddJsonProtocol();
            signalRBuilder.AddHubOptions<ChatHub>(options =>
            {
                options.EnableDetailedErrors = true;
            });

            var mvcBuilder = services.AddMvcCore();
            mvcBuilder.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            mvcBuilder.AddViews();
            mvcBuilder.AddRazorViewEngine();
            mvcBuilder.AddJsonFormatters();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();
            app.UseCookiePolicy();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Default/Error");
                app.UseHsts();
            }

            app.UseSignalR(routes => routes
                .MapHub<ChatHub>("/chathub"));

            app.UseMvc(routes => routes
                .MapRoute("users", "{controller=Users}")
                .MapRoute("default", "{controller=Default}/{action=Index}"));

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    var cachePeriod = env.IsDevelopment() ? "300" : "604800";
                    ctx.Context.Response.Headers.Append("Cache-Control", $"public, max-age={cachePeriod}");
                }
            });

            app.UseHttpsRedirection();
        }
    }
}
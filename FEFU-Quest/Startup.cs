using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System;
using FEFU_Quest.Data;
using Microsoft.EntityFrameworkCore;
using FEFU_Quest.SQlite.Context;
using FEFU_Quest.Domain.Identity;
using FEFU_Quest.Infrastructure.Interfaces;
using FEFU_Quest.Infrastructure.Services;

namespace FEFU_Quest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var sqliteName = Configuration.GetConnectionString("Sqlite");

            services.AddDbContext<FEFU_QuestDBSQlite>(opt => opt
               .UseSqlite(sqliteName)
               .UseLazyLoadingProxies()
           //opt.UseSqlServer(Configuration.GetConnectionString("SqlServer"))
           //.UseLazyLoadingProxies()
           );


            services.AddIdentity<UserDTO, RoleDTO>()
                .AddEntityFrameworkStores<FEFU_QuestDBSQlite>()
                .AddDefaultTokenProviders();

            services.AddTransient<FEFU_QuestDbInitializer>();

            services.AddTransient<IUser, UserService>();
            //services.AddTransient<IFriends, FriendsService>();
            //services.AddTransient<IChat, ChatService>();
            //services.AddTransient<IGroupChat, GroupChatService>();
            //services.AddTransient<IGroup, GroupService>();
            //services.AddTransient<IClash, ClashService>();
            //services.AddTransient<IEmailConfirm, EmailConfirmService>();

            //services.AddTransient<IUserIdProvider, CustomUserIdProvider>();

            //services.AddTransient<ILogInfo, LogInfoService>();

            //services.AddSignalR();

            services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequiredLength = 3;
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredUniqueChars = 3;
            });

            services.ConfigureApplicationCookie(opt =>
            {
                opt.Cookie.Name = "SocialNetCookie";
                opt.Cookie.HttpOnly = true;
                opt.ExpireTimeSpan = TimeSpan.FromDays(10);

                opt.LoginPath = "/Account/Log";
                opt.LogoutPath = "/Account/Logout";
                opt.AccessDeniedPath = "/Account/AccessDenied";

                opt.SlidingExpiration = true;
            });

            services.AddControllersWithViews();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
             FEFU_QuestDbInitializer db)
        {
            db.Initialize();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseOnlineUsers();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapHub<MessageHub>("/chat");

                //endpoints.MapHub<SecretMessageHub>("/secretchat");

                //endpoints.MapHub<GroupMessageHub>("/groupchat");

                //endpoints.MapHub<ClashMessageHub>("/clashchat");

                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

                endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Home}/{action=Index}/{id?}" 
                );

            });

        }
    }
}

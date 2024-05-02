
﻿using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Sport_Accessories.Areas.Identity.Models;
using Sport_Accessories.Data;
using Sport_Accessories.Services;
using Sport_Accessories.SettingModels;

namespace Sport_Accessories
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {

                options.SignIn.RequireConfirmedAccount = true;

                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredUniqueChars = 1;
                options.Lockout.AllowedForNewUsers = false;
                options.Lockout.MaxFailedAccessAttempts = 5;


            })
                .AddDefaultTokenProviders()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            //Add auto mapper
            builder.Services.AddAutoMapper(typeof(Program).Assembly);

            builder.Services.AddRazorPages();
            builder.Services.ConfigureApplicationCookie(
            options => options.AccessDeniedPath = new PathString(
                "/Admin/AccessDenied"));
            builder.Services.ConfigureApplicationCookie(
            options => options.LoginPath = new PathString(
                "/Identity/Account/Login"));

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Read", policy =>
                {
                    policy.RequireClaim("Ability", "Read");
                });
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Create", policy =>
                {
                    policy.RequireClaim("Ability", "Create");
                });
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Edit", policy =>
                {
                    policy.RequireClaim("Ability", "Edit");
                });
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Delete", policy =>
                {
                    policy.RequireClaim("Ability", "Delete");
                });
            });
           

            builder.Services.Configure<SMTPSettings>(builder.Configuration
                                                    .GetSection("SMTP"));

            builder.Services.AddTransient<IEmailSender, EmailSender>();

            builder.Services.AddTransient<ITwoFactorAuthentication,
                                          TwoFactorAuthentication>();

            builder.Services.AddTransient<AbstractProfilePicture, UpdateProfilePicture>();

            builder.Services.AddTransient<IOfferService, OfferService>();


            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            
            app.UseStaticFiles();

            app.UseStatusCodePagesWithReExecute("/Errors/{0}");

            app.UseRouting();
            

            app.UseAuthorization();
            

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            app.Run();

        }
    }
}

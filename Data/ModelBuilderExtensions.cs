using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sport_Accessories.Areas.Identity.Models;
using System.Security.Claims;

namespace Sport_Accessories.Data
{
    public static class ModelBuilderExtensions
    {
        //extend the ModelBuilder class to have a Seed() method for adding a superadmin
        //profile with a role - SuperAdmin as well
        public static void Seed(this ModelBuilder builder)
        {

            //Create the roles and insert them to database
            var super_admin_role = new IdentityRole { Name = "SuperAdministrator" };
            super_admin_role.NormalizedName = super_admin_role.Name.ToUpper();

            var user_role = new IdentityRole { Name = "User" };
            user_role.NormalizedName = user_role.Name.ToUpper();

            var admin_role = new IdentityRole { Name = "Admin" };
            admin_role.NormalizedName = admin_role.Name.ToUpper();

            builder.Entity<IdentityRole>().HasData(super_admin_role);
            builder.Entity<IdentityRole>().HasData(admin_role);
            builder.Entity<IdentityRole>().HasData(user_role);

            //create user, superadmin and admin and insert them to database

            var user = new User { UserName = "User" };
            var admin = new User { UserName = "Admin" };
            var hasher = new PasswordHasher<User>();
            var superadmin = new User { UserName = "SuperAdmin" };
            superadmin.NormalizedUserName = superadmin.UserName.ToUpper();
            superadmin.PasswordHash = hasher.HashPassword(superadmin, "superadmin");
            user.NormalizedUserName = user.UserName.ToUpper();
            user.PasswordHash = hasher.HashPassword(user, "user");
            admin.NormalizedUserName = admin.UserName.ToUpper();
            admin.PasswordHash = hasher.HashPassword(admin, "admin");
            user.EmailConfirmed = true;
            admin.EmailConfirmed = true;
            superadmin.EmailConfirmed = true;

            //Create the admin, user and superadmin
            builder.Entity<User>().HasData(superadmin);
            builder.Entity<User>().HasData(admin);
            builder.Entity<User>().HasData(user);

            //Add claims to the admins for the CUD operations and the SuperAdmin
            //to be able to manage each of their CUD abilities without Read because by
            //default they can read as admins
            Claim edit_claim = new Claim("Ability", "Edit");
            Claim delete_claim = new Claim("Ability", "Delete");
            Claim create_claim = new Claim("Ability", "Create");

            //assign for the default admin multiple claims but the SuperAdmin can
            //manage each of them, also to add admins and assign claims to them and
            //to remove as well 
            var adminClaim = new[]
            {
                new IdentityUserClaim<string>
                {
                    Id = 1,
                    UserId = admin.Id,
                    ClaimType = edit_claim.Type,
                    ClaimValue = edit_claim.Value,
                },
                new IdentityUserClaim<string>
                {
                    Id = 2,
                    UserId = admin.Id,
                    ClaimType = delete_claim.Type,
                    ClaimValue = delete_claim.Value,
                },
                new IdentityUserClaim<string>
                {
                    Id = 3,
                    UserId = admin.Id,
                    ClaimType = create_claim.Type,
                    ClaimValue = create_claim.Value,
                }
            };


            //Assign and insert admin and his claims
            builder.Entity<IdentityUserClaim<string>>().HasData(adminClaim);

            //Assign and insert user and role IDs to their corresponding data
            //in users table

            IdentityUserRole<string> SuperAdmin_SuperAdminRole = new IdentityUserRole<string>
            {
                UserId = superadmin.Id,
                RoleId = super_admin_role.Id
            };

            IdentityUserRole<string> Admin_AdminRole = new IdentityUserRole<string>
            {
                UserId = admin.Id,
                RoleId = admin_role.Id
            };

            IdentityUserRole<string> User_UserRole = new IdentityUserRole<string>
            {
                UserId = user.Id,
                RoleId = user_role.Id
            };

            builder.Entity<IdentityUserRole<string>>().HasData(SuperAdmin_SuperAdminRole);
            builder.Entity<IdentityUserRole<string>>().HasData(Admin_AdminRole);
            builder.Entity<IdentityUserRole<string>>().HasData(User_UserRole);

        }
    }
}

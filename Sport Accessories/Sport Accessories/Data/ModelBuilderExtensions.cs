using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sport_Accessories.Areas.Identity.Models;

namespace Sport_Accessories.Data
{
    public static class ModelBuilderExtensions
    {
        //extend the ModelBuilder class to have a Seed() method for adding a superadmin
        //profile with a role - SuperAdmin as well
        public static void Seed(this ModelBuilder builder)
        {
            var super_admin_role = new IdentityRole { Name = "SuperAdministrator" };
            super_admin_role.NormalizedName = super_admin_role.Name.ToUpper();

            var user_role = new IdentityRole { Name = "User" };
            user_role.NormalizedName = user_role.Name.ToUpper();

            var admin_role = new IdentityRole { Name = "Admin" };
            admin_role.NormalizedName = admin_role.Name.ToUpper();

            builder.Entity<IdentityRole>().HasData(super_admin_role);
            builder.Entity<IdentityRole>().HasData(admin_role);
            builder.Entity<IdentityRole>().HasData(user_role);

            var hasher = new PasswordHasher<User>();
            var admin = new User { UserName = "SuperAdmin" };
            admin.NormalizedUserName = admin.UserName.ToUpper();
            admin.PasswordHash = hasher.HashPassword(admin, "superadmin");

            builder.Entity<User>().HasData(admin);

            IdentityUserRole<string> userRole = new IdentityUserRole<string>
            {
                UserId = admin.Id,
                RoleId = super_admin_role.Id
            };

        }
    }
}

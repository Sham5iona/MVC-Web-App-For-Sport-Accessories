using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Sport_Accessories.Areas.Identity.Models;
using Sport_Accessories.Models;
using Sport_Accessories.Services;

namespace Sport_Accessories.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {

        public DbSet<User> Users { get; set; }
        public DbSet<Log_20118018> Log_20118018 { get; set; }
        public DbSet<Bag> Bags { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Favourite> Favourites { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Product> Products { get; set; }

        //With this empty constructor, I leave the services from the Program.cs file
        //to connect to the database without the need to override the
        //OnConfiguring() method.

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

            //Add all tables to the created 20118018 schema
            builder.HasDefaultSchema("20118018");

            builder.Seed();

            //Relationships

            //when a category is deleted, all products with that category will
            //be deleted as well
            builder.Entity<Category>().HasMany(c => c.Products).WithOne(p => p.Category)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Product>().HasOne(p => p.Photo).WithOne(ph => ph.Product)
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<BagProduct>().HasKey(bp =>
                new { bp.ProductId, bp.BagId });

            builder.Entity<ProductFavourite>().HasKey(pf =>
                new { pf.ProductId, pf.FavoriteId });

            builder.Entity<Product>().HasMany(p => p.BagProducts)
                .WithOne(bp => bp.Product)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Bag>().HasMany(b => b.BagProducts).WithOne(bp => bp.Bag)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Product>().HasMany(p => p.ProductFavourites)
                .WithOne(pf => pf.Product)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Favourite>().HasMany(f => f.ProductFavourites)
                .WithOne(pf => pf.Favourite)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Product>().HasOne(p => p.User).WithMany(u => u.Products);

            //when a user is deleted, his bag list will be deleted as well
            builder.Entity<User>().HasOne(u => u.Bag).WithOne(b => b.User)
                .OnDelete(DeleteBehavior.Cascade);

            //when a user is deleted, his favourites list will be deleted as well
            builder.Entity<User>().HasOne(u => u.Favourite).WithOne(f => f.User)
                .OnDelete(DeleteBehavior.Cascade);

            //Inform EFCore for the triggers in the Users table
            builder.Entity<User>().ToTable(tb => tb.HasTrigger("tg_Users_Update"));

            builder.Entity<User>().ToTable(tb => tb.HasTrigger("tg_Users_Insert"));

            builder.Entity<User>().ToTable(tb => tb.HasTrigger("tg_Users_Delete"));

        }
        public DbSet<Sport_Accessories.Models.ProductFavourite> ProductFavourite { get; set; } = default!;

    }
}
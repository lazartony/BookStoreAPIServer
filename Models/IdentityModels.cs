using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace BookStoreAPIServer.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<WishListItem> WishListItems { get; set; }
        public virtual Cart Cart { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Coupon> Coupons { get; set; }
        public virtual DbSet<WishListItem> WishListItems { get; set; }
        public virtual DbSet<FeaturedItem> FeaturedItems { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<OrderCoupon> OrderCoupons { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<CartItem> CartItems { get; set; }
        public virtual DbSet<CartCoupon> CartCoupons { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //TODO: Unique constraints for order, wishlist and cart
            
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>()
               .HasMany(e => e.Books)
               .WithRequired(e => e.Category);

            modelBuilder.Entity<Book>()
                .HasMany(e => e.OrderItems)
                .WithRequired(e => e.Book)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Book>()
                .HasMany(e => e.CartItems)
                .WithRequired(e => e.Book)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Book>()
                .HasMany(e => e.WishListItems)
                .WithRequired(e => e.Book)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FeaturedItem>()
                .HasRequired(e => e.Book);

            modelBuilder.Entity<Coupon>()
               .HasMany(e => e.OrderCoupons)
               .WithRequired(e => e.Coupon);
            modelBuilder.Entity<Coupon>()
               .HasMany(e => e.CartCoupons)
               .WithRequired(e => e.Coupon);

            modelBuilder.Entity<Order>()
               .HasMany(e => e.OrderItems)
               .WithRequired(e => e.Order);
            modelBuilder.Entity<Order>()
               .HasMany(e => e.OrderCoupons)
               .WithRequired(e => e.Order);

            modelBuilder.Entity<Cart>()
               .HasMany(e => e.CartItems)
               .WithRequired(e => e.Cart);
            modelBuilder.Entity<Cart>()
               .HasMany(e => e.CartCoupons)
               .WithRequired(e => e.Cart);
            modelBuilder.Entity<ApplicationUser>()
               .HasOptional(e => e.Cart)
               .WithRequired(e => e.User);

            modelBuilder.Entity<ApplicationUser>()
               .HasMany(e => e.Orders)
               .WithRequired(e => e.User)
               .WillCascadeOnDelete(false);
            modelBuilder.Entity<ApplicationUser>()
               .HasMany(e => e.WishListItems)
               .WithRequired(e => e.User);
        }
        public void AddTimeStamps()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is IEntityTimeStamps && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((IEntityTimeStamps)entityEntry.Entity).LastUpdatedAt = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((IEntityTimeStamps)entityEntry.Entity).CreatedAt = DateTime.Now;
                }
            }
        }
        public override int SaveChanges()
        {
            AddTimeStamps();
            return base.SaveChanges();
        }
        public override async Task<int> SaveChangesAsync()
        {
            AddTimeStamps();
            return await base.SaveChangesAsync();
        }
    }
}
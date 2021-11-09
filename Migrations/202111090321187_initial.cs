namespace BookStoreAPIServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Author = c.String(nullable: false),
                        ISBN = c.String(),
                        ImageUrl = c.String(),
                        Year = c.Short(),
                        Price = c.Double(nullable: false),
                        Status = c.Int(nullable: false),
                        Position = c.Int(),
                        Category_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.Category_Id, cascadeDelete: true)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "dbo.CartItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Short(nullable: false),
                        Book_Id = c.Int(nullable: false),
                        Cart_Id = c.Int(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        LastUpdatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Carts", t => t.Cart_Id, cascadeDelete: true)
                .ForeignKey("dbo.Books", t => t.Book_Id)
                .Index(t => t.Book_Id)
                .Index(t => t.Cart_Id);
            
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TotalValue = c.Double(nullable: false),
                        TotalDiscount = c.Double(nullable: false),
                        NetPrice = c.Double(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        LastUpdatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.CartCoupons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Coupon_Id = c.Int(nullable: false),
                        Cart_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Coupons", t => t.Coupon_Id, cascadeDelete: true)
                .ForeignKey("dbo.Carts", t => t.Cart_Id, cascadeDelete: true)
                .Index(t => t.Coupon_Id)
                .Index(t => t.Cart_Id);
            
            CreateTable(
                "dbo.Coupons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                        DiscountPercentage = c.Double(nullable: false),
                        DiscountValue = c.Double(nullable: false),
                        MinOrderValue = c.Double(nullable: false),
                        IsClubbable = c.Boolean(nullable: false),
                        Status = c.Int(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        LastUpdatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderCoupons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Coupon_Id = c.Int(nullable: false),
                        Order_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.Order_Id, cascadeDelete: true)
                .ForeignKey("dbo.Coupons", t => t.Coupon_Id, cascadeDelete: true)
                .Index(t => t.Coupon_Id)
                .Index(t => t.Order_Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DeliveryAddress = c.String(nullable: false),
                        OrderStatus = c.Int(nullable: false),
                        User_Id = c.String(nullable: false, maxLength: 128),
                        TotalValue = c.Double(nullable: false),
                        TotalDiscount = c.Double(nullable: false),
                        NetPrice = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Short(nullable: false),
                        Book_Id = c.Int(nullable: false),
                        Order_Id = c.Int(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        LastUpdatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.Order_Id, cascadeDelete: true)
                .ForeignKey("dbo.Books", t => t.Book_Id)
                .Index(t => t.Book_Id)
                .Index(t => t.Order_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.WishListItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Short(nullable: false),
                        Book_Id = c.Int(nullable: false),
                        User_Id = c.String(nullable: false, maxLength: 128),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        LastUpdatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Books", t => t.Book_Id)
                .Index(t => t.Book_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        ImageUrl = c.String(),
                        Status = c.Int(nullable: false),
                        Position = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FeaturedItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Book_Id = c.Int(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        LastUpdatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Books", t => t.Book_Id, cascadeDelete: true)
                .Index(t => t.Book_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.FeaturedItems", "Book_Id", "dbo.Books");
            DropForeignKey("dbo.WishListItems", "Book_Id", "dbo.Books");
            DropForeignKey("dbo.OrderItems", "Book_Id", "dbo.Books");
            DropForeignKey("dbo.Books", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.CartItems", "Book_Id", "dbo.Books");
            DropForeignKey("dbo.Carts", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.CartItems", "Cart_Id", "dbo.Carts");
            DropForeignKey("dbo.CartCoupons", "Cart_Id", "dbo.Carts");
            DropForeignKey("dbo.OrderCoupons", "Coupon_Id", "dbo.Coupons");
            DropForeignKey("dbo.WishListItems", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Orders", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.OrderItems", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.OrderCoupons", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.CartCoupons", "Coupon_Id", "dbo.Coupons");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.FeaturedItems", new[] { "Book_Id" });
            DropIndex("dbo.WishListItems", new[] { "User_Id" });
            DropIndex("dbo.WishListItems", new[] { "Book_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.OrderItems", new[] { "Order_Id" });
            DropIndex("dbo.OrderItems", new[] { "Book_Id" });
            DropIndex("dbo.Orders", new[] { "User_Id" });
            DropIndex("dbo.OrderCoupons", new[] { "Order_Id" });
            DropIndex("dbo.OrderCoupons", new[] { "Coupon_Id" });
            DropIndex("dbo.CartCoupons", new[] { "Cart_Id" });
            DropIndex("dbo.CartCoupons", new[] { "Coupon_Id" });
            DropIndex("dbo.Carts", new[] { "User_Id" });
            DropIndex("dbo.CartItems", new[] { "Cart_Id" });
            DropIndex("dbo.CartItems", new[] { "Book_Id" });
            DropIndex("dbo.Books", new[] { "Category_Id" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.FeaturedItems");
            DropTable("dbo.Categories");
            DropTable("dbo.WishListItems");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.OrderItems");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderCoupons");
            DropTable("dbo.Coupons");
            DropTable("dbo.CartCoupons");
            DropTable("dbo.Carts");
            DropTable("dbo.CartItems");
            DropTable("dbo.Books");
        }
    }
}

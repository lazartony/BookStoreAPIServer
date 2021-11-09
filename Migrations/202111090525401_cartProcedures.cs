namespace BookStoreAPIServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class cartProcedures : DbMigration
    {
        public override void Up()
        {
        //TODO: VAlidate the functions
            Sql("ALTER TABLE dbo.[Carts] DROP COLUMN TotalValue;" +
                "ALTER TABLE dbo.[Carts] DROP COLUMN TotalDiscount;" +
                "ALTER TABLE dbo.[Carts] DROP COLUMN NetPrice;");
            Sql(@"CREATE FUNCTION calcCartTotalValue(@cart_id INT)
                    RETURNS FLOAT
                    AS
                    BEGIN
                        DECLARE @total_value FLOAT
                        SELECT @total_value = SUM(OI.Quantity * B.Price) FROM CartItems AS OI, Books AS B WHERE OI.Cart_Id = @cart_id AND OI.Book_Id = B.Id
                        IF @total_value IS NULL
                            SET @total_value = 0;

                        RETURN @total_value

                    END");

            Sql(@"CREATE FUNCTION calcCartTotalDiscount(@cart_id INT)
	                RETURNS FLOAT
	                AS
	                BEGIN
		                DECLARE @maximum_single_discount FLOAT, @total_clubbable_discount FLOAT, @maximum_discount FLOAT, @cart_value FLOAT
		                SET @cart_value = dbo.calcCartTotalValue(@cart_id)

		                SELECT @maximum_single_discount = MAX(Discount)
		                FROM(SELECT 
			                CASE WHEN(C.DiscountValue!=0 AND (C.DiscountPercentage=0 OR (C.DiscountPercentage * @cart_value)>C.DiscountValue))
			                THEN C.DiscountValue
			                ELSE (C.DiscountPercentage * @cart_value ) 
			                END AS Discount
			                FROM CartCoupons as OC, Coupons AS C WHERE OC.Cart_Id = @cart_id AND OC.Coupon_Id = C.Id AND C.MinOrderValue<=@cart_value) a

		                SELECT @total_clubbable_discount = SUM(Discount)
		                FROM(SELECT 
			                CASE WHEN(C.DiscountPercentage=0 OR (C.DiscountPercentage * @cart_value)>C.DiscountValue)
			                THEN C.DiscountValue
			                ELSE (C.DiscountPercentage * @cart_value ) 
			                END AS Discount
			                FROM CartCoupons as OC, Coupons AS C WHERE OC.Cart_Id = @cart_id AND OC.Coupon_Id = C.Id AND C.MinOrderValue<=@cart_value AND C.IsClubbable = 1) a

		                SELECT @maximum_discount = MAX(Discount)
		                FROM (VALUES (@maximum_single_discount),(@total_clubbable_discount),(0))
		                AS a(Discount)
                        IF @maximum_discount IS NULL
                            SET @maximum_discount = 0;

                        RETURN @maximum_discount
	                END");
            Sql(@"ALTER TABLE dbo.[Carts]
                    ADD TotalValue AS dbo.calcCartTotalValue(Id),
                    TotalDiscount AS dbo.calcCartTotalDiscount(Id),
                    NetPrice AS dbo.calcCartTotalValue(Id) - dbo.calcCartTotalDiscount(Id); ");
        }

        public override void Down()
        {
            //TODO: Add the columns back
            Sql("ALTER TABLE dbo.[Carts] DROP COLUMN TotalValue;" +
                "ALTER TABLE dbo.[Carts] DROP COLUMN TotalDiscount;" +
                "ALTER TABLE dbo.[Carts] DROP COLUMN NetPrice;" +
                @"DROP FUNCTION IF EXISTS calcCartTotalValue
                DROP FUNCTION IF EXISTS validateCartCoupon
                DROP FUNCTION IF EXISTS calcCartTotalDiscount"
                );
        }
    }
}

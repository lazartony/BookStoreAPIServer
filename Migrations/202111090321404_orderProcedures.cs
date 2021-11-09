namespace BookStoreAPIServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class orderProcedures : DbMigration
    {
        public override void Up()
        {
        //TODO: VAlidate the functions
            //Sql("ALTER TABLE dbo.[Orders] DROP COLUMN TotalValue;" +
            //    "ALTER TABLE dbo.[Orders] DROP COLUMN TotalDiscount;" +
            //    "ALTER TABLE dbo.[Orders] DROP COLUMN NetPrice;");
            Sql(@"CREATE FUNCTION calcOrderTotalValue(@order_id INT)
                    RETURNS FLOAT
                    AS
                    BEGIN
                        DECLARE @total_value FLOAT
                        SELECT @total_value = SUM(OI.Quantity * B.Price) FROM OrderItems AS OI, Books AS B WHERE OI.Order_Id = @order_id AND OI.Book_Id = B.Id
                        IF @total_value IS NULL
                            SET @total_value = 0;

                        RETURN @total_value

                    END");

            Sql(@"CREATE FUNCTION calcOrderTotalDiscount(@order_id INT)
	                RETURNS FLOAT
	                AS
	                BEGIN
		                DECLARE @maximum_single_discount FLOAT, @total_clubbable_discount FLOAT, @maximum_discount FLOAT, @order_value FLOAT
		                SET @order_value = dbo.calcOrderTotalValue(@order_id)

		                SELECT @maximum_single_discount = MAX(Discount)
		                FROM(SELECT 
			                CASE WHEN(C.DiscountValue!=0 AND (C.DiscountPercentage=0 OR (C.DiscountPercentage * @order_value)>C.DiscountValue))
			                THEN C.DiscountValue
			                ELSE (C.DiscountPercentage * @order_value ) 
			                END AS Discount
			                FROM OrderCoupons as OC, Coupons AS C WHERE OC.Order_Id = @order_id AND OC.Coupon_Id = C.Id AND C.MinOrderValue<=@order_value) a

		                SELECT @total_clubbable_discount = SUM(Discount)
		                FROM(SELECT 
			                CASE WHEN(C.DiscountPercentage=0 OR (C.DiscountPercentage * @order_value)>C.DiscountValue)
			                THEN C.DiscountValue
			                ELSE (C.DiscountPercentage * @order_value ) 
			                END AS Discount
			                FROM OrderCoupons as OC, Coupons AS C WHERE OC.Order_Id = @order_id AND OC.Coupon_Id = C.Id AND C.MinOrderValue<=@order_value AND C.IsClubbable = 1) a

		                SELECT @maximum_discount = MAX(Discount)
		                FROM (VALUES (@maximum_single_discount),(@total_clubbable_discount),(0))
		                AS a(Discount)
                        IF @maximum_discount IS NULL
                            SET @maximum_discount = 0;

                        RETURN @maximum_discount
	                END");
            Sql(@"ALTER TABLE dbo.[Orders]
                    ADD TotalValue AS dbo.calcOrderTotalValue(Id),
                    TotalDiscount AS dbo.calcOrderTotalDiscount(Id),
                    NetPrice AS dbo.calcOrderTotalValue(Id) - dbo.calcOrderTotalDiscount(Id); ");
        }

        public override void Down()
        {
            //TODO: Add the columns back
            Sql("ALTER TABLE dbo.[Orders] DROP COLUMN TotalValue;" +
                "ALTER TABLE dbo.[Orders] DROP COLUMN TotalDiscount;" +
                "ALTER TABLE dbo.[Orders] DROP COLUMN NetPrice;" +
                @"DROP FUNCTION IF EXISTS calcOrderTotalValue
                DROP FUNCTION IF EXISTS validateOrderCoupon
                DROP FUNCTION IF EXISTS calcOrderTotalDiscount"
                );
        }
    }
}

using BookStoreAPIServer.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookStoreAPIServer.Models
{
    public class Coupon:IEntityTimeStamps
    {
        public Coupon()
        {
            DiscountPercentage = 0;
            DiscountValue = 0;
            MinOrderValue = 0;
            IsClubbable = false;
            Status = Status.InActive;
        }
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        public double DiscountPercentage { get; set; }
        public double DiscountValue { get; set; }
        public double MinOrderValue { get; set; }
        public bool IsClubbable { get; set; }
        public Status Status { get; set; }
        public virtual ICollection<OrderCoupon> OrderCoupons { get; set; }
        public virtual ICollection<CartCoupon> CartCoupons { get; set; }
        public DateTimeOffset CreatedAt { get ; set ; }
        public DateTimeOffset LastUpdatedAt { get ; set ; }
    }
}
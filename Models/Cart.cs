using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookStoreAPIServer.Models
{
    public class Cart:IEntityTimeStamps
    {
        public Cart()
        {
            CartItems = new HashSet<CartItem>();
            CartCoupons = new HashSet<CartCoupon>();
        }
        public int Id { get; set; }
        //[Required]
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<CartCoupon> CartCoupons { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public double TotalValue { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public double TotalDiscount { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public double NetPrice { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
    }
}
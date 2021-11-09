using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookStoreAPIServer.Models
{
    public class CartCoupon : IEntityTimeStamps
    {
        public int Id { get; set; }
        [Required]
        public int Coupon_Id { get; set; }
        [ForeignKey(nameof(Coupon_Id))]
        public virtual Coupon Coupon { get; set; }
        public virtual Cart Cart { get; set; }
        [NotMapped]
        public bool Valid
        {
            get
            {
                if (this.Coupon != null && this.Coupon.MinOrderValue <= this.Cart.TotalValue && this.Coupon.Status == Helpers.Status.Active)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        DateTimeOffset IEntityTimeStamps.CreatedAt { get; set; }
        DateTimeOffset IEntityTimeStamps.LastUpdatedAt { get; set; }
    }
}
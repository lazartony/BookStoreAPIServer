using BookStoreAPIServer.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookStoreAPIServer.Models
{
    public class Order:IEntityTimeStamps
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
            OrderCoupons = new HashSet<OrderCoupon>();
        }
        public int Id { get; set; }
        [Required]
        public string DeliveryAddress { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string User_Id { get; set; }
        [ForeignKey(nameof(User_Id))]
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<OrderCoupon> OrderCoupons { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public double TotalValue { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public double TotalDiscount { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public double NetPrice { get; set; }

        DateTimeOffset IEntityTimeStamps.CreatedAt { get; set; }
        DateTimeOffset IEntityTimeStamps.LastUpdatedAt { get; set; }
    }
}
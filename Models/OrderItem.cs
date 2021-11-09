using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookStoreAPIServer.Models
{
    public class OrderItem:IEntityTimeStamps
    {
        public OrderItem()
        {
            Quantity = 1;
        }
        public int Id { get; set; }
        [Required]
        public short Quantity { get; set; }
        [Required]
        public int Book_Id { get; set; }
        [ForeignKey(nameof(Book_Id))]
        public virtual Book Book { get; set; }
        [Required]
        public int Order_Id { get; set; }
        [ForeignKey(nameof(Order_Id))]
        public virtual Order Order { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
    }
}
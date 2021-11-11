using BookStoreAPIServer.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookStoreAPIServer.Models
{
    public class Book:IEntityTimeStamps
    {
        public Book()
        {
            Title = "";
            Author = "";
            Status = Status.InActive;
        }
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string ImageUrl { get; set; }
        public short? Year { get; set; }
        [Required]
        public double Price { get; set; }
        public Status Status { get; set; }
        public int? Position { get; set; }
        [Required]
        public int Category_Id { get; set; }
        [ForeignKey(nameof(Category_Id))]
        public virtual Category Category { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
        public virtual ICollection<WishListItem> WishListItems { get; set; }
        DateTimeOffset IEntityTimeStamps.CreatedAt { get; set; }
        DateTimeOffset IEntityTimeStamps.LastUpdatedAt { get; set; }
    }
}
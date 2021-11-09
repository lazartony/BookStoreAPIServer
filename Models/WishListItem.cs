using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookStoreAPIServer.Models
{
    public class WishListItem:IEntityTimeStamps
    {
        public int Id { get; set; }
        [Required]
        public short Quantity { get; set; }
        [Required]
        public int Book_Id { get; set; }
        [ForeignKey(nameof(Book_Id))]
        public virtual Book Book { get; set; }
        [Required]
        public string User_Id { get; set; }
        [ForeignKey(nameof(User_Id))]
        public virtual ApplicationUser User { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
    }
}
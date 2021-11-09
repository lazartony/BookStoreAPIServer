using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookStoreAPIServer.Models
{
    public class FeaturedItem : IEntityTimeStamps
    {
        public int Id { get; set; }
        [Required]
        public int Book_Id { get; set; }
        [ForeignKey(nameof(Book_Id))]
        public virtual Book Book { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
    }
}
using BookStoreAPIServer.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookStoreAPIServer.Models
{
    [Table("Categories")]
    public class Category:IEntityTimeStamps
    {
        public Category()
        {
            Status = Status.InActive;
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public Status Status { get; set; }
        public int? Position { get; set; }
        public virtual ICollection<Book> Books { get; set; }
        DateTimeOffset IEntityTimeStamps.CreatedAt { get; set; }
        DateTimeOffset IEntityTimeStamps.LastUpdatedAt { get; set; }
    }
}
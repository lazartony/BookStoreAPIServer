using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStoreAPIServer.Models
{
    public interface IEntityTimeStamps
    {
        DateTimeOffset CreatedAt { get; set; }
        DateTimeOffset LastUpdatedAt { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using BookStoreAPIServer.Models;
using Microsoft.AspNet.Identity;

namespace BookStoreAPIServer.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using BookStoreAPIServer.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Order>("Orders");
    builder.EntitySet<OrderCoupon>("OrderCoupons"); 
    builder.EntitySet<OrderItem>("OrderItems"); 
    builder.EntitySet<ApplicationUser>("ApplicationUsers"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class OrdersController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: odata/Orders
        [Authorize]
        [EnableQuery]
        public IQueryable<Order> GetOrders()
        {
            if (User.IsInRole("Admin"))
            {
                return db.Orders;
            }
            else
            {
                string user_id = User.Identity.GetUserId();
                return db.Orders.Where(order => order.User.Id == user_id);
            }
        }

        // GET: odata/Orders(5)
        [Authorize]
        [EnableQuery]
        public SingleResult<Order> GetOrder([FromODataUri] int key)
        {
            if (User.IsInRole("Admin"))
            {
                return SingleResult.Create(db.Orders.Where(order => order.Id == key));
            }
            else
            {
                string user_id = User.Identity.GetUserId();
                return SingleResult.Create(db.Orders.Where(order => order.Id == key && order.User.Id == user_id));
            }
        }

        //// PUT: odata/Orders(5)
        //public IHttpActionResult Put([FromODataUri] int key, Delta<Order> patch)
        //{
        //    Validate(patch.GetEntity());

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    Order order = db.Orders.Find(key);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    patch.Put(order);

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!OrderExists(key))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return Updated(order);
        //}

        // POST: odata/Orders
        [Authorize]
        public IHttpActionResult Post(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            order.User_Id = User.Identity.GetUserId();
            db.Orders.Add(order);
            db.SaveChanges();
            //Order newOrder = db.Orders.Find(order.Id);
            return Created(db.Orders.Find(order.Id));
        }

        //// PATCH: odata/Orders(5)
        //[AcceptVerbs("PATCH", "MERGE")]
        //public IHttpActionResult Patch([FromODataUri] int key, Delta<Order> patch)
        //{
        //    Validate(patch.GetEntity());

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    Order order = db.Orders.Find(key);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    patch.Patch(order);

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!OrderExists(key))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return Updated(order);
        //}

        // DELETE: odata/Orders(5)
        [Authorize]
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Order order = db.Orders.Find(key);
            if (order == null)
            {
                return NotFound();
            }
            if (order.User.Id == User.Identity.GetUserId())
            {
                order.OrderStatus = Helpers.OrderStatus.Cancelled;
                db.Orders.Remove(order);
                db.SaveChanges();
                return Updated(order);
            }

            return Unauthorized();
        }

        // GET: odata/Orders(5)/OrderCoupons
        [Authorize]
        [EnableQuery]
        public IQueryable<OrderCoupon> GetOrderCoupons([FromODataUri] int key)
        {
            //return db.Orders.Where(m => m.Id == key).SelectMany(m => m.OrderCoupons);
            if (User.IsInRole("Admin"))
            {
                return db.OrderCoupons.Where(oi => oi.Order.Id == key);
            }
            else
            {
                string user_id = User.Identity.GetUserId();
                return db.OrderCoupons.Where(oi => oi.Order.Id == key && oi.Order.User.Id == user_id);
            }
        }

        // GET: odata/Orders(5)/OrderItems
        [Authorize]
        [EnableQuery]
        public IQueryable<OrderItem> GetOrderItems([FromODataUri] int key)
        {
            //return db.Orders.Where(m => m.Id == key).SelectMany(m => m.OrderItems);
            if (User.IsInRole("Admin"))
            {
                return db.OrderItems.Where(oi => oi.Order.Id == key);
            }
            else
            {
                string user_id = User.Identity.GetUserId();
                return db.OrderItems.Where(oi => oi.Order.Id == key && oi.Order.User.Id == user_id);
            }
        }

        // GET: odata/Orders(5)/User
        [Authorize]
        [EnableQuery]
        public SingleResult<ApplicationUser> GetUser([FromODataUri] int key)
        {
            if (User.IsInRole("Admin"))
            {
                return SingleResult.Create(db.Orders.Where(m => m.Id == key).Select(m => m.User));
            }
            else
            {
                string userId = User.Identity.GetUserId();
                return SingleResult.Create(db.Orders.Where(m => m.Id == key && m.User.Id==userId).Select(m => m.User));
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int key)
        {
            return db.Orders.Count(e => e.Id == key) > 0;
        }
    }
}

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
    builder.EntitySet<Cart>("Carts");
    builder.EntitySet<CartCoupon>("CartCoupons"); 
    builder.EntitySet<CartItem>("CartItems"); 
    builder.EntitySet<ApplicationUser>("Users"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class CartsController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: odata/Carts
        [Authorize]
        [EnableQuery]
        public IQueryable<Cart> GetCarts()
        {
            if (User.IsInRole("Admin"))
            {
                return db.Carts;
            }
            else
            return (IQueryable<Cart>)(User as ApplicationUser).Cart;
        }

        // GET: odata/Carts(5)
        [Authorize(Roles = "Admin")]
        [EnableQuery]
        public SingleResult<Cart> GetCart([FromODataUri] int key)
        {
            return SingleResult.Create(db.Carts.Where(cart => cart.Id == key));
        }

        // PUT: odata/Carts(5)
        [Authorize]
        public IHttpActionResult Put([FromODataUri] int key, Delta<Cart> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Cart cart = db.Carts.Find(key);
            if (cart == null)
            {
                return NotFound();
            }
            if (cart.User.Id == User.Identity.GetUserId())
            {
                patch.Put(cart);
            }
            else
            {
                return Unauthorized();
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(cart);
        }

        //// POST: odata/Carts
        //public IHttpActionResult Post(Cart cart)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Carts.Add(cart);
        //    db.SaveChanges();

        //    return Created(cart);
        //}

        // PATCH: odata/Carts(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Cart> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Cart cart = db.Carts.Find(key);
            if (cart == null)
            {
                return NotFound();
            }

            if (cart.User.Id == User.Identity.GetUserId())
            {
                patch.Patch(cart);
            }
            else
            {
                return Unauthorized();
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(cart);
        }

        //// DELETE: odata/Carts(5)
        //public IHttpActionResult Delete([FromODataUri] int key)
        //{
        //    Cart cart = db.Carts.Find(key);
        //    if (cart == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Carts.Remove(cart);
        //    db.SaveChanges();

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        // GET: odata/Carts(5)/CartCoupons
        [Authorize]
        [EnableQuery]
        public IQueryable<CartCoupon> GetCartCoupons([FromODataUri] int key)
        {
            //return db.Carts.Where(m => m.Id == key).SelectMany(m => m.CartCoupons);
            if (User.IsInRole("Admin"))
            {
                return db.CartCoupons.Where(oi => oi.Cart.Id == key);
            }
            else
            {
                string user_id = User.Identity.GetUserId();
                return db.CartCoupons.Where(oi => oi.Cart.Id == key && oi.Cart.User.Id == user_id);
            }
        }

        // GET: odata/Carts(5)/CartItems
        [Authorize]
        [EnableQuery]
        public IQueryable<CartItem> GetCartItems([FromODataUri] int key)
        {
            //return db.Carts.Where(m => m.Id == key).SelectMany(m => m.CartItems);
            if (User.IsInRole("Admin"))
            {
                return db.CartItems.Where(oi => oi.Cart.Id == key);
            }
            else
            {
                string user_id = User.Identity.GetUserId();
                return db.CartItems.Where(oi => oi.Cart.Id == key && oi.Cart.User.Id == user_id);
            }
        }

        // GET: odata/Carts(5)/User
        [Authorize]
        [EnableQuery]
        public SingleResult<ApplicationUser> GetUser([FromODataUri] int key)
        {
            //return SingleResult.Create(db.Carts.Where(m => m.Id == key).Select(m => m.User));
            if (User.IsInRole("Admin"))
            {
                return SingleResult.Create(db.Carts.Where(m => m.Id == key).Select(m => m.User));
            }
            else
            {
                string userId = User.Identity.GetUserId();
                return SingleResult.Create(db.Carts.Where(m => m.Id == key && m.User.Id == userId).Select(m => m.User));
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

        private bool CartExists(int key)
        {
            return db.Carts.Count(e => e.Id == key) > 0;
        }
    }
}

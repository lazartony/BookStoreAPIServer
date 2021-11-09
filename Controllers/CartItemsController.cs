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
    builder.EntitySet<CartItem>("CartItems");
    builder.EntitySet<Book>("Books"); 
    builder.EntitySet<Cart>("Carts"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class CartItemsController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: odata/CartItems
        //[EnableQuery]
        //public IQueryable<CartItem> GetCartItems()
        //{
        //    return db.CartItems;
        //}

        // GET: odata/CartItems(5)
        //[EnableQuery]
        //public SingleResult<CartItem> GetCartItem([FromODataUri] int key)
        //{
        //    return SingleResult.Create(db.CartItems.Where(cartItem => cartItem.Id == key));
        //}

        // PUT: odata/CartItems(5)
        [Authorize]
        public IHttpActionResult Put([FromODataUri] int key, Delta<CartItem> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CartItem cartItem = db.CartItems.Find(key);
            if (cartItem == null)
            {
                return NotFound();
            }
            var userId = User.Identity.GetUserId();
            if(cartItem.Cart.User.Id == userId)
            {
                patch.Put(cartItem);
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
                if (!CartItemExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(cartItem);
        }

        // POST: odata/CartItems
        [Authorize]
        public IHttpActionResult Post(CartItem cartItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.Identity.GetUserId();
            if(cartItem.Cart.User.Id == userId)
            {
                db.CartItems.Add(cartItem);
                db.SaveChanges();

                return Created(cartItem);

            }
            else
            {
                return Unauthorized();
            }
            
        }

        // PATCH: odata/CartItems(5)
        [Authorize]
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<CartItem> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CartItem cartItem = db.CartItems.Find(key);
            if (cartItem == null)
            {
                return NotFound();
            }
            var userId = User.Identity.GetUserId();
            if (cartItem.Cart.User.Id == userId)
            {
                patch.Patch(cartItem);
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
                if (!CartItemExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(cartItem);
        }

        // DELETE: odata/CartItems(5)
        [Authorize]
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            CartItem cartItem = db.CartItems.Find(key);
            if (cartItem == null)
            {
                return NotFound();
            }
            var userId = User.Identity.GetUserId();
            if (cartItem.Cart.User.Id == userId)
            {
                db.CartItems.Remove(cartItem);
                db.SaveChanges();

                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                return Unauthorized();
            }
        }

        // GET: odata/CartItems(5)/Book
        //[EnableQuery]
        //public SingleResult<Book> GetBook([FromODataUri] int key)
        //{
        //    return SingleResult.Create(db.CartItems.Where(m => m.Id == key).Select(m => m.Book));
        //}

        // GET: odata/CartItems(5)/Cart
        //[EnableQuery]
        //public SingleResult<Cart> GetCart([FromODataUri] int key)
        //{
        //    return SingleResult.Create(db.CartItems.Where(m => m.Id == key).Select(m => m.Cart));
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CartItemExists(int key)
        {
            return db.CartItems.Count(e => e.Id == key) > 0;
        }
    }
}

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

namespace BookStoreAPIServer.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using BookStoreAPIServer.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Book>("Books");
    builder.EntitySet<CartItem>("CartItems"); 
    builder.EntitySet<Category>("Categories"); 
    builder.EntitySet<OrderItem>("OrderItems"); 
    builder.EntitySet<WishListItem>("WishListItems"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class BooksController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: odata/Books
        [EnableQuery]
        public IQueryable<Book> GetBooks()
        {
            return db.Books;
        }

        // GET: odata/Books(5)
        [EnableQuery]
        public SingleResult<Book> GetBook([FromODataUri] int key)
        {
            return SingleResult.Create(db.Books.Where(book => book.Id == key));
        }

        // PUT: odata/Books(5)
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Put([FromODataUri] int key, Delta<Book> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Book book = db.Books.Find(key);
            if (book == null)
            {
                return NotFound();
            }

            patch.Put(book);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(book);
        }

        // POST: odata/Books
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Post(Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Books.Add(book);
            db.SaveChanges();

            return Created(book);
        }

        // PATCH: odata/Books(5)
        [Authorize(Roles = "Admin")]
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Book> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Book book = db.Books.Find(key);
            if (book == null)
            {
                return NotFound();
            }

            patch.Patch(book);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(book);
        }

        // DELETE: odata/Books(5)
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Book book = db.Books.Find(key);
            if (book == null)
            {
                return NotFound();
            }

            db.Books.Remove(book);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Books(5)/CartItems
        [Authorize(Roles = "Admin")]
        [EnableQuery]
        public IQueryable<CartItem> GetCartItems([FromODataUri] int key)
        {
            return db.Books.Where(m => m.Id == key).SelectMany(m => m.CartItems);
        }

        // GET: odata/Books(5)/Category
        [Authorize(Roles = "Admin")]
        [EnableQuery]
        public SingleResult<Category> GetCategory([FromODataUri] int key)
        {
            return SingleResult.Create(db.Books.Where(m => m.Id == key).Select(m => m.Category));
        }

        // GET: odata/Books(5)/OrderItems
        [Authorize(Roles = "Admin")]
        [EnableQuery]
        public IQueryable<OrderItem> GetOrderItems([FromODataUri] int key)
        {
            return db.Books.Where(m => m.Id == key).SelectMany(m => m.OrderItems);
        }

        // GET: odata/Books(5)/WishListItems
        [Authorize(Roles = "Admin")]
        [EnableQuery]
        public IQueryable<WishListItem> GetWishListItems([FromODataUri] int key)
        {
            return db.Books.Where(m => m.Id == key).SelectMany(m => m.WishListItems);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookExists(int key)
        {
            return db.Books.Count(e => e.Id == key) > 0;
        }
    }
}

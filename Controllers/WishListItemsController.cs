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
    builder.EntitySet<WishListItem>("WishListItems");
    builder.EntitySet<Book>("Books"); 
    builder.EntitySet<ApplicationUser>("ApplicationUsers"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class WishListItemsController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: odata/WishListItems
        [Authorize]
        [EnableQuery]
        public IQueryable<WishListItem> GetWishListItems()
        {
            if (User.IsInRole("Admin"))
            { 
                return db.WishListItems; 
            }
            else
            {
                string userId = User.Identity.GetUserId();
                return db.WishListItems.Where(e => e.User_Id==userId);
            }
        }

        // GET: odata/WishListItems(5)
        [Authorize]
        [EnableQuery]
        public SingleResult<WishListItem> GetWishListItem([FromODataUri] int key)
        {
            if (User.IsInRole("Admin"))
            {
                return SingleResult.Create(db.WishListItems.Where(wishListItem => wishListItem.Id == key));
            }
            else
            {
                string userId = User.Identity.GetUserId();
                return SingleResult.Create(db.WishListItems.Where(wishListItem => wishListItem.Id == key && wishListItem.User_Id==userId));
            }
        }

        //// PUT: odata/WishListItems(5)
        //public IHttpActionResult Put([FromODataUri] int key, Delta<WishListItem> patch)
        //{
        //    Validate(patch.GetEntity());

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    WishListItem wishListItem = db.WishListItems.Find(key);
        //    if (wishListItem == null)
        //    {
        //        return NotFound();
        //    }

        //    patch.Put(wishListItem);

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!WishListItemExists(key))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return Updated(wishListItem);
        //}

        // POST: odata/WishListItems
        [Authorize]
        public IHttpActionResult Post(WishListItem wishListItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            wishListItem.User_Id = User.Identity.GetUserId();
            db.WishListItems.Add(wishListItem);
            db.SaveChanges();

            return Created(wishListItem);
        }

        // PATCH: odata/WishListItems(5)
        //[AcceptVerbs("PATCH", "MERGE")]
        //public IHttpActionResult Patch([FromODataUri] int key, Delta<WishListItem> patch)
        //{
        //    Validate(patch.GetEntity());

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    WishListItem wishListItem = db.WishListItems.Find(key);
        //    if (wishListItem == null)
        //    {
        //        return NotFound();
        //    }

        //    patch.Patch(wishListItem);

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!WishListItemExists(key))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return Updated(wishListItem);
        //}

        // DELETE: odata/WishListItems(5)
        [Authorize]
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            WishListItem wishListItem = db.WishListItems.Find(key);
            if (wishListItem == null)
            {
                return NotFound();
            }
            string userId = User.Identity.GetUserId();
            if (wishListItem.User_Id == userId)
            {
                db.WishListItems.Remove(wishListItem);
                db.SaveChanges();
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                return Unauthorized();
            }
        }

        //// GET: odata/WishListItems(5)/Book
        //[EnableQuery]
        //public SingleResult<Book> GetBook([FromODataUri] int key)
        //{
        //    return SingleResult.Create(db.WishListItems.Where(m => m.Id == key).Select(m => m.Book));
        //}

        //// GET: odata/WishListItems(5)/User
        //[EnableQuery]
        //public SingleResult<ApplicationUser> GetUser([FromODataUri] int key)
        //{
        //    return SingleResult.Create(db.WishListItems.Where(m => m.Id == key).Select(m => m.User));
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WishListItemExists(int key)
        {
            return db.WishListItems.Count(e => e.Id == key) > 0;
        }
    }
}

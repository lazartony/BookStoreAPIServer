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
    builder.EntitySet<FeaturedItem>("FeaturedItems");
    builder.EntitySet<Book>("Books"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class FeaturedItemsController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: odata/FeaturedItems
        [EnableQuery]
        public IQueryable<FeaturedItem> GetFeaturedItems()
        {
            return db.FeaturedItems;
        }

        // GET: odata/FeaturedItems(5)
        [EnableQuery]
        public SingleResult<FeaturedItem> GetFeaturedItem([FromODataUri] int key)
        {
            return SingleResult.Create(db.FeaturedItems.Where(featuredItem => featuredItem.Id == key));
        }

        // PUT: odata/FeaturedItems(5)
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Put([FromODataUri] int key, Delta<FeaturedItem> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FeaturedItem featuredItem = db.FeaturedItems.Find(key);
            if (featuredItem == null)
            {
                return NotFound();
            }

            patch.Put(featuredItem);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeaturedItemExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(featuredItem);
        }

        // POST: odata/FeaturedItems
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Post(FeaturedItem featuredItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FeaturedItems.Add(featuredItem);
            db.SaveChanges();

            return Created(featuredItem);
        }

        // PATCH: odata/FeaturedItems(5)
        [Authorize(Roles = "Admin")]
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<FeaturedItem> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FeaturedItem featuredItem = db.FeaturedItems.Find(key);
            if (featuredItem == null)
            {
                return NotFound();
            }

            patch.Patch(featuredItem);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeaturedItemExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(featuredItem);
        }

        // DELETE: odata/FeaturedItems(5)
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            FeaturedItem featuredItem = db.FeaturedItems.Find(key);
            if (featuredItem == null)
            {
                return NotFound();
            }

            db.FeaturedItems.Remove(featuredItem);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/FeaturedItems(5)/Book
        //[EnableQuery]
        //public SingleResult<Book> GetBook([FromODataUri] int key)
        //{
        //    return SingleResult.Create(db.FeaturedItems.Where(m => m.Id == key).Select(m => m.Book));
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FeaturedItemExists(int key)
        {
            return db.FeaturedItems.Count(e => e.Id == key) > 0;
        }
    }
}

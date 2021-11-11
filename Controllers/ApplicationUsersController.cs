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
using Microsoft.AspNet.Identity.EntityFramework;

namespace BookStoreAPIServer.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using BookStoreAPIServer.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<ApplicationUser>("ApplicationUsers");
    builder.EntitySet<Cart>("Carts"); 
    builder.EntitySet<IdentityUserClaim>("IdentityUserClaims"); 
    builder.EntitySet<IdentityUserLogin>("IdentityUserLogins"); 
    builder.EntitySet<Order>("Orders"); 
    builder.EntitySet<IdentityUserRole>("IdentityUserRoles"); 
    builder.EntitySet<WishListItem>("WishListItems"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ApplicationUsersController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: odata/ApplicationUsers
        [Authorize(Roles = "Admin")]
        [EnableQuery]
        public IQueryable<ApplicationUser> GetApplicationUsers()
        {
            return db.Users;
        }

        // GET: odata/ApplicationUsers(5)
        [Authorize(Roles = "Admin")]
        [EnableQuery]
        public SingleResult<ApplicationUser> GetApplicationUser([FromODataUri] string key)
        {
            return SingleResult.Create(db.Users.Where(applicationUser => applicationUser.Id == key));
        }

        // PUT: odata/ApplicationUsers(5)
        public IHttpActionResult Put([FromODataUri] string key, Delta<ApplicationUser> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApplicationUser applicationUser = db.Users.Find(key);
            if (applicationUser == null)
            {
                return NotFound();
            }

            patch.Put(applicationUser);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationUserExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(applicationUser);
        }

        // POST: odata/ApplicationUsers
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Post(ApplicationUser applicationUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Users.Add(applicationUser);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ApplicationUserExists(applicationUser.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(applicationUser);
        }

        // PATCH: odata/ApplicationUsers(5)
        [Authorize(Roles = "Admin")]
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] string key, Delta<ApplicationUser> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApplicationUser applicationUser = db.Users.Find(key);
            if (applicationUser == null)
            {
                return NotFound();
            }

            patch.Patch(applicationUser);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationUserExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(applicationUser);
        }

        // DELETE: odata/ApplicationUsers(5)
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Delete([FromODataUri] string key)
        {
            ApplicationUser applicationUser = db.Users.Find(key);
            if (applicationUser == null)
            {
                return NotFound();
            }

            db.Users.Remove(applicationUser);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/ApplicationUsers(5)/Cart
        [Authorize(Roles = "Admin")]
        [EnableQuery]
        public SingleResult<Cart> GetCart([FromODataUri] string key)
        {
            return SingleResult.Create(db.Users.Where(m => m.Id == key).Select(m => m.Cart));
        }

        // GET: odata/ApplicationUsers(5)/Claims
        [Authorize(Roles = "Admin")]
        [EnableQuery]
        public IQueryable<IdentityUserClaim> GetClaims([FromODataUri] string key)
        {
            return db.Users.Where(m => m.Id == key).SelectMany(m => m.Claims);
        }

        // GET: odata/ApplicationUsers(5)/Logins
        [Authorize(Roles = "Admin")]
        [EnableQuery]
        public IQueryable<IdentityUserLogin> GetLogins([FromODataUri] string key)
        {
            return db.Users.Where(m => m.Id == key).SelectMany(m => m.Logins);
        }

        // GET: odata/ApplicationUsers(5)/Orders
        [Authorize(Roles = "Admin")]
        [EnableQuery]
        public IQueryable<Order> GetOrders([FromODataUri] string key)
        {
            return db.Users.Where(m => m.Id == key).SelectMany(m => m.Orders);
        }

        // GET: odata/ApplicationUsers(5)/Roles
        [Authorize(Roles = "Admin")]
        [EnableQuery]
        public IQueryable<IdentityUserRole> GetRoles([FromODataUri] string key)
        {
            return db.Users.Where(m => m.Id == key).SelectMany(m => m.Roles);
        }

        // GET: odata/ApplicationUsers(5)/WishListItems
        [Authorize(Roles = "Admin")]
        [EnableQuery]
        public IQueryable<WishListItem> GetWishListItems([FromODataUri] string key)
        {
            return db.Users.Where(m => m.Id == key).SelectMany(m => m.WishListItems);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ApplicationUserExists(string key)
        {
            return db.Users.Count(e => e.Id == key) > 0;
        }
    }
}

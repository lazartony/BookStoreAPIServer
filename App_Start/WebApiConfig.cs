using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;
using BookStoreAPIServer.Models;

namespace BookStoreAPIServer
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Category>("Categories");
            builder.EntitySet<Book>("Books");
            builder.EntitySet<CartItem>("CartItems");
            builder.EntitySet<FeaturedItem>("FeaturedItems");
            builder.EntitySet<OrderItem>("OrderItems");
            builder.EntitySet<WishListItem>("WishListItems");
            builder.EntitySet<Coupon>("Coupons");
            builder.EntitySet<CartCoupon>("CartCoupons");
            builder.EntitySet<OrderCoupon>("OrderCoupons");
            builder.EntitySet<Order>("Orders");
            builder.EntitySet<OrderItem>("OrderItems");
            builder.EntitySet<ApplicationUser>("ApplicationUsers");
            builder.EntitySet<Cart>("Carts");
            builder.EntitySet<FeaturedItem>("FeaturedItems");
            config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());

        }
    }
}

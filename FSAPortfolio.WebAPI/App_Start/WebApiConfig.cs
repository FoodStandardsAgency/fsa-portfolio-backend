using FSAPortfolio.WebAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FSAPortfolio.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "GetUserByNameAndHash",
                routeTemplate: "api/User",
                defaults: new { controller = "User", action = nameof(UserController.GetUser) }
            );
            config.Routes.MapHttpRoute(
                name: "GetADUserByName",
                routeTemplate: "api/ADUser",
                defaults: new { controller = "User", action = nameof(UserController.GetADUser) }
            );

            config.Routes.MapHttpRoute(
                name: "GetCurrentProjects",
                routeTemplate: "api/Project/Current",
                defaults: new { controller = "Project", action = nameof(ProjectController.GetCurrent) }
            );



            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

        }
    }
}

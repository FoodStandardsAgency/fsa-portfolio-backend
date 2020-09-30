using FSAPortfolio.WebAPI.Controllers;
using FSAPortfolio.WebAPI.Models;
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
                routeTemplate: "api/Users",
                defaults: new { controller = ControllerName<UsersController>(), action = nameof(UsersController.GetUser) }
            );
            config.Routes.MapHttpRoute(
                name: "GetADUserByName",
                routeTemplate: "api/ADUsers",
                defaults: new { controller = ControllerName<UsersController>(), action = nameof(UsersController.GetADUser) }
            );

            config.Routes.MapHttpRoute(
                name: "GetCurrentProjects",
                routeTemplate: "api/Projects/Current",
                defaults: new { controller = ControllerName<ProjectsController>(), action = nameof(ProjectsController.GetCurrent) }
            );
            config.Routes.MapHttpRoute(
                name: "GetNewProjects",
                routeTemplate: "api/Projects/New",
                defaults: new { controller = ControllerName<ProjectsController>(), action = nameof(ProjectsController.GetNew) }
            );
            config.Routes.MapHttpRoute(
                name: "GetCompletedProjects",
                routeTemplate: "api/Projects/Completed",
                defaults: new { controller = ControllerName<ProjectsController>(), action = nameof(ProjectsController.GetCompleted) }
            );
            config.Routes.MapHttpRoute(
                name: "GetLatestProjects",
                routeTemplate: "api/Projects/Latest",
                defaults: new { controller = ControllerName<ProjectsController>(), action = nameof(ProjectsController.GetLatest) }
            );
            config.Routes.MapHttpRoute(
                name: "GetODDLeads",
                routeTemplate: "api/Projects/ODDLeads",
                defaults: new { controller = ControllerName<ProjectsController>(), action = nameof(ProjectsController.GetODDLeads) }
            );

            config.Routes.MapHttpRoute(
                name: "SyncUsers",
                routeTemplate: "api/Sync/SyncUsers",
                defaults: new { controller = ControllerName<SyncController>(), action = nameof(SyncController.SyncUsers) }
            );

            config.Routes.MapHttpRoute(
                name: "SyncStatuses",
                routeTemplate: "api/Sync/SyncStatuses",
                defaults: new { controller = ControllerName<SyncController>(), action = nameof(SyncController.SyncStatuses) }
            );

            config.Routes.MapHttpRoute(
                name: "SyncProject",
                routeTemplate: "api/Sync/SyncProject",
                defaults: new { controller = ControllerName<SyncController>(), action = nameof(SyncController.SyncProject) }
            );




            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

        }

        /// <summary>
        /// Utility method to strip "Controller" from the type name to get the correct name for use with routing configuration.
        /// </summary>
        /// <typeparam name="T">The controller type</typeparam>
        /// <returns>The name of the controller name with "Controller" stripped from the end.</returns>
        private static string ControllerName<T>()
        {
            var name = typeof(T).Name;
            string controllerName = name.EndsWith("Controller") ? name.Substring(0, name.Length - 10) : name;
            return controllerName;
        }
    }
}

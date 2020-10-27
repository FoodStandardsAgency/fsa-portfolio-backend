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

            #region Portfolio Configuration

            #endregion

            #region Portfolio 
            config.Routes.MapHttpRoute(
                name: "PortfolioSummary",
                routeTemplate: "api/Portfolios/{portfolio}/summary",
                defaults: new { controller = ControllerName<PortfoliosController>(), action = nameof(PortfoliosController.Summary) }
            );
            config.Routes.MapHttpRoute(
                name: "PortfolioFilterOptions",
                routeTemplate: "api/Portfolios/{portfolio}/filteroptions",
                defaults: new { controller = ControllerName<PortfoliosController>(), action = nameof(PortfoliosController.FilterOptionsAsync) }
            );
            config.Routes.MapHttpRoute(
                name: "PortfolioQuery",
                routeTemplate: "api/Portfolios/{portfolio}/projects",
                defaults: new { controller = ControllerName<PortfoliosController>(), action = nameof(PortfoliosController.GetFilteredProjectsAsync) }
            );

            #endregion


            #region Projects
            config.Routes.MapHttpRoute(
                name: "NewProjectConfig",
                routeTemplate: "api/Projects/{portfolio}/newproject",
                defaults: new { controller = ControllerName<ProjectsController>(), action = nameof(ProjectsController.GetNewProject) }
            );
            config.Routes.MapHttpRoute(
                name: "EditProjectView",
                routeTemplate: "api/Projects/{projectId}/edit",
                defaults: new { controller = ControllerName<ProjectsController>(), action = nameof(ProjectsController.GetForEdit) }
            );
            config.Routes.MapHttpRoute(
                name: "Project",
                routeTemplate: "api/Projects/{projectId}",
                defaults: new { controller = ControllerName<ProjectsController>(), action = nameof(ProjectsController.Get) }
            );
            #endregion

            #region Legacy
            config.Routes.MapHttpRoute(
                name: "Legacy_GetCurrentProjects",
                routeTemplate: "api/Projects/Legacy/Current",
                defaults: new { controller = ControllerName<LegacyProjectsController>(), action = nameof(LegacyProjectsController.GetCurrent) }
            );
            config.Routes.MapHttpRoute(
                name: "Legacy_GetNewProjects",
                routeTemplate: "api/Projects/Legacy/New",
                defaults: new { controller = ControllerName<LegacyProjectsController>(), action = nameof(LegacyProjectsController.GetNew) }
            );
            config.Routes.MapHttpRoute(
                name: "Legacy_GetCompletedProjects",
                routeTemplate: "api/Projects/Legacy/Completed",
                defaults: new { controller = ControllerName<LegacyProjectsController>(), action = nameof(LegacyProjectsController.GetCompleted) }
            );
            config.Routes.MapHttpRoute(
                name: "Legacy_GetLatestProjects",
                routeTemplate: "api/Projects/Legacy/Latest",
                defaults: new { controller = ControllerName<LegacyProjectsController>(), action = nameof(LegacyProjectsController.GetLatest) }
            );
            config.Routes.MapHttpRoute(
                name: "Legacy_GetODDLeads",
                routeTemplate: "api/Projects/Legacy/ODDLeads",
                defaults: new { controller = ControllerName<LegacyProjectsController>(), action = nameof(LegacyProjectsController.GetODDLeads) }
            );
            config.Routes.MapHttpRoute(
                name: "Legacy_GetUnmatchedODDLeads",
                routeTemplate: "api/Projects/Legacy/UnmatchedODDLeads",
                defaults: new { controller = ControllerName<LegacyProjectsController>(), action = nameof(LegacyProjectsController.GetUnmatchedODDLeads) }
            );
            #endregion

            #region Sync
            config.Routes.MapHttpRoute(
                name: "SyncAll",
                routeTemplate: "api/Sync/SyncAll",
                defaults: new { controller = ControllerName<SyncController>(), action = nameof(SyncController.SyncAll) }
            );
            config.Routes.MapHttpRoute(
                name: "SyncPeople",
                routeTemplate: "api/Sync/SyncPeople",
                defaults: new { controller = ControllerName<SyncController>(), action = nameof(SyncController.SyncPeople) }
            );
            config.Routes.MapHttpRoute(
                name: "SyncUsers",
                routeTemplate: "api/Sync/SyncUsers",
                defaults: new { controller = ControllerName<SyncController>(), action = nameof(SyncController.SyncUsers) }
            );
            config.Routes.MapHttpRoute(
                name: "SyncProject",
                routeTemplate: "api/Sync/SyncProject",
                defaults: new { controller = ControllerName<SyncController>(), action = nameof(SyncController.SyncProject) }
            );
            config.Routes.MapHttpRoute(
                name: "SyncAllProjects",
                routeTemplate: "api/Sync/SyncAllProjects",
                defaults: new { controller = ControllerName<SyncController>(), action = nameof(SyncController.SyncAllProjects) }
            );
            #endregion


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

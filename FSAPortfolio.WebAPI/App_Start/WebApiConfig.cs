using FSAPortfolio.Application.Services;
using FSAPortfolio.Application.Services.Projects;
using FSAPortfolio.WebAPI.App;
using FSAPortfolio.WebAPI.Controllers;
using FSAPortfolio.Application.Models;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Unity;
using Unity.Lifetime;

namespace FSAPortfolio.WebAPI
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

            #region Access Groups
            config.Routes.MapHttpRoute(
                name: "InitAccessGroups",
                routeTemplate: "api/AccessGroups/init",
                defaults: new { controller = ControllerName<AccessGroupsController>(), action = nameof(AccessGroupsController.Init) }
            );
            #endregion

            #region Users
            config.Routes.MapHttpRoute(
                name: "CreateUser",
                routeTemplate: "api/Users/create",
                defaults: new { controller = ControllerName<UsersController>(), action = nameof(UsersController.CreateUser) }
            );
            config.Routes.MapHttpRoute(
                name: "GetIdentity",
                routeTemplate: "api/Users/identity",
                defaults: new { controller = ControllerName<UsersController>(), action = nameof(UsersController.GetIdentity) }
            );
            config.Routes.MapHttpRoute(
                name: "SearchUsers",
                routeTemplate: "api/Users/search",
                defaults: new { controller = ControllerName<UsersController>(), action = nameof(UsersController.SearchUsers) }
            );
            config.Routes.MapHttpRoute(
                name: "GetSuppliers",
                routeTemplate: "api/Users/suppliers",
                defaults: new { controller = ControllerName<UsersController>(), action = nameof(UsersController.GetSuppliers) }
            );
            config.Routes.MapHttpRoute(
                name: "AddSupplier",
                routeTemplate: "api/Users/addsupplier",
                defaults: new { controller = ControllerName<UsersController>(), action = nameof(UsersController.AddSupplier) }
            );
            config.Routes.MapHttpRoute(
                name: "GetUserByNameAndHash",
                routeTemplate: "api/Users/legacy",
                defaults: new { controller = ControllerName<UsersController>(), action = nameof(UsersController.GetUser) }
            );
            config.Routes.MapHttpRoute(
                name: "GetADUserByName",
                routeTemplate: "api/Users/LegacyADUsers",
                defaults: new { controller = ControllerName<UsersController>(), action = nameof(UsersController.GetADUser) }
            );
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
            config.Routes.MapHttpRoute(
                name: "PortfolioExport",
                routeTemplate: "api/Portfolios/{portfolio}/export",
                defaults: new { controller = ControllerName<PortfoliosController>(), action = nameof(PortfoliosController.GetExportProjectsAsync) }
            );
            config.Routes.MapHttpRoute(
                name: "PortfolioImport",
                routeTemplate: "api/Portfolios/{portfolio}/import",
                defaults: new { controller = ControllerName<PortfoliosController>(), action = nameof(PortfoliosController.ImportAsync) }
            );
            config.Routes.MapHttpRoute(
                name: "PortfolioArchive",
                routeTemplate: "api/Portfolios/{portfolio}/archive",
                defaults: new { controller = ControllerName<PortfoliosController>(), action = nameof(PortfoliosController.ArchiveProjectsAsync) }
            );
            config.Routes.MapHttpRoute(
                name: "CreatePortfolio",
                routeTemplate: "api/NewPortfolio",
                defaults: new { controller = ControllerName<PortfoliosController>(), action = nameof(PortfoliosController.CreateAsync) }
            );
            config.Routes.MapHttpRoute(
                name: "AddPortfolioPermission",
                routeTemplate: "api/Portfolios/{portfolio}/permission",
                defaults: new { controller = ControllerName<PortfoliosController>(), action = nameof(PortfoliosController.AddPermissionAsync) }
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
                name: "SyncPortfolios",
                routeTemplate: "api/Sync/SyncPortfolios",
                defaults: new { controller = ControllerName<SyncController>(), action = nameof(SyncController.SyncPortfolios) }
            );
            config.Routes.MapHttpRoute(
                name: "SyncProject",
                routeTemplate: "api/Sync/SyncProject",
                defaults: new { controller = ControllerName<SyncController>(), action = nameof(SyncController.SyncProject) }
            );
            config.Routes.MapHttpRoute(
                name: "SyncAllProjects",
                routeTemplate: "api/Sync/SyncAllProjects/{portfolio}",
                defaults: new { controller = ControllerName<SyncController>(), action = nameof(SyncController.SyncAllProjects), portfolio = RouteParameter.Optional }
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

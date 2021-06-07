using FSAPortfolio.Application.Services;
using FSAPortfolio.Application.Services.Config;
using FSAPortfolio.Application.Services.Projects;
using FSAPortfolio.Common.Logging;
using FSAPortfolio.Entities;
using System;
using System.Web;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace FSAPortfolio.WebAPI
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              AppLog.TraceVerbose($"Creating container and registering types for Unity Dependency Injection...");
              var container = new UnityContainer().AddExtension(new Diagnostic());
              RegisterTypes(container);


              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            container.RegisterFactory<Lazy<PortfolioContext>>(c => new Lazy<PortfolioContext>(() => new PortfolioContext()));
            container.RegisterType<IServiceContext, ServiceContext>(new HierarchicalLifetimeManager());
            container.RegisterType<IProjectDataService, ProjectDataService>(new HierarchicalLifetimeManager());
            container.RegisterType<IPortfolioService, PortfolioService>(new HierarchicalLifetimeManager());
            container.RegisterType<IPortfolioConfigurationService, PortfolioConfigurationService>(new HierarchicalLifetimeManager());
        }
    }
}
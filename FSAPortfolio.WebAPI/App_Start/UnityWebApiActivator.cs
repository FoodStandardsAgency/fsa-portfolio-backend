using FSAPortfolio.Common.Logging;
using System.Web.Http;
using Unity.AspNet.WebApi;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(FSAPortfolio.WebAPI.UnityWebApiActivator), nameof(FSAPortfolio.WebAPI.UnityWebApiActivator.Start))]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(FSAPortfolio.WebAPI.UnityWebApiActivator), nameof(FSAPortfolio.WebAPI.UnityWebApiActivator.Shutdown))]

namespace FSAPortfolio.WebAPI
{
    /// <summary>
    /// Provides the bootstrapping for integrating Unity with WebApi when it is hosted in ASP.NET.
    /// </summary>
    public static class UnityWebApiActivator
    {
        /// <summary>
        /// Integrates Unity when the application starts.
        /// </summary>
        public static void Start() 
        {
            // Use UnityHierarchicalDependencyResolver if you want to use
            // a new child container for each IHttpController resolution.
            AppLog.TraceVerbose($"Starting WebAPI Unity Dependency Injection...");

            var resolver = new UnityHierarchicalDependencyResolver(UnityConfig.Container);
            //var resolver = new UnityDependencyResolver(UnityConfig.Container);

            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }

        /// <summary>
        /// Disposes the Unity container when the application is shut down.
        /// </summary>
        public static void Shutdown()
        {
            UnityConfig.Container.Dispose();
        }
    }
}
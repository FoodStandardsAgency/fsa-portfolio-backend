using AutoMapper;
using FSAPortfolio.WebAPI.Mapping.Organisation;
using FSAPortfolio.WebAPI.Mapping.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Mapping
{
    internal class PortfolioMapper
    {
        internal static MapperConfiguration projectConfig;
        internal static MapperConfiguration configConfig;
        internal static MapperConfiguration updateConfig;

        internal static IMapper ProjectMapper { get; private set; }
        internal static IMapper ConfigMapper { get; private set; }
        internal static IMapper UpdateMapper { get; private set; }
        internal static void Configure()
        {
            projectConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PostgresProjectMappingProfile>();
                cfg.AddProfile<ProjectMappingProfile>();
                cfg.AddProfile<ProjectOptionsMappingProfile>();
            });
            ProjectMapper = projectConfig.CreateMapper();

            configConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PortfolioConfigurationMappingProfile>();
            });
            ConfigMapper = configConfig.CreateMapper();

            updateConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PortfolioConfigUpdateProfile>();
            });
            UpdateMapper = updateConfig.CreateMapper();
        }
    }
}
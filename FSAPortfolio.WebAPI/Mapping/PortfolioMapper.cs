using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Mapping
{
    internal class PortfolioMapper
    {
        internal static MapperConfiguration config;
        internal static MapperConfiguration updateConfig;

        internal static IMapper Mapper { get; private set; }
        internal static IMapper UpdateMapper { get; private set; }
        internal static void Configure()
        {
            config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProjectMappingProfile>();
                cfg.AddProfile<PortfolioConfigurationMappingProfile>();
            });
            Mapper = config.CreateMapper();
            updateConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PortfolioConfigUpdateProfile>();
            });
            UpdateMapper = updateConfig.CreateMapper();
        }
    }
}
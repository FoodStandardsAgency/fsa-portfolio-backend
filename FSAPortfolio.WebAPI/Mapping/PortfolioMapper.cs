﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Mapping
{
    internal class PortfolioMapper
    {
        private static MapperConfiguration config;

        internal static IMapper Mapper { get; private set; }
        internal static void Configure()
        {
            config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProjectMappingProfile>();
            });
            Mapper = config.CreateMapper();
        }
    }
}
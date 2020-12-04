using AutoMapper;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.WebAPI.App.Mapping.ActiveDirectory;
using FSAPortfolio.WebAPI.App.Mapping.Organisation;
using FSAPortfolio.WebAPI.App.Mapping.Projects;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Mapping
{
    internal class PortfolioMapper
    {
        internal static MapperConfiguration projectConfig;
        internal static MapperConfiguration configConfig;
        internal static MapperConfiguration updateConfig;
        internal static MapperConfiguration exportConfig;
        internal static MapperConfiguration activeDirectoryConfig;

        internal static IMapper ProjectMapper { get; private set; }
        internal static IMapper ConfigMapper { get; private set; }
        internal static IMapper UpdateMapper { get; private set; }
        internal static IMapper ExportMapper { get; private set; }
        internal static IMapper ActiveDirectoryMapper { get; private set; }
        internal static void Configure()
        {
            projectConfig = new MapperConfiguration(cfg =>
            {
                cfg.AllowNullCollections = true;
                cfg.AddProfile<PostgresODDProjectMappingProfile>();
                cfg.AddProfile<ProjectViewModelProfile>();
                cfg.AddProfile<ProjectUpdateModelProfile>();
                cfg.AddProfile<ProjectQueryModelProfile>();
                cfg.AddProfile<ProjectEditOptionsMappingProfile>();
            });
            ProjectMapper = projectConfig.CreateMapper();

            configConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PortfolioMappingProfile>();
                cfg.AddProfile<PortfolioConfigurationMappingProfile>();
                cfg.AddProfile<PortfolioSummariesMappingProfile>();
            });
            ConfigMapper = configConfig.CreateMapper();

            updateConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PortfolioConfigUpdateProfile>();
            });
            UpdateMapper = updateConfig.CreateMapper();

            activeDirectoryConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ADUserMappingProfile>();
            });
            ActiveDirectoryMapper = activeDirectoryConfig.CreateMapper();

            exportConfig = new MapperConfiguration(cfg =>
            {
                cfg.AllowNullCollections = true;
                cfg.AddProfile<ProjectExportModelProfile>();
            });
            ExportMapper = exportConfig.CreateMapper();

        }

        internal static ProjectLabelConfigModel GetProjectLabelConfigModel(
            PortfolioConfiguration config,
            PortfolioFieldFlags flags = PortfolioFieldFlags.Read,
            bool includedOnly = false,
            IEnumerable<PortfolioLabelConfig> customLabels = null,
            bool fsaOnly = false)
        {
            return ProjectMapper.Map<ProjectLabelConfigModel>(config, opts => { 
                opts.Items[nameof(PortfolioFieldFlags)] = flags;
                if (includedOnly) opts.Items[nameof(PortfolioLabelConfig.Included)] = includedOnly;
                if (customLabels != null) opts.Items[nameof(PortfolioLabelConfig)] = customLabels;
                if (fsaOnly) opts.Items[nameof(PortfolioFieldFlags.FSAOnly)] = fsaOnly;
            });
        }

        internal static PropertyInfo[] GetUnmappedSourceMembers<TSource, TDest>(MapperConfiguration config)
        {
            TypeMap typeMap = config.FindTypeMapFor<TSource, TDest>();
            var memberMaps = typeMap.MemberMaps.Where(m => !m.Ignored);

            Func<IMemberMap, string> extractSourceMemberName = m =>
            {
                string name = m.SourceMember?.Name;
                if (name == null) name = ((MemberExpression)m.ValueResolverConfig?.SourceMember?.Body)?.Member.Name;
                return name;
            };
            var mappedProperties = memberMaps.Select(m => extractSourceMemberName(m)).Where(n => n != null).ToArray();
            var unmappedProperties = typeof(TSource).GetProperties().Where(p => !mappedProperties.Contains(p.Name)).ToArray();
            return unmappedProperties;
        }
        internal static PropertyInfo[] GetUnmappedDestinationMembers<TSource, TDest>(MapperConfiguration config)
        {
            TypeMap typeMap = config.FindTypeMapFor<TSource, TDest>();
            var memberMaps = typeMap.MemberMaps.Where(m => !m.Ignored);

            Func<IMemberMap, string> extractDestinationMemberName = m =>
            {
                string name = m.DestinationName;
                return name;
            };
            var mappedProperties = memberMaps.Select(m => extractDestinationMemberName(m)).Where(n => n != null).ToArray();
            var unmappedProperties = typeof(TDest).GetProperties().Where(p => !mappedProperties.Contains(p.Name)).ToArray();
            return unmappedProperties;
        }
    }
}
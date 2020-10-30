﻿using AutoMapper;
using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace FSAPortfolio.WebAPI.Mapping.Projects.Resolvers
{
    public class ProjectDataInboundResolver : IValueResolver<ProjectUpdateModel, Project, ICollection<ProjectDataItem>>
    {
        /// <summary>
        /// Unmapped <see cref="ProjectUpdateModel"/> properties
        /// </summary>
        internal static readonly Dictionary<string, PropertyInfo> unmappedProperties;
        static ProjectDataInboundResolver()
        {
            var unmappedToProject = PortfolioMapper.GetUnmappedSourceMembers<ProjectUpdateModel, Project>(PortfolioMapper.projectConfig);
            var unmappedToUpdate = PortfolioMapper.GetUnmappedSourceMembers<ProjectUpdateModel, ProjectUpdateItem>(PortfolioMapper.projectConfig);
            unmappedProperties = unmappedToProject.Intersect(unmappedToUpdate).ToDictionary(p => p.Name);
        }

        public ICollection<ProjectDataItem> Resolve(ProjectUpdateModel source, Project destination, ICollection<ProjectDataItem> destMember, ResolutionContext context)
        {
            var labels = destination.Reservation.Portfolio.Configuration.Labels;
            var portfolioContext = context.Items[nameof(PortfolioContext)] as PortfolioContext;
            var dataItems = destMember?.ToList() ?? new List<ProjectDataItem>();
            foreach (var label in labels)
            {
                if (label.FieldType != PortfolioFieldType.Auto && label.Flags.HasFlag(PortfolioFieldFlags.ProjectData)) // Ignore autogenerated fields
                {
                    PropertyInfo property;
                    if (unmappedProperties.TryGetValue(label.FieldName, out property))
                    {
                        var dataItem = dataItems?.SingleOrDefault(i => i.Label.Id == label.Id);
                        var value = property.GetValue(source);
                        if (value != null && !value.Equals(string.Empty))
                        {
                            if(dataItem == null)
                            {
                                dataItem = new ProjectDataItem() { Label = label };
                                dataItems.Add(dataItem);
                            }
                            dataItem.Value = JsonConvert.SerializeObject(value);
                        }
                        else if (dataItem != null)
                        {
                            dataItems.Remove(dataItem);
                            portfolioContext.ProjectDataItems.Remove(dataItem);
                        }
                    }
                }
            }
            return dataItems;
        }
    }

    public class ProjectDataOutboundMapper
    {
        /// <summary>
        /// Unmapped <see cref="ProjectModel"/> properties
        /// </summary>
        internal static readonly Dictionary<string, PropertyInfo> unmappedProperties;
        static ProjectDataOutboundMapper()
        {
            var unmappedToProject = PortfolioMapper.GetUnmappedDestinationMembers<Project, ProjectModel>(PortfolioMapper.projectConfig);
            unmappedProperties = unmappedToProject.ToDictionary(p => p.Name);
        }
        public static void Map(Project source, ProjectModel model)
        {
            // Project data properties
            foreach(var dataItem in source.ProjectData)
            {
                PropertyInfo property;
                if(unmappedProperties.TryGetValue(dataItem.Label.FieldName, out property))
                {
                    property.SetValue(model, JsonConvert.DeserializeObject(dataItem.Value, property.PropertyType));
                }
            }

            // Unmodelled properties
            var unmodelledPropertiesQuery = from l in source.Reservation.Portfolio.Configuration.Labels
                                            where l.Flags.HasFlag(PortfolioFieldFlags.NotModelled)
                                            join d in source.ProjectData on l.Id equals d.Label_Id into unmd
                                            from d in unmd.DefaultIfEmpty()
                                            select new ProjectPropertyModel() { FieldName = l.FieldName, ProjectDataValue = d?.Value };

            model.Properties = unmodelledPropertiesQuery.ToList();
        }
    }

}
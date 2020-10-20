﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.App.Sync;
using FSAPortfolio.WebAPI.Mapping;
using FSAPortfolio.WebAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace FSAPortfolio.UnitTests
{
    [TestClass]
    public class MappingTests
    {
        [TestMethod]
        public void MappingProfilesAreValid()
        {
            PortfolioMapper.Configure();
            PortfolioMapper.projectConfig.AssertConfigurationIsValid();
            PortfolioMapper.configConfig.AssertConfigurationIsValid();
            PortfolioMapper.updateConfig.AssertConfigurationIsValid();
        }


        [TestMethod]
        public void Map_ProjectModel_Project()
        {
            PortfolioMapper.Configure();
            PortfolioConfiguration config = new PortfolioConfiguration() { LabelGroups = new List<PortfolioLabelGroup>() };
            DefaultFieldLabels labelFactory = new DefaultFieldLabels(config);
            var labels = labelFactory.GetDefaultLabels();

            ProjectModel source = new ProjectModel()
            { 
                rels = new string[] { "a", "b" },
                fsaproc_assurance_gatecompleted = DateTime.Now
            };

            var unmappedToProject = PortfolioMapper.GetUnmappedSourceMembers<ProjectModel, Project>(PortfolioMapper.projectConfig);
            var unmappedToUpdate = PortfolioMapper.GetUnmappedSourceMembers<ProjectModel, ProjectUpdateItem>(PortfolioMapper.projectConfig);

            var unmappedProperties = unmappedToProject.Intersect(unmappedToUpdate).ToArray();

            List<ProjectDataItem> dataItems = new List<ProjectDataItem>();
            foreach (var label in labels)
            {
                if (label.FieldType != PortfolioFieldType.Auto) // Ignore autogenerated fields
                {
                    var property = unmappedProperties.SingleOrDefault(p => p.Name == label.FieldName);
                    if (property != null)
                    {
                        var value = property.GetValue(source);
                        if(value != null)
                        {
                            dataItems.Add(new ProjectDataItem() { Label = label, Value = JsonConvert.SerializeObject(value) });
                        }
                    }
                }
            }


            //var project = PortfolioMapper.ProjectMapper.Map<Project>(source, opt => opt.Items[nameof(PortfolioConfiguration.Labels)] = labels);
        }

    }
}

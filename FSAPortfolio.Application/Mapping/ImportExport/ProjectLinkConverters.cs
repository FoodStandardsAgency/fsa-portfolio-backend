using AutoMapper;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Mapping.ImportExport
{
    public class ProjectLinkExportConverter : ITypeConverter<ProjectLink, string>
    {
        public string Convert(ProjectLink source, string destination, ResolutionContext context)
        {
            string result = null;
            if (!(string.IsNullOrWhiteSpace(source.Name) && string.IsNullOrWhiteSpace(source.Link)))
            {
                result = $"[{source.Name}][{source.Link}]";
            }
            return result;
        }
    }
    public class ProjectLinkImportResolver : ITypeConverter<string, LinkModel>
    {
        public LinkModel Convert(string source, LinkModel destination, ResolutionContext context)
        {
            LinkModel link = new LinkModel();
            if(!string.IsNullOrWhiteSpace(source))
            {
                var match = Regex.Match(source, @"^\[(?<name>.*)\]\[(?<link>.*)\]$");
                if(match.Success)
                {
                    link.Name = match.Groups["name"].Value;
                    link.Link = match.Groups["link"].Value;
                }
            }
            return link;
        }
    }
    public class ProjectLinkCollectionImportResolver : ITypeConverter<string, LinkModel[]>
    {
        public LinkModel[] Convert(string source, LinkModel[] destination, ResolutionContext context)
        {
            LinkModel[] links = null;
            if (!string.IsNullOrWhiteSpace(source))
            {
                var linkStrings = source.Split('|');
                links = linkStrings.Select(s => context.Mapper.Map<LinkModel>(s)).ToArray();
            }
            return links;
        }
    }

}
using AutoMapper;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Mapping.ImportExport
{
    public class DocumentExportConverter : ITypeConverter<Document, string>
    {
        public string Convert(Document source, string destination, ResolutionContext context)
        {
            string result = null;
            if (!(string.IsNullOrWhiteSpace(source.Name) && string.IsNullOrWhiteSpace(source.Link)))
            {
                result = $"[{source.Name}][{source.Link}]";
            }
            return result;
        }
    }
    public class DocumentCollectionExportConverter : ITypeConverter<ICollection<Document>, string>
    {
        public string Convert(ICollection<Document> source, string destination, ResolutionContext context)
        {
            string result = null;
            if (source != null && source.Count > 0)
            {
                result = string.Join("|", source.OrderBy(d => d.Order).Select(s => context.Mapper.Map<string>(s)));
            }
            return result;
        }
    }
}
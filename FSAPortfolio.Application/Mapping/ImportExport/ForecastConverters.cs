using AutoMapper;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.Application.Mapping.ImportExport
{
    public class ForecastExportConverter : ITypeConverter<Forecast, string>
    {
        public string Convert(Forecast source, string destination, ResolutionContext context)
        {
            string result = null;
            if (!(string.IsNullOrWhiteSpace(source.Name) && string.IsNullOrWhiteSpace(source.Amount)))
            {
                result = $"[{source.Name}][{source.Amount}]";
            }
            return result;
        }
    }
    public class ForecastCollectionExportConverter : ITypeConverter<ICollection<Forecast>, string>
    {
        public string Convert(ICollection<Forecast> source, string destination, ResolutionContext context)
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
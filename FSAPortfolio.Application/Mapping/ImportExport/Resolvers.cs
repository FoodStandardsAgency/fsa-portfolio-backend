using AutoMapper;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Application.Models;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace FSAPortfolio.Application.Mapping.ImportExport
{
    public class ProjectExportDateConverter : ITypeConverter<ProjectDate, string>
    {
        public const string FullDateExportFormat = "dd/MM/yyyy";
        public const string MonthDateExportFormat = "MM/yyyy";
        public const string YearDateExportFormat = "yyyy";

        public string Convert(ProjectDate source, string destination, ResolutionContext context)
        {
            string result = string.Empty;
            if (source?.Date != null)
            {
                if (source.Flags.HasFlag(ProjectDateFlags.Day)) result = source.Date.Value.ToString(FullDateExportFormat);
                else if (source.Flags.HasFlag(ProjectDateFlags.Month)) result = source.Date.Value.ToString(MonthDateExportFormat);
                else if (source.Flags.HasFlag(ProjectDateFlags.Year)) result = source.Date.Value.ToString(YearDateExportFormat);
            }
            return result;
        }
    }

    public class ProjectImportDateConverter : ITypeConverter<string, ProjectDateEditModel>
    {
        public const string FullDateExportFormatRegex = @"\d{2}/\d{2}/\d{4}";
        public const string MonthDateExportFormatRegex = @"\d{2}/\d{4}";
        public const string YearDateExportFormatRegex = @"\d{4}";

        private CultureInfo dateFormatProvider = CultureInfo.InvariantCulture;
        public ProjectDateEditModel Convert(string source, ProjectDateEditModel destination, ResolutionContext context)
        {
            ProjectDateEditModel result = new ProjectDateEditModel();
            if(Regex.IsMatch(source, FullDateExportFormatRegex))
            {
                var date = DateTime.ParseExact(source, ProjectExportDateConverter.FullDateExportFormat, dateFormatProvider);
                result.Day = date.Day;
                result.Month = date.Month;
                result.Year = date.Year;
            }
            else if (Regex.IsMatch(source, MonthDateExportFormatRegex))
            {
                var date = DateTime.ParseExact(source, ProjectExportDateConverter.MonthDateExportFormat, dateFormatProvider);
                result.Month = date.Month;
                result.Year = date.Year;
            }
            else if (Regex.IsMatch(source, YearDateExportFormatRegex))
            {
                var date = DateTime.ParseExact(source, ProjectExportDateConverter.YearDateExportFormat, dateFormatProvider);
                result.Year = date.Year;
            }
            return result;
        }
    }

    public class ProjectImportFloatConverter : ITypeConverter<string, float?>
    {
        public float? Convert(string source, float? destination, ResolutionContext context)
        {
            return string.IsNullOrEmpty(source) ? default(float?) : context.Mapper.Map<float>(source);
        }
    }
}
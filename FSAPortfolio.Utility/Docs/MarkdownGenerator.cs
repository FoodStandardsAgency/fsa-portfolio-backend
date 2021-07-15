using FSAPortfolio.ApiClient;
using FSAPortfolio.Application.Models;
using FSAPortfolio.Entities.Organisation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FSAPortfolio.Utility.Docs
{
    public class MarkdownGenerator
    {
        internal static async Task OutputFile(string file)
        {

            // Get the FSAPortfolio.Application xml file path
            var solutionPath = GetSolutionPath();
            var appXmlSetting = ConfigurationManager.AppSettings["FSAPortfolio.Application.XML"];
            var appXmlPath = Path.Combine(solutionPath, appXmlSetting);
            var doc = XElement.Load(appXmlPath);

            // Get the XML doc members
            var members = GetMembers(doc);

            // Get descriptors for all labels
            var config = await GetConfig(ConfigurationManager.AppSettings["portfolio"]);
            var fieldDescriptions = config.Labels.Select(l => new FieldDescription(l)).ToDictionary(fd => fd.FieldName);

            // Get properties for the Models
            var typeToPropertyMap = new Dictionary<string, Dictionary<string, FieldProperty>>();

            AddTypeToMap<ProjectModel>(typeToPropertyMap);
            AddTypeToMap<ProjectUpdateModel>(typeToPropertyMap);
            foreach(var map in typeToPropertyMap.Values)
            {
                foreach(var p in map.Values)
                {
                    if (fieldDescriptions.TryGetValue(p.FieldName, out FieldDescription fd))
                    {
                        fd.SetProperty(p);
                    }
                }
            }


            // Use the properies to fill in summaries on the descriptors
            foreach (var member in members.Where(m => m.IsProperty))
            {
                if (typeToPropertyMap.TryGetValue(member.TypeName, out Dictionary<string, FieldProperty> fieldNameMap))
                {
                    var fieldProperty = fieldNameMap[member.MemberName];
                    if (fieldDescriptions.TryGetValue(fieldProperty.FieldName, out FieldDescription fd))
                    {
                        fd.SetXElement(member);
                    }
                    else
                    {
                        // Ignore?
                    }
                }
                else
                {

                }
            }

            // Sort field descriptions by group
            var groups = from fd in fieldDescriptions.Values
                         group fd by new { fd.Label.GroupOrder, fd.Label.FieldGroup } into groupedFields
                         select groupedFields;

            var fieldsMdFilePath = ConfigurationManager.AppSettings["FieldsMdOutputPath"];
            using (var writer = new StreamWriter(fieldsMdFilePath))
            {
                writer.WriteLine("# Project Field Definitions");
                foreach (var group in groups.OrderBy(g => g.Key.GroupOrder))
                {
                    writer.WriteLine();
                    writer.WriteLine("---");
                    writer.WriteLine();
                    writer.WriteLine($"## {group.Key.FieldGroup}");
                    writer.WriteLine();
                    foreach (var p in group.OrderBy(p => p.Label.FieldOrder))
                    {
                        // Output markdown
                        //writer.WriteLine($"### `{p.Title}` ({p.Label.FieldTypeDescription})");
                        writer.WriteLine($"### {p.Title}");
                        if (p.HasSummary)
                        {
                            writer.WriteLine(p.Summary);
                            writer.WriteLine();
                        }
                        else
                        {
                            writer.WriteLine(p.Description);
                            writer.WriteLine();
                        }

                        // Build notes
                        var notes = new List<string>();
                        if (p.Label.Required) notes.Add("required: projects must have a value set");
                        if (p.IsTracked) notes.Add("has changes explicitly tracked");
                        if (p.Label.IncludedLock) notes.Add("can't be excluded in the portfolio configuration and is always included in project views");
                        //if (!p.Label.AdminOnly && p.Label.AdminOnlyLock) notes.Add("can't be set to *Admin only*");
                        if (p.Label.FSAOnly) notes.Add("is only visible to FSA employees");
                        if (p.Label.MasterField != null)
                        {
                            var masterLabel = config.Labels.Single(l => l.FieldName == p.Label.MasterField);
                            notes.Add($"is dependent on `{masterLabel.FieldTitle}` and can only be included in project views if *{masterLabel.FieldTitle}* is included");
                        }
                        if (notes.Count > 0)
                        {
                            //writer.WriteLine($"> **{p.Title}:**");
                            writer.WriteLine($"> **Notes:**");
                            foreach (var note in notes)
                            {
                                writer.WriteLine($"> - {note}");
                            }
                            writer.WriteLine();
                        }
                    }

                }

            }

        }

        private static void AddTypeToMap<T>(Dictionary<string, Dictionary<string, FieldProperty>> typeToPropertyMap)
        {
            var type = typeof(T);
            var typeName = type.FullName;
            var propertyToFieldNameMap = type.GetProperties()
                .Select(p => new FieldProperty(p))
                .ToDictionary(p => p.PropertyName, p => p);
            typeToPropertyMap.Add(typeName, propertyToFieldNameMap);
        }

        private static IEnumerable<XMLMember> GetMembers(XElement doc)
        {
            return doc.Element("members").Descendants("member").Select(m => new XMLMember(m));
        }

        private static string GetSolutionPath()
        {
            string path = Assembly.GetAssembly(typeof(MarkdownGenerator)).Location;
            var match = Regex.Match(path, @"(?<solutionPath>.*)\\FSAPortfolio\.Utility\\bin\\Debug\\netcoreapp3\.1\\.*");
            if (!match.Success) throw new Exception("Unable to find solution path.");
            var solutionPath = match.Groups["solutionPath"];
            return solutionPath.Value;
        }

        private static async Task<PortfolioConfigModel> GetConfig(string portfolio)
        {
            var url = ConfigurationManager.AppSettings["backendUrl"];
            var apiKey = ConfigurationManager.AppSettings["backendApiKey"];
            var user = ConfigurationManager.AppSettings["testAdminUser"];
            var password = ConfigurationManager.AppSettings["testAdminUserPassword"];

            BackendAPIClient backendClient = new BackendAPIClient(url, apiKey, user, password);
            return await backendClient.GetAsync<PortfolioConfigModel>($"api/PortfolioConfiguration?portfolio={portfolio}");
        }
    }

    public class FieldProperty
    {
        public PropertyInfo Info { get; }
        public string PropertyName { get; }
        public string FieldName { get; }
        public bool IsTracked { get; }
        public FieldProperty(PropertyInfo info)
        {
            Info = info;
            PropertyName = info.Name;
            FieldName = info.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName ?? info.Name;
            IsTracked = (info.GetCustomAttribute<UpdateTrackedAttribute>() != null);
        }
    }

    public class XMLMember
    {
        public string FullName { get; set; }
        public string MemberType { get; set; }
        public string TypeName { get; set; }
        public string MemberName { get; set; }
        public string Summary { get; set; }
        public XMLMember(XElement element)
        {
            // P:FSAPortfolio.Application.Models.ProjectModel.project_name
            var regex = @$"(?<memberType>[A-Z]):(?<typeName>.*)\.(?<memberName>[\w_]*)";
            FullName = element.Attribute("name").Value;
            var match = Regex.Match(FullName, regex);
            if (!match.Success) throw new Exception("Unrecognised member name.");
            MemberType = match.Groups["memberType"].Value;
            TypeName = match.Groups["typeName"].Value;
            MemberName = match.Groups["memberName"].Value;
            Summary = element.Element("summary")?.Value;
        }
        public bool IsProperty => MemberType == "P";
    }

    public class FieldDescription
    {
        public PortfolioLabelModel Label { get; private set; }

        public string FieldName { get; private set; }
        public string Title => Label.FieldTitle;
        public string Description { get; set; }
        public string Summary { get; set; }
        public bool HasSummary => !string.IsNullOrWhiteSpace(Summary);
        public bool IsTracked { get; set; }

        public FieldDescription(PortfolioLabelModel label)
        {
            SetLabel(label);
        }

        internal void SetLabel(PortfolioLabelModel label)
        {
            this.Label = label;
            if(string.IsNullOrWhiteSpace(FieldName))
            {
                FieldName = label.FieldName;
            }
            if(string.IsNullOrWhiteSpace(Description))
            {
                var titleMarkDown = $"*{Title}*";
                switch (label.FieldType)
                {
                    case "auto":
                        Description = $"The {titleMarkDown} for the project. This is read only and the value is automatically generated.";
                        break;
                    case "percentage":
                        Description = $"The {titleMarkDown} for the project, as a percentage value between 0 and 100.";
                        break;
                    case "projectdate":
                        Description = $"The {titleMarkDown} for the project, which can be entered to the day, month or just the year.";
                        break;
                    case "budget":
                        Description = $"The {titleMarkDown} for the project in GBP.";
                        break;
                    case "adusersearch":
                        Description = $"The {titleMarkDown} selected from a searchable list of values.";
                        break;
                    case "adusermultisearch":
                        Description = $"Multiple {titleMarkDown} entries, selected from a searchable list of values.";
                        break;
                    case "ajaxmultisearch":
                        Description = $"Multiple {titleMarkDown} entries selected from a searchable list of values.";
                        break;
                    case "namedlink":
                        Description = $"A descriptive name, and a link URL describing the {titleMarkDown} for the project.";
                        break;
                    case "linkeditemlist":
                        Description = $"The {titleMarkDown} for the project as a list of items, each with a descriptive name and link URL.";
                        break;
                    case "predefinedlist":
                        Description = $"The selected {titleMarkDown}, from a predefined list of options.";
                        break;
                    case "optionlist":
                        Description = $"The selected {titleMarkDown}, from a list of options configured independently for each portfolio.";
                        break;
                    case "milestones":
                        Description = $"The {titleMarkDown} for the project, as an ordered list of milestones with dates.";
                        break;
                    case "phasechoice":
                        Description = $"The {titleMarkDown} for the project, selected from configured values.";
                        break;
                    case "projectupdatetext":
                        Description = $"The {titleMarkDown} for the project. These are a series of date ordered updates. The update text can only be changed on the same day it is entered.";
                        break;
                    case "smallfreetextarea":
                    case "mediumfreetextarea":
                    case "largefreetextarea":
                    case "freetext":
                        Description = $"Text describing the {titleMarkDown} for the project.";
                        break;
                }
            }
        }

        internal void SetXElement(XMLMember member)
        {
            // Get the summary and strip white space
            var builder = new StringBuilder();
            var summary = member.Summary;
            var lines = summary.Split("\n");

            // Need to strip the whitespace at front but leave indents
            int spaceCount = -1;
            foreach (var line in lines.Where(l => !string.IsNullOrWhiteSpace(l)))
            {
                int currentSpaceCount = 0;
                while (currentSpaceCount < line.Length && line[currentSpaceCount] == ' ') currentSpaceCount++;
                if (spaceCount == -1 || currentSpaceCount < spaceCount) spaceCount = currentSpaceCount;
            }

            var outputLines = new string[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                if (string.IsNullOrWhiteSpace(line))
                {
                    outputLines[i] = string.Empty;
                }
                else
                {
                    outputLines[i] = line.Substring(spaceCount).TrimEnd();
                }
            }

            foreach (var line in outputLines)
            {
                builder.AppendLine(line);
            }
            Summary = builder.ToString().Trim();

        }

        internal void SetProperty(FieldProperty fieldProperty)
        {
            if(fieldProperty.IsTracked) IsTracked = true;
        }
    }
}

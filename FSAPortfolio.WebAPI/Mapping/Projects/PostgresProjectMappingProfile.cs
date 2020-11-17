using AutoMapper;
using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Entities.Users;
using FSAPortfolio.PostgreSQL.Projects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Data.Entity;
using FSAPortfolio.WebAPI.App.Sync;
using FSAPortfolio.WebAPI.Mapping.Projects.Resolvers;

namespace FSAPortfolio.WebAPI.Mapping.Projects
{
    public class PostgresProjectMappingProfile : Profile
    {
        public PostgresProjectMappingProfile()
        {
            // Outbound
            Project__latest_projects();

            // Inbound
            project__Project();
            project__ProjectUpdateItem();

        }
        private void project__ProjectUpdateItem()
        {
            CreateMap<project, ProjectUpdateItem>()
                .ForMember(p => p.Id, o => o.Ignore())
                .ForMember(p => p.Project_Id, o => o.Ignore())
                .ForMember(p => p.Project, o => o.Ignore())
                .ForMember(p => p.Person, o => o.Ignore())
                .ForMember(p => p.SyncId, o => o.MapFrom(s => s.id))
                .ForMember(p => p.Timestamp, o => o.MapFrom(s => s.timestamp))
                .ForMember(p => p.Text, o => o.MapFrom(s => s.update))
                .ForMember(p => p.PercentageComplete, o => o.MapFrom(s => s.p_comp))
                .ForMember(p => p.RAGStatus, o => o.MapFrom<string>(new ConfigRAGStatusResolver(true), s => s.rag))
                .ForMember(p => p.Phase, o => o.MapFrom<string>(new ConfigPhaseStatusResolver(true), s => SyncMaps.phaseKeyMap[s.phase ?? "backlog"]))
                .ForMember(p => p.OnHoldStatus, o => o.MapFrom<string>(new ConfigOnHoldStatusResolver(true), s => SyncMaps.onholdKeyMap[s.onhold ?? "n"]))
                .ForMember(p => p.Budget, o => o.MapFrom<DecimalResolver, string>(s => s.budget))
                .ForMember(p => p.Spent, o => o.MapFrom<DecimalResolver, string>(s => s.spent))
                .ForMember(p => p.ExpectedCurrentPhaseEnd, o => o.MapFrom<PostgresProjectDateResolver, string>(s => s.expendp))
                ;
        }

        private void project__Project()
        {
            CreateMap<project, Project>()
                .ForMember(p => p.Name, o => o.MapFrom(s => s.project_name))
                .ForMember(p => p.StartDate, o => o.MapFrom<PostgresProjectDateResolver, string>(s => s.start_date))
                .ForMember(p => p.ActualStartDate, o => o.MapFrom<PostgresProjectDateResolver, string>(s => s.actstart))
                .ForMember(p => p.ExpectedEndDate, o => o.MapFrom<PostgresProjectDateResolver, string>(s => s.expend))
                .ForMember(p => p.HardEndDate, o => o.MapFrom<PostgresProjectDateResolver, string>(s => s.hardend))
                .ForMember(p => p.ActualEndDate, o => o.MapFrom<PostgresProjectDateResolver, string>(s => "00/00/00")) // Isn't one!?
                .ForMember(p => p.Description, o => o.MapFrom(s => s.short_desc))
                .ForMember(p => p.Priority, o => o.MapFrom<NullableIntResolver, string>(s => s.priority_main))
                .ForMember(p => p.Directorate, o => o.MapFrom<DirectorateResolver, string>(s => s.direct))
                .ForMember(p => p.Funded, o => o.MapFrom<PostgresIntResolver, string>(s => s.funded))
                .ForMember(p => p.Confidence, o => o.MapFrom<PostgresIntResolver, string>(s => s.confidence))
                .ForMember(p => p.Benefits, o => o.MapFrom<PostgresIntResolver, string>(s => s.benefits))
                .ForMember(p => p.Criticality, o => o.MapFrom<PostgresIntResolver, string>(s => s.criticality))
                .ForMember(p => p.People, o => o.MapFrom<PostgresTeamCollectionResolver, string>(s => s.team))
                .ForMember(p => p.Lead, o => o.MapFrom<ProjectLeadResolver, string>(s => s.oddlead_email))
                .ForMember(p => p.ServiceLead, o => o.MapFrom<ProjectLeadResolver, string>(s => s.servicelead_email))
                .ForMember(p => p.RelatedProjects, o => o.MapFrom<PostgresProjectCollectionResolver, string>(s => s.rels))
                .ForMember(p => p.DependantProjects, o => o.MapFrom<PostgresProjectCollectionResolver, string>(s => s.dependencies))
                .ForMember(p => p.Category, o => o.MapFrom<ConfigCategoryResolver, string>(s => SyncMaps.categoryKeyMap[s.category ?? "cap"]))
                .ForMember(p => p.Size, o => o.MapFrom<ConfigProjectSizeResolver, string>(s => SyncMaps.sizeKeyMap[s.project_size ?? string.Empty]))
                .ForMember(p => p.BudgetType, o => o.MapFrom<ConfigBudgetTypeResolver, string>(s => SyncMaps.budgetTypeKeyMap[s.budgettype ?? "none"]))
                .ForMember(p => p.ChannelLink, o => o.MapFrom(s => new ProjectLink() { Link = s.link }))
                .ForMember(p => p.Documents, o => o.MapFrom<PostgresDocumentResolver, string>(s => s.documents))

                // TODO: These need migration mappings
                .ForMember(p => p.Subcategories, o => o.Ignore())
                // Ignore these
                .ForMember(p => p.KeyContact1, o => o.Ignore())
                .ForMember(p => p.KeyContact2, o => o.Ignore())
                .ForMember(p => p.KeyContact3, o => o.Ignore())
                .ForMember(p => p.ProjectData, o => o.Ignore())
                .ForMember(p => p.Reservation, o => o.Ignore())
                .ForMember(p => p.Portfolios, o => o.Ignore())
                .ForMember(p => p.Updates, o => o.Ignore())
                .ForMember(p => p.FirstUpdate, o => o.Ignore())
                .ForMember(p => p.LatestUpdate, o => o.Ignore())
                .ForMember(p => p.AuditLogs, o => o.Ignore())
                .ForMember(p => p.Theme, o => o.Ignore())
                .ForMember(p => p.ProjectType, o => o.Ignore())
                .ForMember(p => p.StrategicObjectives, o => o.Ignore())
                .ForMember(p => p.Programme, o => o.Ignore())
                .ForMember(p => p.Supplier, o => o.Ignore())
                // Ignore the keys
                .ForMember(p => p.ProjectReservation_Id, o => o.Ignore())
                .ForMember(p => p.ProjectCategory_Id, o => o.Ignore())
                .ForMember(p => p.ProjectSize_Id, o => o.Ignore())
                .ForMember(p => p.BudgetType_Id, o => o.Ignore())
                .ForMember(p => p.Lead_Id, o => o.Ignore())
                .ForMember(p => p.ServiceLead_Id, o => o.Ignore())
                .ForMember(p => p.FirstUpdate_Id, o => o.Ignore())
                .ForMember(p => p.LatestUpdate_Id, o => o.Ignore())
            ;
        }

        private void Project__latest_projects()
        {
            CreateMap<Project, latest_projects>()
                .ForMember(p => p.id, o => o.MapFrom(s => s.LatestUpdate.SyncId))
                .ForMember(p => p.project_id, o => o.MapFrom(s => s.Reservation.ProjectId))
                .ForMember(p => p.project_name, o => o.MapFrom(s => s.Name))
                .ForMember(p => p.start_date, o => o.MapFrom(s => s.StartDate))
                .ForMember(p => p.short_desc, o => o.MapFrom(s => s.Description))
                .ForMember(p => p.phase, o => o.MapFrom(s => s.LatestUpdate.Phase.ViewKey))
                .ForMember(p => p.category, o => o.MapFrom(s => s.Category.ViewKey))
                .ForMember(p => p.rag, o => o.MapFrom(s => s.LatestUpdate.RAGStatus.ViewKey))
                .ForMember(p => p.update, o => o.MapFrom(s => s.LatestUpdate.Text))
                .ForMember(p => p.oddlead_email, o => o.MapFrom(s => s.Lead.Email))
                .ForMember(p => p.servicelead_email, o => o.MapFrom(s => s.ServiceLead.Email))
                .ForMember(p => p.priority_main, o => o.MapFrom(s => s.Priority.HasValue ? s.Priority.Value.ToString("D2") : string.Empty))
                .ForMember(p => p.funded, o => o.MapFrom(s => s.Funded.ToString()))
                .ForMember(p => p.confidence, o => o.MapFrom(s => s.Confidence.ToString()))
                .ForMember(p => p.priorities, o => o.MapFrom(s => s.Priorities.ToString()))
                .ForMember(p => p.benefits, o => o.MapFrom(s => s.Benefits.ToString()))
                .ForMember(p => p.criticality, o => o.MapFrom(s => s.Criticality.ToString()))
                .ForMember(p => p.budget, o => o.MapFrom(s => s.LatestUpdate.Budget))
                .ForMember(p => p.spent, o => o.MapFrom(s => s.LatestUpdate.Spent))
                .ForMember(p => p.timestamp, o => o.MapFrom(s => s.LatestUpdate.Timestamp))

                .ForMember(p => p.rels, o => o.MapFrom(s => string.Join(", ", s.RelatedProjects.Select(rp => rp.Reservation.ProjectId))))
                .ForMember(p => p.dependencies, o => o.MapFrom(s => string.Join(", ", s.DependantProjects.Select(rp => rp.Reservation.ProjectId))))
                .ForMember(p => p.team, o => o.MapFrom(s => s.People))
                .ForMember(p => p.onhold, o => o.MapFrom(s => s.LatestUpdate.OnHoldStatus.Name))
                .ForMember(p => p.expend, o => o.MapFrom(s => s.ExpectedEndDate))
                .ForMember(p => p.hardend, o => o.MapFrom(s => s.HardEndDate))
                .ForMember(p => p.actstart, o => o.MapFrom(s => s.ActualStartDate))
                .ForMember(p => p.project_size, o => o.MapFrom(s => s.Size.ViewKey))
                .ForMember(p => p.budgettype, o => o.MapFrom(s => s.BudgetType.ViewKey))
                .ForMember(p => p.direct, o => o.MapFrom(s => s.Directorate.ViewKey))
                .ForMember(p => p.expendp, o => o.MapFrom(s => s.LatestUpdate.ExpectedCurrentPhaseEnd))
                .ForMember(p => p.p_comp, o => o.MapFrom(s => s.LatestUpdate.PercentageComplete))
                .ForMember(p => p.max_time, o => o.MapFrom(s => s.LatestUpdate.Timestamp))
                .ForMember(p => p.min_time, o => o.MapFrom(s => s.FirstUpdate.Timestamp))
                .ForMember(p => p.g6team, o => o.MapFrom(s => s.Lead.Team))
                .ForMember(p => p.new_flag, o => o.MapFrom(s => s.IsNew ? "Y" : "N"))

                // TODO: don't think were using latest_projects anymore - verify then delete
                .ForMember(p => p.subcat, o => o.Ignore()) 
                .ForMember(p => p.oddlead, o => o.Ignore())
                .ForMember(p => p.servicelead, o => o.Ignore())
                .ForMember(p => p.documents, o => o.Ignore())
                .ForMember(p => p.pgroup, o => o.Ignore())
                .ForMember(p => p.link, o => o.Ignore())
                .ForMember(p => p.toupdate, o => o.Ignore())
                .ForMember(p => p.oddlead_role, o => o.Ignore())
                ;
        }

    }

    public class PostgresIntResolver : IMemberValueResolver<object, object, string, int>
    {
        public int Resolve(object source, object destination, string sourceMember, int destMember, ResolutionContext context)
        {
            int result;
            return int.TryParse(sourceMember, out result) ? result : 0;
        }
    }


    public class PostgresProjectDateResolver : IMemberValueResolver<object, object, string, ProjectDate>
    {
        public ProjectDate Resolve(object source, object destination, string date, ProjectDate destMember, ResolutionContext context)
        {
            ProjectDate result = new ProjectDate();
            if(!string.IsNullOrWhiteSpace(date))
            {
                var parts = date.Split('/');
                if(parts.Length == 3)
                {
                    int day, month, year;
                    if (int.TryParse(parts[2], out year) && year > 0)
                    {
                        result.Flags |= ProjectDateFlags.Year;
                        if (int.TryParse(parts[0], out day) && day > 0) result.Flags |= ProjectDateFlags.Day;
                        if (int.TryParse(parts[1], out month) && month > 0) result.Flags |= ProjectDateFlags.Month;
                        if (month == 0) month = 12;
                        if (day == 0) day = DateTime.DaysInMonth(year, month);
                        result.Date = new DateTime(year, month, day);
                    }
                }
            }
            return result;
        }
    }

    public class PostgresDateResolver : IMemberValueResolver<object, object, string, DateTime?>
    {
        public DateTime? Resolve(object source, object destination, string date, DateTime? destMember, ResolutionContext context)
        {
            DateTime result;
            return DateTime.TryParseExact(date, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out result) ? (DateTime?)result : null;
        }
    }

    public class PostgresProjectCollectionResolver : IMemberValueResolver<object, Project, string, ICollection<Project>>
    {
        public ICollection<Project> Resolve(object source, Project destination, string sourceMember, ICollection<Project> destMember, ResolutionContext context)
        {
            var portfolioContext = (PortfolioContext)context.Items[nameof(PortfolioContext)];
            var result = new List<Project>();
            if (!string.IsNullOrEmpty(sourceMember))
            {
                // Add missing related projects
                var projectIds = sourceMember.Split(',');
                foreach (var relatedProjectId in projectIds)
                {
                    var trimmedId = relatedProjectId.Trim();
                    if (!result.Any(p => p.Reservation.ProjectId == trimmedId))
                    {
                        var project = portfolioContext.Projects.Include(p => p.Reservation).SingleOrDefault(p => p.Reservation.ProjectId == trimmedId);
                        if (project != null)
                        {
                            result.Add(project);
                        }
                    }
                }
            }
            return result;
        }
    }

    public class PostgresTeamCollectionResolver : IMemberValueResolver<object, Project, string, ICollection<Person>>
    {
        public ICollection<Person> Resolve(object source, Project destination, string sourceMember, ICollection<Person> destMember, ResolutionContext context)
        {
            var portfolioContext = (PortfolioContext)context.Items[nameof(PortfolioContext)];
            var result = new List<Person>();
            if (!string.IsNullOrEmpty(sourceMember))
            {
                Func<string, string, Person, bool> nameCheck = (f, s, p) => string.Equals(f, p.Firstname, StringComparison.OrdinalIgnoreCase) && string.Equals(s, p.Surname, StringComparison.OrdinalIgnoreCase);
                var peoplesNames = sourceMember.Split(',');
                foreach (var personsName in peoplesNames)
                {
                    var names = personsName.Split(' ').Where(n => !string.IsNullOrEmpty(n)).ToArray();
                    if(names.Length == 2)
                    {
                        var firstName = names[0];
                        var surname = names[1];
                        if (!result.Any(p => nameCheck(firstName, surname, p)))
                        {
                            var person =
                                portfolioContext.People.Local.SingleOrDefault(p => nameCheck(firstName, surname, p)) ??
                                portfolioContext.People.AsEnumerable().SingleOrDefault(p => nameCheck(firstName, surname, p));
                            if (person != null)
                            {
                                result.Add(person);
                            }
                        }
                    }
                }
            }
            return result;
        }
    }

    public class PostgresDocumentResolver : IMemberValueResolver<project, Project, string, ICollection<Document>>
    {
        public ICollection<Document> Resolve(project source, Project destination, string sourceMember, ICollection<Document> destMember, ResolutionContext context)
        {
            List<Document> documents = new List<Document>(destMember);
            if(!string.IsNullOrWhiteSpace(sourceMember))
            {
                documents = new List<Document>();
                var parts = sourceMember.Split(',');
                for(int i = 0; i < parts.Length; i+=2)
                {
                    var name = parts[i];
                    var link = (i + 1 < parts.Length) ? parts[i + 1] : null;

                    if(!documents.Any(d => d.Name == name && d.Link == link))
                    {
                        var document = new Document() { Name = name, Link = link };
                        documents.Add(document);
                    }
                }
            }
            return documents;
        }
    }
}
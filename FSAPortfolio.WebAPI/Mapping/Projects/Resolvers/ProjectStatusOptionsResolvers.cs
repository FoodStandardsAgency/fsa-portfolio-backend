using AutoMapper;
using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.App.Config;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Mapping.Projects.Resolvers
{
    public abstract class BaseProjectOptionResolver<T> : IMemberValueResolver<object, Project, string, T>
        where T : class, IProjectOption, new()
    {
        private string option;

        protected BaseProjectOptionResolver(string option)
        {
            this.option = option;
        }
        protected abstract T GetOption(Project destination, string viewKey, ResolutionContext context);
        public T Resolve(object source, Project destination, string sourceMember, T destMember, ResolutionContext context)
        {
            T result = null;
            if (!string.IsNullOrWhiteSpace(sourceMember))
            {
                result =  sourceMember == destMember?.ViewKey ? destMember : GetOption(destination, sourceMember, context);
                if (result == null) throw new PortfolioConfigurationException($"Unrecognised {option} key [{sourceMember}]");
            }
            return result;
        }
    }

    public class ConfigCategoryResolver : BaseProjectOptionResolver<ProjectCategory>
    {
        public ConfigCategoryResolver() : base(nameof(ProjectUpdateModel.category)) { }

        protected override ProjectCategory GetOption(Project destination, string viewKey, ResolutionContext context)
            => destination.Reservation.Portfolio.Configuration.Categories.SingleOrDefault(c => c.ViewKey == viewKey);
    }
    public class DirectorateResolver : BaseProjectOptionResolver<Directorate>
    {
        public DirectorateResolver() : base(nameof(ProjectUpdateModel.direct)) { }

        protected override Directorate GetOption(Project destination, string viewKey, ResolutionContext context)
        {
            // TODO: this is a database query each time - could cache directorates.
            var portfolioContext = (PortfolioContext)context.Items[nameof(PortfolioContext)];
            var directorate = portfolioContext.Directorates.SingleOrDefault(c => c.ViewKey == viewKey);
            return directorate;
        }
    }

    public class ConfigSubcategoryResolver : IMemberValueResolver<object, Project, string[], ICollection<ProjectCategory>>
    {
        public ICollection<ProjectCategory> Resolve(object source, Project destination, string[] sourceMember, ICollection<ProjectCategory> destMember, ResolutionContext context)
        {
            ICollection<ProjectCategory> result = null;
            if (sourceMember != null && sourceMember.Length > 0)
            {
                result = destination.Reservation.Portfolio.Configuration.Categories.Where(c => sourceMember.Contains(c.ViewKey)).ToList();
                if(result.Count != sourceMember.Length)
                {
                    var errors = Array.FindAll(sourceMember, s => !result.Any(r => r.ViewKey == s));
                    if(errors.Length > 0)
                    {
                        throw new PortfolioConfigurationException($"Unrecognised {nameof(ProjectUpdateModel.subcat)} keys [{string.Join(", ", errors)}]");
                    }
                }
            }
            return result;
        }
    }

    public class ConfigProjectSizeResolver : BaseProjectOptionResolver<ProjectSize>
    {
        public ConfigProjectSizeResolver() : base(nameof(ProjectUpdateModel.project_size)) { }
        protected override ProjectSize GetOption(Project destination, string viewKey, ResolutionContext context)
            => destination.Reservation.Portfolio.Configuration.ProjectSizes.SingleOrDefault(c => c.ViewKey == viewKey);
    }

    public class ConfigBudgetTypeResolver : BaseProjectOptionResolver<BudgetType>
    {
        public ConfigBudgetTypeResolver() : base(nameof(ProjectUpdateModel.budgettype)) { }
        protected override BudgetType GetOption(Project destination, string viewKey, ResolutionContext context)
        {
            ICollection<BudgetType> budgetTypes = destination.Reservation.Portfolio.Configuration.BudgetTypes;
            return
                budgetTypes.SingleOrDefault(c => c.ViewKey == viewKey) ??
                budgetTypes.SingleOrDefault(c => c.ViewKey == BudgetTypeConstants.NotSetViewKey);
        }
    }

}
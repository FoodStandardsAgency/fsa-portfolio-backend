using AutoMapper;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.App.Config;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Mapping.Projects.Resolvers
{
    public abstract class BaseProjectUpdateOptionResolver<T> : IMemberValueResolver<object, ProjectUpdateItem, string, T>
        where T : class, new()
    {
        private string option;

        protected BaseProjectUpdateOptionResolver(string option)
        {
            this.option = option;
        }
        protected abstract T GetOption(ProjectUpdateItem destination, string viewKey);
        public T Resolve(object source, ProjectUpdateItem destination, string sourceMember, T destMember, ResolutionContext context)
        {
            T result = null;
            if (!string.IsNullOrWhiteSpace(sourceMember))
            {
                result = GetOption(destination, sourceMember);
                if (result == null) throw new PortfolioConfigurationException($"Unrecognised {option} key [{sourceMember}]");
            }
            return result;
        }
    }

    public class ConfigRAGStatusResolver : BaseProjectUpdateOptionResolver<ProjectRAGStatus>
    {
        public ConfigRAGStatusResolver() : base(nameof(ProjectUpdateModel.rag)) { }
        protected override ProjectRAGStatus GetOption(ProjectUpdateItem destination, string viewKey)
            => destination.Project.Reservation.Portfolio.Configuration.RAGStatuses.SingleOrDefault(c => c.ViewKey == viewKey);
    }

    public class ConfigPhaseStatusResolver : BaseProjectUpdateOptionResolver<ProjectPhase>
    {
        public ConfigPhaseStatusResolver() : base(nameof(ProjectUpdateModel.phase)) { }
        protected override ProjectPhase GetOption(ProjectUpdateItem destination, string viewKey)
            => destination.Project.Reservation.Portfolio.Configuration.Phases.SingleOrDefault(c => c.ViewKey == viewKey);
    }

    public class ConfigOnHoldStatusResolver : BaseProjectUpdateOptionResolver<ProjectOnHoldStatus>
    {
        public ConfigOnHoldStatusResolver() : base(nameof(ProjectUpdateModel.onhold)) { }
        protected override ProjectOnHoldStatus GetOption(ProjectUpdateItem destination, string viewKey)
            => destination.Project.Reservation.Portfolio.Configuration.OnHoldStatuses.SingleOrDefault(c => c.ViewKey == viewKey);
    }

}
using AutoMapper;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.App.Config;
using FSAPortfolio.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Mapping.Projects.Resolvers
{
    public abstract class BaseProjectUpdateOptionResolver<T> : IMemberValueResolver<object, ProjectUpdateItem, string, T>
        where T : class, IProjectOption, new()
    {
        private string option;
        private bool forceResync;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <param name="forceResync">Force the selection of the option, don't just rely on matching view keys.
        /// This is for use during sync in case the project has moved between portfolios.
        /// </param>
        protected BaseProjectUpdateOptionResolver(string option, bool forceResync = false)
        {
            this.option = option;
            this.forceResync = forceResync;
        }
        protected abstract T GetOption(ProjectUpdateItem destination, string viewKey);
        public T Resolve(object source, ProjectUpdateItem destination, string sourceMember, T destMember, ResolutionContext context)
        {
            T result = null;
            if (!string.IsNullOrWhiteSpace(sourceMember))
            {
                if (destMember?.ViewKey == sourceMember && !forceResync)
                {
                    result = destMember;
                }
                else
                {
                    result = GetOption(destination, sourceMember);
                }
                if (result == null) throw new PortfolioConfigurationException($"Unrecognised {option} key [{sourceMember}]");
            }
            return result;
        }
    }

    public class ConfigRAGStatusResolver : BaseProjectUpdateOptionResolver<ProjectRAGStatus>
    {
        public ConfigRAGStatusResolver() : base(nameof(ProjectUpdateModel.rag)) { }
        public ConfigRAGStatusResolver(bool forceResync = false) : base(nameof(ProjectUpdateModel.rag), forceResync) { }
        protected override ProjectRAGStatus GetOption(ProjectUpdateItem destination, string viewKey)
            => destination.Project.Reservation.Portfolio.Configuration.RAGStatuses.SingleOrDefault(c => c.ViewKey == viewKey);
    }

    public class ConfigPhaseStatusResolver : BaseProjectUpdateOptionResolver<ProjectPhase>
    {
        public ConfigPhaseStatusResolver() : base(nameof(ProjectUpdateModel.phase)) { }
        public ConfigPhaseStatusResolver(bool forceResync = false) : base(nameof(ProjectUpdateModel.phase), forceResync) { }
        protected override ProjectPhase GetOption(ProjectUpdateItem destination, string viewKey)
            => destination.Project.Reservation.Portfolio.Configuration.Phases.SingleOrDefault(c => c.ViewKey == viewKey);
    }

    public class ConfigOnHoldStatusResolver : BaseProjectUpdateOptionResolver<ProjectOnHoldStatus>
    {
        public ConfigOnHoldStatusResolver() : base(nameof(ProjectUpdateModel.onhold)) { }
        public ConfigOnHoldStatusResolver(bool forceResync = false) : base(nameof(ProjectUpdateModel.onhold), forceResync) { }
        protected override ProjectOnHoldStatus GetOption(ProjectUpdateItem destination, string viewKey)
            => destination.Project.Reservation.Portfolio.Configuration.OnHoldStatuses.SingleOrDefault(c => c.ViewKey == viewKey);
    }

}
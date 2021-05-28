using AutoMapper;
using FSAPortfolio.WebAPI.DTO;
using FSAPortfolio.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.UnitTests.TestMappings
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            // For cloning project DTO and contained objects
            CreateMap<GetProjectDTO<ProjectEditViewModel>, GetProjectDTO<ProjectEditViewModel>>();
            CreateMap<ProjectEditViewModel, ProjectEditViewModel>();
            CreateMap<ProjectLabelConfigModel, ProjectLabelConfigModel>();
            CreateMap<ProjectEditOptionsModel, ProjectEditOptionsModel>();

            // For converting the edit model to an update
            CreateMap<ProjectEditViewModel, ProjectUpdateModel>();
            CreateMap<SelectItemModel, string>().ConstructUsing(s => s.Value);

            // For cloning the config model
            CreateMap<PortfolioConfigModel, PortfolioConfigModel>();
            
        }
    }
}

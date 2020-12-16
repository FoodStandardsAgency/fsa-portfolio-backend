using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.UnitTests.TestMappings
{
    public class TestMapper
    {
        internal static MapperConfiguration projectConfig;
        internal static IMapper ProjectMapper { get; private set; }
        static TestMapper()
        {
            projectConfig = new MapperConfiguration(cfg =>
            {
                cfg.AllowNullCollections = true;
                cfg.AddProfile<ProjectProfile>();
            });
            ProjectMapper = projectConfig.CreateMapper();
        }
    }
}

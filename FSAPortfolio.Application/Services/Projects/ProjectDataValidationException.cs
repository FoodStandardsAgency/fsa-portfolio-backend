using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.Application.Services.Projects
{
    public class ProjectDataValidationException : Exception
    {
        public ProjectDataValidationException(string message) : base(message)
        {
        }
        public ProjectDataValidationException(string message, Exception e) : base(message, e)
        {
        }
    }
}
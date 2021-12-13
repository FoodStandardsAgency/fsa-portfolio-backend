using AutoMapper;
using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Entities.Users;
using FSAPortfolio.Application.Services.Users;
using FSAPortfolio.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.Application.Mapping.Projects.Resolvers
{
    public class PostgresPersonFromEmailResolver : IMemberValueResolver<object, object, string, Person>
    {
        public Person Resolve(object source, object destination, string sourceMember, Person destMember, ResolutionContext context)
        {
            if (sourceMember != null && sourceMember.Equals("amy-kate.wych@food.gov.uk", StringComparison.OrdinalIgnoreCase))
                sourceMember = "amy-kate.lynch@food.gov.uk";

            var portfolioContext = (PortfolioContext)context.Items[nameof(PortfolioContext)];
            return portfolioContext.People.SingleOrDefault(p => p.Email == sourceMember);
        }
    }

}
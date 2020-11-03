﻿using AutoMapper;
using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Entities.Users;
using FSAPortfolio.WebAPI.App.Users;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Mapping.Projects.Resolvers
{
    public class ProjectLeadResolver : IMemberValueResolver<object, object, string, Person>
    {
        public Person Resolve(object source, object destination, string sourceMember, Person destMember, ResolutionContext context)
        {
            var portfolioContext = (PortfolioContext)context.Items[nameof(PortfolioContext)];
            return portfolioContext.People.SingleOrDefault(p => p.Email == sourceMember);
        }
    }

}
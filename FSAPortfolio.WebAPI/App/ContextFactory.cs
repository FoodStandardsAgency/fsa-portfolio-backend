using FSAPortfolio.Entites;
using FSAPortfolio.PostgreSQL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.App
{
    public static class ContextFactory
    {
        public static PortfolioContext NewPortfolioContext()
        {
            PortfolioContext newContext;
            var database = ConfigurationManager.AppSettings["database"]?.ToLower() ?? "mssql";
            switch (database)
            {
                case "mssql":
                    newContext = NewMsSqlContext();
                    break;
                case "postgres":
                    newContext = NewPostgresContext();
                    break;
                default:
                    throw new ConfigurationErrorsException($"Unrecognised value for database appSetting: {database}");
            }
            return newContext;
        }
        public static PortfolioContext NewPostgresContext() => new MigratePortfolioContext();
        public static PortfolioContext NewMsSqlContext() => new PortfolioContext();

    }
}
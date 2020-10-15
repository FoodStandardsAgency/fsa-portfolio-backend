CREATE TABLE [dbo].[PortfolioConfigAuditLogs] (
    [Id]                        INT            IDENTITY (1, 1) NOT NULL,
    [PortfolioConfiguration_Id] INT            NOT NULL,
    [AuditType]                 NVARCHAR (50)  NULL,
    [Timestamp]                 DATETIME       NOT NULL,
    [Text]                      NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.PortfolioConfigAuditLogs] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.PortfolioConfigAuditLogs_dbo.PortfolioConfigurations_PortfolioConfiguration_Id] FOREIGN KEY ([PortfolioConfiguration_Id]) REFERENCES [dbo].[PortfolioConfigurations] ([Portfolio_Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_PortfolioConfiguration_Id]
    ON [dbo].[PortfolioConfigAuditLogs]([PortfolioConfiguration_Id] ASC);


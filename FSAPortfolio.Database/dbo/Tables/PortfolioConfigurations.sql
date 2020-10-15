CREATE TABLE [dbo].[PortfolioConfigurations] (
    [Portfolio_Id]      INT NOT NULL,
    [CompletedPhase_Id] INT NULL,
    CONSTRAINT [PK_dbo.PortfolioConfigurations] PRIMARY KEY CLUSTERED ([Portfolio_Id] ASC),
    CONSTRAINT [FK_dbo.PortfolioConfigurations_dbo.Portfolios_Portfolio_Id] FOREIGN KEY ([Portfolio_Id]) REFERENCES [dbo].[Portfolios] ([Id]),
    CONSTRAINT [FK_dbo.PortfolioConfigurations_dbo.ProjectPhases_CompletedPhase_Id] FOREIGN KEY ([CompletedPhase_Id]) REFERENCES [dbo].[ProjectPhases] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_CompletedPhase_Id]
    ON [dbo].[PortfolioConfigurations]([CompletedPhase_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Portfolio_Id]
    ON [dbo].[PortfolioConfigurations]([Portfolio_Id] ASC);


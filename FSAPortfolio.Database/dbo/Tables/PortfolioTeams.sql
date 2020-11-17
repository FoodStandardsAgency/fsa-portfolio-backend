CREATE TABLE [dbo].[PortfolioTeams] (
    [Portfolio_Id] INT NOT NULL,
    [Team_Id]      INT NOT NULL,
    CONSTRAINT [PK_dbo.PortfolioTeams] PRIMARY KEY CLUSTERED ([Portfolio_Id] ASC, [Team_Id] ASC),
    CONSTRAINT [FK_dbo.PortfolioTeams_dbo.Portfolios_Portfolio_Id] FOREIGN KEY ([Portfolio_Id]) REFERENCES [dbo].[Portfolios] ([Id]),
    CONSTRAINT [FK_dbo.PortfolioTeams_dbo.Teams_Team_Id] FOREIGN KEY ([Team_Id]) REFERENCES [dbo].[Teams] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Team_Id]
    ON [dbo].[PortfolioTeams]([Team_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Portfolio_Id]
    ON [dbo].[PortfolioTeams]([Portfolio_Id] ASC);


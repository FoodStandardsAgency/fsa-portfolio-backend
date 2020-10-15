CREATE TABLE [dbo].[PortfolioProjects] (
    [Portfolio_Id]                  INT NOT NULL,
    [Project_ProjectReservation_Id] INT NOT NULL,
    CONSTRAINT [PK_dbo.PortfolioProjects] PRIMARY KEY CLUSTERED ([Portfolio_Id] ASC, [Project_ProjectReservation_Id] ASC),
    CONSTRAINT [FK_dbo.PortfolioProjects_dbo.Portfolios_Portfolio_Id] FOREIGN KEY ([Portfolio_Id]) REFERENCES [dbo].[Portfolios] ([Id]),
    CONSTRAINT [FK_dbo.PortfolioProjects_dbo.Projects_Project_ProjectReservation_Id] FOREIGN KEY ([Project_ProjectReservation_Id]) REFERENCES [dbo].[Projects] ([ProjectReservation_Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Project_ProjectReservation_Id]
    ON [dbo].[PortfolioProjects]([Project_ProjectReservation_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Portfolio_Id]
    ON [dbo].[PortfolioProjects]([Portfolio_Id] ASC);


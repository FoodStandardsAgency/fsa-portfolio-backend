CREATE TABLE [dbo].[ProjectPhases] (
    [Id]               INT           IDENTITY (1, 1) NOT NULL,
    [ViewKey]          NVARCHAR (20) NULL,
    [Name]             NVARCHAR (50) NULL,
    [Order]            INT           NOT NULL,
    [Configuration_Id] INT           NOT NULL,
    CONSTRAINT [PK_dbo.ProjectPhases] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.ProjectPhases_dbo.PortfolioConfigurations_Configuration_Id] FOREIGN KEY ([Configuration_Id]) REFERENCES [dbo].[PortfolioConfigurations] ([Portfolio_Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Configuration_Id_Name]
    ON [dbo].[ProjectPhases]([Configuration_Id] ASC, [Name] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Configuration_Id_ViewKey]
    ON [dbo].[ProjectPhases]([Configuration_Id] ASC, [ViewKey] ASC);


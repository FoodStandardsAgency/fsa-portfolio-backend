CREATE TABLE [dbo].[ProjectSizes] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [ViewKey]          NVARCHAR (20)  NULL,
    [Name]             NVARCHAR (250) NULL,
    [Order]            INT            NOT NULL,
    [Configuration_Id] INT            NOT NULL,
    CONSTRAINT [PK_dbo.ProjectSizes] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.ProjectSizes_dbo.PortfolioConfigurations_Configuration_Id] FOREIGN KEY ([Configuration_Id]) REFERENCES [dbo].[PortfolioConfigurations] ([Portfolio_Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Configuration_Id_Name]
    ON [dbo].[ProjectSizes]([Configuration_Id] ASC, [Name] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Configuration_Id_ViewKey]
    ON [dbo].[ProjectSizes]([Configuration_Id] ASC, [ViewKey] ASC);


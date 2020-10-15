CREATE TABLE [dbo].[PortfolioLabelGroups] (
    [Id]               INT           IDENTITY (1, 1) NOT NULL,
    [Configuration_Id] INT           NOT NULL,
    [Name]             NVARCHAR (50) NULL,
    [Order]            INT           NOT NULL,
    CONSTRAINT [PK_dbo.PortfolioLabelGroups] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.PortfolioLabelGroups_dbo.PortfolioConfigurations_Configuration_Id] FOREIGN KEY ([Configuration_Id]) REFERENCES [dbo].[PortfolioConfigurations] ([Portfolio_Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Configuration_Id]
    ON [dbo].[PortfolioLabelGroups]([Configuration_Id] ASC);


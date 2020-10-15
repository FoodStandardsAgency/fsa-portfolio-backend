CREATE TABLE [dbo].[Portfolios] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [ViewKey]     NVARCHAR (10)   NULL,
    [ShortName]   NVARCHAR (20)   NULL,
    [Name]        NVARCHAR (250)  NULL,
    [Description] NVARCHAR (1000) NULL,
    [IDPrefix]    NVARCHAR (10)   NULL,
    CONSTRAINT [PK_dbo.Portfolios] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_IDPrefix]
    ON [dbo].[Portfolios]([IDPrefix] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Name]
    ON [dbo].[Portfolios]([Name] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ShortName]
    ON [dbo].[Portfolios]([ShortName] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ViewKey]
    ON [dbo].[Portfolios]([ViewKey] ASC);


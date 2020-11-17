CREATE TABLE [dbo].[Directorates] (
    [Id]      INT            IDENTITY (1, 1) NOT NULL,
    [ViewKey] NVARCHAR (10)  NULL,
    [Name]    NVARCHAR (250) NULL,
    [Order]   INT            NOT NULL,
    CONSTRAINT [PK_dbo.Directorates] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Name]
    ON [dbo].[Directorates]([Name] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ViewKey]
    ON [dbo].[Directorates]([ViewKey] ASC);


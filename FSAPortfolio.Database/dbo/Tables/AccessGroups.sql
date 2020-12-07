CREATE TABLE [dbo].[AccessGroups] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [ViewKey]     NVARCHAR (50) NULL,
    [Description] NVARCHAR (50) NULL,
    CONSTRAINT [PK_dbo.AccessGroups] PRIMARY KEY CLUSTERED ([Id] ASC)
);






GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ViewKey]
    ON [dbo].[AccessGroups]([ViewKey] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Description]
    ON [dbo].[AccessGroups]([Description] ASC);


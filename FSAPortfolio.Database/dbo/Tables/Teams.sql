CREATE TABLE [dbo].[Teams] (
    [Id]      INT            IDENTITY (1, 1) NOT NULL,
    [ViewKey] NVARCHAR (MAX) NULL,
    [Name]    NVARCHAR (MAX) NULL,
    [Order]   INT            NOT NULL,
    CONSTRAINT [PK_dbo.Teams] PRIMARY KEY CLUSTERED ([Id] ASC)
);


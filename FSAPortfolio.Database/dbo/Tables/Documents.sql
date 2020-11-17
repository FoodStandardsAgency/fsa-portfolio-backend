CREATE TABLE [dbo].[Documents] (
    [Id]    INT            IDENTITY (1, 1) NOT NULL,
    [Name]  NVARCHAR (150) NULL,
    [Link]  NVARCHAR (250) NULL,
    [Order] INT            NOT NULL,
    CONSTRAINT [PK_dbo.Documents] PRIMARY KEY CLUSTERED ([Id] ASC)
);

